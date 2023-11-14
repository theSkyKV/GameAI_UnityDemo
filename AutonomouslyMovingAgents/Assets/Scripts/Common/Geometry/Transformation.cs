using UnityEngine;

namespace Common.Geometry
{
	public class Transformation
	{
		public static Vector3 VectorToWorldSpace(Vector3 v, Vector3 position, Vector3 forward, Vector3 side)
		{
			Vector3 result = new Vector3();

			result.x = forward.x * v.z + side.x * v.x + position.x;
			result.y = 0;
			result.z = forward.z * v.z + side.z * v.x + position.z;

			return result;
		}

		public static Vector3 VectorToWorldSpace(Vector3 v, Transform transform)
		{
			Vector3 result = new Vector3();

			result.x = transform.forward.x * v.z + transform.right.x * v.x + transform.position.x;
			result.y = 0;
			result.z = transform.forward.z * v.z + transform.right.z * v.x + transform.position.z;

			return result;
		}

		public static Vector3 VectorToLocalSpace(Vector3 v, Vector3 position, Vector3 forward, Vector3 side)
		{
			Vector3 result = new Vector3();

			result.x = forward.x * (position.z - v.z) + forward.z * (v.x - position.x);
			result.y = 0;
			result.z = side.x * (v.z - position.z) + side.z * (position.x - v.x);

			return result;
		}

		public static Vector3 VectorToLocalSpace(Vector3 v, Transform transform)
		{
			Vector3 result = new Vector3();

			result.x = transform.forward.x * (transform.position.z - v.z) + transform.forward.z * (v.x - transform.position.x);
			result.y = 0;
			result.z = transform.right.x * (v.z - transform.position.z) + transform.right.z * (transform.position.x - v.x);

			return result;
		}
	}
}
