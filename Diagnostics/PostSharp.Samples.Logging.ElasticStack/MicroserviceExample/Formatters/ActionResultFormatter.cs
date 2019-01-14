using Microsoft.AspNetCore.Mvc;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Formatters;

namespace MicroserviceExample.Formatters
{
    public class ActionResultFormatter<T> : Formatter<ActionResult<T>>
    {
        public override void Write( UnsafeStringBuilder stringBuilder, ActionResult<T> value )
        {
            LoggingServices.Formatters.Get<T>().Write( stringBuilder, value.Value );
        }
    }

    public class ActionResultFormatter : Formatter<IActionResult>
    {
        public override void Write( UnsafeStringBuilder stringBuilder, IActionResult value )
        {
            var specificFormatter = LoggingServices.Formatters.Get(value.GetType() );
            if ( specificFormatter != this )
            {
                specificFormatter.Write(stringBuilder, value );
            }
            else
            {
                const string suffix = "Result";
                var name = value.GetType().Name;

                if ( name.EndsWith(suffix ) )
                {
                    name = name.Substring(0, name.Length - suffix.Length );
                }

                stringBuilder.Append('{' );
                stringBuilder.Append(name );
                stringBuilder.Append('}' );
            }
        }
    }
}
