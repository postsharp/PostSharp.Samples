using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.ExtensionMethods;
using System;
using System.Linq;

namespace ApplicationInsightsLib
{
    public static class ApplicationInsightsExtensions
    {
        public static LoggerConfiguration MyApplicationInsights( this LoggerSinkConfiguration  sinkConfiguration )
        {
            return sinkConfiguration.ApplicationInsightsTraces("4f9d01cc-aa54-4270-ad8f-04b89088b8a2", logEventToTelemetryConverter: LogEventToTelemetryConverter);
        }
        private static ITelemetry LogEventToTelemetryConverter(LogEvent logEvent, IFormatProvider formatProvider)
        {
            // Original code: https://github.com/serilog/serilog-sinks-applicationinsights

            // first create a default TraceTelemetry using the sink's default logic
            // .. but without the log level, and (rendered) message (template) included in the Properties
            ITelemetry telemetry;

            if (logEvent.Exception != null)
            {
                telemetry = logEvent.ToDefaultExceptionTelemetry(
                formatProvider,
                includeLogLevelAsProperty: false,
                includeRenderedMessageAsProperty: false,
                includeMessageTemplateAsProperty: false);
            }
            else
            {
                telemetry = logEvent.ToDefaultTraceTelemetry(
                formatProvider,
                includeLogLevelAsProperty: false,
                includeRenderedMessageAsProperty: false,
                includeMessageTemplateAsProperty: false);
            }

            ISupportProperties telemetryProperties = (ISupportProperties)telemetry;


            if (logEvent.Properties.ContainsKey("operation_Id"))
            {
                telemetry.Context.Operation.Id = logEvent.Properties["operation_Id"].ToString();
                telemetryProperties.Properties.Remove("parentId");
            }

            if (logEvent.Properties.ContainsKey("operation_parentId"))
            {
                telemetry.Context.Operation.ParentId = logEvent.Properties["operation_parentId"].ToString();
                telemetryProperties.Properties.Remove("operation_parentId");
            }
            
            return telemetry;
        }

    }
}
