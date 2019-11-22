using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BlazorUI.Shared.Services.Metrics
{
    //[Log(AttributeExclude = true)]
    public class ThreadLocalSample
    {
        private readonly Thread _ownerThread = Thread.CurrentThread;
        private MetricAccessor[] _metrics = new MetricAccessor[1024];
        private readonly SampleCollector _parent;
        private readonly Stack<MetricAccessor> _contextStack = new Stack<MetricAccessor>();

        public ThreadLocalSample(SampleCollector parent)
        {
            _parent = parent;
        }

        public void EnterMethod(CapturedMetadata method)
        {
            _contextStack.Push(GetAccessor(method));
        }

        public void ExitMethod(CapturedMetadata method, in ExcludedTime excludedData)
        {
            if (_contextStack.Pop().Metadata != method)
            {
                throw new InvalidOperationException();
            }

            if (_contextStack.Count > 0)
            {
                var parentContext = _contextStack.Peek();
                parentContext.AddExclusion(excludedData);
            }

        }

        public bool GetSample(CapturedMetadata method, out CapturedMetric data)
        {
            MetricAccessor metric;
            if (_metrics.Length <= method.Index || (metric = _metrics[method.Index]) == null)
            {
                data = default;
                return false;
            }
            else
            {
                metric.GetData(out data);
                return true;
            }
        }

        public void AddSample(CapturedMetadata method, in CapturedMetric data)
        {
            var sampleAccessor = GetAccessor(method);

            sampleAccessor.AddData(data);

        }

        private MetricAccessor GetAccessor(CapturedMetadata method)
        {
#if DEBUG
            if (_ownerThread != Thread.CurrentThread)
            {
                throw new InvalidOperationException("Cannot get a mutable MetricAccessor from a different thread than the owner thread.");
            }
#endif

            if (_metrics.Length <= method.Index)
            {
                Array.Resize(ref _metrics, ((_parent.ProfiledMethodCount / 1024) + 1) * 1024);
            }

            var sampleAccessor = _metrics[method.Index];
            if (sampleAccessor == null)
            {
                _metrics[method.Index] = sampleAccessor = new MetricAccessor(method);
            }

            return sampleAccessor;
        }
    }
}
