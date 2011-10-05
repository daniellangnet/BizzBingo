using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBingo.Web.Models;
using BizzBingo.Web.Models.Home;

namespace BizzBingo.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            var words = Session.Query<Word>().ToList();
            model.Top = words;
            model.Newest = words;
            return View(model);
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Detail(Guid id)
        {
            Word word = Session.Load<Word>(id);
            return View(word);
        }

        public ActionResult Upvote(Guid id)
        {
            Word word = Session.Load<Word>(id);
            word.UpVotes = word.UpVotes + 1;
            Session.Store(word);
            Session.SaveChanges();
            return RedirectToAction("Detail", new {id = id});
        }

        public ActionResult Downvote(Guid id)
        {

            Word word = Session.Load<Word>(id);
            word.DownVotes = word.DownVotes + 1;
            Session.Store(word);
            Session.SaveChanges();
            return RedirectToAction("Detail", new { id = id });
        }

        public ActionResult Share(Word word)
        {
            if (string.IsNullOrWhiteSpace(word.Value) || string.IsNullOrWhiteSpace(word.Description))
                return Json(true);

            word.Id = Guid.NewGuid();
            word.CreatedOn = DateTime.Now;
            word.LcId = "1033";
            Session.Store(word);
            Session.SaveChanges();
            return Json(true);
        }
    }
}
