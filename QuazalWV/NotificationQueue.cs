using QuazalWV.DDL.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace QuazalWV
{
	public class NotificationQueueEntry
	{
		public NotificationQueueEntry(uint _timeout, QClient _client, NotificationEvent eventData)
		{
			client = _client;
			timeout = _timeout;
			data = eventData;

			timer = new Stopwatch();
			timer.Start();
		}

		public QClient client;
		public Stopwatch timer;
		public NotificationEvent data;
		public uint timeout;
	}

	public static class NotificationQueue
	{
		private static readonly object _sync = new object();
		private static List<NotificationQueueEntry> quene = new List<NotificationQueueEntry>();

		public static void AddNotification(NotificationEvent eventData, QClient client, uint timeout)
		{
			var qItem = new NotificationQueueEntry(timeout, client, eventData);

			lock (_sync)
			{
				quene.Add(qItem);
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
						RMC.SendNotification(handler, n.client, n.data);

						n.timer.Stop();
						quene.RemoveAt(i);
						i--;
					}
				}
			}
		}
	}
}
