using System;
using System.Net.Http;
using System.Threading.Tasks;
using ClientExample;
using PostSharp.Aspects;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Serialization;
using static PostSharp.Patterns.Diagnostics.SemanticMessageBuilder;

[assembly: InstrumentOutgoingRequestsAspect( AttributeTargetAssemblies = "System.Net.Http", 
    AttributeTargetTypes = "System.Net.Http.HttpClient", 
    AttributeTargetMembers = "regex:(Get*|Delete|Post|Push|Patch)Async" )]

namespace ClientExample
{
    [PSerializable]
    internal class InstrumentOutgoingRequestsAspect : MethodInterceptionAspect
    {
        private static readonly LogSource logger = LogSource.Get();

        public override async Task OnInvokeAsync( MethodInterceptionArgs args )
        {
            var http = (HttpClient) args.Instance;

            
            var verb = Trim( args.Method.Name, "Async" );

            using ( var activity = logger.Default.OpenActivity(  Semantic( verb,  ("Url", args.Arguments[0] ) ) ) )
            {
                try
                {
                    
                    http.DefaultRequestHeaders.Remove( "Request-Id" );
                    http.DefaultRequestHeaders.Add( "Request-Id", activity.ContextId );

                    var t = base.OnInvokeAsync( args );

                    // We need to call Suspend/Resume because we're calling LogActivity from an aspect and 
                    // aspects are not automatically enhanced.
                    // In other code, this is done automatically.
                    if ( !t.IsCompleted )
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

                    
                    if ( response.IsSuccessStatusCode )
                    {
                        activity.SetOutcome( LogLevel.Info, Semantic( "Succeeded", ("StatusCode", response.StatusCode)));
                    }
                    else
                    {
                        activity.SetOutcome( LogLevel.Warning, Semantic( "Failed", ("StatusCode", response.StatusCode)));
                    }

                }
                catch ( Exception e )
                {
                    activity.SetException( e );
                    throw;
                }
                finally
                {
                    http.DefaultRequestHeaders.Remove( "Request-Id" );
                }
            }

        }

        private static string Trim( string s, string suffix )
        {
            if ( s.EndsWith( suffix ) )
            {
                return s.Substring( 0, s.Length - suffix.Length );
            }
            else
            {
                return s;
            }
        }
    }
}