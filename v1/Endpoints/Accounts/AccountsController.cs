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
        
        #region ACCOUNT ENDPOINTS
        /// <summary>
        /// Retrieves an account information for specific application
        /// </summary>
        /// <param name="idOrHash">Account Primary SID (Guid) or Hash Identifier (Hash Value in JWT)</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("{idOrHash:length(32)}")]
        [HierarchicalRoute("{idOrHash:length(32)}.json")]
        [HierarchicalRoute("{idOrHash:length(36)}")]
        [HierarchicalRoute("{idOrHash:length(36)}.json")]
        public IHttpActionResult GetEntityInformation(string idOrHash)
        {
            //HASH = 32 CHARS
            //GUID = 36 CHARS
            if (idOrHash.Length == 32)
            {
                //FIND AND APPLICATION / USER MATCH
                //MD5({user_token}:{app_token}) -> For queryng a aplication profile
                throw new NotImplementedException();
            }
            else if (idOrHash.Length == 36)
            {
                //FIND AND ACCOUNT (CAN BE APPLICATION OR CAN BE A USER)
                return new Services.GetEntityInformation(idOrHash);
            }

            throw new KeyNotFoundException();
        }

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
#endregion
        #region --> ACCOUNT/ADDRESSES

        /// <summary>
        /// Retrieves the addresses list from the target account
        /// </summary>
        /// <param name="account">Account Token</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("/{account:Guid}/Addresses")]
        [Gale.Security.Oauth.Jwt.Authorize(Scopes = "delivery")]
        public IHttpActionResult GetAccountAddresses(string account)
        {
            return new Services.Delivery.GetAccountAddresses(account);
        }

        /// <summary>
        /// Retrieves the addresses list from the current account
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("/Me/Addresses")]
        [Gale.Security.Oauth.Jwt.Authorize(Scopes = "delivery")]
        public IHttpActionResult GetPersonalAccountAddresses()
        {
            var user = this.User.PrimarySid();
            return GetAccountAddresses(user.ToString());
        }

        /// <summary>
        /// Create a new Address for the target account
        /// </summary>
        /// <param name="account">Account Token</param>
        /// <param name="address">Address Model</param>
        /// <returns></returns>
        [HttpPost]
        [HierarchicalRoute("/{account:Guid}/Addresses")]
        [Gale.Security.Oauth.Jwt.Authorize(Scopes = "delivery")]
        public IHttpActionResult CreateNewAdddressForAccount(string account, Models.Delivery.NewAddress address)
        {
            return new Services.Delivery.CreateAddress(account, address);
        }

        /// <summary>
        /// Create a new Address for the current account
        /// </summary>
        /// <param name="address">Address Model</param>
        /// <returns></returns>
        [HttpPost]
        [HierarchicalRoute("Me/Addresses")]
        [Gale.Security.Oauth.Jwt.Authorize(Scopes = "delivery")]
        public IHttpActionResult CreateNewAdddressForAccount(Models.Delivery.NewAddress address)
        {
            var user = this.User.PrimarySid();
            return new Services.Delivery.CreateAddress(user, address);
        }
        #endregion

        #region ENTITY INFORMATION (CAN BE USER, APPLICATION, ETC)
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

        /// <summary>
        /// Retrieves the avatar image for the current account
        /// </summary>
        /// <param name="s">You may request images anywhere from 1px up to 2048px, however note that many users have lower resolution images, so requesting larger sizes may result in pixelation/low-quality images.</param>
        /// <returns></returns>
        [HttpGet]
        [HierarchicalRoute("Me/Avatar")]
        [HierarchicalRoute("Me/Avatar.jpg")]
        [Gale.Security.Oauth.Jwt.Authorize]
        [WebApi.OutputCache.V2.CacheOutput(ClientTimeSpan = WebApiConfig.AvatarCacheInSeconds, ServerTimeSpan = WebApiConfig.AvatarCacheInSeconds)]
        public IHttpActionResult GetAvatarEntity([FromUri]int s = 100)
        {
            var user = this.User.PrimarySid();
            return GetAvatarEntity(user.ToString(), s);
        }
        #endregion
    }
}