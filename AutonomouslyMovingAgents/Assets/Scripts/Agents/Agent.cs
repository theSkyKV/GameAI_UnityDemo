using SteeringBehaviours;
using UnityEngine;

namespace Agents
{
	public class Agent : MonoBehaviour
	{
		[SerializeField]
		private Agent _target;

		[SerializeField]
		[Min(0.01f)]
		private float _mass = 1.0f;

		[SerializeField]
		private SteeringBehaviour _steering;

		public Vector3 Velocity { get; private set; } = Vector3.zero;
		public float Speed => Velocity.magnitude;
		public float MaxSpeed { get; private set; } = 5.0f;
		public Agent Target => _target;

		private void Awake()
		{
			_steering.Init(this);
		}

		private void Update()
		{
			Vector3 steeringForce;
			steeringForce = _steering.Calculate();
			var acceleration = steeringForce / _mass;
			Velocity += acceleration * Time.deltaTime;
			Velocity = Vector3.ClampMagnitude(Velocity, MaxSpeed);
			transform.position += Velocity * Time.deltaTime;

			if (Vector3.SqrMagnitude(Velocity) > 0.0000001f)
				transform.forward = Velocity.normalized;
		}
	}
}