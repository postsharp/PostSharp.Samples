using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.EventSource;
using System;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;

namespace PostSharp.Samples.Logging.Etw.CustomSource
{
  [Log(AttributeExclude = true)]
  [EventSource(Name = "MyEventSource")]
  [Guid("971d5f08-f366-421c-a25c-3aed379d5eb2")]
  class MyEventSource : PostSharpEventSource
  {
    private const string format = "{0} | {1} | {2} | {3}";
    private const string invalidOperationExceptionMessage = "Use the Log method. This method is metadata-only.";
    private const byte version = 2;

    [Event(EventIds.MethodEntry, Level = EventLevel.Verbose, Message = format, Opcode = EventOpcode.Start, Version = version, Task = Tasks.Method)]
    internal void MethodEntry(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.MethodSuccess, Level = EventLevel.Verbose, Message = format, Opcode = EventOpcode.Stop, Version = version, Task = Tasks.Method)]
    internal void MethodSuccess(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.MethodException, Level = EventLevel.Warning, Message = format, Opcode = EventOpcode.Stop, Version = version, Task = Tasks.Method)]
    internal void MethodException(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.MethodOvertime, Level = EventLevel.Warning, Message = format, Opcode = EventOpcode.Stop, Version = version, Task = Tasks.Method)]
    internal void MethodOvertime(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.AsyncMethodAwait, Level = EventLevel.Verbose, Message = format, Opcode = EventOpcode.Suspend, Version = version, Task = Tasks.Method)]
    internal void AsyncMethodAwait(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.AsyncMethodResume, Level = EventLevel.Verbose, Message = format, Opcode = EventOpcode.Resume, Version = version, Task = Tasks.Method)]
    internal void AsyncMethodResume(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.ValueChanged, Level = EventLevel.Verbose, Message = format, Opcode = EventOpcode.Info, Version = version, Task = Tasks.Message)]
    internal void ValueChanged(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.CustomVerbose, Level = EventLevel.Verbose, Message = format, Opcode = EventOpcode.Info, Version = version, Task = Tasks.Message)]
    internal void CustomVerbose(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.CustomInfo, Level = EventLevel.Informational, Message = format, Opcode = EventOpcode.Info, Version = version, Task = Tasks.Message)]
    internal void CustomInfo(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.CustomWarning, Level = EventLevel.Warning, Message = format, Opcode = EventOpcode.Info, Version = version, Task = Tasks.Message)]
    internal void CustomWarning(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.CustomError, Level = EventLevel.Error, Message = format, Opcode = EventOpcode.Info, Version = version, Task = Tasks.Message)]
    internal void CustomError(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.CustomCritical, Level = EventLevel.Critical, Message = format, Opcode = EventOpcode.Info, Version = version, Task = Tasks.Message)]
    internal void CustomCritical(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.CustomActivityEntry, Level = EventLevel.Verbose, Message = format, Opcode = EventOpcode.Start, Version = version, Task = Tasks.CustomActivity)]
    internal void CustomActivityEntry(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.CustomActivityException, Level = EventLevel.Warning, Message = format, Opcode = EventOpcode.Stop, Version = version, Task = Tasks.CustomActivity)]
    internal void CustomActivityException(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.CustomActivitySuccess, Level = EventLevel.Verbose, Message = format, Opcode = EventOpcode.Stop, Version = version, Task = Tasks.CustomActivity)]
    internal void CustomActivitySuccess(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.CustomActivityFailure, Level = EventLevel.Warning, Message = format, Opcode = EventOpcode.Stop, Version = version, Task = Tasks.CustomActivity)]
    internal void CustomActivityFailure(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.IteratorYield, Level = EventLevel.Verbose, Message = format, Opcode = EventOpcode.Stop, Version = version, Task = Tasks.Method)]
    internal void IteratorYield(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.IteratorMoveNext, Level = EventLevel.Verbose, Message = format, Opcode = EventOpcode.Start, Version = version, Task = Tasks.Method)]
    internal void IteratorMoveNext(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }



    [Event(EventIds.ExecutionPoint, Level = EventLevel.Verbose, Message = format, Opcode = EventOpcode.Info, Version = version, Task = Tasks.Method)]
    internal void ExecutionPoint(string role, string type, string context, string message, int indentLevel)
    {
      throw new InvalidOperationException(invalidOperationExceptionMessage);
    }

    // We need to repeat this, otherwise the ETW framework for .NET will ignore the class.
    public new static class Tasks
    {
      public const EventTask Method = PostSharpEventSource.Tasks.Method;
      public const EventTask CustomActivity = PostSharpEventSource.Tasks.CustomActivity;
      public const EventTask Message = PostSharpEventSource.Tasks.Message;
    }

  }
}
