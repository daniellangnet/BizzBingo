using System;
using System.Linq;
using System.Web.Mvc;
using BizzBingo.Web.Models;
using BizzBingo.Web.Models.Home;

namespace BizzBingo.Web.Controllers
{
    using Helper;

    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            model.Top = Session.Query<Word>().OrderByDescending(x => x.UpVotes).Take(5).ToList();
            model.Newest = Session.Query<Word>().OrderByDescending(x => x.CreatedOn).Take(5).ToList(); ;
            return View(model);
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Share(Word word)
        {
            if (string.IsNullOrWhiteSpace(word.Title) || string.IsNullOrWhiteSpace(word.Description))
                return Json(true);

            word.Id = Guid.NewGuid();
            word.CreatedOn = DateTime.Now;
            word.LcId = "1033";
            word.Slug = SearchForSlug(word.Title);
            Session.Store(word);
            Session.SaveChanges();
            return Json(true);
        }

        private string SearchForSlug(string title)
        {
            string slug = title.ToSlug();
            if (IsSlugAlreadyTaken(slug))
            {
                string newSlug = slug + "-I";
                SearchForSlug(newSlug);
            }

            return slug;
        }

        private bool IsSlugAlreadyTaken(string slug)
        {
            var result = Session.Query<Word>().Where(x => x.Slug == slug).SingleOrDefault();
            if (result == null) return false;

            return true;
        }
    }
}
