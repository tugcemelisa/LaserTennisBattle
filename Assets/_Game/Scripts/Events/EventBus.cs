using System.Collections.Generic;

namespace LaserTennis
{
	public static class EventBus<TEvent>
	{
		private static readonly List<EventListener<TEvent>> Listeners = new List<EventListener<TEvent>>();

		public static void AddListener(EventListener<TEvent> eventListener)
		{
			Listeners.Add(eventListener);
		}

		public static void RemoveListener(EventListener<TEvent> eventListener)
		{
			Listeners.Remove(eventListener);
		}

		public static void Emit(object sender, TEvent e)
		{
			for (int i = Listeners.Count - 1; i >= 0; i--)
			{
				Listeners[i]?.Invoke(sender, e);
			}
		}
	}

	public delegate void EventListener<in TEvent>(object sender, TEvent e);
}