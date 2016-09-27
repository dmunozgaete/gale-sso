using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace API.Endpoints.Oauth2.Decorators
{
    /// <summary>
    /// Catch all Exception , and build a custom Response message in "Dialog Format"
    /// </summary>
    public class DialogFormatWhenException : ExceptionFilterAttribute
    {
        /// <summary>
        /// On Exception Ocurred
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            var html = "";
            var _version = 2;

            if (context.Exception is Gale.Exception.RestException)
            {
                //Know Error, Show "nicely"
                html = API.Endpoints.Oauth2.Templates.EmbeddedResolver.GetStringContent(
                    _version,
                    "error/developer-error-dialog.cshtml",
                    context.Exception as Gale.Exception.RestException
                );
            }
            else
            {
                //Ooops, something went wrong!!!, but again....
                //show nicely !!
                html = API.Endpoints.Oauth2.Templates.EmbeddedResolver.GetStringContent(
                    _version,
                    "error/developer-error-dialog.cshtml",
                    new
                    {
                        Code = "UNHANDLED_EXCEPTION",
                        Message = context.Exception.Message
                    }
                );
            }

            //-----------------------------------------------------------------------------
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(html)
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
            context.Response = response; //Show Dialog "Format"
            //-----------------------------------------------------------------------------

        }
    }
}