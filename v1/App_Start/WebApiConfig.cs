using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Services.Description;
using Swashbuckle.Application;

namespace API
{

    /// <summary>
    /// WEB API Global Configuration
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Roles with Administrator Privileges
        /// </summary>
        public const string RootRoles = "ROOT";

        /// <summary>
        /// 1 Day 
        /// </summary>
        public const int AvatarCacheInSeconds = 86400;

        /// <summary>
        /// 1 Day 
        /// </summary>
        public const int Oauth2CacheInSeconds = 86400;



        /// <summary>
        /// Register Config Variables
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            //--------------------------------------------------------------------------------------------------------------------------------------------
            // Web API routes defaults
            config.EnableGaleRoutes();      // If you want manual versioning, don't send the api version
            config.SetJsonDefaultFormatter();   // Google Chrome Fix (default formatter is xml :/)
            //--------------------------------------------------------------------------------------------------------------------------------------------

            //--------------------------------------------------------------------------------------------------------------------------------------------
            // Swagger Documentation Enable
            // In Azure Deployment , Azure remove all XML :/, so we need to change the documentation file name
            String key_enabled = "Gale:Swagger:Enabled";
            String key_title = "Gale:Swagger:Title";
            if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(key_enabled))
            {
                Boolean isEnabled = false;
                Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings[key_enabled], out isEnabled);

                if (isEnabled)
                {
                    String SwaggerTitle = null;
                    if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(key_title))
                    {
                        SwaggerTitle = System.Configuration.ConfigurationManager.AppSettings[key_title];
                    }
                    // Enable Swagger
                    config.EnableSwagger("documentation.config", SwaggerTitle);
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------------------------

        }
    }
}
