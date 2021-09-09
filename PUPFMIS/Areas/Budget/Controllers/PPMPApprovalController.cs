using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [RouteArea("budget")]
    [RoutePrefix("approval/ppmp")]
    [Route("{action}")]
    [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.BudgetOfficer + ", " + SystemRoles.BudgetAdmin)]
    public class PPMPApprovalController : Controller
    {
        private PPMPApprovalDAL ppmpApprovalDAL = new PPMPApprovalDAL();

        [Route("dashboard")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            PPMPApprovalDashboardVM dashboard = new PPMPApprovalDashboardVM();
            dashboard.PPMPFiscalYears = ppmpApprovalDAL.GetFiscalYears();
            //dashboard.BudgetProposalsSubmitted = ppmpApprovalDAL.GetBudgetProposalsSubmitted();
            //dashboard.PPMPsToReview = ppmpApprovalDAL.GetPPMPsToEvaluate();
            //dashboard.PPMPEvaluated = ppmpApprovalDAL.GetPPMPsEvaluated();
            //dashboard.BudgetProposalsReviewed = ppmpApprovalDAL.GetBudgetProposalsReviewed();
            //dashboard.ProposedMOOEBudget = ppmpApprovalDAL.GetProposeMOOEBudget();
            //dashboard.ProposedCapitalOutlayBudget = ppmpApprovalDAL.GetProposedCapitalOutlayBudget();
            //dashboard.ApprovedMOOEBudget = ppmpApprovalDAL.GetApprovedMOOEBudget();
            //dashboard.ApprovedCapitalOutlayBudget = ppmpApprovalDAL.GetApprovedCapitalOutlayBudget();
            //dashboard.MOOE = ppmpApprovalDAL.GetMOOESummary(DateTime.Now.Year + 1);
            //dashboard.CapitalOutlay = ppmpApprovalDAL.GetCaptialOutlaySummary(DateTime.Now.Year + 1);
            return View("Dashboard", dashboard);
        }

        [Route("{FiscalYear}")]
        [ActionName("offices-list")]
        public ActionResult PPMPOfficeList(int FiscalYear)
        {
            return View("IndexOfficeList", ppmpApprovalDAL.GetOffices(FiscalYear));
        }

        [ActionName("ppmp-list")]
        [Route("{FiscalYear}/{Department}/accounts")]
        public ActionResult PPMPList(int FiscalYear, string Department)
        {
            ViewBag.Department = Department;
            ViewBag.FiscalYear = FiscalYear;
            var ppmpList = ppmpApprovalDAL.GetPPMPs(FiscalYear, Department).Where(d => d.PPMPStatus == PPMPStatus.ForwardedToBudgetOffice).ToList();
            return View("PPMPList", ppmpList);
        }

        [ActionName("evaluate")]
        [Route("{ReferenceNo}/evaluate")]
        public ActionResult Details(string ReferenceNo)
        {
            PPMPEvaluationVM ppmpEvaluation = ppmpApprovalDAL.GetEvaluationDetails(ReferenceNo);
            if (ppmpEvaluation == null)
            {
                return RedirectToAction("record-not-found", "Errors", new { Area = "" });
            }
            ViewBag.FundSources = new SelectList(ppmpApprovalDAL.GetFundSources(), "FUND_CLUSTER", "FUND_DESC");
            return View("PPMPEvaluation", ppmpEvaluation);
        }

        [HttpPost]
        [ActionName("evaluate")]
        [ValidateAntiForgeryToken]
        [Route("{ReferenceNo}/evaluate")]
        public ActionResult Details(PPMPEvaluationVM Evaluation)
        {
            return Json(new { result = ppmpApprovalDAL.EvaluatePPMP(Evaluation) });
        }

        //[HttpPost]
        //[ActionName("update-details")]
        //[Route("{FiscalYear}/{OfficeCode}/{UACS}/details")]
        //public ActionResult UpdateDetails(PPMPEvaluationVM PPMPEvaluationVM, string FiscalYear, string OfficeCode, string UACS, string updateAction)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View("DetailsAccountLineItems", PPMPEvaluationVM);
        //    }
        //    if (updateAction == "update")
        //    {
        //        foreach (var item in PPMPEvaluationVM.NewSpendingItems)
        //        {
        //            if (item.ApprovalAction == "Approved")
        //            {
        //                if (item.ReducedQuantity <= 0)
        //                {
        //                    ModelState.AddModelError("", "Quantity should be at least 1.");
        //                    ViewBag.FundSource = new SelectList(ppmpApprovalDAL.GetFundSource(), "FUND_CLUSTER", "FUND_DESC");
        //                    return PartialView("DetailsAccountLineItems", PPMPEvaluationVM);
        //                }
        //                if (item.ReducedQuantity > item.Quantity)
        //                {
        //                    ModelState.AddModelError("", "Reduced quantity should not be greater than the original quantity");
        //                    ViewBag.FundSource = new SelectList(ppmpApprovalDAL.GetFundSource(), "FUND_CLUSTER", "FUND_DESC");
        //                    return PartialView("_Form", PPMPEvaluationVM);
        //                }
        //                item.EstimatedCost = item.UnitCost * item.ReducedQuantity;
        //            }
        //            if (item.ApprovalAction == "Disapproved")
        //            {
        //                item.EstimatedCost = 0.00m;
        //            }
        //        }
        //        PPMPEvaluationVM.ApprovedBudget = PPMPEvaluationVM.NewSpendingItems.Sum(d => d.EstimatedCost);
        //        ViewBag.FundSource = new SelectList(ppmpApprovalDAL.GetFundSource(), "FUND_CLUSTER", "FUND_DESC");
        //        return PartialView("_Form", PPMPEvaluationVM);
        //    }
        //    else
        //    {
        //        return Json(ppmpApprovalDAL.SaveApproval(PPMPEvaluationVM, User.Identity.Name));
        //    }
        //}

        //[ActionName("print-ppmp")]
        //[Route("{ReferenceNo}/view")]
        //public ActionResult PrintPPMP(string ReferenceNo)
        //{
        //    var stream = ppmpApprovalBL.ViewPPMP(ReferenceNo, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
        //    Response.Clear();
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.AddHeader("content-length", stream.Length.ToString());
        //    //Response.AddHeader("content-disposition", "attachment; filename=" + ReferenceNo + ".pdf");
        //    Response.ContentType = "application/pdf";
        //    Response.BinaryWrite(stream.ToArray());
        //    stream.Close();
        //    Response.End();

        //    return RedirectToAction("list", new { Area = "end-users" });
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ppmpApprovalDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
