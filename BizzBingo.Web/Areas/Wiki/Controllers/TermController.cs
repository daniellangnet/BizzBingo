using BizzBingo.Web.Infrastructure;
using BizzBingo.Web.Infrastructure.Raven.Indexes;
using Raven.Client.Linq;

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
        public ActionResult Reaction(CreateReactionModel model, CurrentUserInformation currentUser)
        {
            Term word = Session.Load<Term>(model.TermId);

            Reaction reaction = new Reaction();
            reaction.Id = Guid.NewGuid();
            reaction.CreatedOn = DateTime.UtcNow;
            reaction.Name = model.Name;
            reaction.Reason = model.Reason;
            reaction.Title = model.Title;
            reaction.IsPositive = model.IsPositive;

            ActionType forUserFeed = ActionType.AddGoodReaction;

            if (reaction.IsPositive)
            {
                word.UpVotes = word.UpVotes + 1;
            }
            else
            {
                word.DownVotes = word.DownVotes + 1;
                forUserFeed = ActionType.AddBadReaction;
            }

            if (word.Reactions == null) word.Reactions = new List<Reaction>();
            word.Reactions.Add(reaction);

            if (currentUser.IsAuthenticated)
            {
                reaction.Name = currentUser.Name;
                reaction.SharedByUserId = currentUser.Id;
                var user = Session.Load<User>(currentUser.Id);
                user.ReputationPoints += ActionPoints.AddReaction;

                if (user.ActionFeed == null) user.ActionFeed = new List<Web.Models.Action>();
                user.ActionFeed.Add(new Web.Models.Action() { Time = DateTime.UtcNow, Type = forUserFeed, ReactionIdContext = reaction.Id, TermIdContext = model.TermId });

                Session.Store(user);
            }

            Session.Store(word);
            Session.SaveChanges();
            return RedirectToAction("Detail", new { slug = word.Slug });
        }

        [HttpGet]
        public ViewResult Detail(string slug, CurrentUserInformation currentUser)
        {
            //ToDo: Number of Request 2 - not 1. Include doesn´t work.
            Term word = Session.Query<Term>().Where(x => x.Slug == slug)
                                             .SingleOrDefault();

            word.Views = word.Views + 1;
            Session.Store(word);

            DetailTermViewModel model = new DetailTermViewModel();
            if (word.Supporters != null)
            {
                model.Supporters = Session.Query<User>().Where(x => x.Id.In(word.Supporters)).ToList();
            }
            else
            {
                model.Supporters = new List<User>();
            }

            model.Id = word.Id;
            model.Views = word.Views;
            model.Title = word.Title;
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
            if (word.Reactions != null)
            {
                model.PositiveReaction = word.Reactions.Where(x => x.IsPositive == true).ToList();
                model.NegativeReaction = word.Reactions.Where(x => x.IsPositive == false).ToList();
            }
            else
            {
                model.PositiveReaction = new List<Reaction>();
                model.NegativeReaction = new List<Reaction>();
            }

            model.CurrentUserInformation = currentUser;

            return View(model);
        }

        [HttpPost]
        public JsonResult Supporter(Guid termId, CurrentUserInformation currentUser)
        {
            Term word = Session.Load<Term>(termId);

            if(word.Supporters == null) word.Supporters = new List<Guid>();

            word.Supporters.Add(currentUser.Id);

            Session.Store(word);

            if (currentUser.IsAuthenticated)
            {
                var user = Session.Load<User>(currentUser.Id);
                user.ReputationPoints += ActionPoints.AddSupport;

                if (user.ActionFeed == null) user.ActionFeed = new List<Web.Models.Action>();
                user.ActionFeed.Add(new Web.Models.Action() { Time = DateTime.UtcNow, Type = ActionType.AddSupport, TermIdContext = termId });

                Session.Store(user);
            }
            
            Session.SaveChanges();

            return Json(word);
        }

        [HttpPost]
        public JsonResult Resource(CreateResourceModel model, CurrentUserInformation currentUser)
        {
            Term word = Session.Load<Term>(model.TermId);

            Resource resource = new Resource();
            resource.Id = Guid.NewGuid();
            resource.CreatedOn = DateTime.UtcNow;
            resource.Title = model.Title;
            resource.Description = model.Description;
            resource.Url = model.Url;
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
            return Json(word);
        }

        public ViewResult Search(string searchTerm)
        {
            SearchResultModel result = new SearchResultModel();
            result.SearchTerm = searchTerm;
            result.Results = Session.Query<Term, SearchIndex>().Where(x => x.Title.StartsWith(searchTerm)).ToList();

            return View(result);
        }
    }
}
