using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using API.Endpoints.Oauth2.Resources;
using RazorTemplates.Core;

namespace API.Endpoints.Oauth2.Services.Auth
{
    /// <summary>
    /// Render a Authentication Dialog (HTML)
    /// </summary>
    public class GetAuthDialog : Gale.REST.Http.HttpBaseActionResult
    {
        HttpRequestMessage _request;
        int _version;
        System.Collections.Specialized.NameValueCollection _parameters;

        String _response_type;
        String _client_id;
        String _redirect_uri;
        String _prompt;
        String _scope;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">Request Message</param>
        /// <param name="version">Oauth2 assets version</param>
        /// <param name="parameters">Parameters Collections</param>
        public GetAuthDialog(HttpRequestMessage request, int version, System.Collections.Specialized.NameValueCollection parameters)
        {
            _version = version;
            _request = request;

            //Fast reading
            _response_type = parameters[RFC6749Names.RESPONSE_TYPE];
            _client_id = parameters[RFC6749Names.CLIENT_ID];
            _redirect_uri = parameters[RFC6749Names.REDIRECT_URI];
            _prompt = parameters[RFC6749Names.PROMPT];
            _scope = parameters[RFC6749Names.SCOPE];

            //Keep the reference to render
            _parameters = parameters;

            //Validate Each Parameters and his option's
        }

        /// <summary>
        /// Retrieves an individual cookie from the cookies collection
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public string GetCookie(string cookieName)
        {
            CookieHeaderValue cookie = _request.Headers.GetCookies(cookieName).FirstOrDefault();
            if (cookie != null)
            {
                return cookie[cookieName].Value;
            }

            return null;
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
            // DB Execution
            using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_OAUT_OBT_Aplicacion"))
            {
                svc.Parameters.Add("APPL_IdentificadorCliente", _client_id);
                var repo = this.ExecuteQuery(svc);

                var application = repo.GetModel<Models.Auth.Application>().FirstOrDefault();
                var authorized_scopes = repo.GetModel<Models.Auth.ScopesRequested>(1);
                var authenticators = repo.GetModel<Models.Auth.AuthenticatorAvailable>(2);

                //ADD EACH AUTHENTICATOR AVAILABLE IN THE CONFIGURATION ("prefix auth_")
                var authenticators_count = authenticators.Count;

                foreach (var authenticator in authenticators)
                {
                    _parameters.Add("auth_" + authenticator.name, authenticator.description);
                }
                _parameters.Add(AUTHENTICATORS.AUTHENTICATORS_COUNTS, authenticators_count.ToString());

                //BUILD THE SCOPE TEXT =), AND VALIDATE 
                var app_requested_scopes = _parameters[RFC6749Names.SCOPE].Split(',');
                var scope_disclaimer = new System.Text.StringBuilder();
                for (int index = 0; app_requested_scopes.Length > index; index++)
                {
                    var isValid = false;
                    var scope = app_requested_scopes[index];
                    if (authorized_scopes.Any((s) => s.identifier == scope))
                    {
                        isValid = true;
                        var fragment = Resources.OAuth2.ResourceManager.GetString(scope);

                        scope_disclaimer.Append(fragment);
                        if (app_requested_scopes.Length > 1)
                        {
                            var separator = ",";
                            if (index == app_requested_scopes.Length - 1)
                            {
                                separator = "";
                            }
                            else if (index == app_requested_scopes.Length - 2)
                            {
                                separator = " y ";
                            }
                            scope_disclaimer.Append(separator);
                        }
                    }
                    if (!isValid)
                    {
                        throw new Gale.Exception.RestException("SCOPE_INVALID", Resources.OAuth2.SCOPE_REQUESTED_IS_NOT_VALID);
                    }
                }
                scope_disclaimer.Append(".");
                _parameters.Add(RFC6749Names.SCOPE_DISCLAIMER, scope_disclaimer.ToString());



                //------------------------------------------------------------------------------------------------------------------------
                //GUARD EXCEPTION
                Gale.Exception.RestException.Guard(() => application == null, "APPLICATION_INVALID", Resources.OAuth2.ResourceManager);
                //------------------------------------------------------------------------------------------------------------------------

                //Add to the parameters
                _parameters.Add(RFC6749Names.APP_TOKEN, application.token.ToString());
                _parameters.Add(RFC6749Names.APP_NAME, application.name);

            }
            //------------------------------------------------------------------------------------------------------

            bool isIdentified = false;

            //------------------------------------------------------------------------------------------------------
            //CHECK COOKIE IF THE USER ALREADY IS LOGGED
            var user_cookie = GetCookie(RFC6749Names.COOKIE_CURRENT_USER);
            if (user_cookie != null && user_cookie.isGuid())
            {
                //------------------------------------------------------------------------------------------------------
                // Try to Get the Information from the last logged user
                using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_OAUT_OBT_RegenerarAcceso"))
                {
                    svc.Parameters.Add("APPL_Token", _parameters[RFC6749Names.APP_TOKEN]);
                    svc.Parameters.Add("USUA_Token", user_cookie);

                    var repo = this.ExecuteQuery(svc);

                    var state = repo.GetModel<Models.Auth.RegenerationResult>().FirstOrDefault();

                    //Succesfully regeneration User???
                    if (state != null && state.user_token != System.Guid.Empty)
                    {
                        isIdentified = true;
                        var needConsent = true;

                        //Add to the parameters
                        _parameters.Add(RFC6749Names.USER_TOKEN, state.user_token.ToString());
                        _parameters.Add(RFC6749Names.USER_NAME, state.user_name);

                        if (state.authorized)
                        {
                            // Is the second or more, logged time for the user in the application
                            // (already accepted the authorization scopes)
                            // according to the prompt option , show the consent dialog or not..
                            switch (_prompt.ToLower())
                            {
                                case "none":
                                    needConsent = false;
                                    break;
                                case "consent":
                                    needConsent = true;
                                    break;
                            }
                        }

                        if (needConsent)
                        {
                            // Is the first time for the user in the logged application, 
                            // (still not approved the authorization scopes, or prompt option)
                            // show the consent dialog, to accept the scopes
                            var consentDialog = new GetConsentDialog(
                                _request,
                                _version,
                                _parameters
                            );
                            return consentDialog.ExecuteAsync(cancellationToken);
                        }
                        else
                        {
                            // ALL IS OK!!, GET THE REDIRECT SUCCESS PROCESS
                            // AND RETURN CONTEXT!
                            var success = new Services.Auth.SuccessAuth(
                                _request,
                                _version,
                                _parameters
                            );
                            return success.ExecuteAsync(cancellationToken);
                        }
                    }
                }
                //------------------------------------------------------------------------------------------------------
            }
            //------------------------------------------------------------------------------------------------------

            //After all... the user is not Identified??
            if (!isIdentified)
            {
                //Add the next url , to post when a successfully auth is process
                _parameters.Add(RFC6749Names.NEXT, Gale.Serialization.ToBase64(_request.RequestUri.ToString()));

                //Assume Unidentified User, show the authentication dialog
                html = API.Endpoints.Oauth2.Templates.EmbeddedResolver.GetStringContent(
                    _version,
                    "auth/authentication-dialog.cshtml",
                    _parameters
                );
            }

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