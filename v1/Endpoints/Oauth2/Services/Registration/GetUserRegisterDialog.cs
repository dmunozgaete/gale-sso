using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using API.Endpoints.Oauth2.Resources;

namespace API.Endpoints.Oauth2.Services.Registration
{
    public class GetUserRegisterDialog : Gale.REST.Http.HttpBaseActionResult
    {
        int _version;
        HttpRequestMessage _request;
        System.Collections.Specialized.NameValueCollection _parameters;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">Request Message</param>
        /// <param name="version">Oauth2 assets version</param>
        /// <param name="parameters">Parameters Collections</param>
        public GetUserRegisterDialog(HttpRequestMessage request, int version, System.Collections.Specialized.NameValueCollection parameters)
        {
            _version = version;
            _request = request;
            _parameters = parameters;
        }


        /// <summary>
        /// Async Process
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            var html = "";

            //------------------------------------------------------------------------------------------------------
            //Get the User registration form
            html = API.Endpoints.Oauth2.Templates.EmbeddedResolver.GetStringContent(
                _version,
                "registration/create-account-dialog.cshtml",
                _parameters
            );

            //-----------------------------------------------------------------------------
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(html)
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
            return Task.FromResult(response);
            //-----------------------------------------------------------------------------
        }


    }
}