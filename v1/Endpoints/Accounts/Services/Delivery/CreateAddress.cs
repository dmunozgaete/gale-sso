using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace API.Endpoints.Accounts.Services.Delivery
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateAddress : Gale.REST.Http.HttpCreateActionResult<Models.Delivery.NewAddress>
    {
        String _account;

        public CreateAddress(String account, Models.Delivery.NewAddress address)
            : base(address)
        {
            _account = account;
        }

        public override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            //------------------------------------------------------------------------------------------------------
            // GUARD EXCEPTIONS
            Gale.Exception.RestException.Guard(() => this.Model == null, "ADDRESS_EMPTY", API.Errors.ResourceManager);
            Gale.Exception.RestException.Guard(() => this.Model.type == null, "ADDRESSTYPE_EMPTY", API.Errors.ResourceManager);
            Gale.Exception.RestException.Guard(() => this.Model.address == null, "ADDRESS_INVALID", API.Errors.ResourceManager);
            Gale.Exception.RestException.Guard(() => this.Model.locations == null, "LOCATION_EMPTY", API.Errors.ResourceManager);
            Gale.Exception.RestException.Guard(() => this.Model.locations.Count ==0, "AT_LEAST_ONE_LOCATION_ISREQUIRED", API.Errors.ResourceManager);
            //------------------------------------------------------------------------------------------------------

            //OPTIONAL BUT MUST BE DECLARATED
            if (this.Model.phones == null)
            {
                this.Model.phones = new List<Models.Delivery.NewPhone>();
            }

            //------------------------------------------------------------------------------------------------------
            // DB Execution
            using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_DESP_INS_Direccion"))
            {
                svc.Parameters.Add("USUA_Token", _account);
                svc.Parameters.Add("TIDI_Identificador", this.Model.type);
                svc.Parameters.Add("DIRE_Direccion", this.Model.address);
                svc.Parameters.Add("DIRE_Numero", this.Model.number);
                svc.Parameters.Add("DIRE_NumeroDeptoOCasa", this.Model.houseOrDepto);
                svc.Parameters.Add("DIRE_Pais", this.Model.country);

                svc.AddTableType<Models.Delivery.NewPhone>("Telefonos", this.Model.phones);
                svc.AddTableType<Models.Delivery.NewLocation>("Ubicaciones", this.Model.locations);

                var repo = this.ExecuteQuery(svc);
                var newAddress = repo.GetModel<Models.Delivery.VW_Address>().FirstOrDefault();
                var newPhones = repo.GetModel<Models.Delivery.AddressPhone>(1);
                var newLocations = repo.GetModel<Models.Delivery.AddressLocation>(2);

                newAddress.locations = newLocations;
                newAddress.phones = newPhones;
                newAddress.fullLocation = String.Join(", ", newLocations.Select(add => add.name));

                //Create Response
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.Created)
                {
                    Content = new ObjectContent<Object>(
                        newAddress,
                        new Gale.REST.Http.Formatter.KqlFormatter()
                    )
                };
                return Task.FromResult(response);

            }
            //------------------------------------------------------------------------------------------------------

        }
    }
}