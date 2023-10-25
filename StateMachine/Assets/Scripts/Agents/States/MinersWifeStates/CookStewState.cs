using FSM;
using Messaging;
using UnityEngine;

namespace Agents.States.MinersWifeStates
{
	public class CookStewState : State<MinersWife>
	{
		private float _elapsedTime;
		private const float CookingTime = 2.0f;

		public override void Enter(MinersWife agent)
		{
			Debug.Log("Putting the stew in the oven");
			_elapsedTime = 0;
		}

		public override void Execute(MinersWife agent)
		{
			if (_elapsedTime < CookingTime)
			{
				_elapsedTime += Time.deltaTime;
				return;
			}

			agent.GameWorld.MessageDispatcher.DispatchMessage(agent, agent.GameWorld.Miner, MessageType.StewReady);
			agent.StateMachine.ChangeState(new DoHouseWorkState());
		}

		public override void Exit(MinersWife agent)
		{
			Debug.Log("Puttin' the stew on the table");
		}

		public override bool OnMessage(MinersWife agent, Telegram message)
		{
			return false;
		}
	}
}
