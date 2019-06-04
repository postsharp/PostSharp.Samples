using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using PostSharp.Patterns.Formatters;

namespace PostSharp.Samples.Logging.CustomBackend.ServiceStack
{
  public class ServiceStackLogRecordBuilder : TextLogRecordBuilder
  {
    public ServiceStackLogRecordBuilder(TextLoggingBackend backend) : base(backend)
    {
    }

    protected override void Write(UnsafeString message)
    {
      var log = ((ServiceStackLoggingTypeSource) TypeSource).Log;
      var messageString = message.ToString();

      switch (Level)
      {
        case LogLevel.None:
          break;

        case LogLevel.Trace:
        case LogLevel.Debug:
          if (Exception == null)
          {
            log.Debug(messageString);
          }
          else
          {
            log.Debug(messageString, Exception);
          }

          break;

        case LogLevel.Info:
          if (Exception == null)
          {
            log.Info(messageString);
          }
          else
          {
            log.Info(messageString, Exception);
          }

          break;

        case LogLevel.Warning:
          if (Exception == null)
          {
            log.Warn(messageString);
          }
          else
          {
            log.Warn(messageString, Exception);
          }

          break;

        case LogLevel.Error:
          if (Exception == null)
          {
            log.Error(messageString);
          }
          else
          {
            log.Error(messageString, Exception);
          }

          break;

        case LogLevel.Critical:
          if (Exception == null)
          {
            log.Fatal(messageString);
          }
          else
          {
            log.Fatal(messageString, Exception);
          }

          break;
      }
    }
  }
}