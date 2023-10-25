using Messaging;
using UnityEngine;

namespace Entities
{
	public abstract class BaseEntity : MonoBehaviour
	{
		public abstract bool HandleMessage(Telegram message);
	}
}
