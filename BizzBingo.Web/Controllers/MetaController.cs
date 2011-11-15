using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBingo.Web.Controllers
{
    public class MetaController : Controller
    {
        /// <summary>
        /// Some scripts should not be rendered if we are in development mode (e.g. analytics) . To check if we are in "dev-mode", we check the host value of the request. 
        /// </summary>
        /// <param name="preventFromRenderingInDevelopment">If host == localhost and this value is true - emptyresult</param>
        /// <param name="name">name of the script</param>
        /// <returns>EmptyResult or ViewResult</returns>
        public ActionResult Render3rdPartyScripts(bool preventFromRenderingInDevelopment, string name)
        {
            string prefix = "3rdPartyScripts_";

            if (preventFromRenderingInDevelopment)
            {
                if (this.Request.Url.Host == "localhost")
                {
                    return new EmptyResult();
                }
            }

            string script = prefix + name;
            return View(script);
        }

    }
}
