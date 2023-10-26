using Agents.States.MinerStates;
using Entities;
using FSM;
using GameWorlds;
using Locations;
using Messaging;
using System.Linq;

namespace Agents
{
	public class Miner : BaseEntity
	{
		private int _thirst;
		private int _fatigue;

		public GameWorld GameWorld { get; private set; }
		public StateMachine<Miner> StateMachine { get; private set; }
		public Location Location { get; private set; }
		public int GoldCarried { get; private set; }
		public int MoneyInBank { get; private set; }
		public float MovementSpeed => 5.0f;

		// the amount of gold a miner must have before he feels he can go home
		public int ComfortLevel => 4;
		// the amount of nuggets a miner can carry
		public int MaxNuggets => 3;
		// above this value a miner is thirsty
		public int ThirstLevel => 5;
		// above this value a miner is sleepy
		public int TirednessThreshold => 5;

		public void Init(GameWorld gameWorld)
		{
			GameWorld = gameWorld;
			var mine = gameWorld.Locations.FirstOrDefault(l => l is Mine);

			StateMachine = new StateMachine<Miner>(this);
			StateMachine.ChangeState(new GoToLocationState(mine));
		}

		private void Update()
		{
			StateMachine.Update();
		}

		public void ChangeLocation(Location location)
		{
			Location = location;
		}

		public void AddToGoldCarried(int value)
		{
			GoldCarried += value;

			if (GoldCarried < 0)
				GoldCarried = 0;
		}

		public void AddToMoneyInBank(int value)
		{
			MoneyInBank += value;

			if (MoneyInBank < 0)
				MoneyInBank = 0;
		}

		public bool IsThirsty()
		{
			if (_thirst >= ThirstLevel)
				return true;

			return false;
		}

		public bool IsFatigued()
		{
			if (_fatigue > TirednessThreshold)
				return true;

			return false;
		}

		public bool PocketsFull()
		{
			if (GoldCarried >= MaxNuggets)
				return true;

			return false;
		}

		public void IncreaseFatigue()
		{
			_fatigue++;
		}

		public void DecreaseFatigue()
		{
			_fatigue--;
		}

		public void IncreaseThirst()
		{
			_thirst++;
		}

		public void BuyAndDrinkAWhiskey()
		{
			_thirst = 0;
			MoneyInBank -= 2;
		}

		public override bool HandleMessage(Telegram message)
		{
			return StateMachine.HandleMessage(message);
		}
	}
}
