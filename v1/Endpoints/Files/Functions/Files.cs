using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Endpoints.Files.Functions
{
    /// <summary>
    /// Files Function's
    /// </summary>
    public class Files
    {
        /// <summary>
        /// Retrieve the File Data Saved in the Database
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Models.FileData Get(String token)
        {
            var db = Gale.Db.Factories.FactoryResolver.Resolve();

            //------------------------------------------------------------------------------------------------------
            // DB Execution
            using (Gale.Db.DataService svc = new Gale.Db.DataService("PA_MAE_OBT_ArchivoBinario"))
            {
                svc.Parameters.Add("ARCH_Token", token);

                Gale.Db.EntityRepository rep = db.ExecuteQuery(svc);

                Models.FileData file = rep.GetModel<Models.FileData>().FirstOrDefault();
                
                return file;
            }
            //------------------------------------------------------------------------------------------------------

        }

    }
}