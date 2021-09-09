using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;
using FluentValidation.Results;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("infrastructure-unit")]
    [RoutePrefix("work-requirements")]
    [UserAuthorization(Roles = SystemRoles.InfrastructureImplementingUnit)]
    public class WorkRequirementsController : Controller
    {
        WorkRequirementsBDL requirementsBDL = new WorkRequirementsBDL();

        [Route("")]
        [ActionName("list")]
        public ActionResult Index()
        {
            return View("Index", requirementsBDL.GetRequirements(false));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureRequirements Requirement = requirementsBDL.GetRequirements(id);
            if (Requirement == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            return PartialView(Requirement);
        }

        public ActionResult Create()
        {
            ViewBag.RequirementClassificationReference = new SelectList(requirementsBDL.GetClassifications(), "ID", "ClassificationName");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InfrastructureRequirements RequirementModel)
        {
            Validate(RequirementModel);
            if (ModelState.IsValid)
            {
                return Json(new { result = requirementsBDL.AddRequirements(RequirementModel) });
            }
            ViewBag.RequirementClassificationReference = new SelectList(requirementsBDL.GetClassifications(), "ID", "ClassificationName", RequirementModel.RequirementClassificationReference);
            return PartialView(RequirementModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureRequirements Requirement = requirementsBDL.GetRequirements(id);
            if (Requirement == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequirementClassificationReference = new SelectList(requirementsBDL.GetClassifications(), "ID", "ClassificationName", Requirement.RequirementClassificationReference);
            return PartialView(Requirement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InfrastructureRequirements RequirementModel)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = requirementsBDL.UpdateRequirements(RequirementModel) });
            }
            ViewBag.RequirementClassificationReference = new SelectList(requirementsBDL.GetClassifications(), "ID", "ClassificationName", RequirementModel.RequirementClassificationReference);
            return PartialView(RequirementModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureRequirements Requirement = requirementsBDL.GetRequirements(id);
            if (Requirement == null)
            {
                return HttpNotFound();
            }
            return PartialView(Requirement);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return Json(new { result = requirementsBDL.PurgeRequirements(id) });
        }

        [Route("restore-list")]
        [ActionName("restoreindex")]
        public ActionResult RestoreIndex()
        {
            return View(requirementsBDL.GetRequirements(true));
        }

        public ActionResult Restore(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureRequirements Requirement = requirementsBDL.GetRequirements(id);
            if (Requirement == null)
            {
                return HttpNotFound();
            }
            return PartialView(Requirement);
        }

        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public ActionResult RestorConfirmed(int id)
        {
            return Json(new { result = requirementsBDL.RestoreRequirements(id) });
        }

        private void Validate(InfrastructureRequirements Requirement)
        {
            InfrastructureRequirementsValidator _validator = new InfrastructureRequirementsValidator();
            ValidationResult _validationResult = _validator.Validate(Requirement);
            if (!_validationResult.IsValid)
            {
                foreach (ValidationFailure _result in _validationResult.Errors)
                {
                    ModelState.AddModelError(_result.PropertyName, _result.ErrorMessage);
                }
            }
        }
    }
}