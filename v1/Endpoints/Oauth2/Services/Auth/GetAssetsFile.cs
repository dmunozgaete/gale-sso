using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using RazorTemplates.Core;

namespace API.Endpoints.Oauth2.Services.Auth
{
    public class GetAssetsFile : Gale.REST.Http.HttpBaseActionResult
    {
        int _version;
        String _route;
        dynamic _model;

        /// <summary>
        /// Get a Static File and Serve
        /// </summary>
        /// <param name="version">static file version</param>
        /// <param name="route">route to embedded file (without version)</param>
        public GetAssetsFile(int version, String route)
        {
            _version = version;
            _route = route;
            _model = null;
        }

        /// <summary>
        /// Get a Static File and Serve
        /// </summary>
        /// <param name="version">static file version</param>
        /// <param name="route">route to embedded file (without version)</param>
        /// <param name="model">dynamic model with values to replace in the asset file</param>
        public GetAssetsFile(int version, String route, dynamic model)
        {
            _version = version;
            _route = route;
            _model = model;
        }

        /// <summary>
        /// Async Process
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            //----------------------------------

            System.IO.FileInfo finfo = new System.IO.FileInfo(_route);
            String mediaType = System.Web.MimeMapping.GetMimeMapping(_route); ;
            System.Net.Http.HttpContent content;

            switch (finfo.Extension)
            {
                case ".png":
                case ".jpg":
                case ".svg":
                case ".woff":
                case ".ttf":
                case ".eot":
                    var svg = API.Endpoints.Oauth2.Templates.EmbeddedResolver.GetEmbeddedStream(_version, _route);
                    content = new StreamContent(svg);
                    break;
                case ".json":
                case ".css":
                case ".html":
                default:
                    var text = API.Endpoints.Oauth2.Templates.EmbeddedResolver.GetStringContent(_version, _route, _model);
                    content = new StringContent(text);
                    break;
                case ".cshtml":
                    mediaType = "text/html";
                    var template = API.Endpoints.Oauth2.Templates.EmbeddedResolver.GetStringContent(_version, _route, _model);
                    content = new StringContent(template);
                    break;
                case ".map":
                    HttpResponseMessage notFound = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                    return Task.FromResult(notFound);
            }
            //----------------------------------

            //-----------------------------------------------------------------------------
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = content
            };

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mediaType);
            return Task.FromResult(response);
            //-----------------------------------------------------------------------------

        }
    }
}