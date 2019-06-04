using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;

namespace PostSharp.Samples.WeakEvent
{
  /// <summary>
  ///   Aspect that, when applied to an event, prevents the target event from holding a strong reference to event handlers.
  ///   Therefore, the aspect prevents the event to prevent clients to be garbage collected.
  /// </summary>
  [PSerializable]
  [LinesOfCodeAvoided(6)]
  public sealed class WeakEventAttribute : EventInterceptionAspect, IInstanceScopedAspect
  {
    [PNonSerialized] private volatile int cleanUpCounter;

    [PNonSerialized] private ImmutableArray<object> handlers;

    [PNonSerialized] private bool initialized;

    [PNonSerialized] private SpinLock spinLock;


    /// <summary>
    ///   Creates an instance of the aspect for a specific instance of the target class (when the target event is not
    ///   static). A call of this method is followed by a call of RuntimeInitializeInstance.
    /// </summary>
    object IInstanceScopedAspect.CreateInstance(AdviceArgs adviceArgs)
    {
      return MemberwiseClone();
    }

    /// <summary>
    ///   Initializes the current instance of the aspect (when the target event is not static).
    /// </summary>
    void IInstanceScopedAspect.RuntimeInitializeInstance()
    {
      if (!initialized)
      {
        cleanUpCounter = 0;
        initialized = true;
        handlers = ImmutableArray<object>.Empty;
      }
    }

    /// <summary>
    ///   Initializes the current instance of the aspect (whether target event is static or not).
    /// </summary>
    public override void RuntimeInitialize(EventInfo eventInfo)
    {
      if (eventInfo.AddMethod.IsStatic)
      {
        if (!initialized)
        {
          cleanUpCounter = 0;
          initialized = true;
          handlers = ImmutableArray<object>.Empty;
        }
      }
    }

    #region Add

    /// <summary>
    ///   Method invoked when (instead of) a new handler is added to the target event.
    /// </summary>
    /// <param name="args">Context information.</param>
    public override void OnAddHandler(EventInterceptionArgs args)
    {
      // Add the handler to our own list.
      if (AddHandler(args.Handler))
      {
        args.AddHandler(null);
      }


      // Register the handler to the client to prevent garbage collection of the handler.
      DelegateReferenceKeeper.AddReference(args.Handler);
    }

    private bool AddHandler(Delegate handler)
    {
      var lockTaken = false;

      try
      {
        spinLock.Enter(ref lockTaken);

        handlers = handlers.Add(new WeakReference(handler));

        return handlers.Length == 1;
      }
      finally
      {
        if (lockTaken)
        {
          spinLock.Exit();
        }
      }
    }

    #endregion

    #region Remove

    /// <summary>
    ///   Method invoked when (instead of) a new handler is removed from the target event.
    /// </summary>
    /// <param name="args">Context information.</param>
    public override void OnRemoveHandler(EventInterceptionArgs args)
    {
      // Remove the handler from our own list.
      if (RemoveHandler(args.Handler))
      {
        args.RemoveHandler(null);
      }

      // Remove the handler from the client.
      DelegateReferenceKeeper.RemoveReference(args.Handler);
    }


    private bool RemoveHandler(Delegate handler)
    {
      var lockTaken = false;
      try
      {
        spinLock.Enter(ref lockTaken);

        handlers =
          handlers.RemoveAll(
            o => ((WeakReference) o).Target?.Equals(handler) ?? false);

        return handlers.IsEmpty;
      }
      finally
      {
        if (lockTaken)
        {
          spinLock.Exit();
        }
      }
    }

    #endregion

    #region Invoke

    /// <summary>
    ///   Method invoked when (instead of) a the target event is raised.
    /// </summary>
    /// <param name="args">Context information.</param>
    public override void OnInvokeHandler(EventInterceptionArgs args)
    {
      // Note that args.Handler == null because it's the value we added in OnAddHandler, but it really does not matter.

      InvokeHandler(args.Arguments.ToArray());
    }


    private void InvokeHandler(object[] args)
    {
      var lastCleanUpCounter = -1;


      // Take a snapshot of the handlers list.
      var invocationList = handlers;

      var needCleanUp = false;

      foreach (var obj in invocationList)
      {
        var handler = (Delegate) ((WeakReference) obj).Target;

        if (handler == null)
        {
          if (!needCleanUp)
          {
            needCleanUp = true;
            lastCleanUpCounter = cleanUpCounter;
          }

          continue;
        }


        handler.DynamicInvoke(args);
      }

      if (needCleanUp && lastCleanUpCounter == cleanUpCounter)
      {
        if (lastCleanUpCounter == cleanUpCounter)
        {
          var lockTaken = false;
          try
          {
            spinLock.Enter(ref lockTaken);
            handlers = handlers.RemoveAll(w => !((WeakReference) w).IsAlive);

            Interlocked.Increment(ref cleanUpCounter);
          }
          finally
          {
            if (lockTaken)
            {
              spinLock.Exit();
            }
          }
        }
      }
    }

    #endregion
  }
}