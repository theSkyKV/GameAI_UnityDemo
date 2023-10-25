using FSM;
using Messaging;
using UnityEngine;

namespace Agents.States.MinerStates
{
	public class EatStewState : State<Miner>
	{
		public override void Enter(Miner agent)
		{
			Debug.Log("Smells Reaaal goood Elsa!");
		}

		public override void Execute(Miner agent)
		{
			Debug.Log("Tastes real good too!");
			agent.StateMachine.RevertToPreviousState();
		}

		public override void Exit(Miner agent)
		{
			Debug.Log("Thankya li'lle lady. Ah better get back to whatever ah wuz doin'");
		}

		public override bool OnMessage(Miner agent, Telegram message)
		{
			return false;
		}
	}
}
