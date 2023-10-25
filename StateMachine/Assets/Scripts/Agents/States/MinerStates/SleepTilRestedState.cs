using FSM;
using Locations;
using Messaging;
using System.Linq;
using UnityEngine;

namespace Agents.States.MinerStates
{
	public class SleepTilRestedState : State<Miner>
	{
		private float _elapsedTime;
		private const float Cooldown = 2.0f;

		public override void Enter(Miner agent)
		{
			if (agent.Location is not Home)
			{
				var home = agent.GameWorld.Locations.FirstOrDefault(l => l is Home);
				agent.StateMachine.ChangeState(new GoToLocationState(home));
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

			if (!agent.IsFatigued())
			{
				Debug.Log("All mah fatigue has drained away. Time to find more gold!");
				var mine = agent.GameWorld.Locations.FirstOrDefault(l => l is Mine);
				agent.StateMachine.ChangeState(new GoToLocationState(mine));
			}
			else
			{
				agent.DecreaseFatigue();
				Debug.Log("ZZZZZ.....");
			}

			_elapsedTime = 0;
		}

		public override void Exit(Miner agent)
		{}

		public override bool OnMessage(Miner agent, Telegram message)
		{
			switch(message.Message)
			{
				case MessageType.StewReady:
					agent.StateMachine.ChangeState(new EatStewState());
					return true;
			}

			return false;
		}
	}
}
