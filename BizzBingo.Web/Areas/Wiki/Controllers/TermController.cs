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
    using Models;
    using Web.Controllers;
    using Web.Models;
    using Type = Web.Models.Type;

    public class TermController : BaseController
    {
        [HttpGet]
        public JsonResult Top()
        {
            var result = Session.Query<Term>().OrderBy(x => x.UpVotes).Take(5).ToList();
            return Json(result);
        }

        [HttpGet]
        public JsonResult Latest()
        {
            var result = Session.Query<Term>().OrderByDescending(x => x.CreatedOn).Take(5).ToList();
            return Json(result);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Detail(string slug)
        {
            Term word = Session.Query<Term>()
                               .Where(x => x.Slug == slug)
                               .SingleOrDefault();
            word.Views = word.Views + 1;
            Session.Store(word);

            DetailTermViewModel model = new DetailTermViewModel();
            model.Id = word.Id;
            model.Views = word.Views;
            model.Title = word.Title;
            model.Description = word.Description;
            model.DownVotes = word.DownVotes;
            model.UpVotes = word.UpVotes;
            model.Slug = word.Slug;
            model.CreatedOn = word.CreatedOn.ToShortDateString();
            if (word.Resources == null) model.Resources = new List<Resource>();
            else model.Resources = word.Resources;

            return View(model);
        }

        [HttpPut]
        public JsonResult Upvote(Guid id)
        {
            Term word = Session.Load<Term>(id);
            word.UpVotes = word.UpVotes + 1;
            Session.Store(word);
            Session.SaveChanges();
            return Json(word);
        }

        [HttpGet]
        public JsonResult Lookup(string url)
        {
            if (url.ToLower().StartsWith("http://www.youtube.com/watch?v="))
            {
                Uri youTubeLink = new Uri(url);
                var parameters = System.Web.HttpUtility.ParseQueryString(youTubeLink.Query);
                var video = parameters["v"];

                
                if (string.IsNullOrWhiteSpace(video)) return Json(false);

                Uri youTubeApi = new Uri(string.Format("http://gdata.youtube.com/feeds/api/videos/{0}", video));
                YouTubeRequestSettings settings = new YouTubeRequestSettings(null, null);

                Google.YouTube.YouTubeRequest request = new YouTubeRequest(settings);
                var result = request.Retrieve<Video>(youTubeApi);

                Resource suggestion = new Resource();
                suggestion.Title = result.Title;
                suggestion.Description = result.Description;

                return Json(suggestion, JsonRequestBehavior.AllowGet);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult Resource(CreateResourceModel model)
        {
            Term word = Session.Load<Term>(model.TermId);

            Resource resource = new Resource();
            resource.Id = Guid.NewGuid();
            resource.CreatedOn = DateTime.UtcNow;
            resource.Title = model.Title;
            resource.Description = model.Description;
            resource.Url = model.Url;

            if (resource.Url.ToLower().StartsWith("http://www.youtube.com/watch?v="))
            {
                resource.Provider = Provider.YouTube;
                resource.Type = Type.Video;
            }
            else if (resource.Url.ToLower().StartsWith("http://speakerdeck.com/u/"))
            {
                resource.Provider = Provider.Speakersdeck;
                resource.Type = Type.Presentation;
            }

            if (word.Resources == null) word.Resources = new List<Resource>();
            word.Resources.Add(resource);

            Session.Store(word);
            Session.SaveChanges();
            return Json(word);
        }

        [HttpPut]
        public JsonResult Downvote(Guid id)
        {
            Term word = Session.Load<Term>(id);
            word.DownVotes = word.DownVotes + 1;
            Session.Store(word);
            Session.SaveChanges();
            return Json(word);
        }

    }
}
