using System.Web.Mvc;

namespace PUPFMIS.Areas.Procurement
{
    public class ProcurementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "procurement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "procurement_default",
                "procurement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "PUPFMIS.Areas.Procurement.Controllers" }
            );
        }
    }
}