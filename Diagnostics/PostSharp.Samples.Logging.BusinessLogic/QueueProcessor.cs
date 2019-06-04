using PostSharp.Patterns.Diagnostics;
using System;
using System.Threading;
using static PostSharp.Patterns.Diagnostics.FormattedMessageBuilder;

namespace PostSharp.Samples.Logging.BusinessLogic
{
  public class QueueProcessor
  {
    private static readonly LogSource logSource = LogSource.Get();

    public static void ProcessQueue(string queuePath)
    {
      ProcessItem(new QueueItem(56));

      ProcessItem(new QueueItem(145));

      ProcessItem(new QueueItem(67));
    }

    private static void ProcessItem(QueueItem item)
    {
      var activity = logSource.Default.OpenActivity(Formatted("Processing item {item}", item));
      try
      {
        var request = RequestStorage.GetRequest(item.Id);

        if (item.Id == 56)
        {
          logSource.Warning.Write(Formatted("The entity {id} has been marked for deletion.", item.Id));
          activity.SetSuccess();
          return;
        }

        if (item.Id == 145)
          RequestStorage.GetUser(0);
        else
          RequestStorage.GetUser(14);


        Thread.Sleep(125);

        activity.SetSuccess();
      }
      catch (Exception e)
      {
        activity.SetException(e);
      }
    }
  }
}