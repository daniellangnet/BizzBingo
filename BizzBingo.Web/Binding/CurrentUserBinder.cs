using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBingo.Web.Infrastructure;
using BizzBingo.Web.Infrastructure.Raven;
using BizzBingo.Web.Models;
using Raven.Client.Linq;

namespace BizzBingo.Web.Binding
{
    public class CurrentUserBinder : IModelBinder<CurrentUserInformation>
    {
        /// <summary>
        /// BindModel Methode.
        /// </summary>
        /// <param name="controllerContext">
        /// Controllercontext for modelbinding.
        /// </param>
        /// <param name="bindingContext">
        /// Bindingcontext for modelbinding.
        /// </param>
        /// <returns>
        /// Gibt ein ActingUserInformation Objekt zurück. IsAuthenticated gibt an, ob dieser authentifiziert ist oder nicht.
        /// </returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            const string currentUserKey = "CurrentUser";

            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            if(HttpContext.Current.User.Identity.IsAuthenticated == false)
                return new CurrentUserInformation() { IsAuthenticated = false };

            if(HttpContext.Current.Session[currentUserKey] == null)
            {
                using(var session = DocumentStoreHolder.DocumentStore.OpenSession())
                {
                    var user = session.Query<User>().Search(x => x.Name, HttpContext.Current.User.Identity.Name).SingleOrDefault();

                    CurrentUserInformation currentUser = new CurrentUserInformation();
                    currentUser.Id = user.Id;
                    currentUser.LastSignIn = user.LastSignIn;
                    currentUser.Name = user.Name;
                    currentUser.RegisteredOn = user.RegisteredOn;
                    currentUser.TwitterId = user.TwitterId;
                    currentUser.IsAuthenticated = true;

                    HttpContext.Current.Session[currentUserKey] = currentUser;
                    return currentUser;
                }
            }

            return HttpContext.Current.Session[currentUserKey] as CurrentUserInformation;
        }
    }
}