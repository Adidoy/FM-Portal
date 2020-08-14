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
    [RoutePrefix("pipeline")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin + ", " + SystemRoles.ProcurementAdministrator + ", " + SystemRoles.ProcurementPlanningChief + ", " + SystemRoles.ProjectCoordinator)]
    public class ProcurementPipelinesController : Controller
    {
        private ProcurementPipelineDAL procurementPipeline = new ProcurementPipelineDAL();

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
            var apps = procurementPipeline.GetAPPs();
            return View("Index", apps);
        }

        [ActionName("project-index")]
        [Route("{ReferenceNo}/project-index")]
        public ActionResult ProjectIndex(string ReferenceNo)
        {
            var pipeline = procurementPipeline.GetProcurementProgams(ReferenceNo).Where(d => d.HasSchedule == false).ToList();
            return View("ProjectsIndex", pipeline);
        }

        [Authorize(Roles = SystemRoles.ProcurementStaff)]
        [ActionName("my-projects")]
        [Route("{ReferenceNo}/my-projects")]
        public ActionResult MyProjects(string ReferenceNo)
        {
            var pipeline = procurementPipeline.GetProcurementProgams(ReferenceNo).Where(d => d.HasSchedule == false).ToList();
            return View("ProjectsIndex", pipeline);
        }

        [Route("{ReferenceNo}/assignment-index")]
        [ActionName("assignment-index")]
        public ActionResult AssignmentIndex(string ReferenceNo)
        {
            var pipeline = procurementPipeline.GetUnassignedProcurementProgams(ReferenceNo);
            return View("AssignmentIndex", pipeline);
        }

        [ActionName("set-schedule")]
        [Route("{PAPCode}/set-schedule")]
        public ActionResult Details(string PAPCode)
        {
            var procurementProjectDetails = procurementPipeline.GetProcurementProgramDetailsByPAPCode(PAPCode);
            return View("Details", procurementProjectDetails);
        }

        [HttpPost]
        [ActionName("set-schedule")]
        [Route("{PAPCode}/set-schedule")]
        public ActionResult Details(ProcurementProjectsVM ProcurementProject)
        {
            ProcurementTimelineValidation validator = new ProcurementTimelineValidation();
            ValidationResult validationResult = validator.Validate(ProcurementProject.Schedule);
            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure error in validationResult.Errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
                return PartialView("_Form", ProcurementProject);
            }
            var accountsDAL = new AccountsManagementBL();
            var userRole = accountsDAL.GetUserAccountsList().Where(d => d.Email == User.Identity.Name).FirstOrDefault().UserRole;
            return Json(new { result = procurementPipeline.SetSchedule(ProcurementProject), role = userRole });
        }

        [ActionName("assign-project")]
        [Route("{PAPCode}/assign-project")]
        public ActionResult Assignment(string PAPCode)
        {
            var procurementProjectDetails = procurementPipeline.GetProcurementProgramDetailsByPAPCode(PAPCode);
            ViewBag.ProjectCoordinator = new SelectList(procurementPipeline.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
            ViewBag.ProjectSupport = new SelectList(procurementPipeline.GetProcurementEmployees(), "EmployeeCode", "EmployeeName");
            return View("Assignment", procurementProjectDetails);
        }

        [HttpPost]
        [ActionName("assign-project")]
        [Route("{PAPCode}/assign-project")]
        public ActionResult Assignment(ProcurementProjectsVM ProcurementProject)
        {
            return Json(new { result = procurementPipeline.AssignProject(ProcurementProject) });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                procurementPipeline.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
