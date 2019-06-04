using System;

namespace PostSharp.Samples.WeakEvent
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      // The logic of this sample needs to be separated into methods,
      // because .NET Framework CLR doesn't collect objects within a scope of a method.

      var weakReference = AddCollectedEventClient();

      GC.Collect();

      Console.WriteLine("Client is alive: {0} (should be False)", weakReference.IsAlive);

      // Raise the event when the client is dead.
      EventClient.EventHandlerCount = 0;
      MyEvent(null, EventArgs.Empty);
      Console.WriteLine("EventHandlerCount: {0} (should be 0)", EventClient.EventHandlerCount);
    }

    private static WeakReference AddCollectedEventClient()
    {
      var eventClient = new EventClient();
      MyEvent += eventClient.EventHandler;

      // Forcing GC here to prove that we are not collecting the handler when the client is alive.
      GC.Collect();

      // Raise the event when the client is alive.
      MyEvent(null, EventArgs.Empty);
      Console.WriteLine("EventHandlerCount: {0} (should be 1)", EventClient.EventHandlerCount);


      // Cause the client to be collected.
      return new WeakReference(eventClient);
    }

    [WeakEvent]
    private static event EventHandler MyEvent;
  }

  internal class EventClient
  {
    public static int EventHandlerCount;

    public void EventHandler(object sender, EventArgs e)
    {
      Console.WriteLine("Oops!");
      EventHandlerCount++;
    }
  }
}