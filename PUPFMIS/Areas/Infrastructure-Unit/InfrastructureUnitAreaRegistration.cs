using System.Web.Mvc;

namespace PUPFMIS.Areas.InfrastructureUnit
{
    public class InfrastructureUnitAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "infrastructure-unit";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "infrastructure-unit_default",
                "infrastructure-unit/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}