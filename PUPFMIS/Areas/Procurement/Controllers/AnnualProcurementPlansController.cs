using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.Areas.Procurement.Controllers
{
    [Route("{action}")]
    [RouteArea("procurement")]
    [RoutePrefix("annual-procurement-plan")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin + ", " + SystemRoles.ProcurementAdministrator + ", " + SystemRoles.ProcurementPlanningChief + ", " + SystemRoles.BACSECHead)]
    public class AnnualProcurementPlansController : Controller
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
            appDashboard.PPMPsEvaluated = db.APPDetails.Where(d => d.ProjectCoordinator != null).Count();
            return View("Dashboard", appDashboard);
        }

        [ActionName("list")]
        [Route("{FiscalYear}")]
        [Route("{FiscalYear}/list")]
        public ActionResult Index(int FiscalYear)
        {
            return View("Index", appDAL.GetAnnualProcurementPlans(FiscalYear));
        }

        [Route("select-year")]
        [ActionName("select-year")]
        public ActionResult SelectYear()
        {
            ViewBag.FiscalYear = new SelectList(appDAL.GetPPMPFiscalYears());
            return View("CreateSelectYear");
        }

        [HttpPost]
        [Route("select-year")]
        [ActionName("select-year")]
        public ActionResult SelectYear(int FiscalYear)
        {
            ViewBag.FiscalYear = new SelectList(appDAL.GetPPMPFiscalYears());
            return RedirectToAction("create-app", new { FiscalYear = FiscalYear });
        }

        [Route("{FiscalYear}/create")]
        [ActionName("create-app")]
        public ActionResult Create(int FiscalYear)
        {
            var IsCSEAvailable = db.APPHeader.Where(d => d.FiscalYear == FiscalYear && d.APPType == APPTypes.APPCSE).Count() == 0 ? false : true;
            var AnnualProcurementPlan = appDAL.ConsolidateAPP(FiscalYear);
            ViewBag.FiscalYear = FiscalYear;
            ViewBag.ModesOfProcurement = appDAL.GetModesOfProcurement();
            ViewBag.IsCSEAvailable = IsCSEAvailable;
            return View("create", AnnualProcurementPlan);
        }

        [HttpPost]
        [Route("{FiscalYear}/create")]
        [ActionName("create-app")]
        public ActionResult Create(List<APPLineItemVM> APPLineItems, int FiscalYear)
        {
            return Json(new { result = appDAL.PostAPP(APPLineItems, FiscalYear, User.Identity.Name) });
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