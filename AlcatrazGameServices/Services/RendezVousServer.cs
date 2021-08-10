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
	public class RendezVousServer : BaseBackgroundService
	{
		public static readonly string ServiceName = "RendezVous";
		public static readonly uint ServerPID = 2;
		public override int StartIntervalMilliseconds => 1;

		private readonly IOptions<QConfiguration> _serverConfig;

		public static UdpClient listener;
		public static QPacketHandlerPRUDP packetHandler;

		public RendezVousServer(ILogger<RendezVousServer> logger, IOptions<QConfiguration> serverConfig) : base(logger)
		{
			_serverConfig = serverConfig;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			// register service
			ServiceFactoryDSF.RegisterDSFServices();

			Log.LogFunction = (int priority, string s, Color color) =>
			{
				if (priority <= 2)
				{
					if (color.R == 255 && color.G == 0)
						_logger.LogError(s);
					else
						_logger.LogInformation(s);
				}
			};

			QConfiguration.Instance = _serverConfig.Value;
			var listenPort = QConfiguration.Instance.RDVServerPort;

			listener = new UdpClient(listenPort);
			packetHandler = new QPacketHandlerPRUDP(listener, ServerPID, listenPort, ServiceName);

			return base.StartAsync(cancellationToken);
		}

		public override Task StopAsync(CancellationToken cancellationToken)
		{
			listener.Close();

			return base.StopAsync(cancellationToken);
		}

		static Task<UdpReceiveResult> CurrentRecvTask = null;

		protected override async Task Process()
		{
			// use non-blocking recieve
			if (CurrentRecvTask != null)
			{
				if (CurrentRecvTask.IsCompleted)
				{
					var result = CurrentRecvTask.Result;
					CurrentRecvTask = null;
					packetHandler.ProcessPacket(result.Buffer, result.RemoteEndPoint);
				}
				else if (CurrentRecvTask.IsCanceled || CurrentRecvTask.IsFaulted)
				{
					CurrentRecvTask = null;
				}
			}

			if (CurrentRecvTask == null)
				CurrentRecvTask = listener.ReceiveAsync();
		}
	}
}
