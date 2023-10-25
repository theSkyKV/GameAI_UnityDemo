using Agents;
using Locations;
using Messaging;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorlds
{
	public class GameWorld : MonoBehaviour
	{
		public Miner Miner { get; private set; }
		public MinersWife MinersWife { get; private set; }
		public MessageDispatcher MessageDispatcher { get; private set; }

		private Location[] _locations;
		public IReadOnlyCollection<Location> Locations => _locations;

		private void Awake()
		{
			MessageDispatcher = new MessageDispatcher();
			Miner = FindAnyObjectByType<Miner>(FindObjectsInactive.Include);
			MinersWife = FindAnyObjectByType<MinersWife>(FindObjectsInactive.Include);
			_locations = FindObjectsByType<Location>(FindObjectsSortMode.None);
			Miner.Init(this);
			MinersWife.Init(this);
		}
	}
}
