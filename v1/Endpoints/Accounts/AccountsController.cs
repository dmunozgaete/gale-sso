using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace API.Endpoints.Accounts
{
    /// <summary>
    /// Accounts Controller
    /// </summary>
    [ApplicationInsights.TrackExceptionFilter]
    public class AccountsController : Gale.REST.RestController
    {
        #region ACCOUNT APPLICATION INFORMATION
        /// <summary>
        /// Retrieves an account information for specific application
        /// </summary>
        /// <param name="hash">Hash Identifier (Hash Value in JWT)</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("{hash:length(32)}")]
        [HierarchicalRoute("{hash:length(32)}.json")]
        public IHttpActionResult GetByHash(string hash)
        {
            //MD5({user_token}:{app_token}) -> For queryng a aplication profile

            return null;
        }
        #endregion

        #region ENTITY INFORMATION (CAN BE USER, APPLICATION, ETC)
        /// <summary>
        /// Retrieves the personal information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("Me")]
        [Gale.Security.Oauth.Jwt.Authorize]
        public IHttpActionResult GetPersonalEntityInformation()
        {
            var account = this.User.PrimarySid();
            return GetEntityInformation(account);
        }

        /// <summary>
        /// Retrieves an account information
        /// </summary>
        /// <param name="account">Entity Guid (Primary Sid)</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("{account:Guid}")]
        [HierarchicalRoute("{account:Guid}.json")]
        public IHttpActionResult GetEntityInformation(string account)
        {
            return new Services.GetEntityInformation(account);
        }

        /// <summary>
        /// Retrieves the avatar image for an account
        /// </summary>
        /// <param name="account">Entity Guid (Primary Sid)</param>
        /// <param name="s">You may request images anywhere from 1px up to 2048px, however note that many users have lower resolution images, so requesting larger sizes may result in pixelation/low-quality images.</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("{account:Guid}/Avatar")]
        [HierarchicalRoute("{account:Guid}.jpg")]
        [WebApi.OutputCache.V2.CacheOutput(ClientTimeSpan = WebApiConfig.AvatarCacheInSeconds, ServerTimeSpan = WebApiConfig.AvatarCacheInSeconds)]
        public IHttpActionResult GetAvatarEntity(string account, [FromUri]int s = 100)
        {
            return new Services.GetAccountAvatar(account, s);
        }
        #endregion
    }
}