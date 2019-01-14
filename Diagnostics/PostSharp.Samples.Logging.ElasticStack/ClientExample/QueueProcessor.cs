using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Patterns.Diagnostics;

namespace ClientExample
{
    public class QueueProcessor
    {
        private static readonly Logger logger = Logger.GetLogger();

        private static HttpClient http = new HttpClient();

        static QueueProcessor()
        {

        }

        public static async Task ProcessQueue( string queuePath )
        {
            await ProcessItem( new QueueItem( 1, Verb.Create, "orange" ) );
            await ProcessItem( new QueueItem( 2, Verb.Create, "pear" ) );
            await ProcessItem( new QueueItem( 3, Verb.Create, "grape" ) );
            await ProcessItem( new QueueItem( 1, Verb.Get ) );
            await ProcessItem( new QueueItem( 2, Verb.Get ) );
            await ProcessItem( new QueueItem( 3, Verb.Get ) );
            await ProcessItem( new QueueItem( 3, Verb.Create, "grapefruit" ) );
            await ProcessItem( new QueueItem( 3, Verb.Delete, "grapefruit" ) );
            await ProcessItem( new QueueItem( 3, Verb.Get ) );

        }

        private static async Task ProcessItem( QueueItem item )
        {
            try
            {

                var url = $"http://localhost:5005/api/values/{item.Id}";
                var stringContent = item.Value == null ? null : new StringContent( "\"" + item.Value + "\"", Encoding.UTF8, "application/json" );
                HttpResponseMessage response;


                switch ( item.Verb )
                {
                    case Verb.Get:
                        response = await http.GetAsync( url );
                        break;

                    case Verb.Create:
                        response = await http.PostAsync( url, stringContent );
                        break;

                    case Verb.AddOrUpdate:
                        response = await http.PutAsync( url, stringContent );
                        break;

                    default:
                        throw new NotImplementedException();
                }

                response.EnsureSuccessStatusCode();

                var responseValue = await response.Content.ReadAsStringAsync();
            }
            catch ( Exception e )
            {
                logger.WriteException( LogLevel.Warning, e, "Ignoring exception and continuing." );
            }

        }

    }
}