using PostSharp.Patterns.Diagnostics;

namespace BlazorUI.Shared.Services.Metrics
{
    //[Log(AttributeExclude = true)]
    public struct ExcludedTime
    {
        public long CpuTime;
        public long ThreadTime;

        public ExcludedTime(long cpuTime, long threadTime)
        {
            CpuTime = cpuTime;
            ThreadTime = threadTime;
        }
    }
}
