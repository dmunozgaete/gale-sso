﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Description;
using System.Web;

namespace API.Endpoints.Files
{

    /// <summary>
    /// File API
    /// </summary>
    public class FilesController : Gale.REST.RestController
    {

        /// <summary>
        /// Retrieves a File Content
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public IHttpActionResult Get(string id)
        {
            return new Services.View(id);
        }


        /// <summary>
        /// Create a Temporary File  (Must be Change the flag to permanently after)
        /// </summary>
        /// <returns></returns>
        //[Gale.Security.Oauth.Jwt.Authorize]
        public IHttpActionResult Post()
        {
            var executor = System.Guid.NewGuid().ToString(); //User.PrimarySid();
            return new Services.Upload(this.Request, executor);
        }



    }

}