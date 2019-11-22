using PostSharp.Patterns.Diagnostics;
using System;
using System.Threading;

namespace BlazorUI.Shared.Services.Metrics
{
    //[Log(AttributeExclude = true)]
    public class MetricAccessor
    {
        public static MetricAccessor Empty = new MetricAccessor(null);
        private volatile int _version;
        private CapturedMetric _data;

        public MetricAccessor(CapturedMetadata metadata)
        {
            Metadata = metadata;
        }

        public CapturedMetadata Metadata { get; }


        public int Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }

        private bool IsWriting => _version % 2 == 1;

        public void AddData(in CapturedMetric data)
        {
#if DEBUG
            if (IsWriting)
            {
                throw new InvalidOperationException();
            }
#endif
            var localVersion = _version;
            _version = localVersion + 1;
            Thread.MemoryBarrier();

            _data.AddData(data);

            Thread.MemoryBarrier();
            _version = localVersion + 2;

        }

        public void AddExclusion(in ExcludedTime data)
        {
#if DEBUG
            if (IsWriting)
            {
                throw new InvalidOperationException();
            }
#endif
            var localVersion = _version;
            _version = localVersion + 1;
            Thread.MemoryBarrier();

            _data.AddExclusion(data);

            Thread.MemoryBarrier();
            _version = localVersion + 2;

        }

        public void GetData(out CapturedMetric data)
        {
            var spinWait = new SpinWait();

            while (true)
            {
                var versionBefore = _version;

                while (IsWriting)
                {
                    spinWait.SpinOnce();
                }


                data = _data;

                if (_version == versionBefore)
                {
                    return;
                }

                spinWait.SpinOnce();
            }

        }
    }
}
