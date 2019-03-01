using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostSharp.Samples.Profiling
{
    public static class ProfilingServices
    {

        private static readonly Stopwatch stopwatch = Stopwatch.StartNew();

        internal static long GetTimestamp() => stopwatch.ElapsedTicks + 1;

        public static long StartTimestamp { get; private set; }


        public static bool IsEnabled => Publisher != null;

        internal static SampleCollector Collector { get; } = new SampleCollector();
        internal static MetricPublisher Publisher { get; private set; }

        public static void Initialize( TelemetryClient client, TimeSpan publishPeriod )
        {
            Publisher = new MetricPublisher(Collector, client);
            StartTimestamp = GetTimestamp();
            Publisher.Start(publishPeriod);
        }

        public static void Uninitialize()
        {
            Publisher?.Dispose();
            Publisher = null;
        }
    }
}
