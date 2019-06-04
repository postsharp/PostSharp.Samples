using System;
using System.Threading;

namespace PostSharp.Samples.Profiling
{
  internal class MetricAccessor
  {
    public static MetricAccessor Empty = new MetricAccessor(null);
    private volatile int _version;
    private MetricData _data;

    public MetricAccessor(MetricMetadata metadata)
    {
      this.Metadata = metadata;
    }

    public MetricMetadata Metadata { get; }


    public int Version
    {
      get
      {
        return this._version;
      }
      set
      {
        this._version = value;
      }
    }

    private bool IsWriting => this._version % 2 == 1;

    public void AddData(in MetricData data)
    {
#if DEBUG
      if (this.IsWriting)
        throw new InvalidOperationException();
#endif
      var localVersion = this._version;
      this._version = localVersion + 1;
      Thread.MemoryBarrier();

      this._data.AddData(data);

      Thread.MemoryBarrier();
      this._version = localVersion + 2;

    }

    public void AddExclusion(in ExcludedTimeData data)
    {
#if DEBUG
      if (this.IsWriting)
        throw new InvalidOperationException();
#endif
      var localVersion = this._version;
      this._version = localVersion + 1;
      Thread.MemoryBarrier();

      this._data.AddExclusion(data);

      Thread.MemoryBarrier();
      this._version = localVersion + 2;

    }

    public void GetData(out MetricData data)
    {
      var spinWait = new SpinWait();

      while (true)
      {
        var versionBefore = this._version;

        while (this.IsWriting)
        {
          spinWait.SpinOnce();
        }


        data = this._data;

        if (this._version == versionBefore)
        {
          return;
        }

        spinWait.SpinOnce();
      }

    }

  }
}
