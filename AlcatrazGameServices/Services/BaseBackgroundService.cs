using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alcatraz.GameServices.Services
{
	public abstract class BaseBackgroundService : IHostedService
	{
		private Timer _timer;
		protected ILogger _logger;
		public abstract int StartIntervalMilliseconds { get; }

		public BaseBackgroundService(ILogger logger)
		{
			_logger = logger;
		}

		//----------------------------------------------------------
		private void DoWork(object state)
		{
			// discard the result
			_ = Process();
		}

		//----------------------------------------------------------

		public virtual Task StartAsync(CancellationToken cancellationToken)
        {
			_logger.LogInformation("Background service is STARTED.");

			_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(StartIntervalMilliseconds));
			return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
			_logger.LogInformation("Background service is STOPPED.");
			await Task.Yield();
		}

        protected abstract Task Process();
    }
}
