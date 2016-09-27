using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace API.Endpoints.Applications
{
    /// <summary>
    /// Application Controller
    /// </summary>
    public class ApplicationsController : Gale.REST.RestController
    {
        /// <summary>
        /// Register an Application to enable access for securified endpoints (JWT Access)
        /// </summary>
        /// <param name="application">Register Model</param>
        /// <returns></returns>
        [HttpPost]
        [Swashbuckle.Swagger.Annotations.SwaggerResponseRemoveDefaults]
        [Swashbuckle.Swagger.Annotations.SwaggerResponse(HttpStatusCode.NoContent)]
        [Swashbuckle.Swagger.Annotations.SwaggerResponse(HttpStatusCode.BadRequest)]
        //[Gale.Security.Oauth.Jwt.Authorize(Roles = WebApiConfig.RootRoles)]
        public IHttpActionResult Post(Models.NewAplication application)
        {
            string executor = System.Guid.NewGuid().ToString();  //this.User.PrimarySid();
            return new Services.Create(executor, application);
        }
    }
}