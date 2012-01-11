using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BizzBingo.Web.Infrastructure;
using BizzBingo.Web.Infrastructure.Raven.Indexes;
using BizzBingo.Web.Models;
using BizzBingo.Web.Models.Home;
using Action = BizzBingo.Web.Models.Action;

namespace BizzBingo.Web.Controllers
{

    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Profile(CurrentUserInformation currentUser)
        {
            var user = Session.Load<User>(currentUser.Id);

            //var result = Session.Query<UserActivityFeedIndex.UserFeedResult, UserActivityFeedIndex>().Where(x => x.Name == currentUser.Name).ToList(); ;

            return View(user);
        }
    }
}
