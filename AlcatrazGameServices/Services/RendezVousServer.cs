using DSFServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QNetZ;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Alcatraz.GameServices.Services
{
	public class RendezVousServer : QPRUDPServiceBase
	{
		public override string ServiceName => "RendezVous";
		public override ushort ListenPort => _serverConfig.Value.RDVServerPort;
		public override uint ServerPID => 2;

		private readonly IOptions<QConfiguration> _serverConfig;

		public RendezVousServer(ILogger<RendezVousServer> logger, IOptions<QConfiguration> serverConfig) : base(logger)
		{
			_serverConfig = serverConfig;

			QConfiguration.Instance = serverConfig.Value;

			// register service
			ServiceFactoryDSF.RegisterDSFServices();

			// configure logging
			QLog.EnablePacketLogging = false;
			QLog.EnableFileLogging = false;
			QLog.LogFunction = (int priority, string s, Color color) =>
			{
				if (priority <= 1)
				{
					if (color.R == 255 && color.G == 0)
						_logger.LogError(s);
					else
						_logger.LogInformation(s);
				}
			};
		}
	}
}
