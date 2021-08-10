using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QNetZ;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Alcatraz.GameServices.Services
{
	public abstract class QPRUDPServiceBase : IHostedService
	{
		private Timer _timer;
		protected ILogger _logger;
		public abstract string ServiceName { get; }
		public abstract ushort ListenPort { get; }
		public abstract uint ServerPID { get; }

		public UdpClient listener;
		public QPacketHandlerPRUDP packetHandler;

		public QPRUDPServiceBase(ILogger logger)
		{
			_logger = logger;
		}

		//----------------------------------------------------------

		public Task StartAsync(CancellationToken cancellationToken)
        {
			_logger.LogInformation($"{ServiceName} is STARTED.");

			listener = new UdpClient(ListenPort);
			packetHandler = new QPacketHandlerPRUDP(listener, ServerPID, ListenPort, ServiceName);

			_timer = new Timer(Process, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1));
			return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
			_timer.Change(Timeout.Infinite, Timeout.Infinite);
			CurrentRecvTask = null;
			listener.Close();

			_logger.LogInformation($"{ServiceName} is STOPPED.");

			await Task.Yield();
		}

		Task<UdpReceiveResult> CurrentRecvTask = null;

		protected void Process(object state)
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
