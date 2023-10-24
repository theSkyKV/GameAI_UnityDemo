using FSM;
using Locations;
using Messaging;
using System.Linq;
using UnityEngine;

namespace Agents.States.MinerStates
{
	public class DepositGoldState : State<Miner>
	{
		public override void Enter(Miner agent)
		{
			if (agent.Location is not Bank)
			{
				var bank = agent.GameWorld.Locations.FirstOrDefault(l => l is Bank);
				agent.StateMachine.ChangeState(new GoToLocationState(bank));
			}
		}

		public override void Execute(Miner agent)
		{
			agent.AddToMoneyInBank(agent.GoldCarried);
			agent.AddToGoldCarried(-agent.GoldCarried);
			agent.IncreaseThirst();

			Debug.Log($"Depositing gold. Total savings now: {agent.MoneyInBank}");

			if (agent.MoneyInBank >= agent.ComfortLevel)
			{
				Debug.Log("WooHoo! Rich enough for now. Back home to mah li'lle lady");
				var home = agent.GameWorld.Locations.FirstOrDefault(l => l is Home);
				agent.StateMachine.ChangeState(new GoToLocationState(home));
			}
			else
			{
				var mine = agent.GameWorld.Locations.FirstOrDefault(l => l is Mine);
				agent.StateMachine.ChangeState(new GoToLocationState(mine));
			}
		}

		public override void Exit(Miner agent)
		{
			Debug.Log("Leavin' the bank");
		}

		public override bool OnMessage(Miner agent, Telegram message)
		{
			return false;
		}
	}
}
