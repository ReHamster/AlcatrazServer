using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.DDL;
using System.Collections.Generic;
using System.Diagnostics;

namespace DSFServices
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
						SendNotification(handler, n.client, n.data);

						n.timer.Stop();
						quene.RemoveAt(i);
						i--;
					}
				}
			}
		}
		public static void SendNotification(QPacketHandlerPRUDP handler, QClient client, NotificationEvent eventData)
		{
			var packet = new QPacket();

			// FIXME: is this even valid?
			packet.m_oSourceVPort = new QPacket.VPort(0x31);
			packet.m_oDestinationVPort = new QPacket.VPort(0x3f);

			packet.type = QPacket.PACKETTYPE.DATA;
			packet.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_RELIABLE | QPacket.PACKETFLAG.FLAG_NEED_ACK };
			packet.payload = new byte[0];
			packet.m_bySessionID = client.sessionID;

			var rmc = new RMCPacket();

			rmc.proto = RMCProtocolId.NotificationEventManager;
			rmc.methodID = 1;

			QLog.WriteLine(1, "Sending NotificationEvent");
			QLog.WriteLine(1, DDLSerializer.ObjectToString(eventData));

			RMC.SendRequestPacket(handler, packet, rmc, client, new RMCPRequestDDL<NotificationEvent>(eventData), true, 0);
		}
	}
}
