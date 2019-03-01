using System;
using System.Linq;

namespace PostSharp.Samples.Profiling
{
    internal struct ExcludedTimeData
    {
        public long CpuTime;
        public long ThreadTime;

        public ExcludedTimeData(long cpuTime, long threadTime)
        {
            this.CpuTime = cpuTime;
            this.ThreadTime = threadTime;
        }
    }
}
