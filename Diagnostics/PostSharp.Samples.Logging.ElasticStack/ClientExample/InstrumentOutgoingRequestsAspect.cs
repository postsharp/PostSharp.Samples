using ClientExample;
using PostSharp.Aspects;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Formatters;
using PostSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static PostSharp.Patterns.Diagnostics.SemanticMessageBuilder;

// The following attribute intercepts all calls to the specified methods of HttpClient.
[assembly: InstrumentOutgoingRequestsAspect(
    AttributeTargetAssemblies = "System.Net.Http",
    AttributeTargetTypes = "System.Net.Http.HttpClient",
    AttributeTargetMembers = "regex:(Get*|Delete|Post|Push|Patch)Async")]

namespace ClientExample
{
  [PSerializable]
  internal class InstrumentOutgoingRequestsAspect : MethodInterceptionAspect
  {
    private static readonly LogSource logSource = LogSource.Get();

    public override async Task OnInvokeAsync(MethodInterceptionArgs args)
    {
      var http = (HttpClient) args.Instance;
      var verb = Trim(args.Method.Name, "Async");

      using (var activity = logSource.Default.OpenActivity(Semantic(verb, ("Url", args.Arguments[0]))))
      {
        try
        {
          // TODO: this implementation conflicts with System.Diagnostics.Activity and therefore Application Insights.


          // Remove headers.
          http.DefaultRequestHeaders.Remove("Request-Id");
          http.DefaultRequestHeaders.Remove("Correlation-Context");


          // Set Request-Id header.
          http.DefaultRequestHeaders.Add("Request-Id", activity.Context.SyntheticId);


          // Generate the Correlation-Context header.
          UnsafeStringBuilder correlationContextBuilder = null;
          var propertyNames = new HashSet<string>();
          try
          {
            activity.Context.ForEachProperty((LoggingProperty property, object value, ref object _) =>
            {
              if (!property.IsBaggage || !propertyNames.Add(property.Name)) return;

              if (correlationContextBuilder == null)
              {
                propertyNames = new HashSet<string>();
                correlationContextBuilder = new UnsafeStringBuilder(1024);
              }

              if (correlationContextBuilder.Length > 0)
              {
                correlationContextBuilder.Append(", ");
              }

              correlationContextBuilder.Append(property.Name);
              correlationContextBuilder.Append('=');

              var formatter =
                              property.Formatter ?? LoggingServices.Formatters.Get(value.GetType());

              formatter.Write(correlationContextBuilder, value);
            });

            if (correlationContextBuilder != null)
            {
              http.DefaultRequestHeaders.Add("Correlation-Context", correlationContextBuilder.ToString());
            }
          }
          finally
          {
            correlationContextBuilder?.Dispose();
          }


          var t = base.OnInvokeAsync(args);

          // We need to call Suspend/Resume because we're calling LogActivity from an aspect and 
          // aspects are not automatically enhanced.
          // In other code, this is done automatically.
          if (!t.IsCompleted)
          {
            activity.Suspend();
            try
            {
              await t;
            }
            finally
            {
              activity.Resume();
            }
          }


          var response = (HttpResponseMessage) args.ReturnValue;


          if (response.IsSuccessStatusCode)
          {
            activity.SetOutcome(LogLevel.Info, Semantic("Succeeded", ("StatusCode", response.StatusCode)));
          }
          else
          {
            activity.SetOutcome(LogLevel.Warning, Semantic("Failed", ("StatusCode", response.StatusCode)));
          }
        }
        catch (Exception e)
        {
          activity.SetException(e);
          throw;
        }
        finally
        {
          http.DefaultRequestHeaders.Remove("Request-Id");
        }
      }
    }

    private static string Trim(string s, string suffix)
        => s.EndsWith(suffix) ? s.Substring(0, s.Length - suffix.Length) : s;
  }
}