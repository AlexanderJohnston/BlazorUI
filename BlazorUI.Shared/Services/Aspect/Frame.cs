using BlazorUI.Shared.Services.Metrics;
using PostSharp.Patterns.Diagnostics;
using System;
using System.Diagnostics;

namespace BlazorUI.Shared.Services.Aspect
{
    //[Log(AttributeExclude = true)]
    public static class ReferenceFrame
    {
        /// <summary>
        ///     Exposed to compile-time PostSharp aspects via MethodCallData to emit the code necessary for collecting samples.
        /// </summary>
        public static SampleCollector StaticCollector = new SampleCollector();

        /// <summary>
        ///     MetricsService is responsible for exposing this method for compile-time safety checks.
        /// </summary>
        public static Func<bool> CheckEnabled;

        public static Stopwatch Time = Stopwatch.StartNew();

        public static TimeSpan Moment = TimeSpan.FromTicks(1);

        public static TimeSpan Period;

        public static TimeSpan Now() => Time.Elapsed.Add(Moment);

        public static TimeSpan StartTime { get; set; }

        public static void Start() => StartTime = Now();
    }
}
