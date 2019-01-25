using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using PostSharp.Patterns.Diagnostics;

namespace MicroserviceExample
{
    [Log(AttributeExclude = true)]
    public class SampledLoggingActionFilter : IAsyncActionFilter
    {
        private static readonly Random random = new Random();
        private static LoggingVerbosityConfiguration verbosityManager;

        public static void Initialize( LoggingBackend backend )
        {
            verbosityManager = backend.CreateVerbosityConfiguration();
        }

        public static bool IsInitialized => verbosityManager != null;

        private static bool IsLogged(ActionExecutingContext context)
        {
            lock (random)
            {
                // Log 10% of requests.
                return random.NextDouble() < 0.1;
            }
        }
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if ( IsInitialized && IsLogged(context))
            {
                using ( verbosityManager.Use() )
                {
                    await next();
                }
            }
            else
            {
                await next();
            }

        }
    }
}