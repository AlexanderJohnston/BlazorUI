using BlazorUI.Shared.Services.Aspect;
using BlazorUI.Shared.Services.Metrics;
using Microsoft.Extensions.Hosting;
using PostSharp.Patterns.Diagnostics;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorUI.Shared.Services
{
    [Log(AttributeExclude = true)]
    public class MetricsService : IHostedService
    {
        public bool IsEnabled => Publisher != null;

        public MetricPublisher Publisher { get; private set; }


        public MetricsService(IMetricsClient client, TimeSpan period)
        {
            ReferenceFrame.Period = period;
            var collector = ReferenceFrame.StaticCollector.Initialize(this);
            Publisher = new MetricPublisher(this, collector, client);
            ReferenceFrame.CheckEnabled = () => IsEnabled;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ReferenceFrame.Start();
            Publisher.Start(ReferenceFrame.Period);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Publisher.Dispose();
            return Task.CompletedTask;
        }

    }
}
