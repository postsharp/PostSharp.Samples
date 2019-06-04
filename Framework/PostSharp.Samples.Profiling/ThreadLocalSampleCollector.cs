using System;
using System.Collections.Generic;
using System.Threading;

namespace PostSharp.Samples.Profiling
{
  internal class ThreadLocalSampleCollector
  {
    private readonly Thread _ownerThread = Thread.CurrentThread;
    private MetricAccessor[] _metrics = new MetricAccessor[1024];
    private readonly SampleCollector _parent;
    private readonly Stack<MetricAccessor> _contextStack = new Stack<MetricAccessor>();

    public ThreadLocalSampleCollector(SampleCollector parent)
    {
      this._parent = parent;
    }

    public void EnterMethod(MetricMetadata method)
    {
      this._contextStack.Push(this.GetAccessor(method));
    }

    public void ExitMethod(MetricMetadata method, in ExcludedTimeData excludedData)
    {
      if (this._contextStack.Pop().Metadata != method)
      {
        throw new InvalidOperationException();
      }

      if (_contextStack.Count > 0)
      {
        var parentContext = _contextStack.Peek();
        parentContext.AddExclusion(excludedData);
      }

    }

    public bool GetSample(MetricMetadata method, out MetricData data)
    {
      MetricAccessor metric;
      if (this._metrics.Length <= method.Index || (metric = this._metrics[method.Index]) == null)
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

    public void AddSample(MetricMetadata method, in MetricData data)
    {



      var sampleAccessor = GetAccessor(method);

      sampleAccessor.AddData(data);

    }

    private MetricAccessor GetAccessor(MetricMetadata method)
    {
#if DEBUG
      if (this._ownerThread != Thread.CurrentThread)
      {
        throw new InvalidOperationException("Cannot get a mutable MetricAccessor from a different thread than the owner thread.");
      }
#endif

      if (this._metrics.Length <= method.Index)
      {
        Array.Resize(ref this._metrics, ((this._parent.ProfiledMethodCount / 1024) + 1) * 1024);
      }

      var sampleAccessor = this._metrics[method.Index];
      if (sampleAccessor == null)
      {
        this._metrics[method.Index] = sampleAccessor = new MetricAccessor(method);
      }

      return sampleAccessor;
    }
  }
}
