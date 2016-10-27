using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace API.Endpoints.Accounts.Services.Delivery
{
    /// <summary>
    /// Get the addresses for the user
    /// </summary>
    public class GetAccountAddresses : Gale.REST.Http.HttpReadActionResult<String>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="account"></param>
        public GetAccountAddresses(String account) : base(account) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            //------------------------------------------------------------------------------------------------------
            // GUARD EXCEPTIONS
            Gale.Exception.RestException.Guard(() => this.Model == null, "ACCOUNT_EMPTY", API.Errors.ResourceManager);
            //------------------------------------------------------------------------------------------------------

            //------------------------------------------------------------------------------------------------------
            // DB Execution
            using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_DESP_OBT_Direcciones"))
            {
                svc.Parameters.Add("USUA_Token", this.Model);

                var repo = this.ExecuteQuery(svc);
                var db_addresses = repo.GetModel<Models.Delivery.VT_DESP_Direccion>();
                var db_phones = repo.GetModel<Models.Delivery.TB_DESP_Telefono>(1);
                var db_locations = repo.GetModel<Models.Delivery.TB_DESP_DireccionUbicacion>(2);


                var addresses = new List<Models.Delivery.VW_Address>();
                db_addresses.ForEach((db_address) =>
                {
                    #region GET PHONES FOR THE CURRENT ADDRESS
                    var addressPhones = new List<Models.Delivery.AddressPhone>();

                    var db_address_phones = db_phones
                        .Where(phone => phone.DIRE_Codigo == db_address.DIRE_Codigo);

                    foreach (var db_phone in db_address_phones)
                    {
                        addressPhones.Add(new Models.Delivery.AddressPhone()
                        {
                            areaCode = db_phone.TELE_CodigoArea,
                            countryCode = db_phone.TELE_CodigoPais,
                            number = db_phone.TELE_Numero,
                            updatedAt = db_phone.TELE_FechaActualizacion
                        });
                    }
                    #endregion

                    #region GET LOCATIONS FOR THE CURRENT ADDRESS
                    var addressLocations = new List<Models.Delivery.AddressLocation>();

                    var db_address_locations = db_locations
                        .Where(loca => loca.DIRE_Codigo == db_address.DIRE_Codigo).OrderBy(loca => loca.UBIC_TIUB_Codigo);



                    foreach (var db_location in db_address_locations)
                    {
                        addressLocations.Add(new Models.Delivery.AddressLocation()
                        {
                            id = db_location.UBIC_Codigo,
                            name = db_location.UBIC_Nombre,
                            type = db_location.UBIC_TIUB_Codigo
                        });
                    }
                    #endregion

                    //ADD EACH ADDRESS TO STACK
                    addresses.Add(new Models.Delivery.VW_Address()
                    {
                        address = db_address.DIRE_Direccion,
                        country = db_address.DIRE_Pais,
                        houseOrDepto = db_address.DIRE_NumeroDeptoOCasa,
                        number = db_address.DIRE_Numero,
                        token = db_address.DIRE_Token,
                        type_identifier = db_address.TIDI_Identificador,
                        type_name = db_address.TIDI_Nombre,
                        updatedAt = db_address.DIRE_FechaActualizacion,
                        phones = addressPhones,
                        locations = addressLocations,
                        fullLocation = String.Join(", ", addressLocations.Select(add => add.name))
                    });

                });

                //Create Response
                var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new ObjectContent<Object>(
                        addresses,
                        new Gale.REST.Http.Formatter.KqlFormatter()
                    )
                };
                return Task.FromResult(response);

            }
            //------------------------------------------------------------------------------------------------------

        }
    }
}