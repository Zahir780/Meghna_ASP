using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESL.DataAccess.Repository
{

    public class JobScheduler : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<JobScheduler> _logger;
        private Timer _timer;
        private readonly IBackgroundTaskQueue _iBackgroundTaskQueue;
        public JobScheduler(ILogger<JobScheduler> logger, 
            IBackgroundTaskQueue iBackgroundTaskQueue)
        {
            _logger = logger;
            _iBackgroundTaskQueue = iBackgroundTaskQueue;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(12));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            DateTime time = DateTime.Now;
            if(count > 1)
                time = time.AddMinutes(-1);

            _iBackgroundTaskQueue.salesRenduring(time);
            //_logger.LogInformation("Timed Hosted Service is working. Count: {Count}", time.Minute +" :"+time.Second);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
