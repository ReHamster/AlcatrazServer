using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace QNetZ
{
	public static class Global
	{
		public static uint pidCounter = 0x1234;     // 0x84504
		public static uint RVCIDCounter = 0xBB98E;

		public static uint dummyFriendPidCounter = 0x1235;

		static List<ClientInfo> clients = new List<ClientInfo>();
		public static Stopwatch uptime = new Stopwatch();

		public static ClientInfo GetClientByConnection(QClient connection)
		{
			foreach (ClientInfo client in clients)
			{
				if (client.endpoint.Address.ToString() == connection.endpoint.Address.ToString() && 
					client.endpoint.Port == connection.endpoint.Port)
					return client;
			}
			return null;
		}

		public static ClientInfo GetClientByPID(uint pid)
		{
			foreach (ClientInfo client in clients)
			{
				if (client.PID == pid)
					return client;
			}
			return null;
		}

		public static ClientInfo GetClientByUsername(string userName)
		{
			foreach (ClientInfo client in clients)
			{
				if (client.name == userName)
					return client;
			}
			return null;
		}

		public static ClientInfo CreateClient(QClient connection)
		{
			var client = new ClientInfo();
			client.endpoint = connection.endpoint;
			client.PID = pidCounter++;
			client.RVCID = RVCIDCounter++;
			clients.Add(client);

			return client;
		}

		public static void PurgeAllClients()
		{
			clients.Clear();
		}

		public static void DropClient(ClientInfo client)
		{
			clients.Remove(client);
		}
	}
}
