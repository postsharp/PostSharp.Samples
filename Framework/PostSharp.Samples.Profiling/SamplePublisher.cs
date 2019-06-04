using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PostSharp.Samples.Profiling
{
    internal class MetricPublisher : IDisposable
    {
        readonly SampleCollector sampleCollector;
        private readonly TelemetryClient telemetryClient;
        private MetricData[] lastSamples = new MetricData[0];
        Timer timer;
        bool inProgress;

        public MetricPublisher(SampleCollector collector, TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
            this.sampleCollector = collector;
        }

        public void Start(TimeSpan period)
        {
            this.timer?.Dispose();
            this.timer = new Timer(this.OnTime, null, (int)period.TotalMilliseconds, (int)period.TotalMilliseconds);
        }

        private void OnTime(object state)
        {
            if (this.inProgress)
            {
                return;
            }

            this.inProgress = true;
            try
            {
                this.PublishMetrics();

            }
            finally
            {
                this.inProgress = false;
            }
        }

        public void PublishMetrics() => this.PublishMetrics(this.sampleCollector.MetricsMetadata, this.sampleCollector.GetMetrics());

        private void PublishMetrics(IReadOnlyList<MetricMetadata> methods, MetricData[] metrics)
        {
            for ( var i = 0; i < metrics.Length; i++)
            {
                MetricData lastSample;
                if ( i < this.lastSamples.Length )
                {
                    lastSample = this.lastSamples[i];
                }
                else
                {
                    lastSample = new MetricData { Timestamp = ProfilingServices.StartTimestamp };
                }
                    

                metrics[i].GetDifference(lastSample, out var differenceSample);

                this.PublishMetric(methods[i], differenceSample);


            }

            this.lastSamples = metrics;
        }

        private void PublishMetric( MetricMetadata method, in MetricData metric )
        {

            if (metric.ExecutionCount > 0)
            {

                var metrics = new Dictionary<string, double>
                {
                    ["ExceptionCount"] = metric.ExceptionCount,
                    ["ExecutionCount"] = metric.ExecutionCount,
                    ["CpuTime"] = metric.CpuTimeSpan.TotalMilliseconds,
                    ["ThreadTime"] = metric.ThreadTimeSpan.TotalMilliseconds,
                    ["ExclusiveCpuTime"] = metric.ExclusiveCpuTimeSpan.TotalMilliseconds,
                    ["ExclusiveThreadTime"] = metric.ExclusiveThreadTimeSpan.TotalMilliseconds,
                    ["AsyncTime"] = metric.AsyncTimeSpan.TotalMilliseconds,
                    ["SampleTime"] = metric.SampleTimeSpan.TotalMilliseconds
                };

                Console.WriteLine(string.Format("TrackEvent: Name='{0,-70}', {1}", method.Name, string.Join(", ", metrics.Select(p => $"{p.Key}={p.Value,10}"))));

                
                this.telemetryClient.TrackEvent(method.Name, metrics: metrics);
            }
        }

        public void Dispose()
        {
            this.timer?.Dispose();
        }
    }
}
