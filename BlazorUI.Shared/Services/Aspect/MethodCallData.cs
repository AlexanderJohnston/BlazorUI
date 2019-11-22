using PostSharp.Patterns.Diagnostics;
using BlazorUI.Shared.Services.Metrics;

namespace BlazorUI.Shared.Services.Aspect
{
    [Log(AttributeExclude = true)]
    public struct MethodCallData
    {
        internal CapturedMetric MetricData;

        private long _kernelTimestamp;
        private long _threadTimestamp;
        private long _userTimestamp;
        private long _asyncTimestamp;
        ThreadLocalSample _collector;

        internal void Start(CapturedMetadata metadata)
        {
            _asyncTimestamp = ReferenceFrame.Now().Ticks;
            Metadata = metadata;
            MetricData.ExecutionCount = 1;

            Resume();
        }

        internal bool IsNull => _asyncTimestamp == 0;

        internal CapturedMetadata Metadata { get; private set; }


        internal void Resume()
        {
            _threadTimestamp = ReferenceFrame.Now().Ticks;
            _collector = ReferenceFrame.StaticCollector.GetCollector();
            _collector.EnterMethod(Metadata);
        }

        internal void Stop()
        {
            Pause();

            MetricData.AsyncTime = ReferenceFrame.Now().Ticks - _asyncTimestamp;
        }

        internal void Pause()
        {
            Win32.GetThreadTimes(Win32.GetCurrentThread(), out _, out _, out var kernelTime, out var userTime);


            var cpuTime = (kernelTime - this._kernelTimestamp) + (userTime - this._userTimestamp);

            var threadTime = ReferenceFrame.Now().Ticks - this._threadTimestamp;

            this.MetricData.CpuTime += cpuTime;
            this.MetricData.ThreadTime += threadTime;

            this._collector.ExitMethod(this.Metadata, new ExcludedTime(cpuTime, threadTime));
        }

        internal void AddException() => this.MetricData.ExceptionCount++;
    }
}
