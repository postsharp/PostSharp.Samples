using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostSharp.Samples.Profiling
{
    internal struct MetricData
    {
        public long CpuTime;
        public long ThreadTime;
        public long AsyncTime;
        public long ExcludedThreadTime;
        public long ExcludedCpuTime;
        public int ExecutionCount;
        public int ExceptionCount;
        public long Timestamp;

        public MetricData(  long cpuTime, long threadTime, long asyncTime, long excludedCpuTime, long excludedThreadTime, int executionCount, int exceptionCount, long timestamp)
        {
            this.CpuTime = cpuTime;
            this.ThreadTime = threadTime;
            this.AsyncTime = asyncTime;
            this.ExcludedThreadTime = excludedThreadTime;
            this.ExcludedCpuTime = excludedCpuTime;
            this.ExecutionCount = executionCount;
            this.ExceptionCount = exceptionCount;
            this.Timestamp = timestamp;
        }

        public TimeSpan AsyncTimeSpan => TimeSpan.FromSeconds((double)this.AsyncTime / Stopwatch.Frequency);

        public TimeSpan ThreadTimeSpan => TimeSpan.FromSeconds((double)this.ThreadTime/ Stopwatch.Frequency);

        public TimeSpan CpuTimeSpan => TimeSpan.FromSeconds(this.CpuTime / Win32.GetThreadTimesFrequency);


        public TimeSpan ExclusiveThreadTimeSpan => TimeSpan.FromSeconds((double)(this.ThreadTime - this.ExcludedThreadTime) / Stopwatch.Frequency);

        public TimeSpan ExclusiveCpuTimeSpan => TimeSpan.FromSeconds( (this.CpuTime - this.ExcludedCpuTime) / Win32.GetThreadTimesFrequency);

        public TimeSpan SampleTimeSpan => TimeSpan.FromSeconds((double)this.Timestamp / Stopwatch.Frequency);


        public void AddData( in MetricData data )
        {
            this.CpuTime += data.CpuTime;
            this.ThreadTime += data.ThreadTime;
            this.AsyncTime += data.AsyncTime;
            this.ExcludedCpuTime += data.ExcludedCpuTime;
            this.ExcludedThreadTime += data.ExcludedThreadTime;
            this.ExecutionCount += data.ExecutionCount;
            this.ExceptionCount += data.ExceptionCount;
        }

        public void AddExclusion( in ExcludedTimeData data)
        {
            this.ExcludedCpuTime += data.CpuTime;
            this.ExcludedThreadTime += data.ThreadTime;
        }

        public void GetDifference( in MetricData reference, out MetricData difference )
        {
            difference = new MetricData(
                this.CpuTime - reference.CpuTime,
                this.ThreadTime - reference.ThreadTime,
                this.AsyncTime - reference.AsyncTime,
                this.ExcludedCpuTime - reference.ExcludedCpuTime,
                this.ExcludedThreadTime - reference.ExcludedThreadTime,
                this.ExecutionCount - reference.ExecutionCount,
                this.ExceptionCount - reference.ExceptionCount,
                this.Timestamp - reference.Timestamp);
                 
        }

        internal void SetInvalid() => this.CpuTime = -1;

        public bool IsInvalid => this.CpuTime < 0;
    }
}
