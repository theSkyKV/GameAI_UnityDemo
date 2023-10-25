using Agents.States.MinersWifeStates;
using Entities;
using FSM;
using GameWorlds;
using Messaging;

namespace Agents
{
	public class MinersWife : BaseEntity
	{
		public GameWorld GameWorld { get; private set; }
		public StateMachine<MinersWife> StateMachine { get; private set; }

		public void Init(GameWorld gameWorld)
		{
			GameWorld = gameWorld;

			StateMachine = new StateMachine<MinersWife>(this);
			StateMachine.ChangeState(new DoHouseWorkState());
			StateMachine.ChangeGlobalState(new WifesGlobalState());
		}

		private void Update()
		{
			StateMachine.Update();
		}

		public override bool HandleMessage(Telegram message)
		{
			return StateMachine.HandleMessage(message);
		}
	}
}
