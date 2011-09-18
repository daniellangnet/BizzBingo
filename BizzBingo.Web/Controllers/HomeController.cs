using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBingo.Web.Models;

namespace BizzBingo.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var words = Session.Query<Word>().Take(16);

            foreach (var word in words)
            {
                ViewBag.Message += "," + word.Value;
            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
