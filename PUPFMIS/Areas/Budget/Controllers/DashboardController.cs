using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.Budget.Controllers
{
    [Route("{action}")]
    [RouteArea("budget")]
    [RoutePrefix("approval")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser + ", " + SystemRoles.BudgetOfficer + ", " + SystemRoles.BudgetOfficer)]
    public class DashboardController : Controller
    {
        private PPMPApprovalDashboardDAL ppmpApprovalDAL = new PPMPApprovalDashboardDAL();

        [Route("dashboard")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            PPMPApprovalDashboardVM dashboard = new PPMPApprovalDashboardVM();
            dashboard.PPMPFiscalYears = ppmpApprovalDAL.GetPPMPFiscalYears();
            dashboard.BudgetProposalsSubmitted = ppmpApprovalDAL.GetBudgetProposalsSubmitted();
            dashboard.PPMPsToReview = ppmpApprovalDAL.GetPPMPsToEvaluate();
            dashboard.PPMPEvaluated = ppmpApprovalDAL.GetPPMPsEvaluated();
            dashboard.BudgetProposalsReviewed = ppmpApprovalDAL.GetBudgetProposalsReviewed();
            dashboard.ProposedMOOEBudget = ppmpApprovalDAL.GetProposeMOOEBudget();
            dashboard.ProposedCapitalOutlayBudget = ppmpApprovalDAL.GetProposedCapitalOutlayBudget();
            dashboard.ApprovedMOOEBudget = ppmpApprovalDAL.GetApprovedMOOEBudget();
            dashboard.ApprovedCapitalOutlayBudget = ppmpApprovalDAL.GetApprovedCapitalOutlayBudget();
            dashboard.MOOE = ppmpApprovalDAL.GetMOOESummary(DateTime.Now.Year + 1);
            dashboard.CapitalOutlay = ppmpApprovalDAL.GetCaptialOutlaySummary(DateTime.Now.Year + 1);
            return View("Dashboard", dashboard );
        }

        protected override void Dispose(bool disposing)
        {
            ppmpApprovalDAL.Dispose();
        }
    }
}