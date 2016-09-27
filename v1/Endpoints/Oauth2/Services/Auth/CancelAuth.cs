using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using API.Endpoints.Oauth2.Resources;

namespace API.Endpoints.Oauth2.Services.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class CancelAuth : Gale.REST.Http.HttpBaseActionResult
    {
        HttpRequestMessage _request;
        int _version;
        System.Collections.Specialized.NameValueCollection _parameters;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">Request Message</param>
        /// <param name="version">Oauth2 assets version</param>
        /// <param name="parameters">Parameters Collections</param>
        public CancelAuth(HttpRequestMessage request, int version, System.Collections.Specialized.NameValueCollection parameters)
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
            //-----------------------------------------------------------------------------
            var redirect_uri = _parameters[RFC6749Names.REDIRECT_URI];

            System.Text.StringBuilder uri = new System.Text.StringBuilder(redirect_uri);


            //Build the URL
            switch (_parameters[RFC6749Names.RESPONSE_TYPE])
            {
                case "token":
                    //ACCESS_TOKEN
                    uri.Append("#error=");
                    uri.Append(_parameters[RFC6749Names.ERROR]);
                    break;
                case "code":
                    //CODE

                    //TODO:
                    break;
            };

            //State parameter
            uri.Append("&state=");
            uri.Append(_parameters[RFC6749Names.STATE]);


            //RETURN CONTEXT
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(uri.ToString());
            return Task.FromResult(response);
            //-----------------------------------------------------------------------------
        }


    }
}