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

		public static ushort serverBindPort = 21006; // 3074

		public static uint pidCounter = 0x1234;		// 0x84504

		public static uint dummyFriendPidCounter = 0x1235;

		public static List<ClientInfo> clients = new List<ClientInfo>();
		public static Stopwatch uptime = new Stopwatch();
		public static ClientInfo GetOrCreateClient(IPEndPoint ep)
		{
			foreach (ClientInfo c in clients)
			{
				if (c.endpoint.Address.ToString() == ep.Address.ToString() && c.endpoint.Port == ep.Port)
					return c;
			}

			var client = new ClientInfo();
			client.endpoint = ep;
			client.PID = pidCounter++;
			clients.Add(client);

			return client;
		}
	}
}
