using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.Procurement.Controllers
{
    [Route("{action}")]
    [RouteArea("procurement")]
    [RoutePrefix("my-projects")]
    [Authorize(Roles = SystemRoles.ProjectCoordinator)]
    public class MyProjectsController : Controller
    {
        MyProjectsDAL myProjectsDAL = new MyProjectsDAL();
        MyProjectsBL myProjectsBL = new MyProjectsBL();
        SuppliersBL supplier = new SuppliersBL();

        [Route("")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            MyProjectsDashboard dashboard = new MyProjectsDashboard();
            dashboard.ProjectsWithSetTimeLine = myProjectsDAL.GetProcurementProgramDetails(User.Identity.Name);
            //dashboard.NoOfProjectsWithSetTimeLine = myProjectsDAL.GetProjectsWithSetTimeLine(User.Identity.Name).Count();
            //dashboard.ProjectsWithoutSetTimeLine = myProjectsDAL.GetProjectsWithoutSetTimeLine(User.Identity.Name);
            //dashboard.NoOfProjectsWithoutSetTimeLine = myProjectsDAL.GetProjectsWithoutSetTimeLine(User.Identity.Name).Count();
            //dashboard.ProjectsWithoutSetTimeLine = myProjectsDAL.get
            dashboard.TotalProjectsAssignedToMe = myProjectsDAL.GetTotalProjectsAssigned(User.Identity.Name);
            dashboard.TotalProjectsCoordinated = myProjectsDAL.GetTotalProjectsCoordinated(User.Identity.Name);
            dashboard.TotalProjectsSupported = myProjectsDAL.GetTotalProjectsupported(User.Identity.Name);
            return View(dashboard);
        }

        [ActionName("details")]
        [Route("{PAPCode}/details")]
        public ActionResult Details(string PAPCode)
        {
            if(PAPCode == null)
            {
                return HttpNotFound();
            }
            var procurementPrograms = myProjectsDAL.GetProcurementProgramDetailsByPAPCode(PAPCode);
            ViewBag.Supplier = procurementPrograms.Supplier == null ? new SelectList(myProjectsDAL.GetSuppliers(), "ID", "SupplierName") : new SelectList(myProjectsDAL.GetSuppliers(), "ID", "SupplierName", procurementPrograms.Supplier);
            ViewBag.ModeOfProcurement = procurementPrograms.ModeOfProcurement == null ? new SelectList(myProjectsDAL.GetModesOfPrpcurement(procurementPrograms.APPModeOfProcurement), "ID", "ModeOfProcurementName") : new SelectList(myProjectsDAL.GetModesOfPrpcurement(procurementPrograms.APPModeOfProcurement), "ID", "ModeOfProcurementName", procurementPrograms.ModeOfProcurement);
            if(procurementPrograms == null)
            {
                return HttpNotFound();
            }
            return View(procurementPrograms);
        }

        [HttpPost]
        [ActionName("open-pr-submission")]
        [Route("purchase-request/open-submission")]
        public ActionResult OpenSubmission(string PAPCode, int ModeOfProcurement)
        {
            return Json(new { result = myProjectsDAL.OpenPRSubmission(PAPCode, ModeOfProcurement) });
        }

        [HttpPost]
        [ActionName("close-pr-submission")]
        [Route("{PAPCode}/purchase-request/close-submission")]
        public ActionResult CloseSubmission(string PAPCode)
        {
            return Json(new { result = myProjectsDAL.ClosePRSubmission(PAPCode) });
        }

        [HttpPost]
        [ActionName("update-preprocurement")]
        [Route("update-preprocurement")]
        public ActionResult UpdatePreProcurement(ProcurementProjectsVM ProcurementProgram)
        {
            DateTime Date;
            if (ProcurementProgram.PreProcurementConference != null && !DateTime.TryParseExact(ProcurementProgram.PreProcurementConference.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "PreProcurementConference", result = "Please select a valid date." });
            }
            if (ProcurementProgram.ActualPreProcurementConference != null && !DateTime.TryParseExact(ProcurementProgram.ActualPreProcurementConference.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "ActualPreProcurementConference", result = "Please select a valid date." });
            }
            return Json(new { noError = true, result = myProjectsDAL.UpdatePreProcurement(ProcurementProgram, User.Identity.Name) });
        }

        [HttpPost]
        [ActionName("update-postingOfIB")]
        [Route("update-posting-of-IB")]
        public ActionResult UpdatePosting(ProcurementProjectsVM ProcurementProgram)
        {
            DateTime Date;
            if (ProcurementProgram.PostingOfIB != null && !DateTime.TryParseExact(ProcurementProgram.PostingOfIB.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "PostingOfIB", result = "Please select a valid date." });
            }
            if (ProcurementProgram.ActualPostingOfIB != null && !DateTime.TryParseExact(ProcurementProgram.ActualPostingOfIB.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "ActualPostingOfIB", result = "Please select a valid date." });
            }
            return Json(new { noError = true, result = myProjectsDAL.UpdatePostingOfIB(ProcurementProgram, User.Identity.Name) });
        }

        [HttpPost]
        [ActionName("update-prebid-conference")]
        [Route("update-prebid-conference")]
        public ActionResult UpdatePreBid(ProcurementProjectsVM ProcurementProgram)
        {
            DateTime Date;
            if (ProcurementProgram.PostingOfIB != null && !DateTime.TryParseExact(ProcurementProgram.PostingOfIB.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "PostingOfIB", result = "Please select a valid date." });
            }
            if (ProcurementProgram.ActualPostingOfIB != null && !DateTime.TryParseExact(ProcurementProgram.ActualPostingOfIB.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "ActualPostingOfIB", result = "Please select a valid date." });
            }
            return Json(new { noError = true, result = myProjectsDAL.UpdatePreBid(ProcurementProgram, User.Identity.Name) });
        }

        [HttpPost]
        [ActionName("update-evaluation-of-bids")]
        [Route("update-evaluation-of-bids")]
        public ActionResult UpdateBidsEvaluation(ProcurementProjectsVM ProcurementProgram)
        {
            DateTime Date;
            if (ProcurementProgram.OpeningOfBids != null && !DateTime.TryParseExact(ProcurementProgram.OpeningOfBids.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "OpeningOfBids", result = "Please select a valid date." });
            }
            if (ProcurementProgram.PrelimenryExamination != null && !DateTime.TryParseExact(ProcurementProgram.PrelimenryExamination.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "PrelimenryExamination", result = "Please select a valid date." });
            }
            if (ProcurementProgram.DetailedExamination != null && !DateTime.TryParseExact(ProcurementProgram.DetailedExamination.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "DetailedExamination", result = "Please select a valid date." });
            }
            return Json(new { noError = true, result = myProjectsDAL.UpdateBidsEvaluation(ProcurementProgram, User.Identity.Name) });
        }

        [HttpPost]
        [ActionName("update-post-qualification")]
        [Route("update-post-qualification")]
        public ActionResult UpdatePostQualification(ProcurementProjectsVM ProcurementProgram)
        {
            DateTime Date;
            if (ProcurementProgram.PostQualification != null && !DateTime.TryParseExact(ProcurementProgram.PostQualification.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "PostQualification", result = "Please select a valid date." });
            }
            if (ProcurementProgram.ActualPostQualification != null && !DateTime.TryParseExact(ProcurementProgram.ActualPostQualification.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "ActualPostQualification", result = "Please select a valid date." });
            }
            if (ProcurementProgram.PostQualificationReportedToBAC != null && !DateTime.TryParseExact(ProcurementProgram.PostQualificationReportedToBAC.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "PostQualificationReportedToBAC", result = "Please select a valid date." });
            }
            return Json(new { noError = true, result = myProjectsDAL.UpdatePostQualification(ProcurementProgram, User.Identity.Name) });
        }

        [HttpPost]
        [ActionName("update-notice-of-award")]
        [Route("update-notice-of-award")]
        public ActionResult UpdateNoticeOfAward(ProcurementProjectsVM ProcurementProgram)
        {
            DateTime Date;
            if (ProcurementProgram.NOAIssuance != null && !DateTime.TryParseExact(ProcurementProgram.NOAIssuance.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "NOAIssuance", result = "Please select a valid date." });
            }
            if (ProcurementProgram.BACResolutionCreated != null && !DateTime.TryParseExact(ProcurementProgram.BACResolutionCreated.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "BACResolutionCreated", result = "Please select a valid date." });
            }
            if (ProcurementProgram.HOPEForwarded != null && !DateTime.TryParseExact(ProcurementProgram.HOPEForwarded.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "HOPEForwarded", result = "Please select a valid date." });
            }
            if (ProcurementProgram.PMOReceived != null && !DateTime.TryParseExact(ProcurementProgram.PMOReceived.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "PMOReceived", result = "Please select a valid date." });
            }
            
            return Json(new { noError = true, result = myProjectsDAL.UpdateBACResoNoticeOfAward(ProcurementProgram, User.Identity.Name) });
        }

        [HttpPost]
        [ActionName("update-noa-issuance")]
        [Route("update-noa-issuance")]
        public ActionResult UpdateNoticeOfAwardIssuance(ProcurementProjectsVM ProcurementProgram)
        {
            DateTime Date;
            if (ProcurementProgram.ActualNOAIssuance != null && !DateTime.TryParseExact(ProcurementProgram.ActualNOAIssuance.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "ActualNOAIssuance", result = "Please select a valid date." });
            }
            if (ProcurementProgram.NOASignedByHOPE != null && !DateTime.TryParseExact(ProcurementProgram.NOASignedByHOPE.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "NOASignedByHOPE", result = "Please select a valid date." });
            }
            if (ProcurementProgram.NOAReceivedBySupplier != null && !DateTime.TryParseExact(ProcurementProgram.NOAReceivedBySupplier.Value.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Date))
            {
                return Json(new { noError = false, property = "NOAReceivedBySupplier", result = "Please select a valid date." });
            }
            return Json(new { noError = true, result = myProjectsDAL.UpdateNoticeOfAwardIssuance(ProcurementProgram, User.Identity.Name) });
        }

        [HttpPost]
        [ActionName("update-contract-details")]
        [Route("update-contract-details")]
        public ActionResult UpdateContractDetails(ProcurementProjectsVM ProcurementProgram)
        {
            return Json(new { noError = true, result = myProjectsDAL.UpdateContractDetails(ProcurementProgram, User.Identity.Name) });
        }

        [ActionName("print-noa")]
        [Route("{PAPCode}/notice-of-award/print")]
        public ActionResult GenerateNOA(string PAPCode)
        {
            var stream = myProjectsBL.GenerateNoticeOfAward(PAPCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();
            return RedirectToAction("details", new { Area = "end-users", PAPCode = PAPCode });
        }

        [ActionName("print-purchase-order")]
        [Route("{PAPCode}/purchase-order/print")]
        public ActionResult GeneratePurchaseOrder(string PAPCode)
        {
            var stream = myProjectsBL.GeneratePurchaseOrder(PAPCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();
            return RedirectToAction("details", new { Area = "end-users", PAPCode = PAPCode });
        }
    }
}