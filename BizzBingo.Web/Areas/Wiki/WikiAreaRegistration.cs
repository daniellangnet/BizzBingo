using System.Web.Mvc;

namespace BizzBingo.Web.Areas.Wiki
{
    public class WikiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Wiki";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Wiki_Word_SEO",
                "Wiki/{slug}",
                new { controller = "Word", action = "Detail" }
            );

            context.MapRoute(
                "Wiki_default",
                "Wiki/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
