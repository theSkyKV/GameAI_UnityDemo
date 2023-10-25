using FSM;
using Messaging;

namespace Agents.States.MinersWifeStates
{
	public class WifesGlobalState : State<MinersWife>
	{
		public override void Enter(MinersWife agent)
		{}

		public override void Execute(MinersWife agent)
		{}

		public override void Exit(MinersWife agent)
		{}

		public override bool OnMessage(MinersWife agent, Telegram message)
		{
			switch(message.Message)
			{
				case MessageType.HiHoneyImHome:
					agent.StateMachine.ChangeState(new CookStewState());
					return true;
			}

			return false;
		}
	}
}
