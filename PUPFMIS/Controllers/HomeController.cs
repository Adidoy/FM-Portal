using System.Web.Mvc;
using PUPFMIS.Models;

namespace PUPFMIS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["BidRegistrationOption"] = BidRegistrationOptions.WinningBidsOnly;
            if (User.IsInRole(SystemRoles.ResponsibilityCenterPlanner))
            {
                return RedirectToAction("dashboard", "StrategicPlanning", new { Area = "responsibility-centers" });
            }
            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}