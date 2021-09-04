using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcatrazLauncher.Helpers
{
	/// <summary>
	/// Очередь для асинхронного выполнения. ExecuteEvents можно выполнить в Application.Idle
	/// </summary>
	class UIEventQueue : IEventQueue
	{
		private static UIEventQueue _queues = new UIEventQueue();
		public static UIEventQueue Get() { return _queues; }

		private List<Action> EventList;

		public UIEventQueue()
		{
			EventList = new List<Action>();
		}

		public override void Add(Action act)
		{
			lock (EventList)
			{
				EventList.Add(act);
			}
		}

		public override void ExecuteEvents()
		{
			lock (EventList)
			{
				foreach (var action in EventList)
				{
					action();
				}

				EventList.Clear();
			}
		}
	}
}
