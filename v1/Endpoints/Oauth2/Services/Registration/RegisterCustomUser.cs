using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Cryptography;

namespace API.Endpoints.Oauth2.Services.Registration
{
    /// <summary>
    /// Register Account in SSO with full basic information for an user (Custom Authenticator)
    /// </summary>
    public class RegisterCustomUser : Gale.REST.Http.HttpCreateActionResult<Models.Registration.NewAccount>
    {
     
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="account">User Minimal Information for an Account</param>
        public RegisterCustomUser(Models.Registration.NewAccount account)
            : base(account)
        {
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
            Gale.Exception.RestException.Guard(() => String.IsNullOrEmpty(Model.fullname), "FULLNAME_EMPTY", API.Errors.ResourceManager);
            Gale.Exception.RestException.Guard(() => String.IsNullOrEmpty(Model.email), "EMAIL_EMPTY", API.Errors.ResourceManager);
            Gale.Exception.RestException.Guard(() => String.IsNullOrEmpty(Model.password), "PASSWORD_EMPTY", API.Errors.ResourceManager);
            //------------------------------------------------------------------------------------------------------

            //------------------------------------------------------------------------------------------------------
            // DB Execution
            Guid newUserToken = System.Guid.Empty;
            using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_AUT_INS_Usuario"))
            {
                svc.Parameters.Add("USUA_Email", Model.email);
                svc.Parameters.Add("USUA_NombreCompleto", Model.fullname);
                svc.Parameters.Add("USUA_Contrasena", Gale.Security.Cryptography.MD5.GenerateHash(Model.password));
                if (Model.avatar != null && Model.avatar != System.Guid.Empty)
                {
                    svc.Parameters.Add("ARCH_Token", Model.avatar);
                }

                newUserToken = (Guid)this.ExecuteScalar(svc);
            }
            //------------------------------------------------------------------------------------------------------

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Created)
            {
                Content = new ObjectContent<object>(new
                {
                    token = newUserToken
                },
                System.Web.Http.GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
            return Task.FromResult(response);
        }
    }
}