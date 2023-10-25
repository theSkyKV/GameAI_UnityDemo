using FSM;
using Locations;
using Messaging;
using System.Linq;
using UnityEngine;

namespace Agents.States.MinerStates
{
	public class DigForNuggetState : State<Miner>
	{
		private float _elapsedTime;
		private const float Cooldown = 2.0f;

		public override void Enter(Miner agent)
		{
			if (agent.Location is not Mine)
			{
				var mine = agent.GameWorld.Locations.FirstOrDefault(l => l is Mine);
				agent.StateMachine.ChangeState(new GoToLocationState(mine));
			}

			_elapsedTime = 0;
		}

		public override void Execute(Miner agent)
		{
			if (_elapsedTime < Cooldown)
			{
				_elapsedTime += Time.deltaTime;
				return;
			}

			agent.AddToGoldCarried(1);
			agent.IncreaseFatigue();
			agent.IncreaseThirst();

			Debug.Log("Pickin' up a nugget");

			if (agent.PocketsFull())
			{
				var bank = agent.GameWorld.Locations.FirstOrDefault(l => l is Bank);
				agent.StateMachine.ChangeState(new GoToLocationState(bank));
			}

			if (agent.IsThirsty())
			{
				var tavern = agent.GameWorld.Locations.FirstOrDefault(l => l is Tavern);
				agent.StateMachine.ChangeState(new GoToLocationState(tavern));
			}

			_elapsedTime = 0;
		}

		public override void Exit(Miner agent)
		{
			Debug.Log("Ah'm leavin' the goldmine with mah pockets full o' sweet gold");
		}

		public override bool OnMessage(Miner agent, Telegram message)
		{
			return false;
		}
	}
}
