using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RazorTemplates.Core;

namespace API.Endpoints.Oauth2.Templates
{
    /// <summary>
    /// Resolve Embedded Resource
    /// </summary>
    public class EmbeddedResolver
    {
        // <summary>
        /// Get a Static File and Serve
        /// </summary>
        /// <param name="version">static file version</param>
        /// <param name="route">route to embedded file (without version)</param>
        public static String GetStringContent(int version, String route)
        {
            return GetStringContent(version, route, new { });
        }

        // <summary>
        /// Get a Static File and Serve
        /// </summary>
        /// <param name="version">static file version</param>
        /// <param name="route">route to embedded file (without version)</param>
        /// <param name="model">dynamic model with values to replace in the asset file</param>
        public static String GetStringContent(int version, String route, dynamic model)
        {
            //----------------------------------
            System.IO.Stream stream = GetEmbeddedStream(version, route);

            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                var view = Template.Compile(reader.ReadToEnd());
                stream.Dispose();   //Manual Disposing
                return view.Render(model);
            }
            //----------------------------------
        }

        /// <summary>
        /// Get a Static File and Serve
        /// </summary>
        /// <param name="version">static file version</param>
        /// <param name="route">route to embedded file (without version)</param>
        /// <returns></returns>
        public static System.IO.Stream GetEmbeddedStream(int version, String route)
        {
            //----------------------------------
            var assembly = typeof(API.Endpoints.Oauth2.Templates.EmbeddedResolver).Assembly;
            var path = route
                .Replace("/", ".")   //Path to embedded file
                .Replace("+", ".");  //Extension file

            String resourcePath = String.Format("API.Endpoints.Oauth2.Templates.v{0}.{1}", version, path);
            System.IO.Stream stream = assembly.GetManifestResourceStream(resourcePath);

            //------------------------------------------------------------------------------------------------------
            // GUARD EXCEPTIONS
            Gale.Exception.RestException.Guard(() => stream == null, System.Net.HttpStatusCode.NotFound, "TEMPLATE_DONT_EXIST", path);
            //------------------------------------------------------------------------------------------------------

            //Cant Directly dispose, because if send in a HttpRequestMessage, async... will fails
            return stream;

            //----------------------------------
        }

    }
}