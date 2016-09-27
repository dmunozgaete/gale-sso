using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using API.Endpoints.Oauth2.Resources;

namespace API.Endpoints.Oauth2.Services.Auth
{
    /// <summary>
    /// Approve / Modify the scope access, and success the flow process
    /// </summary>
    public class ApproveScopes : Gale.REST.Http.HttpBaseActionResult
    {
        HttpRequestMessage _request;
        int _version;
        System.Collections.Specialized.NameValueCollection _parameters;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="request">Request Message</param>
        /// <param name="version">Oauth2 assets version</param>
        /// <param name="parameters">Parameters Collections</param>
        public ApproveScopes(HttpRequestMessage request, int version, System.Collections.Specialized.NameValueCollection parameters)
        {
            _version = version;
            _request = request;
            _parameters = parameters;
        }

        /// <summary>
        /// Async Process
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            var scopes = _parameters[RFC6749Names.SCOPE].Split(',');
            String user = _parameters[RFC6749Names.USER_TOKEN];
            String app = _parameters[RFC6749Names.APP_TOKEN];

            //IF the hash has two values , it's for request the user profile in the application
            String hash = Gale.Security.Cryptography.MD5.GenerateHash(String.Format("{0}:{1}", user, app));

            //-----------------------------------------------------------------------------
            // Update / Approve the scopes for the application/user ^^
            using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_OAUT_ACT_AccesoAplicacionCuenta"))
            {
                svc.Parameters.Add("APPL_Token", app);
                svc.Parameters.Add("USUA_Token", user);
                svc.Parameters.Add("ENAP_Hash", hash);

                //Already validate, so create a scope table and update =)!
                var t_scopes = new List<Models.Auth.T_Scope>();
                foreach (var scope in scopes)
                {
                    t_scopes.Add(new Models.Auth.T_Scope()
                    {
                        scope = scope
                    });
                }
                svc.AddTableType<Models.Auth.T_Scope>("Accesos", t_scopes);

                this.ExecuteAction(svc);
            }

            //-----------------------------------------------------------------------------
            //Finally =)!, Finalize the flow OAuth
            var success = new Services.Auth.SuccessAuth(_request, _version, _parameters);
            return success.ExecuteAsync(cancellationToken);
            //-----------------------------------------------------------------------------
        }


    }
}