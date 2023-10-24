using Messaging;
using UnityEngine;

namespace FSM
{
	public class StateMachine<T> where T : class
	{
		private T _owner;

		public State<T> CurrentState { get; private set; }
		public State<T> PreviousState { get; private set; }
		public State<T> GlobalState { get; private set; }

		public StateMachine(T owner)
		{
			_owner = owner;
			CurrentState = null;
			PreviousState = null;
			GlobalState = null;
		}

		public void Update()
		{
			GlobalState?.Execute(_owner);
			CurrentState?.Execute(_owner);
		}

		public void ChangeState(State<T> newState)
		{
			if (newState == null)
			{
				Debug.Log("new state is NULL");
				return;
			}

			PreviousState = CurrentState;
			CurrentState?.Exit(_owner);
			CurrentState = newState;
			CurrentState.Enter(_owner);
		}

		public void ChangeGlobalState(State<T> newState)
		{
			if (newState == null)
			{
				Debug.Log("new state is NULL");
				return;
			}

			GlobalState?.Exit(_owner);
			GlobalState = newState;
			GlobalState.Enter(_owner);
		}

		public bool HandleMessage(Telegram message)
		{
			if (CurrentState != null && CurrentState.OnMessage(_owner, message))
				return true;

			if (GlobalState != null && GlobalState.OnMessage(_owner, message))
				return true;

			return false;
		}
	}
}