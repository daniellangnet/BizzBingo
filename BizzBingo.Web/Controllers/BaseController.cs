using System.Web.Mvc;
using Raven.Client;

namespace BizzBingo.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public new IDocumentSession Session { get; set; }
    }
}
