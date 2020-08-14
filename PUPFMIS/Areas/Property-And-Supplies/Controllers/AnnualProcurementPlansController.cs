using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.Property.Controllers
{
    [Route("{action}")]
    [RoutePrefix("app/cse")]
    [RouteArea("property-and-supplies")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin + ", " + SystemRoles.PropertyDirector + ", " + SystemRoles.SuppliesChief + ", " + SystemRoles.ProcurementAdministrator + ", " + SystemRoles.ProcurementPlanningChief)]
    public class AnnualProcurementPlansController : Controller
    {
        private AnnualProcurementPlanCSEDAL appcseDAL = new AnnualProcurementPlanCSEDAL();
        private AnnualProcurementPlanCSEBL appcseBL = new AnnualProcurementPlanCSEBL();

        [ActionName("create-cse")]
        [Route("{FiscalYear}/create")]
        public ActionResult CreateCSE(int FiscalYear)
        {
            AnnualProcurementPlanCSEVM appcse = appcseDAL.GetAPPCSE(FiscalYear);
            return View("CreateCSE", appcse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("post-app-cse")]
        [Route("{FiscalYear}/create")]
        public ActionResult CreateCSE(AnnualProcurementPlanCSEVM APPCSEViewModel)
        {
            if(appcseDAL.PostAPPCSE(APPCSEViewModel, User.Identity.Name))
            {
                if(User.IsInRole(SystemRoles.ProcurementAdministrator) || User.IsInRole(SystemRoles.ProcurementPlanningChief))
                {
                    return RedirectToAction("dashboard", "APPs", new { Area = "procurement" });
                }
                if (User.IsInRole(SystemRoles.PropertyDirector) || User.IsInRole(SystemRoles.SuppliesChief))
                {
                    return RedirectToAction("dashboard", "APPCSEDashboard", new { Area = "property-and-supplies" });
                }
                else
                {
                    return RedirectToAction("dashboard", "APPs", new { Area = "procurement" });
                }
            }
            return RedirectToAction("dashboard", "APPCSEDashboard", new { Area = "property-and-supplies" });
        }

        [ActionName("print-app-cse")]
        [Route("{ReferenceNo}/print")]
        public ActionResult PrintAPPCSE(string ReferenceNo)
        {
            var stream = appcseBL.GenerateAPPCSE(ReferenceNo, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            //Response.AddHeader("content-disposition", "attachment; filename=" + ReferenceNo + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("list", new { Area = "end-users" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                appcseDAL.Dispose();
                appcseBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
