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
                    // In a real app, the Twitter username would likely be used
                    // to log the user into the application.
                    ////FormsAuthentication.RedirectFromLoginPage(screenName, false);
                }
                else
                {
                    TwitterConsumer.StartSignInWithTwitter(false).Send();
                }
            }

            return new EmptyResult();
        }


        private static HttpCookie CreateAuthCookie(String username, String token, Boolean persistent)
        {

            // Let ASP.NET create a regular auth cookie 

            var cookie = FormsAuthentication.GetAuthCookie(username, persistent);



            // Modify the cookie to add access-token data

            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate,

                                                          ticket.Expiration, ticket.IsPersistent, token);

            cookie.Value = FormsAuthentication.Encrypt(newTicket);

            return cookie;

        }

    }
}
