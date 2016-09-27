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
    /// Get the Entity Information
    /// </summary>
    public class GetEntityInformation : Gale.REST.Http.HttpReadActionResult<String>
    {
        /// <summary> 
        /// Constructor
        /// </summary>
        /// <param name="token">Token de la entidad consultada</param>
        public GetEntityInformation(String token) : base(token) { }

        /// <summary>
        /// Obtiene la foto del usuario
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            //------------------------------------------------------------------------------------------------------
            // GUARD EXCEPTIONS
            Gale.Exception.RestException.Guard(() => this.Model == null, "ACCOUNT_EMPTY", Resources.Accounts.ResourceManager);
            Gale.Exception.RestException.Guard(() => !this.Model.isGuid(), "ACCOUNT_INVALID_FORMAT", Resources.Accounts.ResourceManager);
            //------------------------------------------------------------------------------------------------------

            //------------------------------------------------------------------------------------------------------
            // DB Execution
            using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_OAUT_OBT_InformacionCuenta"))
            {
                svc.Parameters.Add("ENTI_Token", this.Model);

                var repo = this.ExecuteQuery(svc);
                var account = repo.GetModel<Models.Account>().FirstOrDefault();

                //Create Response
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new ObjectContent<Object>(
                        account,
                        new Gale.REST.Http.Formatter.KqlFormatter()
                    )
                };
                return Task.FromResult(response);

            }
            //------------------------------------------------------------------------------------------------------

        }

    }
}