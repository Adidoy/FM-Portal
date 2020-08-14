using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Web.Mvc;

namespace PUPFMIS.Areas.EndUsers.Controllers
{
    [Route("{action}")]
    [RouteArea("end-users")]
    [RoutePrefix("projects")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
    public class DashboardController : Controller
    {
        private DashboardDAL dashboardDAL = new DashboardDAL();

        [Route("dashboard")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            ProjectPlanningDashboardVM dashboard = new ProjectPlanningDashboardVM();
            string UserEmail = User.Identity.Name;
            dashboard.ProjectFiscalYears = dashboardDAL.GetProjectFiscalYears(UserEmail);
            dashboard.PPMPFiscalYears = dashboardDAL.GetPPMPFiscalYears(UserEmail);
            dashboard.NumberOfProjects = dashboardDAL.GetNumberOfProjects(UserEmail);
            dashboard.NumberOfProjectsPostedToPPMP = dashboardDAL.GetNumberOfProjectsPosted(UserEmail);
            dashboard.NumberOfNewPPMP = dashboardDAL.GetNumberOfNewPPMP(UserEmail);
            dashboard.NumberOfPPMPSubmitted = dashboardDAL.GetNumberOfPPMPSubmitted(UserEmail);
            dashboard.NumberOfPPMPs = dashboardDAL.GetNumberOfPPMPs(UserEmail);
            dashboard.NumberOfApprovedPPMPs = dashboardDAL.GetNumberOfApprovedPPMPs(UserEmail);
            dashboard.PercentageOfPosting = (dashboard.NumberOfProjects == 0) ? "0.00%" : ((decimal)dashboard.NumberOfProjectsPostedToPPMP / (decimal)dashboard.NumberOfProjects).ToString("0.0%");
            dashboard.PercentageOfSubmission = (dashboard.NumberOfPPMPs == 0) ? "0.00%" : (dashboard.NumberOfPPMPSubmitted / (decimal)dashboard.NumberOfPPMPs).ToString("0.0%");
            dashboard.PercentageOfApproval = (dashboard.NumberOfPPMPSubmitted == 0) ? "0.00%" : (dashboard.NumberOfApprovedPPMPs / (decimal)dashboard.NumberOfPPMPs).ToString("0.0%");
            dashboard.ProposedBudget = dashboardDAL.GetProposedBudget(UserEmail);
            dashboard.ApprovedBudget = dashboardDAL.GetApprovedBudget(UserEmail);
            dashboard.OngoingBudget = dashboardDAL.GetOngoingBudget(UserEmail);
            dashboard.Switchboard = dashboardDAL.GetSwitchBoard(UserEmail);
            return View("dashboard", dashboard);
        }

        [Route("{ReferenceNo}/details")]
        [ActionName("view-switchboard-details")]
        public ActionResult SwitchBoardDetails(string ReferenceNo)
        {
            var switchBoardBody = dashboardDAL.GetSwitchBoardBody(ReferenceNo);
            return View("SwitchBoardDetails", switchBoardBody);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dashboardDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}