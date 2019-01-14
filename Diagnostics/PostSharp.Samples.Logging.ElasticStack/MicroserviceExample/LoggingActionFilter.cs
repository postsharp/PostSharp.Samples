using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using PostSharp.Patterns.Diagnostics;
using static PostSharp.Patterns.Diagnostics.SemanticMessageBuilder;

namespace MicroserviceExample
{
    [Log( AttributeExclude = true )]
    public class LoggingActionFilter : IAsyncActionFilter
    {
        private static readonly LogSource logger = LogSource.Get();

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            // Read the Request-Id header so we can assign it to the activity.
            string parentOperationId = context.HttpContext.Request.Headers["Request-Id"];

            OpenActivityOptions options = default;

            if (!string.IsNullOrEmpty(parentOperationId))
            {
                options.ParentId = parentOperationId;
            }
            else
            {
                options.IsRoot = true;
            }

            var request = context.HttpContext.Request;
            using (var activity = logger.Default.OpenActivity(Semantic("Request", ("Path", request.Path),
                ("Query", request.QueryString), ("Method", request.Method)), options))
            {
                try
                {
                    await next();

                    var response = context.HttpContext.Response;

                    if (response.StatusCode >= (int) HttpStatusCode.OK && response.StatusCode <= (int) 299)
                    {
                        // Success.
                        activity.SetOutcome( LogLevel.Info, Semantic("Success", ("StatusCode", response.StatusCode)));
                    }
                    else
                    {
                        // Failure.
                        activity.SetOutcome( LogLevel.Warning, Semantic("Failure", ("StatusCode", response.StatusCode)));
                    }
                    
                }
                catch (Exception e)
                {
                    activity.SetException(e);
                }
                
            }
                
        



    }
    }
}
