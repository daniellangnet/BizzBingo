using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBingo.Web.Controllers
{
    using System.Web.Security;
    using DotNetOpenAuth.OAuth;
    using DotNetOpenAuth.OAuth.Messages;
    using Infrastructure.DotNetOAuth;

    public class AuthController : Controller
    {
        public ActionResult Index()
        {
            if (TwitterConsumer.IsTwitterConsumerConfigured)
            {
                string screenName;
                int userId;
                if (TwitterConsumer.TryFinishSignInWithTwitter(out screenName, out userId))
                {
                    FormsAuthentication.SetAuthCookie(screenName, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TwitterConsumer.StartSignInWithTwitter(false).Send();
                }
            }

            return new HttpStatusCodeResult(500);
        }

    }
}
