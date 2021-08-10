using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QNetZ;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Alcatraz.GameServices.Services
{
	public class BackendServicesServer : QPRUDPServiceBase
	{
		public override string ServiceName => "BackendServices";
		public override ushort ListenPort => _serverConfig.Value.BackendServiceServerPort;
		public override uint ServerPID => 2;

		private readonly IOptions<QConfiguration> _serverConfig;

		public BackendServicesServer(ILogger<BackendServicesServer> logger, IOptions<QConfiguration> serverConfig) : base(logger)
		{
			_serverConfig = serverConfig;
		}
	}
}
