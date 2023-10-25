using Entities;
using UnityEngine;

namespace Messaging
{
	public class MessageDispatcher
	{
		private void Discharge(BaseEntity receiver, Telegram message)
		{
			if (!receiver.HandleMessage(message))
				Debug.Log("Message not handled");
		}

		public void DispatchMessage(BaseEntity sender, BaseEntity receiver, MessageType message)
		{
			var telegram = new Telegram(sender, receiver, message);
			Discharge(receiver, telegram);
		}
	}
}
