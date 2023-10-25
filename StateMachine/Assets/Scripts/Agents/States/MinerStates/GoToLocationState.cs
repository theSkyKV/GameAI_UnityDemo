using FSM;
using Locations;
using Messaging;
using UnityEngine;

namespace Agents.States.MinerStates
{
	public class GoToLocationState : State<Miner>
	{
		private Location _destionation;

		public GoToLocationState(Location destination)
		{
			_destionation = destination;
		}

		public override void Enter(Miner agent)
		{}

		public override void Execute(Miner agent)
		{
			agent.transform.position = Vector3.MoveTowards
				(agent.transform.position, _destionation.transform.position, agent.MovementSpeed * Time.deltaTime);

			if (Vector3.SqrMagnitude(_destionation.transform.position - agent.transform.position) < 1.0f)
			{
				agent.ChangeLocation(_destionation);

				switch(_destionation)
				{
					case Home:
						agent.StateMachine.ChangeState(new SleepTilRestedState());
						Debug.Log("Hi honey I'm home");
						agent.GameWorld.MessageDispatcher.DispatchMessage(agent, 
							agent.GameWorld.MinersWife, MessageType.HiHoneyImHome);
						break;
					case Tavern:
						agent.StateMachine.ChangeState(new QuenchThirstState());
						break;
					case Mine:
						agent.StateMachine.ChangeState(new DigForNuggetState());
						break;
					case Bank:
						agent.StateMachine.ChangeState(new DepositGoldState());
						break;
				}
			}
		}

		public override void Exit(Miner agent)
		{}

		public override bool OnMessage(Miner agent, Telegram message)
		{
			return false;
		}
	}
}
