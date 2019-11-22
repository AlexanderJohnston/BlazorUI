using BlazorUI.Shared.Services.Aspect;
using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BlazorUI.Shared.Services.Metrics
{
    [Log(AttributeExclude = true)]
    public class MetricPublisher : IDisposable
    {
        private readonly MetricsService ProfilingService;
        readonly SampleCollector _sampleCollector;
        private readonly IMetricsClient _client;
        private CapturedMetric[] lastSamples = new CapturedMetric[0];
        Timer timer;
        bool inProgress;

        public MetricPublisher(MetricsService service, SampleCollector collector, IMetricsClient client)
        {
            ProfilingService = service;
            _client = client;
            _sampleCollector = collector;
        }

        public void Start(TimeSpan period)
        {
            timer?.Dispose();
            timer = new Timer(OnTime, null, (int)period.TotalMilliseconds, (int)period.TotalMilliseconds);
        }

        private void OnTime(object state)
        {
            if (inProgress)
            {
                return;
            }

            inProgress = true;
            try
            {
                PublishMetrics();

            }
            finally
            {
                inProgress = false;
            }
        }

        public void PublishMetrics() => PublishMetrics(_sampleCollector.Metadata, _sampleCollector.GetMetrics());

        private void PublishMetrics(IReadOnlyList<CapturedMetadata> methods, CapturedMetric[] metrics)
        {
            for (var i = 0; i < metrics.Length; i++)
            {
                CapturedMetric lastSample;
                if (i < lastSamples.Length)
                {
                    lastSample = lastSamples[i];
                }
                else
                {
                    lastSample = new CapturedMetric { Timestamp = ReferenceFrame.StartTime.Ticks };
                }


                metrics[i].GetDifference(lastSample, out var differenceSample);

                PublishMetric(methods[i], differenceSample);


            }

            lastSamples = metrics;
        }

        private void PublishMetric(CapturedMetadata method, in CapturedMetric metric)
        {

            if (metric.ExecutionCount > 0)
            {

                var metrics = new Dictionary<string, double>
                {
                    ["ExceptionCount"] = metric.ExceptionCount,
                    ["ExecutionCount"] = metric.ExecutionCount,
                    ["CpuTime"] = metric.CpuTimeSpan.TotalMilliseconds,
                    ["ThreadTime"] = metric.ThreadTimeSpan.TotalMilliseconds,
                    ["ExclusiveCpuTime"] = metric.ExclusiveCpuTimeSpan.TotalMilliseconds,
                    ["ExclusiveThreadTime"] = metric.ExclusiveThreadTimeSpan.TotalMilliseconds,
                    ["AsyncTime"] = metric.AsyncTimeSpan.TotalMilliseconds,
                    ["SampleTime"] = metric.SampleTimeSpan.TotalMilliseconds
                };

                _client.TrackEvent(method.Name, metrics: metrics);
            }
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}