using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QNetZ
{
	public class QConfiguration
	{
		public string ServerBindAddress { get; set; }
		public ushort RDVServerPort { get; set; }
		public ushort BackendServiceServerPort { get; set; }
		public string ServerFilesPath { get; set; }
		public string SandboxAccessKey { get; set; }            // Server access key. Affects packet checksum;
		public string DbConnectionString { get; set; }
		public int DbType { get; set; }
		public int LogLevel { get; set; }						// not used in debug server. See Services/RendezVousServer.cs

		public byte SandboxAccessKeyCheckSum
		{
			get
			{
				if (SandboxAccessKey == null)
					return 0;

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

		public static QConfiguration MakeDevelopmentConfiguration(string serverBindAddress)
		{
			var cfg = new QConfiguration();

			cfg.ServerBindAddress = serverBindAddress ?? "127.0.0.1";
			cfg.RDVServerPort = 21005;
			cfg.BackendServiceServerPort = 21006;

			cfg.ServerFilesPath = "ServerFiles/";

			cfg.SandboxAccessKey = "8dtRv2oj";            // Server access key. Affects packet checksum;

			cfg.DbType = 0;
			cfg.DbConnectionString = "Data Source=database.sqlite";

			return cfg;
		}
	}
}
