using UnityEngine;

namespace GameWorlds
{
	public class Wall : MonoBehaviour
	{
		[SerializeField]
		private Vector3 _from;

		[SerializeField]
		private Vector3 _to;

		[SerializeField]
		private Vector3 _n;

		public Vector3 From => _from;
		public Vector3 To => _to;
		public Vector3 N => _n;
	}
}
