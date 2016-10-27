using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Endpoints.Oauth2.Resources
{
    /// <summary>
    /// Registration Constant's
    /// </summary>
    public class NewAccount
    {
        public const String NATIONALID = "nationalId";
        public const String NAMES = "names";
        public const String EMAIL = "email";
        public const String PASSWORD = "password";
        public const String AVATAR = "avatar";


        //For Validation
        public const string HTML_BREAK = "<br />";
        public const String REGEX_EMAIL = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
    }
}