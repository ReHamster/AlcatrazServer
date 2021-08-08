using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace QuazalWV
{
	public static class Global
	{
		public static readonly string ServerFilesPath = "ServerFiles/";

		public static readonly string keyDATA = "CD&ML";        // default ancient Quazal encryption key (RC4)
		public static string accessKey = "8dtRv2oj";            // Server access key. Affects packet checksum; TODO: make configurable

		public static readonly int packetFragmentSize = 963;

		public static string serverBindAddress = "127.0.0.1";

		public static ushort RDVServerPort = 21005;
		public static ushort BackendServiceServerPort = 21006;

		public static uint pidCounter = 0x1234;     // 0x84504
		public static uint RVCIDCounter = 0xBB98E;

		public static uint dummyFriendPidCounter = 0x1235;

		public static List<ClientInfo> clients = new List<ClientInfo>();
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
	}
}
