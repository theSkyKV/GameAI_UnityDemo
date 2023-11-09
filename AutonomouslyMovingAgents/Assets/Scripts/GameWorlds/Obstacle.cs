using UnityEngine;

namespace GameWorlds
{
	public class Obstacle : MonoBehaviour
	{
		public bool IsTagged { get; private set; }

		public float Radius { get; private set; } = 0.5f;

		public void Tag()
		{
			IsTagged = true;
		}

		public void Untag()
		{
			IsTagged = false;
		}
	}
}
