using System;
using PostSharp.Patterns.Diagnostics;

namespace PostSharp.Samples.Logging.Loupe
{
  /// <summary>
  /// <see cref="LoggingTypeSource"/> for <see cref="LoupeLoggingBackend"/>.
  /// </summary>
  [Log(AttributeExclude = true)]
  public class LoupeLoggingTypeSource : LoggingTypeSource
  {
    /// <summary>
    /// Initializes a new <see cref="LoupeLoggingTypeSource"/>.
    /// </summary>
    /// <param name="parent">The parent <see cref="LoggingNamespaceSource"/>.</param>
    /// <param name="type">The source <see cref="Type"/>.</param>
    public LoupeLoggingTypeSource(LoggingNamespaceSource parent, Type type) : base(parent, type.FullName, type)
    {

    }

    /// <inheritdoc />
    protected override bool IsBackendEnabled(LogLevel level)
    {
      return true;
    }
  }
}
