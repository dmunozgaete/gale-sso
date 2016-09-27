using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using ImageProcessor;
using ImageProcessor.Imaging.Formats;

namespace API.Endpoints.Accounts.Services
{
    /// <summary>
    /// Get the account avatar (or dummy)
    /// </summary>
    public class GetAccountAvatar : Gale.REST.Http.HttpBaseActionResult
    {
        string _token;
        int _size;

        /// <summary> 
        /// Constructor
        /// </summary>
        /// <param name="token">Token de la cuenta</param>
        /// <param name="size">Size</param>
        public GetAccountAvatar(String token, int size)
        {
            _token = token;
            _size = size;
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

            System.IO.Stream stream = null;
            String contentType = "image/jpeg";

            var min = 10;
            var max = 2048;
            if (_size < min) { _size = min; }
            if (_size > max) { _size = max; }

            //------------------------------------------------------------------------------------------------------
            // DB Execution
            using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_AUT_OBT_AvatarCuenta"))
            {
                svc.Parameters.Add("ENTI_Token", _token);

                var repo = this.ExecuteQuery(svc);
                var file = repo.GetModel<API.Endpoints.Files.Models.FileData>().FirstOrDefault();
                if (file == null)
                {
                    //GET A DUMMY ACCOUNT AVATAR
                    stream = API.Endpoints.Oauth2.Templates.EmbeddedResolver.GetEmbeddedStream(0, "images.dummy-application.jpg");
                }
                else
                {
                    stream = new System.IO.MemoryStream(file.binary.ToArray());
                    contentType = file.contentType;
                }

                Size size = new Size(_size, _size);
                System.IO.MemoryStream outStream = new System.IO.MemoryStream();    //Cant dispose
                using (ImageFactory imageFactory = new ImageFactory())
                {
                    imageFactory.Load(stream).Resize(size).Save(outStream);
                }
                stream.Dispose();

                //Create Response
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StreamContent(outStream),
                };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                return Task.FromResult(response);

            }
            //------------------------------------------------------------------------------------------------------

        }

    }
}