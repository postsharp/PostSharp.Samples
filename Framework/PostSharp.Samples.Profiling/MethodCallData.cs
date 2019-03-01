using System;
using System.Diagnostics;

namespace PostSharp.Samples.Profiling
{
    public struct MethodCallData
    {
        
        internal MetricData MetricData;

        private long _kernelTimestamp;
        private long _threadTimestamp;
        private long _userTimestamp;
        private long _asyncTimestamp;
        ThreadLocalSampleCollector _threadLocalCollector;

        internal void Start(MetricMetadata metadata)
        {
            this._asyncTimestamp = ProfilingServices.GetTimestamp();
            this.Metadata = metadata;
            this.MetricData.ExecutionCount = 1;

            this.Resume();
        }

        public bool IsNull => this._asyncTimestamp == 0;

        internal MetricMetadata Metadata { get; private set; }
        
     
        internal void Resume()
        {
            this._threadTimestamp = ProfilingServices.GetTimestamp();

            Win32.GetThreadTimes(Win32.GetCurrentThread(), out _, out _, out this._kernelTimestamp, out this._userTimestamp);

            this._threadLocalCollector = ProfilingServices.Collector.GetThreadLocalCollector();
            this._threadLocalCollector.EnterMethod(this.Metadata);
        }

        internal void Stop()
        {
            this.Pause();

            this.MetricData.AsyncTime = ProfilingServices.GetTimestamp() - this._asyncTimestamp;
        }

        internal void Pause()
        {
            Win32.GetThreadTimes(Win32.GetCurrentThread(), out _, out _, out long kernelTime, out long userTime);


            long cpuTime = (kernelTime - this._kernelTimestamp) + (userTime - this._userTimestamp);

            long threadTime = ProfilingServices.GetTimestamp() - this._threadTimestamp;

            this.MetricData.CpuTime += cpuTime;
            this.MetricData.ThreadTime += threadTime;

            this._threadLocalCollector.ExitMethod(this.Metadata, new ExcludedTimeData(cpuTime, threadTime));
        }

        internal void AddException() => this.MetricData.ExceptionCount++;
    }
}