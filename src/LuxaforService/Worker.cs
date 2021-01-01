using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LuxaforService.BusyChecks;

namespace LuxaforService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly LuxaforClient _client;
        private readonly List<IBusyCheck> _busyChecks;

        private bool _isBusy;
        
        public Worker(ILogger<Worker> logger, IEnumerable<IBusyCheck> busyChecks, LuxaforClient client)
        {
            _logger = logger;
            _client = client;
            _busyChecks = busyChecks.ToList();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _client.SetBusyStatus(false);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var isBusy = _busyChecks.Any(check => check.IsBusy());
                if (isBusy != _isBusy)
                {
                    _isBusy = isBusy;
                    await _client.SetBusyStatus(_isBusy);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
