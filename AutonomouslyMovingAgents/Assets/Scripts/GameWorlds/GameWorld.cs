using Agents;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorlds
{
	public class GameWorld : MonoBehaviour
	{
		private Obstacle[] _obstacles;
		public IReadOnlyCollection<Obstacle> Obstacles => _obstacles;

		private Agent[] _agents;

		private void Awake()
		{
			_obstacles = FindObjectsByType<Obstacle>(FindObjectsSortMode.None);
			_agents = FindObjectsByType<Agent>(FindObjectsSortMode.None);
			foreach (var agent in _agents)
				agent.Init(this);
		}

		public void TagObstaclesWithinViewRange(Agent agent, float radius)
		{
			foreach (var obstacle in _obstacles)
				obstacle.Untag();

			foreach (var obstacle in _obstacles)
			{
				var toObstacle = obstacle.transform.position - agent.transform.position;
				if (Vector3.SqrMagnitude(toObstacle) <= radius * radius)
					obstacle.Tag();
			}
		}
	}
}
