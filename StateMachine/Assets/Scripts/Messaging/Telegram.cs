using Entities;

namespace Messaging
{
	public enum MessageType
	{
		HiHoneyImHome,
		StewReady,
		Default
	}

	public class Telegram
	{
		public BaseEntity Sender { get; private set; }
		public BaseEntity Receiver { get; private set; }
		public MessageType Message { get; private set; }
		public float DispatchTime { get; private set; }

		public const float SmallestDelay = 0.25f;

		public Telegram()
		{
			Sender = null;
			Receiver = null;
			Message = MessageType.Default;
			DispatchTime = -1;
		}

		public Telegram(BaseEntity sender, BaseEntity receiver, MessageType message, float dispatchTime)
		{
			Sender = sender;
			Receiver = receiver;
			Message = message;
			DispatchTime = dispatchTime;
		}
	}
}
