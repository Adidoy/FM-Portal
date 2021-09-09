using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.Property_And_Supplies.Controllers
{
    [Route("{action}")]
    [RouteArea("property-and-supplies")]
    [RoutePrefix("inspection-and-accpetance")]
    [Authorize(Roles = SystemRoles.InspectionChief + ", " + SystemRoles.InspectionOfficer)]
    public class InspectionAndAcceptanceController : Controller
    {
        private InspectionDAL inspectionDAL = new InspectionDAL();

        [Route("")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            var inspectionDashboard = new InspectionDashboard();
            inspectionDashboard.SupplyDeliveriesForInspection = inspectionDAL.GetSupplyDeliveriesList();
            inspectionDashboard.PPEDeliveriesForInspection = inspectionDAL.GetPPEDeliveriesList();
            inspectionDashboard.ReportYears = inspectionDAL.GetReportYears();
            return View("Dashboard", inspectionDashboard);
        }

        [Route("reports/{ReportYear}")]
        [ActionName("index")]
        public ActionResult Index(int ReportYear)
        {
            var inspectionList = inspectionDAL.GetInspectionList(ReportYear);
            return View("Index", inspectionList);
        }

        [Route("supplies/{DRRNumber}/details")]
        [ActionName("details-supplies")]
        public ActionResult DetailsSupplies(string DRRNumber)
        {
            var inspection = inspectionDAL.GetInspectionSuppliesSetup(DRRNumber);
            ViewBag.InspectedBy = new SelectList(inspectionDAL.GetInspectorPool(), "EmployeeCode", "EmployeeName");
            return View("DetailsSupplies", inspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("post-inspection-supplies")]
        public ActionResult PostSuppliesInspection(InspectionSuppliesDeliveredVM Inspection)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = inspectionDAL.PostSupplyInspection(Inspection, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return PartialView("_DetailsSupplies", Inspection);
        }

        [Route("{ReferenceNo}/print")]
        [ActionName("print-iar")]
        public ActionResult PrintIAR(string ReferenceNo)
        {
            var inspectionBL = new InspectionBL();
            var stream = inspectionBL.GenerateSuppliesInspectionAndAcceptanceReport(ReferenceNo, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=Inspection and Acceptance Report - " + ReferenceNo + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                inspectionDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}