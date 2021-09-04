using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcatrazLauncher.Helpers
{
	/// <summary>
	/// помощник очереди для выполнения функций результата выполнения запроса
	/// </summary>
	public abstract class IEventQueue
	{
		abstract public void Add(Action act);

		abstract public void ExecuteEvents();
	}

	/// <summary>
	/// простой вид обработчика без очереди - выполняется напрямую
	/// </summary>
	class BasicEventQueue : IEventQueue
	{
		private static BasicEventQueue _queues = new BasicEventQueue();
		public static BasicEventQueue Get() { return _queues; }

		public override void Add(Action act)
		{
			act();
		}

		public override void ExecuteEvents()
		{
		}
	}
}
