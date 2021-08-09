using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNetZ
{
	public class QConfiguration
	{
		public string ServerBindAddress;
		public ushort RDVServerPort;
		public ushort BackendServiceServerPort;
		public string ServerFilesPath;
		public string SandboxAccessKey;            // Server access key. Affects packet checksum;

		public byte SandboxAccessKeyCheckSum
		{
			get
			{
				return (byte)Encoding.ASCII.GetBytes(SandboxAccessKey).Sum(b => b);
			}
		}


		// -------------------------------------------------------- static shit
		static QConfiguration _instance;
		public static QConfiguration Instance {
			get {
				return _instance;
			}
			set {
				_instance = value;
			}
		}

		public static QConfiguration MakeDevelopmentConfiguration()
		{
			var cfg = new QConfiguration();

			cfg.ServerBindAddress = "127.0.0.1";
			cfg.RDVServerPort = 21005;
			cfg.BackendServiceServerPort = 21006;

			cfg.ServerFilesPath = "ServerFiles/";

			cfg.SandboxAccessKey = "8dtRv2oj";            // Server access key. Affects packet checksum;

			return cfg;
		}
	}
}
