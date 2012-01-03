using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBingo.Web.Infrastructure.Raven;
using BizzBingo.Web.Models;

namespace BizzBingo.Web.Infrastructure.Binder
{
    public class CurrentUserBinder : IModelBinder<CurrentUser>
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext");
            }

            if (HttpContext.Current.Session[SessionKeys.CurrentUser] is User)
            {
                return HttpContext.Current.Session[SessionKeys.CurrentUser];
            }

            using (var session = DocumentStoreHolder.DocumentStore.OpenSession())
            {
                var result = session.Load<User>().Where(x => x.Name == HttpContext.Current.User.Identity.Name).SingleOrDefault();
                HttpContext.Current.Session.Add(SessionKeys.CurrentUser, result);
                return result;
            }
        }
    }
}