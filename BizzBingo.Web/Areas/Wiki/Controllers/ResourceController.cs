namespace BizzBingo.Web.Areas.Wiki.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web.Mvc;
    using System.Xml.Linq;
    using Google.GData.Client;
    using Google.YouTube;
    using Infrastructure.Raven.Indexes;
    using Models;
    using Web.Controllers;
    using Web.Models;

    public class ResourceController : BaseController
    {
        [HttpGet]
        [ChildActionOnly]
        public ViewResult MostRecent()
        {
            var result = Session.Query<LatestResourceIndex.LatestResourceResult, LatestResourceIndex>().Where(x => x.Type != "link").OrderByDescending(x => x.CreatedOn).Take(2).ToList(); ;
            return View(result);
        }

    }
}
