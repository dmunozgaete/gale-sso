using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Endpoints.Oauth2.Resources;

namespace API.Endpoints.Oauth2
{
    /// <summary>
    /// Oauth2 Flow Dialog
    /// </summary>
    [ApplicationInsights.TrackExceptionFilter]
    public class Oauth2Controller : Gale.REST.RestController
    {
        #region AUTH FLOW

        #region AUTH FLOW -> RENDER'S
        /// <summary>
        /// This endpoint is the target of the initial request. 
        /// It handles active session lookup, authenticating the user, and user consent.
        /// </summary>
        /// <param name="version">flow version</param>
        /// <param name="response_type">JavaScript applications should use token. This tells the Authorization Server to return the access token in the fragment.</param>
        /// <param name="client_id">Identifies the client that is making the request. The value passed in this parameter must exactly match the value shown in the registration form</param>
        /// <param name="redirect_uri">Determines where the response is sent. The value of this parameter must exactly match one of the values listed for this project in the application details</param>
        /// <param name="prompt">Space-delimited, case-sensitive list of prompts to present the user. If you don't specify this parameter, the user will be prompted only the first time your app requests access. (none,consent)</param>
        /// <param name="country">User Localizaction Country</param>
        /// <param name="state">Any string</param>
        /// <param name="scope">Space-delimited set of permissions that the application requests.</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("v{version:int}/auth")]
        [API.Endpoints.Oauth2.Decorators.DialogFormatWhenException]
        public IHttpActionResult auth(
            [FromUri]int version,
            [FromUri]String client_id,
            [FromUri]String redirect_uri,
            [FromUri]String response_type = "token",
            [FromUri]String prompt = "consent",
            [FromUri]String country = "CL",
            [FromUri]String state = null,
            [FromUri]String scope = null)
        {

            //------------------------------------------------------------------------------------------------------------------------
            //Create a collection Parameters for more simply use
            var parameters = new System.Collections.Specialized.NameValueCollection();
            parameters.Add(RFC6749Names.CLIENT_ID, client_id);
            parameters.Add(RFC6749Names.REDIRECT_URI, redirect_uri);
            parameters.Add(RFC6749Names.RESPONSE_TYPE, response_type);
            parameters.Add(RFC6749Names.COUNTRY, country);
            parameters.Add(RFC6749Names.PROMPT, prompt);
            parameters.Add(RFC6749Names.SCOPE, scope);
            parameters.Add(RFC6749Names.STATE, state);
            //------------------------------------------------------------------------------------------------------------------------

            //------------------------------------------------------------------------------------------------------------------------
            //GUARD EXCEPTION
            //--[ Restrictions according to RFC https://tools.ietf.org/html/rfc6749#section-4.4
            checkParameter(parameters, RFC6749Names.CLIENT_ID);
            checkParameter(parameters, RFC6749Names.REDIRECT_URI);
            checkParameter(parameters, RFC6749Names.COUNTRY);
            checkParameter(parameters, RFC6749Names.RESPONSE_TYPE);
            checkParameter(parameters, RFC6749Names.PROMPT);
            checkParameter(parameters, RFC6749Names.SCOPE);
            //------------------------------------------------------------------------------------------------------------------------

            return new Services.Auth.GetAuthDialog(this.Request, version, parameters);
        }

        /// <summary>
        /// Validate the credentials
        /// </summary>
        /// <param name="version">flow version</param>
        /// <returns></returns>
        [HttpPost]
        [HierarchicalRoute("v{version:int}/validate")]
        [API.Endpoints.Oauth2.Decorators.DialogFormatWhenException]
        public IHttpActionResult validate([FromUri]int version)
        {

            var parameters = this.Request.Content.ReadAsFormDataAsync().Result;

            //------------------------------------------------------------------------------------------------------------------------
            //GUARD EXCEPTION
            //--[ Restrictions according to RFC https://tools.ietf.org/html/rfc6749#section-4.4
            Gale.Exception.RestException.Guard(() => parameters == null, "PARAMETERS_REQUIRED", Resources.OAuth2.ResourceManager);
            checkParameter(parameters, RFC6749Names.CLIENT_ID);
            checkParameter(parameters, RFC6749Names.REDIRECT_URI);
            checkParameter(parameters, RFC6749Names.COUNTRY);
            checkParameter(parameters, RFC6749Names.RESPONSE_TYPE);
            checkParameter(parameters, RFC6749Names.PROMPT);
            checkParameter(parameters, RFC6749Names.USERNAME);
            checkParameter(parameters, RFC6749Names.PASSWORD);
            checkParameter(parameters, RFC6749Names.SCOPE);
            //------------------------------------------------------------------------------------------------------------------------

            return new Services.Auth.ValidateAuthCredentials(this.Request, version, parameters);

        }

