using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace API.Endpoints.Files.Services
{
    /// <summary>
    /// Authentication API
    /// </summary>
    public class View : Gale.REST.Http.HttpBaseActionResult
    {
        string _token;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token">Token del Archivo</param>
        public View(String token)
        {
            _token = token;
        }

        /// <summary>
        /// Obtiene la foto del usuario
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            //------------------------------------------------------------------------------------------------------
            // GUARD EXCEPTIONS
            Gale.Exception.RestException.Guard(() => _token == null, "FILE_EMPTY", API.Errors.ResourceManager);
            Gale.Exception.RestException.Guard(() => !_token.isGuid(), "FILE_INVALID_FORMAT", API.Errors.ResourceManager);
            //------------------------------------------------------------------------------------------------------

            //------------------------------------------------------------------------------------------------------
            // DB Execution
            var file = Functions.Files.Get(_token);

            if (file == null)
            {
                return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
            }

            //Create Response
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StreamContent(new System.IO.MemoryStream(file.binary.ToArray())),
            };

            //Cache Control Time (in Minutes)
            int cacheInMinutes = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Gale:Image:Client:Cache"]);

            //CACHE CONTROL (WORK IN PRODUCTION STAGE)
            response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(cacheInMinutes),
                Public = true
            };

            //Add Content-Type Header
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.contentType);

            return Task.FromResult(response);

            //------------------------------------------------------------------------------------------------------

        }

    }
}