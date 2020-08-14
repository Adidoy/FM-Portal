using System.Web.Mvc;
using System.Web.Routing;

namespace PUPFMIS.Areas.Budget
{
    public class BudgetAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "budget";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "budget_default",
                "budget/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "PUPFMIS.Areas.Budget.Controllers" }
            );
        }
    }
}