        /// <summary>
        /// Logout Current Account
        /// </summary>
        /// <param name="version">flow version</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("v{version:int}/logout")]
        [API.Endpoints.Oauth2.Decorators.DialogFormatWhenException]
        public IHttpActionResult validate([FromUri]int version, [FromUri]String next)
        {
            return new Services.Auth.Logout(this.Request, version, next);
        }
        #endregion

        #region AUTH FLOW -> SUCCESS, APPROVE AND ERROR SERVICES

        /// <summary>
        /// Final Process, end the flow, and return context to the user
        /// </summary>
        /// <param name="version">flow version</param>
        /// <returns></returns>
        [HttpPost]
        [HierarchicalRoute("v{version:int}/success")]
        [API.Endpoints.Oauth2.Decorators.DialogFormatWhenException]
        public IHttpActionResult success([FromUri]int version)
        {
            var parameters = this.Request.Content.ReadAsFormDataAsync().Result;

            //------------------------------------------------------------------------------------------------------------------------
            //GUARD EXCEPTION
            //--[ Restrictions according to RFC https://tools.ietf.org/html/rfc6749#section-4.4
            Gale.Exception.RestException.Guard(() => parameters == null, "PARAMETERS_REQUIRED", Resources.OAuth2.ResourceManager);
            checkParameter(parameters, RFC6749Names.CLIENT_ID);
            checkParameter(parameters, RFC6749Names.REDIRECT_URI);
            checkParameter(parameters, RFC6749Names.RESPONSE_TYPE);
            checkParameter(parameters, RFC6749Names.PROMPT);
            checkParameter(parameters, RFC6749Names.SCOPE);
            //------------------------------------------------------------------------------------------------------------------------

            return new Services.Auth.SuccessAuth(this.Request, version, parameters);

        }

        /// <summary>
        /// Approve / Modify the scope, and succes the flow
        /// </summary>
        /// <param name="version">flow version</param>
        /// <returns></returns>
        [HttpPost]
        [HierarchicalRoute("v{version:int}/approve")]
        [API.Endpoints.Oauth2.Decorators.DialogFormatWhenException]
        public IHttpActionResult approve([FromUri]int version)
        {
            var parameters = this.Request.Content.ReadAsFormDataAsync().Result;

            //------------------------------------------------------------------------------------------------------------------------
            //GUARD EXCEPTION
            //--[ Restrictions according to RFC https://tools.ietf.org/html/rfc6749#section-4.4
            Gale.Exception.RestException.Guard(() => parameters == null, "PARAMETERS_REQUIRED", Resources.OAuth2.ResourceManager);
            checkParameter(parameters, RFC6749Names.CLIENT_ID);
            checkParameter(parameters, RFC6749Names.REDIRECT_URI);
            checkParameter(parameters, RFC6749Names.RESPONSE_TYPE);
            checkParameter(parameters, RFC6749Names.APP_TOKEN);
            checkParameter(parameters, RFC6749Names.USER_TOKEN);
            checkParameter(parameters, RFC6749Names.PROMPT);
            checkParameter(parameters, RFC6749Names.SCOPE);
            //------------------------------------------------------------------------------------------------------------------------

            return new Services.Auth.ApproveScopes(this.Request, version, parameters);

        }

        /// <summary>
        /// Error Process, end the flow, and return context to the user
        /// </summary>
        /// <param name="version">flow version</param>
        /// <returns></returns>
        [HttpPost]
        [HierarchicalRoute("v{version:int}/error")]
        [API.Endpoints.Oauth2.Decorators.DialogFormatWhenException]
        public IHttpActionResult error([FromUri]int version)
        {
            var parameters = this.Request.Content.ReadAsFormDataAsync().Result;

            //------------------------------------------------------------------------------------------------------------------------
            //GUARD EXCEPTION
            //--[ Restrictions according to RFC https://tools.ietf.org/html/rfc6749#section-4.4
            Gale.Exception.RestException.Guard(() => parameters == null, "PARAMETERS_REQUIRED", Resources.OAuth2.ResourceManager);
            checkParameter(parameters, RFC6749Names.REDIRECT_URI);
            checkParameter(parameters, RFC6749Names.RESPONSE_TYPE);
            checkParameter(parameters, RFC6749Names.ERROR);
            //------------------------------------------------------------------------------------------------------------------------

            return new Services.Auth.CancelAuth(this.Request, version, parameters);

        }
        #endregion

        #region AUTH FLOW -> FOR DESKTOP OAUTH2 PROCESS
        /// <summary>
        ///  If you are using this in a webview within a desktop app,the "redirect_uri" must be set to
        /// </summary>
        /// <param name="version">flow version</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("v{version:int}/connect/oauth2_callback.html")]
        public IHttpActionResult oauth2_callback([FromUri]int version, [FromUri]String origin = null)
        {
            return new Services.Auth.GetAssetsFile(version, "auth/oauth2_callback.cshtml", new
            {
                origin = origin
            });
        }


