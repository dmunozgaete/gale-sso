using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace API.ApplicationInsights
{
    /// <summary>
    /// Catch all Exception , and send this exception to the telemetry Application Insights
    /// </summary>
    public class TrackExceptionFilter : ExceptionFilterAttribute
    {

        /// <summary>
        /// On Exception Ocurred
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            var configurationKey = "ApplicationInsights:IntrumentationKey";
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(configurationKey))
            {
                var trackID = System.Configuration.ConfigurationManager.AppSettings[configurationKey];
                Microsoft.ApplicationInsights.TelemetryClient client = new Microsoft.ApplicationInsights.TelemetryClient();

                Dictionary<String, String> extraParameters = new Dictionary<String, String>();
                extraParameters.Add("Request", context.Request.RequestUri.ToString());

                client.Context.InstrumentationKey = trackID;
                client.TrackException(context.Exception, extraParameters);
            }
        }
    }
}