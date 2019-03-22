using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace My.Core.Windows.Service
{
    public class MyServiceHost : IHostedService, IDisposable
    {
        private Timer paymentProcessorTimer;
        private readonly IProcessor processor;
        private readonly IConfiguration configuration;
        private readonly ILogger<MyServiceHost> logger;

        public MyServiceHost(IProcessor processor, IConfiguration configuration, ILogger<MyServiceHost> logger)
        {
            this.processor = processor;
            this.configuration = configuration;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Service started");
            var processingIntervalInSeconds = Convert.ToDouble(configuration["BatchProcessingIntervalInSeconds"]);

            paymentProcessorTimer = new Timer((e) => { processor.Execute(); }, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(processingIntervalInSeconds));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Service stopped");
            paymentProcessorTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            paymentProcessorTimer?.Dispose();
        }
    }
}
