using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BizzBingo.Web.Infrastructure.DotNetOAuth;
using BizzBingo.Web.Models;
using Raven.Client.Linq;

namespace BizzBingo.Web.Controllers
{
    
    public class AuthController : BaseController
    {
        public ActionResult Index()
        {
            if (TwitterConsumer.IsTwitterConsumerConfigured)
            {
                string screenName;
                int userId;
                string accessToken;
                if (TwitterConsumer.TryFinishSignInWithTwitter(out screenName, out userId, out accessToken))
                {
                    var user = Session.Query<User>().Search(x => x.TwitterId, userId.ToString()).SingleOrDefault();

                    if (user == null)
                    {
                        user = new User();
                        user.Id = Guid.NewGuid();
                        user.TwitterId = userId.ToString();
                        user.RegisteredOn = DateTime.UtcNow;
                    }

                    user.Name = screenName;
                    user.OAuthAccessToken = accessToken;
                    user.LastSignIn = DateTime.UtcNow;

                    Session.Store(user);
                    Session.SaveChanges();

                    FormsAuthentication.SetAuthCookie(screenName, false);

                    if (HttpContext.Request.UrlReferrer != null && string.IsNullOrWhiteSpace(HttpContext.Request.UrlReferrer.AbsoluteUri) == false)
                        return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TwitterConsumer.StartSignInWithTwitter(false).Send();
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
