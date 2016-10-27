using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Endpoints.Oauth2.Resources
{
    /// <summary>
    /// Naming Conventions for resources attributes
    /// </summary>
    public class RFC6749Names
    {
        public const String REDIRECT_URI = "redirect_uri";
        public const String CLIENT_ID = "client_id";
        public const String RESPONSE_TYPE = "response_type";
        public const String PROMPT = "prompt";
        public const String SCOPE = "scope";
        public const String STATE = "state";
        public const String CSRF = "csrf";
        public const String COUNTRY = "country";

        //NOT IN THE RFC, BUT FOR SUPPORT
        public const String USERNAME = "username";
        public const String PASSWORD = "password";
        public const String FRIENDLY_ERROR = "friendly_error";

        public const String APP_NAME = "app_name";
        public const String APP_TOKEN = "app_token";

        public const String COOKIE_CURRENT_USER = "c_user";

        public const String USER_TOKEN = "user_token";
        public const String USER_NAME = "user_name";


        public const String NEXT = "next";
        public const String ERROR = "error";
        public const String VALIDATE_ACTION = "validate";

        public const String SCOPE_DISCLAIMER = "scopes_disclaimer";

    }

}