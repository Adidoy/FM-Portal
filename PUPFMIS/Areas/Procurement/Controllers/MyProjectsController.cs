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

        [Route("")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            MyProjectsDashboard dashboard = new MyProjectsDashboard();
            dashboard.ProjectsWithSetTimeLine = myProjectsDAL.GetProcurementProgramDetails(User.Identity.Name);
            dashboard.NoOfProjectsWithSetTimeLine = myProjectsDAL.GetProjectsWithSetTimeLine(User.Identity.Name).Count();
            dashboard.ProjectsWithoutSetTimeLine = myProjectsDAL.GetProjectsWithoutSetTimeLine(User.Identity.Name);
            dashboard.NoOfProjectsWithoutSetTimeLine = myProjectsDAL.GetProjectsWithoutSetTimeLine(User.Identity.Name).Count();
            dashboard.TotalProjectsAssignedToMe = myProjectsDAL.GetTotalProjectsAssigned(User.Identity.Name);
            dashboard.TotalProjectsCoordinated = myProjectsDAL.GetTotalProjectsCoordinated(User.Identity.Name);
            dashboard.TotalProjectsSupported = myProjectsDAL.GetTotalProjectsupported(User.Identity.Name);
            return View(dashboard);
        }

        [ActionName("details")]
        [Route("{PAPCode}/details")]
        public ActionResult Details(string PAPCode)
        {
            var procurementPrograms = myProjectsDAL.GetProcurementProgramDetailsByPAPCode(PAPCode);
            return View(procurementPrograms);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("open-pr-submission")]
        [Route("{PAPCode}/purchase-request/open-submission")]
        public ActionResult OpenSubmission(string PAPCode)
        {
            return Json(new { result = myProjectsDAL.OpenPRSubmission(PAPCode) });
        }
    }
}