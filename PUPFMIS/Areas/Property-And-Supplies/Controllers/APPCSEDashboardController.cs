//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;
//using PUPFMIS.BusinessAndDataLogic;

//namespace PUPFMIS.Areas.Property_And_Supplies.Controllers
//{
//    [Route("{action}")]
//    [RoutePrefix("app")]
//    [RouteArea("property-and-supplies")]
//    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin + ", " + SystemRoles.PropertyDirector + ", " + 
//        SystemRoles.SuppliesChief + ", " + SystemRoles.ProcurementAdministrator + ", " + SystemRoles.ProcurementPlanningChief)]
//    public class APPCSEDashboardController : Controller
//    {
//        private APPCSEDashboardDAL dashboardDAL = new APPCSEDashboardDAL();

//        [Route("dashboard")]
//        [ActionName("dashboard")]
//        public ActionResult Dashboard()
//        {
//            var dashboard = dashboardDAL.GetAPPCSEDashboard();
//            return View("Dashboard", dashboard);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                dashboardDAL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
