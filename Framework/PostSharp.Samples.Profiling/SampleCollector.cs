using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace PostSharp.Samples.Profiling
{
    internal class SampleCollector
    {
        // This is the maximal duration allowed to collect the metrics from all threads for a single method.
        static readonly long timestampTolerance = Stopwatch.Frequency / 100;

        private readonly ThreadLocal<ThreadLocalSampleCollector> _threadLocalCollectors;
        public IList<ThreadLocalSampleCollector> ThreadLocalCollectors => this._threadLocalCollectors.Values;

        private readonly object registrationLock = new object();

        private MetricMetadata[] _metricsMetadata = new MetricMetadata[1024];

        public int ProfiledMethodCount { get; private set; }
        internal MetricMetadata[] MetricsMetadata { get => this._metricsMetadata; }

        public SampleCollector()
        {
            this._threadLocalCollectors = new ThreadLocal<ThreadLocalSampleCollector>(() => new ThreadLocalSampleCollector(this), true);
        }

        public MetricMetadata RegisterMethod( MethodBase method )
        {
            lock (this.registrationLock )
            {
                MetricMetadata profiledMethod = new MetricMetadata(method, this.ProfiledMethodCount );

                if ( this.MetricsMetadata.Length <= this.ProfiledMethodCount)
                {
                    Array.Resize(ref this._metricsMetadata, this.MetricsMetadata.Length * 2);
                }

                this.MetricsMetadata[ profiledMethod.Index ] = profiledMethod;
                this.ProfiledMethodCount++;
                return profiledMethod;
            }
            
        }

        public ThreadLocalSampleCollector GetThreadLocalCollector() => this._threadLocalCollectors.Value;


        public MetricData[] GetMetrics()
        {
            
            MetricMetadata[] profiledMethodsCopy;
            MetricData[] metrics;

            lock (this.registrationLock )
            {
                profiledMethodsCopy = this.MetricsMetadata;
                metrics = new MetricData[this.ProfiledMethodCount];
            }

            var treadLocalCollectorsCopy = this._threadLocalCollectors.Values;

            long timestamp = ProfilingServices.GetTimestamp();

            for (int i = 0; i < metrics.Length; i++)
            {
                MetricMetadata method = profiledMethodsCopy[i];

                int attempts = 0;
                while ( true )
                {
                    metrics[i].Timestamp = timestamp;

                    foreach (var threadLocalCollector in treadLocalCollectorsCopy)
                    {


                        if (threadLocalCollector.GetSample(method, out MetricData threadLocalData))
                        {
                            metrics[i].AddData(threadLocalData);
                        }
                    }

                    timestamp = ProfilingServices.GetTimestamp();


                    // Detect if our thread has been preempted, and retry if so.
                    if ( timestamp - metrics[i].Timestamp < timestampTolerance )
                    {
                        break;
                    }
                    else
                    {
                        metrics[i] = default;
                        attempts++;

                        if ( attempts > 3 )
                        {
                            Console.WriteLine("Too many non-voluntary preemptions in the publisher thread. ");
                            metrics[i].SetInvalid();
                            break;
                        }

                    }
            
                } 

                

            }

            return metrics;

        }

     
        
    }
}
