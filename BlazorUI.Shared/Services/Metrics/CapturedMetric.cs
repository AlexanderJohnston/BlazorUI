/// Inspired by the PostSharp.Samples.Profiling example. This class borrows heavily from it.
using PostSharp.Patterns.Diagnostics;
using System;
using System.Diagnostics;

namespace BlazorUI.Shared.Services.Metrics
{
    //[Log(AttributeExclude = true)]
    public struct CapturedMetric
    {
        public long CpuTime;
        public long ThreadTime;
        public long AsyncTime;
        public long ExcludedThreadTime;
        public long ExcludedCpuTime;
        public int ExecutionCount;
        public int ExceptionCount;
        public long Timestamp;

        public CapturedMetric(long cpuTime, long threadTime, long asyncTime, long excludedThreadTime, long excludedCpuTime, int executionCount, int exceptionCount, long timestamp)
        {
            CpuTime = cpuTime;
            ThreadTime = threadTime;
            AsyncTime = asyncTime;
            ExcludedThreadTime = excludedThreadTime;
            ExcludedCpuTime = excludedCpuTime;
            ExecutionCount = executionCount;
            ExceptionCount = exceptionCount;
            Timestamp = timestamp;
        }

        public TimeSpan AsyncTimeSpan => TimeSpan.FromSeconds((double)AsyncTime / Stopwatch.Frequency);

        public TimeSpan ThreadTimeSpan => TimeSpan.FromSeconds((double)ThreadTime / Stopwatch.Frequency);

        public TimeSpan CpuTimeSpan => TimeSpan.FromTicks(CpuTime);

        public TimeSpan ExclusiveThreadTimeSpan => TimeSpan.FromSeconds((double)(ThreadTime - ExcludedThreadTime) / Stopwatch.Frequency);

        public TimeSpan ExclusiveCpuTimeSpan => TimeSpan.FromTicks((CpuTime - ExcludedCpuTime));

        public TimeSpan SampleTimeSpan => TimeSpan.FromSeconds((double)Timestamp / Stopwatch.Frequency);

        public void AddData(in CapturedMetric data)
        {
            CpuTime += data.CpuTime;
            ThreadTime += data.ThreadTime;
            AsyncTime += data.AsyncTime;
            ExcludedCpuTime += data.ExcludedCpuTime;
            ExcludedThreadTime += data.ExcludedThreadTime;
            ExecutionCount += data.ExecutionCount;
            ExceptionCount += data.ExceptionCount;
        }

        public void AddExclusion(in ExcludedTime data)
        {
            ExcludedCpuTime += data.CpuTime;
            ExcludedThreadTime += data.ThreadTime;
        }

        public void GetDifference(in CapturedMetric reference, out CapturedMetric difference) => difference = new CapturedMetric(
                CpuTime - reference.CpuTime,
                ThreadTime - reference.ThreadTime,
                AsyncTime - reference.AsyncTime,
                ExcludedCpuTime - reference.ExcludedCpuTime,
                ExcludedThreadTime - reference.ExcludedThreadTime,
                ExecutionCount - reference.ExecutionCount,
                ExceptionCount - reference.ExceptionCount,
                Timestamp - reference.Timestamp);

        internal void SetInvalid() => CpuTime = -1;

        public bool IsInvalid => CpuTime < 0;
    }
}
