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
	public class BackendServicesServer : BaseBackgroundService
	{
		public static readonly string ServiceName = "BackendServices";
		public static readonly uint ServerPID = 4;
		public override int StartIntervalMilliseconds => 1;

		private readonly IOptions<QConfiguration> _serverConfig;

		public static UdpClient listener;
		public static QPacketHandlerPRUDP packetHandler;

		public BackendServicesServer(ILogger<BackendServicesServer> logger, IOptions<QConfiguration> serverConfig) : base(logger)
		{
			_serverConfig = serverConfig;
		}

		public override Task StartAsync(CancellationToken cancellationToken)
		{
			QConfiguration.Instance = _serverConfig.Value;
			var listenPort = QConfiguration.Instance.BackendServiceServerPort;

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
			try
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
			catch (Exception ex)
			{
				_logger.LogError(ex.Message + ex.StackTrace);
			}
		}
	}
}
