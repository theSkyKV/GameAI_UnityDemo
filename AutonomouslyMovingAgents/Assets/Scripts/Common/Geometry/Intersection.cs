using GameWorlds;
using UnityEngine;

namespace Common.Geometry
{
	public class Intersection
	{
		public static Vector3 CalculateNearestPointToWall(Wall wall, Transform agent)
		{
			// Ax + By + C = 0 - the line equation
			float x1, x2, y1, y2, x0, y0, A, B, C, x, y;

			x1 = wall.From.z;
			y1 = wall.From.x;
			x2 = wall.To.z;
			y2 = wall.To.x;
			x0 = agent.position.z;
			y0 = agent.position.x;
			A = y2 - y1;
			B = x1 - x2;
			C = x2 * y1 - x1 * y2;

			// (x, y) - the nearest point on the line to the agent position
			x = (B * (B * x0 - A * y0) - A * C) / (A * A + B * B);
			y = (A * (-B * x0 + A * y0) - B * C) / (A * A + B * B);

			return new Vector3(y, 0, x);
		}

		public static Vector3 GetPerpendicularToWall(Wall wall, Transform agent)
		{
			var nearestPoint = CalculateNearestPointToWall(wall, agent);
			return nearestPoint - agent.position;
		}
	}
}
