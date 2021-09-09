using System.Web.Mvc;

namespace PUPFMIS.Areas.Suppliers
{
    public class SuppliersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "suppliers";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "suppliers_default",
                "suppliers/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}