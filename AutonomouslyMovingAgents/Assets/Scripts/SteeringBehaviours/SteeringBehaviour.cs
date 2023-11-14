using Agents;
using Common.Geometry;
using GameWorlds;
using System.Collections.Generic;
using UnityEngine;

namespace SteeringBehaviours
{
	public class SteeringBehaviour : MonoBehaviour
	{
		[SerializeField]
		[Min(0f)]
		private float _weightSeek = 1.0f;

		[SerializeField]
		[Min(0f)]
		private float _weightFlee = 1.0f;

		[SerializeField]
		[Min(0f)]
		private float _weightArrive = 1.0f;

		[SerializeField]
		[Min(0f)]
		private float _weightPursuit = 1.0f;

		[SerializeField]
		[Min(0f)]
		private float _weightEvade = 1.0f;

		[SerializeField]
		[Min(0f)]
		private float _weightWander = 1.0f;

		[SerializeField]
		[Min(0f)]
		private float _weightObstacleAvoidance = 1.0f;

		private Agent _agent;

		private Vector3 _wanderTarget = Vector3.zero;
		private Vector3 _deltaWanderTarget = Vector3.zero;

		private const float WanderDistance = 2.0f;
		private const float WanderRadius = 1.2f;
		private const float WanderJitterPerSec = 80.0f;

		private enum Deceleration
		{
			Fast = 1,
			Normal = 2,
			Slow = 3
		}

		public void Init(Agent agent)
		{
			_agent = agent;

			var theta = Random.value * Mathf.PI * 2;
			_wanderTarget = new Vector3(WanderRadius * Mathf.Cos(theta), 0, WanderRadius * Mathf.Sin(theta));
		}

		public Vector3 Calculate()
		{
			Vector3 force = Vector3.zero;
			force += Seek(_agent.Target.transform.position) * _weightSeek;
			force += Flee(_agent.Target.transform.position) * _weightFlee;
			force += Arrive(_agent.Target.transform.position, Deceleration.Slow) * _weightArrive;
			force += Pursuit(_agent.Target) * _weightPursuit;
			force += Evade(_agent.Target) * _weightEvade;
			force += Wander() * _weightWander;
			force += ObstacleAvoidance(_agent.GameWorld.Obstacles) * _weightObstacleAvoidance;

			return force;
		}

		private Vector3 Seek(Vector3 targetPosition)
		{
			var desiredVelocity = Vector3.Normalize(targetPosition - _agent.transform.position) * _agent.MaxSpeed;
			return desiredVelocity - _agent.Velocity;
		}

		private Vector3 Flee(Vector3 targetPosition)
		{
			var panicDistance = 10.0f;
			if (Vector3.SqrMagnitude(targetPosition - _agent.transform.position) > panicDistance * panicDistance)
				return Vector3.zero;

			var desiredVelocity = Vector3.Normalize(_agent.transform.position - targetPosition) * _agent.MaxSpeed;
			return desiredVelocity - _agent.Velocity;
		}

		private Vector3 Arrive(Vector3 targetPosition, Deceleration deceleration)
		{
			var toTarget = targetPosition - _agent.transform.position;
			var distance = toTarget.magnitude;

			if (distance > 0)
			{
				var decelerationTweaker = 0.3f;
				var speed = distance / ((float)deceleration * decelerationTweaker);
				speed = Mathf.Clamp(speed, speed, _agent.MaxSpeed);
				var desiredVelocity = toTarget * speed / distance;

				return desiredVelocity - _agent.Velocity;
			}

			return Vector3.zero;
		}

		private Vector3 Pursuit(Agent evader)
		{
			var toEvader = evader.transform.position - _agent.transform.position;
			var relativeHeading = Vector3.Dot(_agent.transform.forward, evader.transform.forward);
			if (Vector3.Dot(toEvader, _agent.transform.forward) > 0 && relativeHeading < -0.95f)
				return Seek(evader.transform.position);

			var lookAheadTime = toEvader.magnitude / (_agent.MaxSpeed + evader.Speed);
			return Seek(evader.transform.position + evader.Velocity * lookAheadTime);
		}

		private Vector3 Evade(Agent pursuer)
		{
			var toPursuer = pursuer.transform.position - _agent.transform.position;
			var threatRange = 10.0f;
			if (toPursuer.sqrMagnitude > threatRange * threatRange)
				return Vector3.zero;

			var lookAheadTime = toPursuer.magnitude / (_agent.MaxSpeed + pursuer.Speed);
			return Flee(pursuer.transform.position + pursuer.Velocity * lookAheadTime);
		}

		private Vector3 Wander()
		{
			var jitterThisTimeSlice = WanderJitterPerSec * Time.deltaTime;
			_deltaWanderTarget.x = Random.Range(-1.0f, 1.0f) * jitterThisTimeSlice;
			_deltaWanderTarget.z = Random.Range(-1.0f, 1.0f) * jitterThisTimeSlice;
			_wanderTarget += _deltaWanderTarget;
			_wanderTarget.Normalize();
			_wanderTarget *= WanderRadius;
			var target = _wanderTarget + Vector3.forward * WanderDistance;
			return target - _agent.transform.position;
		}

		private Vector3 ObstacleAvoidance(IReadOnlyCollection<Obstacle> obstacles)
		{
			var detectionBoxLength = _agent.MinDetectionBoxLength *	(1 + _agent.Speed / _agent.MaxSpeed);
			_agent.GameWorld.TagObstaclesWithinViewRange(_agent, detectionBoxLength);

			Obstacle closestIntersectingObstacle = null;
			var distanceToClosestObstacle = float.MaxValue;
			Vector3 localPosOfClosestObstacle = new Vector3();

			foreach (var obstacle in obstacles)
			{
				if (!obstacle.IsTagged)
					continue;

				var obstaclePos = obstacle.transform.position;
				var localPos = Transformation.VectorToLocalSpace(obstaclePos, _agent.transform);

				if (localPos.z < 0)
					continue;

				var expandedRadius = obstacle.Radius + _agent.DetectionBoxRadius;
				if (localPos.x >= expandedRadius)
					continue;

				var sqrtPart = Mathf.Sqrt(expandedRadius * expandedRadius - localPos.x * localPos.x);
				var intersectionPoint = localPos.z - sqrtPart;
				if (intersectionPoint <= 0)
					intersectionPoint = localPos.z + sqrtPart;

				if (intersectionPoint < distanceToClosestObstacle)
				{
					distanceToClosestObstacle = intersectionPoint;
					closestIntersectingObstacle = obstacle;
					localPosOfClosestObstacle = localPos;
				}
			}

			if (closestIntersectingObstacle == null)
				return Vector3.zero;

			Vector3 steeringForce = new Vector3();

			var multiplier = 1.0f + (detectionBoxLength - localPosOfClosestObstacle.z) / detectionBoxLength;

			steeringForce.x = (closestIntersectingObstacle.Radius - localPosOfClosestObstacle.x) * multiplier;

			var brakingWeight = 0.2f;

			steeringForce.z = (closestIntersectingObstacle.Radius - localPosOfClosestObstacle.z) * brakingWeight;

			return Transformation.VectorToWorldSpace(steeringForce, _agent.transform);
		}
	}
}
