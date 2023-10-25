using FSM;
using Messaging;
using UnityEngine;

namespace Agents.States.MinersWifeStates
{
	public class DoHouseWorkState : State<MinersWife>
	{
		private float _elapsedTime;
		private const float Cooldown = 2.0f;

		public override void Enter(MinersWife agent)
		{
			Debug.Log("Time to do some more housework!");
			_elapsedTime = 0;
		}

		public override void Execute(MinersWife agent)
		{
			if (_elapsedTime < Cooldown)
			{
				_elapsedTime += Time.deltaTime;
				return;
			}

			switch (Random.Range(0, 3))
			{
				case 0:
					Debug.Log("Moppin' the floor");
					break;
				case 1:
					Debug.Log("Washin' the dishes");
					break;
				case 2:
					Debug.Log("Makin' the bed");
					break;
			}

			_elapsedTime = 0;
		}

		public override void Exit(MinersWife agent)
		{}

		public override bool OnMessage(MinersWife agent, Telegram message)
		{
			return false;
		}
	}
}
