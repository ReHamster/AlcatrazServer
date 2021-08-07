using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace QuazalWV
{
	public class NotificationQueueEntry
	{
		public QClient client;
		public Stopwatch timer;
		public uint timeout;
		public uint source;
		public uint type;
		public uint subType;
		public uint param1;
		public uint param2;
		public uint param3;
		public string paramStr;

		public NotificationQueueEntry(QClient c, uint time, uint src, uint t, uint st, uint p1, uint p2, uint p3, string ps)
		{
			client = c;
			timer = new Stopwatch();
			timer.Start();
			timeout = time;
			source = src;
			type = t;
			subType = st;
			param1 = p1;
			param2 = p2;
			param3 = p3;
			paramStr = ps;
		}

		public void Execute(QPacketHandlerPRUDP handler)
		{
			RMC.SendNotification(handler, client, source, type, subType, param1, param2, param3, paramStr);
		}
	}
}
