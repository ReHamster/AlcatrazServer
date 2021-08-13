using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNetZ
{
	// FIXME: rename to secure client? Or UserInfo?
	public class ClientInfo
	{
		public uint PID;
		public uint RVCID;

		//public IPEndPoint endpoint;
		public DateTime lastRecv;

		public uint stationID;
		public string accountId;
		public string name;
		public string pass;
	}
}
