using BlazorUI.Shared.Services.Aspect;
using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace BlazorUI.Shared.Services.Metrics
{
    //[Log(AttributeExclude = true)]
    public class SampleCollector
    {
        // This is the maximal duration allowed to collect the metrics from all threads for a single method.
        static readonly long timestampTolerance = Stopwatch.Frequency / 100;

        public MetricsService ProfilingService;

        private ThreadLocal<ThreadLocalSample> _collectors;
        public IList<ThreadLocalSample> Collectors => _collectors.Values;

        private readonly object registrationLock = new object();

        private CapturedMetadata[] _metricsMetadata = new CapturedMetadata[1024];

        public int ProfiledMethodCount { get; private set; }
        internal CapturedMetadata[] Metadata => _metricsMetadata;

        /// <summary>
        ///     If <see langword="SampleCollector"/> is stored in a static field then use this to get a safe reference.
        /// </summary>
        /// <param name="profileService"></param>
        /// <returns></returns>
        public SampleCollector Initialize(MetricsService profileService)
        {
            ProfilingService = profileService;
            _collectors = new ThreadLocal<ThreadLocalSample>(() => new ThreadLocalSample(this), true);
            return this;
        }

        public CapturedMetadata RegisterMethod(MethodBase method)
        {
            lock (registrationLock)
            {
                var profiledMethod = new CapturedMetadata(method, ProfiledMethodCount);

                if (Metadata.Length <= ProfiledMethodCount)
                {
                    Array.Resize(ref _metricsMetadata, Metadata.Length * 2);
                }

                Metadata[profiledMethod.Index] = profiledMethod;
                ProfiledMethodCount++;
                return profiledMethod;
            }

        }

        public ThreadLocalSample GetCollector() => _collectors.Value;


        public CapturedMetric[] GetMetrics()
        {

            CapturedMetadata[] profiledMethodsCopy;
            CapturedMetric[] metrics;

            lock (registrationLock)
            {
                profiledMethodsCopy = Metadata;
                metrics = new CapturedMetric[ProfiledMethodCount];
            }

            var treadLocalCollectorsCopy = _collectors.Values;

            var timestamp = ReferenceFrame.Now();

            for (var i = 0; i < metrics.Length; i++)
            {
                var method = profiledMethodsCopy[i];

                var attempts = 0;
                while (true)
                {
                    metrics[i].Timestamp = timestamp.Ticks;

                    foreach (var threadLocalCollector in treadLocalCollectorsCopy)
                    {


                        if (threadLocalCollector.GetSample(method, out var threadLocalData))
                        {
                            metrics[i].AddData(threadLocalData);
                        }
                    }

                    timestamp = ReferenceFrame.Now();


                    // Detect if our thread has been preempted, and retry if so.
                    if (timestamp.Ticks - metrics[i].Timestamp < timestampTolerance)
                    {
                        break;
                    }
                    else
                    {
                        metrics[i] = default;
                        attempts++;

                        if (attempts > 3)
                        {
                            Console.WriteLine("Too many non-voluntary preemptions in the publisher thread. ");
                            metrics[i].SetInvalid();
                            break;
                        }

                    }

                }



            }

            return metrics;

        }
    }
}