        #endregion

        #endregion

        #region REGISTRATION FLOW

        #region REGISTRATION FLOW --> RENDER'S

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="version">Version 1.0</param>
        /// <param name="next">Url to continue base64()</param>
        /// <param name="country">Country</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("v{version:int}/register")]
        [API.Endpoints.Oauth2.Decorators.DialogFormatWhenException]
        public IHttpActionResult RegisterUser([FromUri]int version, [FromUri]string next, [FromUri]string country)
        {
            var parameters = new System.Collections.Specialized.NameValueCollection();
            parameters.Add(RFC6749Names.NEXT, next);
            parameters.Add(RFC6749Names.COUNTRY, country);    //Default

            return new Services.Registration.GetUserRegisterDialog(this.Request, version, parameters);
        }

        /// <summary>
        /// Validate and save the user into the SSO
        /// </summary>
        /// <param name="version">Version to acquire</param>
        /// <param name="next">base64(uri)</param>
        /// <param name="country">User Localization Country</param>
        /// <returns></returns>
        [HttpPost]
        [HierarchicalRoute("v{version:int}/register")]
        [API.Endpoints.Oauth2.Decorators.DialogFormatWhenException]
        public IHttpActionResult SaveAndRegisterUser(int version, [FromUri]string next, [FromUri]string country)
        {
            var parameters = this.Request.Content.ReadAsFormDataAsync().Result;
            parameters.Add(RFC6749Names.NEXT, next);
            parameters.Add(RFC6749Names.COUNTRY, country); 

            return new Services.Registration.RegisterUser(this.Request, version, parameters);
        }
        #endregion

        #endregion

        #region --> FUNCTIONS AND VARIABLES
        /// <summary>
        /// Create a CSRF key with this signature, to avoid Potencial Injection's
        ///  TODO: http://stackoverflow.com/questions/2004666/get-unique-machine-id
        /// </summary>
        const string csrf_signature = "DyA:V-IJaDz=M/UyN{OfZ9GUAEEWTAE";

        /// <summary>
        /// Validate the parameter
        /// </summary>
        /// <param name="parameters">data collection</param>
        /// <param name="key">parameter to validate</param>
        private void checkParameter(System.Collections.Specialized.NameValueCollection parameters, String key)
        {
            var isValid = false;
            if (parameters.AllKeys.Contains(key))
            {
                isValid = !String.IsNullOrEmpty(parameters[key]);
            }

            //------------------------------------------------------------------------------------------------------------------------
            //GUARD EXCEPTION
            Gale.Exception.RestException.Guard(() => !isValid,
                String.Format("{0}_REQUIRED",
                key.ToUpper()),
                Resources.OAuth2.ResourceManager
            );
            //------------------------------------------------------------------------------------------------------------------------
        }
        #endregion
        #region --> ASSET'S FOLDER
        /// <summary>
        /// Resource File Assets (1 day cache)
        /// </summary>
        /// <param name="version">flow version</param>
        /// <param name="assetfolder">Main Asset Folder</param>
        /// <param name="file">File Name</param>
        /// <param name="extension">File Extension</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("v{version:int}/{assetfolder}/{file}.{extension}")]
        [WebApi.OutputCache.V2.CacheOutput(ClientTimeSpan = WebApiConfig.Oauth2CacheInSeconds, ServerTimeSpan = WebApiConfig.Oauth2CacheInSeconds)]
        public IHttpActionResult resource_file(
            [FromUri]int version,
            [FromUri]String assetfolder,
            [FromUri]String file,
            [FromUri]String extension)
        {
            string fullroute = String.Format("{0}/{1}.{2}", assetfolder, file, extension);
            return new Services.Auth.GetAssetsFile(version, fullroute);
        }

        /// <summary>
        /// Resource File Assets
        /// </summary>
        /// <param name="version">flow version</param>
        /// <param name="assetfolder">Main Asset Folder</param>
        /// <param name="subfolder">Sub Asset Folder</param>
        /// <param name="file">File Name</param>
        /// <param name="extension">File Extension</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("v{version:int}/{assetfolder}/{subfolder}/{file}.{extension}")]
        [WebApi.OutputCache.V2.CacheOutput(ClientTimeSpan = WebApiConfig.Oauth2CacheInSeconds, ServerTimeSpan = WebApiConfig.Oauth2CacheInSeconds)]
        public IHttpActionResult resource_file(
            [FromUri]int version,
            [FromUri]String assetfolder,
            [FromUri]String subfolder,
            [FromUri]String file,
            [FromUri]String extension)
        {
            string fullroute = String.Format("{0}/{1}/{2}.{3}", assetfolder, subfolder, file, extension);
            return new Services.Auth.GetAssetsFile(version, fullroute);
        }
        #endregion
    }
}