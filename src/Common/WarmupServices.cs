using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProductmanagementCore.Common
{
    public class WarmupServices : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly HttpClient httpClient;

        public WarmupServices(ILogger<WarmupServices> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
        }



        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting IHostedService...");

            httpClient.GetAsync("https://localhost:5001/api/Order/getall");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StoppingIHostedService...");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
