using FSM;
using Locations;
using Messaging;
using System.Linq;
using UnityEngine;

namespace Agents.States.MinerStates
{
	public class QuenchThirstState : State<Miner>
	{
		public override void Enter(Miner agent)
		{
			if (agent.Location is not Tavern)
			{
				var tavern = agent.GameWorld.Locations.FirstOrDefault(l => l is Tavern);
				agent.StateMachine.ChangeState(new GoToLocationState(tavern));
			}
		}

		public override void Execute(Miner agent)
		{
			agent.BuyAndDrinkAWhiskey();
			Debug.Log("That's mighty fine sippin' liquer");
			var mine = agent.GameWorld.Locations.FirstOrDefault(l => l is Mine);
			agent.StateMachine.ChangeState(new GoToLocationState(mine));
		}

		public override void Exit(Miner agent)
		{
			Debug.Log("Leaving the saloon, feelin' good");
		}

		public override bool OnMessage(Miner agent, Telegram message)
		{
			return false;
		}
	}
}
