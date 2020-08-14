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

namespace PUPFMIS.Areas.Procurement.Controllers
{
    [Route("{action}")]
    [RouteArea("procurement")]
    [RoutePrefix("annual-procurement-plan")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin + ", " + SystemRoles.ProcurementAdministrator + ", " + SystemRoles.ProcurementPlanningChief)]
    public class APPsController : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private AnnualProcurementPlanDAL appDAL = new AnnualProcurementPlanDAL();
        private AnnualProcurementPlanBL appBL = new AnnualProcurementPlanBL();

        [Route("dashboard")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            APPDashboardVM appDashboard = new APPDashboardVM();
            appDashboard.CSEFiscalYears = appDAL.GetCSEFiscalYears();
            appDashboard.APPCSEFiscalYears = appDAL.GetAPPCSEFiscalYears();
            appDashboard.APPFiscalYears = appDAL.GetAPPFiscalYears();
            appDashboard.PPMPsToBeReviewed = db.APPHeader.Count();
            appDashboard.PPMPsEvaluated = db.APPInstitutionalItems.Where(d => d.ProjectCoordinator != null).Count() + db.APPProjectItems.Where(d => d.ProjectCoordinator != null).Count();
            return View("Dashboard", appDashboard);
        }
        
        [ActionName("list")]
        [Route("{FiscalYear}")]
        [Route("{FiscalYear}/list")]
        public ActionResult Index(int FiscalYear)
        {
            return View("Index", appDAL.GetAnnualProcurementPlans(FiscalYear));
        }

        [Route("create")]
        [ActionName("create-app")]
        public ActionResult Create()
        {
            var AnnualProcurementPlan = appDAL.GetAnnualProcurementPlan(appDAL.GetPPMPFiscalYears()[0]);
            ViewBag.FiscalYear = new SelectList(appDAL.GetPPMPFiscalYears());
            return View("create", AnnualProcurementPlan);
        }

        [HttpPost]
        [Route("create")]
        [ActionName("create-app")]
        public ActionResult Create(AnnualProcurementPlanVM APPViewModel)
        {
            if(appDAL.PostAPP(APPViewModel, User.Identity.Name))
            {
                return RedirectToAction("dashboard", "APPs", new { Area = "procurement" });
            }
            ViewBag.FiscalYear = new SelectList(appDAL.GetPPMPFiscalYears());
            return View("create", appDAL.GetAnnualProcurementPlan(appDAL.GetPPMPFiscalYears()[0]));
        }

        [ActionName("print-app")]
        [Route("{ReferenceNo}/print")]
        public ActionResult PrintPPMP(string ReferenceNo)
        {
            var stream = appBL.GenerateAPPReport(ReferenceNo, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            //Response.AddHeader("content-disposition", "attachment; filename=" + ReferenceNo + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("list", new { Area = "procurement" });
        }

        [ActionName("update")]
        [Route("{ReferenceNo}/status-update")]
        public ActionResult UpdateStatus(string ReferenceNo)
        {
            var APPHeader = appDAL.GetAnnualProcurementPlan(ReferenceNo);
            return View("UpdateStatus", APPHeader);
        }

        [HttpPost]
        [ActionName("update")]
        [Route("{ReferenceNo}/status-update")]
        public ActionResult UpdateStatus(AnnualProcurementPlanVM APPViewModel)
        {
            return Json(new { result = appDAL.UpdateAPP(APPViewModel, User.Identity.Name) });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                appDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
