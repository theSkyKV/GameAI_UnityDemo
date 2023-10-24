using Agents;
using Locations;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorlds
{
	public class GameWorld : MonoBehaviour
	{
		private Miner _miner;
		private MinersWife _minersWife;

		private Location[] _locations;
		public IReadOnlyCollection<Location> Locations => _locations;

		private void Awake()
		{
			_miner = FindAnyObjectByType<Miner>(FindObjectsInactive.Include);
			_minersWife = FindAnyObjectByType<MinersWife>(FindObjectsInactive.Include);
			_locations = FindObjectsByType<Location>(FindObjectsSortMode.None);
			_miner.Init(this);
		}
	}
}
