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
    public class SuccessAuth : Gale.REST.Http.HttpBaseActionResult
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
        public SuccessAuth(HttpRequestMessage request, int version, System.Collections.Specialized.NameValueCollection parameters)
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

                   
                    //BUILD A JWT TOKEN
                    Gale.Security.Oauth.Jwt.Wrapper jwt = null;
                    using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_OAUT_OBT_InformacionAcceso"))
                    {
                        svc.Parameters.Add("APPL_Token", _parameters[RFC6749Names.APP_TOKEN]);
                        svc.Parameters.Add("USUA_Token", _parameters[RFC6749Names.USER_TOKEN]);

                        Gale.Db.EntityRepository rep = this.ExecuteQuery(svc);

                        Models.Auth.User account = rep.GetModel<Models.Auth.User>(0).FirstOrDefault();
                        Models.Auth.Application application = rep.GetModel<Models.Auth.Application>(1).FirstOrDefault();
                        Models.Auth.UserApplication userApplication = rep.GetModel<Models.Auth.UserApplication>(2).FirstOrDefault();
                        Gale.Db.EntityTable<Models.Auth.Role> roles = rep.GetModel<Models.Auth.Role>(3);

                        //------------------------------------------------------------------------------------------------------------------------
                        //GUARD EXCEPTION
                        Gale.Exception.RestException.Guard(() => account == null, "REGENERATION_JWT_ERROR", Resources.OAuth2.ResourceManager);
                        Gale.Exception.RestException.Guard(() => application == null, "REGENERATION_JWT_ERROR", Resources.OAuth2.ResourceManager);
                        //------------------------------------------------------------------------------------------------------------------------

                        //------------------------------------------------------------------------------------------------------------------------
                        //GET THE EXPIRATION TIME STABLISHED FOR THE APPLICATION
                        Int32 expiration = application.expiration;  
                        List<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>();
                        claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.PrimarySid, account.token.ToString()));
                        claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, account.fullname));
                        claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, account.email));
                        claims.Add(new System.Security.Claims.Claim("hash", userApplication.hash));
                        claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.GroupSid, account.type_identifier));


                        //ADD EACH SCOPE AUTHORIZED BY THE USER
                        var scopes = _parameters[RFC6749Names.SCOPE].Split(',');
                        foreach (var scope in scopes)
                        {
                            claims.Add(new System.Security.Claims.Claim("scope", scope));
                        }

                        // ADD EACH ROLE ASSIGNED TO THE USER FOR THE APPLICATION
                        roles.ForEach((role) =>
                        {
                            claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role.identifier));
                        });
                        jwt = Gale.Security.Oauth.Jwt.Manager.CreateToken(claims, DateTime.Now.AddMinutes(expiration));
                        //------------------------------------------------------------------------------------------------------------------------
                    }


                    //ACCESS_TOKEN
                    uri.Append("#access_token=");
                    uri.Append(jwt.access_token);

                    uri.Append("&token_type=");
                    uri.Append(jwt.token_type);

                    uri.Append("&expires_in=");
                    uri.Append(jwt.expires_in);
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