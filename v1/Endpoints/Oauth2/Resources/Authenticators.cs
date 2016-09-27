using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Endpoints.Oauth2.Resources
{
    /// <summary>
    /// Available Authenticators
    /// </summary>
    public class AUTHENTICATORS
    {
        public const String AUTHENTICATORS_COUNTS = "auth_counts";
        public const String CUSTOM = "auth_custo";
        public const String FACEBOOK = "auth_fbook";
        public const String GOOGLE = "auth_googl";
        public const String LDAP = "auth_wldap";
        public const String LINKEDIN = "auth_linke";
        public const String TWITTER = "auth_twitt";
    }

}