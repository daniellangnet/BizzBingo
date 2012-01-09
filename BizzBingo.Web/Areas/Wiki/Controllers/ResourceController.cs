using System.Configuration;
using System.Web.Routing;
using BizzBingo.Web.Infrastructure;
using BizzBingo.Web.Infrastructure.Services;
using Embedly;
using Embedly.OEmbed;

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

        //ToDo: Validation of CreateResourceModel
        [HttpPost]
        public JsonResult Add(CreateResourceModel model, CurrentUserInformation currentUser)
        {
            Term word = Session.Load<Term>(model.TermId);

            Resource resource = new Resource();
            resource.Id = Guid.NewGuid();
            resource.CreatedOn = DateTime.UtcNow;
            resource.Title = model.Title;
            resource.Description = model.Description;
            resource.Url = model.Url;
            resource.EmbedCode = model.EmbedCode;
            resource.ThumbnailUrl = model.Thumbnail;
            resource.Type = model.Type;
            resource.ViaSource = model.ViaSource;

            if (word.Resources == null) word.Resources = new List<Resource>();
            word.Resources.Add(resource);

            if (currentUser.IsAuthenticated)
            {
                resource.SharedByUserId = currentUser.Id;
                var user = Session.Load<User>(currentUser.Id);
                user.ReputationPoints += ActionPoints.AddInformation;

                if (user.ActionFeed == null) user.ActionFeed = new List<Web.Models.Action>();
                user.ActionFeed.Add(new Web.Models.Action() { Time = DateTime.UtcNow, Type = ActionType.AddInformation, ResourceIdContext = resource.Id, TermIdContext = model.TermId });

                Session.Store(user);
            }

            Session.Store(word);
            Session.SaveChanges();

            // Pingback for blogger
            UrlHelper helper = new UrlHelper(this.ControllerContext.RequestContext);
            var source = helper.Action("Detail", "Term", new {Slug = word.Slug}, "http");
            Pingback.Send(new Uri(source), new Uri(resource.Url));

            return Json(word);
        }
    }
}
