using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
	public static class NotificationQuene
	{
		private static readonly object _sync = new object();
		private static List<NotificationQueueEntry> quene = new List<NotificationQueueEntry>();

		public static void AddNotification(NotificationQueueEntry n)
		{
			lock (_sync)
			{
				quene.Add(n);
			}
		}

		public static void Update(QPacketHandlerPRUDP handler)
		{
			lock (_sync)
			{
				for (int i = 0; i < quene.Count; i++)
				{
					NotificationQueueEntry n = quene[i];
					if (n.timer.ElapsedMilliseconds > n.timeout)
					{
						n.Execute(handler);
						n.timer.Stop();
						quene.RemoveAt(i);
						i--;
					}
				}
			}
		}
	}
}
