using System.Web.Mvc;

namespace PUPFMIS.Areas.Property
{
    public class PropertyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "property-and-supplies";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "property-and-supplies_default",
                "property-and-supplies/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}