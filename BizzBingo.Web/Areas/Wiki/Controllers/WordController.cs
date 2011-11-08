namespace BizzBingo.Web.Areas.Wiki.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Models;
    using Web.Controllers;

    public class WordController : BaseController
    {
        [HttpGet]
        public JsonResult Top()
        {
            var result = Session.Query<Word>().OrderBy(x => x.UpVotes).Take(5).ToList();
            return Json(result);
        }

        [HttpGet]
        public JsonResult Latest()
        {
            var result = Session.Query<Word>().OrderByDescending(x => x.CreatedOn).Take(5).ToList();
            return Json(result);
        }

        [HttpGet]
        public ViewResult Detail(string slug)
        {
            Word word = Session.Query<Word>()
                               .Where(x => x.Slug == slug)
                               .SingleOrDefault();
            word.Views = word.Views + 1;
            Session.Store(word);
            return View(word);
        }

        public ViewResult Archive()
        {
            return View();
        }

        [HttpPut]
        public JsonResult Upvote(Guid id)
        {
            Word word = Session.Load<Word>(id);
            word.UpVotes = word.UpVotes + 1;
            Session.Store(word);
            Session.SaveChanges();
            return Json(word);
        }

        [HttpPut]
        public JsonResult Downvote(Guid id)
        {
            Word word = Session.Load<Word>(id);
            word.DownVotes = word.DownVotes + 1;
            Session.Store(word);
            Session.SaveChanges();
            return Json(word);
        }

    }
}
