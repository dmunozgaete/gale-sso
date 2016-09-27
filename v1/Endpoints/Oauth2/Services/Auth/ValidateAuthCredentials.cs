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
    /// Validate the credentials for the oauth2 flow
    /// </summary>
    public class ValidateAuthCredentials : Gale.REST.Http.HttpBaseActionResult
    {
        HttpRequestMessage _request;
        int _version;
        System.Collections.Specialized.NameValueCollection _parameters;

        String _client_id;
        String _username;
        String _password;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">Request Message</param>
        /// <param name="version">Oauth2 assets version</param>
        /// <param name="parameters">Parameters Collections</param>
        public ValidateAuthCredentials(HttpRequestMessage request, int version, System.Collections.Specialized.NameValueCollection parameters)
        {
            _version = version;
            _request = request;

            //Fast reading
            _client_id = parameters[RFC6749Names.CLIENT_ID];
            _username = parameters[RFC6749Names.USERNAME];
            _password = parameters[RFC6749Names.PASSWORD];

            //Keep the reference to render
            _parameters = parameters;
        }

        /// <summary>
        /// Async Process
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            HttpResponseMessage response;

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
                _parameters.Add(RFC6749Names.APP_NAME, application.name);
                _parameters.Add(RFC6749Names.APP_TOKEN, application.token.ToString());
            }
            //------------------------------------------------------------------------------------------------------

            //------------------------------------------------------------------------------------------------------
            // DB Execution
            using (Gale.Db.DataService svc = new Gale.Db.DataService("[PA_OAUT_OBT_ValidarAccesoPersonalizado]"))
            {
                svc.Parameters.Add("APPL_Token", _parameters[RFC6749Names.APP_TOKEN]);
                svc.Parameters.Add("NombreUsuario", _username);
                svc.Parameters.Add("Contrasena", Gale.Security.Cryptography.MD5.GenerateHash(_password));

                var repo = this.ExecuteQuery(svc);

                var state = repo.GetModel<Models.Auth.ValidationResult>().FirstOrDefault();

                if (state == null || (state != null && !state.authenticated))
                {
                    //Don't throw a developer Error, 
                    //throw a nicely user friendly error =)!
                    _parameters.Add(RFC6749Names.FRIENDLY_ERROR, Resources.OAuth2.USERNAME_OR_PASSWORD_INCORRECT);

                    //get the "authentication dialog" again with the error
                    String html = API.Endpoints.Oauth2.Templates.EmbeddedResolver.GetStringContent(
                        _version,
                        "auth/authentication-dialog.cshtml",
                        _parameters
                    );

                    response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                    {
                        Content = new StringContent(html)
                    };
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                    return Task.FromResult(response);
                }
                else
                {
                    var user_cookie = new System.Net.Http.Headers.CookieHeaderValue(
                        RFC6749Names.COOKIE_CURRENT_USER,
                        state.user
                    );
                    user_cookie.HttpOnly = true;
                    user_cookie.Secure = true;

                    var cookies = new List<System.Net.Http.Headers.CookieHeaderValue>();
                    cookies.Add(user_cookie);

                    response = new HttpResponseMessage(System.Net.HttpStatusCode.Redirect);

                    var redirect_uri = Gale.Serialization.FromBase64(_parameters[RFC6749Names.NEXT]);
                    response.Headers.Location = new Uri(redirect_uri);
                    response.Headers.AddCookies(cookies);

                    return Task.FromResult(response);
                }
            }
            //------------------------------------------------------------------------------------------------------
        }


    }
}