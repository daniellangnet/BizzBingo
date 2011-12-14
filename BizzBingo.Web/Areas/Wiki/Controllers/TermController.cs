namespace BizzBingo.Web.Areas.Wiki.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Models;
    using Web.Controllers;
    using Web.Models;

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
        public ViewResult Suckness(string slug)
        {
            Term word = Session.Query<Term>()
                               .Where(x => x.Slug == slug)
                               .SingleOrDefault();

            return View(word);
        }

        [HttpGet]
        public ViewResult Awesomeness(string slug)
        {
            Term word = Session.Query<Term>()
                               .Where(x => x.Slug == slug)
                               .SingleOrDefault();

            return View(word);
        }

        [HttpPost]
        public ActionResult Reaction(CreateReactionModel model)
        {
            Term word = Session.Load<Term>(model.TermId);

            Reaction reaction = new Reaction();
            reaction.Id = Guid.NewGuid();
            reaction.CreatedOn = DateTime.UtcNow;
            reaction.Name = model.Name;
            reaction.Story = model.Story;
            reaction.IsPositive = model.IsPositive;

            if (reaction.IsPositive)
            {
                word.UpVotes = word.UpVotes + 1;
            }
            else
            {
                word.DownVotes = word.DownVotes + 1;
            }

            if (word.Reactions == null) word.Reactions = new List<Reaction>();
            word.Reactions.Add(reaction);

            Session.Store(word);
            Session.SaveChanges();
            return RedirectToAction("Detail", new {slug = word.Slug});
        }

        [HttpGet]
        public ViewResult Like(string slug)
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
            model.Resources = new List<DetailResourceViewModel>();
            if (word.Resources != null)
            {
                foreach (var resource in word.Resources)
                {
                    DetailResourceViewModel viewModelForResource = new DetailResourceViewModel();
                    viewModelForResource.Title = resource.Title;
                    viewModelForResource.Url = resource.Url;
                    viewModelForResource.Description = resource.Description;
                    viewModelForResource.Votes = resource.Upvotes - resource.Downvotes;
                    model.Resources.Add(viewModelForResource);
                }
            }

            return View(model);
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
            model.Resources = new List<DetailResourceViewModel>();
            if (word.Resources != null)
            {
                foreach (var resource in word.Resources)
                {
                    DetailResourceViewModel viewModelForResource = new DetailResourceViewModel();
                    viewModelForResource.Title = resource.Title;
                    viewModelForResource.Url = resource.Url;
                    viewModelForResource.Description = resource.Description;
                    viewModelForResource.Votes = resource.Upvotes - resource.Downvotes;
                    model.Resources.Add(viewModelForResource);
                }
            }

            return View(model);
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
            resource.Type = model.Type;

            if (word.Resources == null) word.Resources = new List<Resource>();
            word.Resources.Add(resource);

            Session.Store(word);
            Session.SaveChanges();
            return Json(word);
        }



    }
}
