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
                "Definition",
                "what-is/{slug}",
                new { controller = "Term", action = "Detail" }
            );

            context.MapRoute(
                "Awesome",
                "why/{slug}/is-awesome",
                new { controller = "Term", action = "Awesomeness" }
            );

            context.MapRoute(
                "Suck",
                "why/{slug}/sucks",
                new { controller = "Term", action = "Suckness" }
            );

            context.MapRoute(
                "Wiki_default",
                "Wiki/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
