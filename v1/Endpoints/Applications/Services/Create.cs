using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography;

namespace API.Endpoints.Applications.Services
{
    /// <summary>
    /// Register Application in DB (For API Securified Endpoints)
    /// </summary>
    public class Create : Gale.REST.Http.HttpCreateActionResult<Models.NewAplication>
    {
        private String _executor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executor">Executor</param>
        /// <param name="application">Application Data</param>
        public Create(String executor, Models.NewAplication application)
            : base(application)
        {
            _executor = executor;
        }

        /// <summary>
        /// Async Process
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            //------------------------------------------------------------------------------------------------------
            // GUARD EXCEPTIONS
            Gale.Exception.RestException.Guard(() => Model == null, "BODY_EMPTY", API.Errors.ResourceManager);
            Gale.Exception.RestException.Guard(() => String.IsNullOrEmpty(Model.name), "NAME_EMPTY", API.Errors.ResourceManager);
            Gale.Exception.RestException.Guard(() => String.IsNullOrEmpty(Model.description), "DESCRIPTION_EMPTY", API.Errors.ResourceManager);
            Gale.Exception.RestException.Guard(() => Model.expirationToken <=0, "EXPIRATION_INVALID", API.Errors.ResourceManager);
            //------------------------------------------------------------------------------------------------------

            Guid newAppToken = System.Guid.NewGuid();

            //------------------------------------------------------------------------------------------------------
            //CREATE CONSUMER AND CLIENT_ID 
            // Authentication: Basic Base64(client_id:client_secret) --> Send always in SSL
            var cryptoRandomDataGenerator = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] buffer = new byte[33];   //Password Size (44)
            cryptoRandomDataGenerator.GetBytes(buffer);
            string client_secret = Convert.ToBase64String(buffer);
            string client_id = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(newAppToken.ToString()));
            //------------------------------------------------------------------------------------------------------

            //------------------------------------------------------------------------------------------------------
            // DB Execution
            using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_AUT_INS_Aplicacion"))
            {
                svc.Parameters.Add("ENTI_Token", _executor);
                svc.Parameters.Add("APPL_Token", newAppToken);                 // Send the app token here
                svc.Parameters.Add("APPL_Nombre", Model.name);
                svc.Parameters.Add("APPL_Descripcion", Model.description);
                svc.Parameters.Add("APPL_IdentificadorCliente", client_id); // client_id
                svc.Parameters.Add("APPL_SecretoCliente", client_secret);   // client_secret
                svc.Parameters.Add("APPL_MD5", Gale.Security.Cryptography.MD5.GenerateHash(client_secret));   // client_secret
                svc.Parameters.Add("APPL_ExpiracionToken", Model.expirationToken);
                svc.Parameters.Add("APPL_OrigenesHabilitados", Model.origins);
                if (Model.avatar != null && Model.avatar != System.Guid.Empty)
                {
                    svc.Parameters.Add("ARCH_Token", Model.avatar);
                }

                this.ExecuteAction(svc);
            }
            //------------------------------------------------------------------------------------------------------

           
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Created)
            {
                Content = new ObjectContent<object>(new
                {
                    token = newAppToken
                },
                System.Web.Http.GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
            return Task.FromResult(response);
        }
    }
}