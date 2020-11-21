using PostSharp.Patterns.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostSharp.Samples.Logging.PerRequest
{
  public static class CommonCode
  {
    public static void HelloWorld()
    {
      LogSource.Get().Debug.Write(FormattedMessageBuilder.Formatted("It seems logging is enabled."));
    }
  }
}
