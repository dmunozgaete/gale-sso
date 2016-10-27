using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using API.Endpoints.Oauth2.Resources;

namespace API.Endpoints.Oauth2.Services.Registration
{
    /// <summary>
    /// Validate and register User
    /// </summary>
    public class RegisterUser : Gale.REST.Http.HttpBaseActionResult
    {
        int _version;
        HttpRequestMessage _request;
        System.Collections.Specialized.NameValueCollection _parameters;

        //PARAMETERS FOR FAST READING
        String _country;
        String _next;
        String _nationalId;
        String _email;
        String _names;
        String _password;
        String _avatar;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">Request Message</param>
        /// <param name="version">Oauth2 assets version</param>
        /// <param name="parameters">Parameters Collections</param>
        public RegisterUser(HttpRequestMessage request, int version, System.Collections.Specialized.NameValueCollection parameters)
        {
            _version = version;
            _request = request;
            _parameters = parameters;

            //Fast reading
            _country = parameters[RFC6749Names.COUNTRY];
            _next = parameters[RFC6749Names.NEXT];

            _names = parameters[NewAccount.NAMES];
            _nationalId = parameters[NewAccount.NATIONALID];
            _email = parameters[NewAccount.EMAIL];
            _password = parameters[NewAccount.PASSWORD];
            _avatar = parameters[NewAccount.AVATAR];


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
            Gale.Exception.RestException.Guard(() => _country == null, "COUNTRY_EMPTY", Resources.OAuth2.ResourceManager);
            Gale.Exception.RestException.Guard(() => _next == null, "NEXT_EMPTY", Resources.OAuth2.ResourceManager);
            //------------------------------------------------------------------------------------------------------------------------

            HttpResponseMessage response;

            //------------------------------------------------------------------------------------------------------------------------
            //For the rest of the error's , don't throw a developer error,
            //Don't throw a developer Error, 
            //throw a nicely user friendly error =)!
            var errors = new List<String>();
            if (String.IsNullOrEmpty(_email) || (!String.IsNullOrEmpty(_email) && !Regex.IsMatch(_email, NewAccount.REGEX_EMAIL, RegexOptions.IgnoreCase)))
            {
                //Validate Email 
                errors.Add(Resources.OAuth2.EMAIL_INVALID);
            }
            if (String.IsNullOrEmpty(_names)) { errors.Add(Resources.OAuth2.NAMES_INVALID); }
            if (String.IsNullOrEmpty(_nationalId)) { errors.Add(Resources.OAuth2.NATIONALID_INVALID); }
            if (String.IsNullOrEmpty(_password)) { errors.Add(Resources.OAuth2.PASSWORD_INVALID); }
            if (!String.IsNullOrEmpty(_password) && _password.Length < 6) { errors.Add(Resources.OAuth2.PASSWORD_INVALID_LENGTH); }
            if (errors.Count > 0)
            {
                return RenderFriendlyError(_version,_parameters, errors);
            }
            //------------------------------------------------------------------------------------------------------------------------


            //------------------------------------------------------------------------------------------------------
            // REGISTER USER IN THE DB
            Guid newUserToken = System.Guid.Empty;
            using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_AUT_INS_Usuario"))
            {
                svc.Parameters.Add("USUA_Pais", _country);
                svc.Parameters.Add("USUA_IdentificadorNacional", _nationalId);
                svc.Parameters.Add("USUA_Email", _email);
                svc.Parameters.Add("USUA_NombreCompleto", _names);
                svc.Parameters.Add("USUA_Contrasena", Gale.Security.Cryptography.MD5.GenerateHash(_password));
                if (_avatar != null && _avatar != System.Guid.Empty.ToString())
                {
                    svc.Parameters.Add("ARCH_Token", _avatar);
                }

                try
                {
                    newUserToken = (Guid)this.ExecuteScalar(svc);
                }
                catch (Gale.Exception.SqlClient.CustomDatabaseException ex)
                {
                    var custom_error = Resources.OAuth2.ResourceManager.GetString(ex.Message);
                    if (custom_error == null)
                    {
                        throw ex;
                    }

                    errors.Add(custom_error);
                    return RenderFriendlyError(_version,_parameters, errors);
                }
            }
            //------------------------------------------------------------------------------------------------------


            //-----------------------------------------------------------------------------
            //CREATE AND SAVE THE COOKIE FOR THE USER!!
            var user_cookie = new System.Net.Http.Headers.CookieHeaderValue(
                RFC6749Names.COOKIE_CURRENT_USER,
                newUserToken.ToString()
            );
            user_cookie.HttpOnly = true;
            user_cookie.Secure = true;  //Only Work in Https

#if DEBUG
            user_cookie.Secure = false;  //Work in HTTP (When is debug mode in web.config)
#endif

            var cookies = new List<System.Net.Http.Headers.CookieHeaderValue>();
            cookies.Add(user_cookie);

            response = new HttpResponseMessage(System.Net.HttpStatusCode.Redirect);

            var redirect_uri = Gale.Serialization.FromBase64(_parameters[RFC6749Names.NEXT]);
            response.Headers.Location = new Uri(redirect_uri);
            response.Headers.AddCookies(cookies);

            return Task.FromResult(response);
            //-----------------------------------------------------------------------------
        }

        /// <summary>
        /// Render the RegisterDialog with a friendly Friendly Error (Not developer Error)
        /// </summary>
        /// <param name="version"></param>
        /// <param name="parameters"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> RenderFriendlyError(int version, System.Collections.Specialized.NameValueCollection parameters, List<String> errors)
        {
            //HAS ERROR'S
            parameters.Add(RFC6749Names.FRIENDLY_ERROR, String.Join(NewAccount.HTML_BREAK, errors));

            //get the "authentication dialog" again with the error
            String html = API.Endpoints.Oauth2.Templates.EmbeddedResolver.GetStringContent(
                version,
                "registration/create-account-dialog.cshtml",
                parameters
            );

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(html)
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
            return Task.FromResult(response);
        }
    }
}