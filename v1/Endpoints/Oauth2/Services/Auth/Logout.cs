using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using API.Endpoints.Oauth2.Resources;

namespace API.Endpoints.Oauth2.Services.Auth
{
    /// <summary>
    /// Validate and register User
    /// </summary>
    public class Logout : Gale.REST.Http.HttpBaseActionResult
    {
        int _version;
        HttpRequestMessage _request;
        String _next;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">Request Message</param>
        /// <param name="next">uri to return</param>
        public Logout(HttpRequestMessage request, int version, String next)
        {
            _version = version;
            _request = request;

            _next = next;

        }


        /// <summary>
        /// Async Process
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            //------------------------------------------------------------------------------------------------------------------------
            //GUARD EXCEPTION
            Gale.Exception.RestException.Guard(() => _next == null, "NEXT_EMPTY", Resources.OAuth2.ResourceManager);
            //------------------------------------------------------------------------------------------------------------------------

            HttpResponseMessage response;

            //-----------------------------------------------------------------------------
            //REMOVE THE COOKIE FROM THE HEADERS (WE NEED TO OVERWRITE)
            var user_cookie = new System.Net.Http.Headers.CookieHeaderValue(
                RFC6749Names.COOKIE_CURRENT_USER,
                ""
            );
            user_cookie.Expires = DateTime.Now.AddDays(-1); // make it expire yesterday
            user_cookie.HttpOnly = true;
            user_cookie.Secure = true;  //Only Work in Https

#if DEBUG
            user_cookie.Secure = false;  //Work in HTTP (When is debug mode in web.config)
#endif

            var cookies = new List<System.Net.Http.Headers.CookieHeaderValue>();
            cookies.Add(user_cookie);

            response = new HttpResponseMessage(System.Net.HttpStatusCode.Redirect);

            var redirect_uri = Gale.Serialization.FromBase64(_next);
            response.Headers.Location = new Uri(redirect_uri);
            response.Headers.AddCookies(cookies);

            return Task.FromResult(response);
            //-----------------------------------------------------------------------------
        }
    }
}