using System.Web.Mvc;

namespace PUPFMIS.Areas.ResponsibilityCenters
{
    public class ResponsibilityCentersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "responsibility-centers";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "responsibility-centers_default",
                "responsibility-centers/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "PUPFMIS.Areas.ResponsibilityCenters.Controllers" }
            );
        }
    }
}