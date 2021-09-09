using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Controllers;
using PUPFMIS.Models;
using System.Web.Mvc;

namespace PUPFMIS.Areas.End_Users.Controllers
{
    [Route("{action}")]
    [RouteArea("end-users")]
    [RoutePrefix("projects")]
    [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
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
            //dashboard.ProjectInfraFiscalYears = dashboardDAL.GetInfraRequestFiscalYears(UserEmail);
            dashboard.PPMPFiscalYears = dashboardDAL.GetPPMPFiscalYears(UserEmail);
            dashboard.TotalNumberOfProjects = dashboardDAL.GetTotalNumberOfProjects(UserEmail);
            dashboard.TotalProjectsForwardedResponsibilityCenter = dashboardDAL.GetNumberOfProjectsForwardedToResponsibilityCenters(UserEmail);
            dashboard.TotalProjectsEvaluatedResponsibilityCenter = dashboardDAL.GetNumberOfProjectsEvaluatedByResponsibilityCenters(UserEmail);
            dashboard.NumberOfNewPPMP = dashboardDAL.GetNumberOfNewPPMP(UserEmail);
            dashboard.NumberOfPPMPs = dashboardDAL.GetNumberOfPPMPs(UserEmail);
            dashboard.NumberOfApprovedPPMPs = dashboardDAL.GetNumberOfApprovedPPMPs(UserEmail);
            dashboard.PercentageOfPosting = (dashboard.TotalNumberOfProjects == 0) ? "0.00%" : ((decimal)dashboard.TotalProjectsForwardedResponsibilityCenter / (decimal)dashboard.TotalNumberOfProjects).ToString("0.0%");
            dashboard.PercentageOfSubmission = (dashboard.TotalNumberOfProjects == 0) ? "0.00%" : (dashboard.TotalProjectsEvaluatedResponsibilityCenter / (decimal)dashboard.TotalNumberOfProjects).ToString("0.0%");
            //dashboard.PercentageOfApproval = (dashboard.TotalProjectsForwardedResponsibilityCenter == 0) ? "0.00%" : (dashboard.NumberOfApprovedPPMPs / (decimal)dashboard.NumberOfPPMPs).ToString("0.0%");
            dashboard.ProposedBudget = dashboardDAL.GetProposedBudget(UserEmail);
            //dashboard.ApprovedBudget = dashboardDAL.GetApprovedBudget(UserEmail);
            //dashboard.OngoingBudget = dashboardDAL.GetOngoingBudget(UserEmail);
            dashboard.Switchboard = dashboardDAL.GetSwitchBoard(UserEmail);
            return View("dashboard", dashboard);
        }

        [Route("{ReferenceNo}/details")]
        [ActionName("view-switchboard-details")]
        public ActionResult SwitchBoardDetails(string ReferenceNo)
        {
            var switchBoardBody = dashboardDAL.GetSwitchBoardBody(ReferenceNo);
            return View("SwitchBoardDetails", switchBoardBody);
            //return View();
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