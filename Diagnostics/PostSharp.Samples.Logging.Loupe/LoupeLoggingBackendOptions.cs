using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends;

namespace PostSharp.Samples.Logging.Loupe
{
  /// <summary>
  /// Options for <see cref="LoupeLoggingBackend"/>.
  /// </summary>
  [Log(AttributeExclude = true)]
  public class LoupeLoggingBackendOptions : TextLoggingBackendOptions
  {

  }
}
