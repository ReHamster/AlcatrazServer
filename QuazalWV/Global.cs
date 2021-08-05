﻿using System;
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
		public static int serverBindPort = 21006; // 3074
		public static uint idCounter = 0x12345678;
		public static uint pidCounter = 0x1234;
		public static uint dummyFriendPidCounter = 0x1235;
		public static string sessionURL = "prudp:/address=127.0.0.1;port=21032;RVCID=4660";

		public static List<ClientInfo> clients = new List<ClientInfo>();
		public static Stopwatch uptime = new Stopwatch();

		public static ClientInfo GetClientByEndPoint(IPEndPoint ep)
		{
			foreach (ClientInfo c in clients)
				if (c.endpoint.Address.ToString() == ep.Address.ToString() && c.endpoint.Port == ep.Port)
					return c;

			return null;
		}

		public static ClientInfo GetClientByIDsend(uint id)
		{
			foreach (ClientInfo c in clients)
				if (c.IDsend == id)
					return c;
			WriteLog(1, "Error : Cant find client for id : 0x" + id.ToString("X8"));
			return null;
		}

		public static ClientInfo GetClientByIDrecv(uint id)
		{
			foreach (ClientInfo c in clients)
				if (c.IDrecv == id)
					return c;
			WriteLog(1, "Error : Cant find client for id : 0x" + id.ToString("X8"));
			return null;
		}

		private static void WriteLog(int priority, string s)
		{
			Log.WriteLine(priority, "[Global] " + s);
		}
	}
}
