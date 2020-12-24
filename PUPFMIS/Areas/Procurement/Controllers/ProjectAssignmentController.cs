using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using FluentValidation.Results;
using System.Security.Claims;

namespace PUPFMIS.Areas.Procurement.Controllers
{
    [Route("{action}")]
    [RouteArea("procurement")]
    [RoutePrefix("project/assignment")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin + ", " + SystemRoles.ProcurementAdministrator + ", " + SystemRoles.ProcurementPlanningChief + ", " + SystemRoles.ProjectCoordinator)]
    public class ProjectAssignmentController : Controller
    {
        private ProjectAssignmentDAL projectAssignment = new ProjectAssignmentDAL();

        [Route("dashboard")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            return View("Dashboard");
        }

        [Route("")]
        [ActionName("index")]
        public ActionResult Index()
        {
            var apps = projectAssignment.GetAPPs();
            return View("Index", apps);
        }

        [ActionName("unassigned")]
        [Route("unassigned")]
        public ActionResult Unassigned(string ReferenceNo)
        {
            var pipeline = projectAssignment.GetUnassignedProcurementProgams(ReferenceNo);
            return View("ProjectsIndex", pipeline);
        }

        [Authorize(Roles = SystemRoles.ProcurementStaff)]
        [ActionName("my-projects")]
        [Route("{ReferenceNo}/my-projects")]
        public ActionResult MyProjects(string ReferenceNo)
        {
            var pipeline = projectAssignment.GetProcurementProgramDetailsByPAPCode(ReferenceNo);
            return View("ProjectsIndex", pipeline);
        }

        [Route("{ReferenceNo}/assignment-index")]
        [ActionName("assignment-index")]
        public ActionResult AssignmentIndex(string ReferenceNo)
        {
            var pipeline = projectAssignment.GetUnassignedProcurementProgams(ReferenceNo);
            return View("AssignmentIndex", pipeline);
        }

        [ActionName("assign-project")]
        [Route("{PAPCode}/assign-project")]
        public ActionResult Assignment(string PAPCode)
        {
            var procurementProjectDetails = projectAssignment.GetProcurementProgramDetailsByPAPCode(PAPCode);
            ViewBag.ProjectCoordinator = new SelectList(projectAssignment.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
            ViewBag.ProjectSupport = new SelectList(projectAssignment.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
            return View("Assignment", procurementProjectDetails);
        }

        [HttpPost]
        [ActionName("assign-project")]
        [Route("{PAPCode}/assign-project")]
        public ActionResult Assignment(ProcurementProjectsVM ProcurementProject)
        {
            return Json(new { result = projectAssignment.AssignProject(ProcurementProject) });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                projectAssignment.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
