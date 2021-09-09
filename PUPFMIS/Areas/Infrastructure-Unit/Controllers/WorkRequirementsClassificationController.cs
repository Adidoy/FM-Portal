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
    [RoutePrefix("work-classifications")]
    [UserAuthorization(Roles = SystemRoles.InfrastructureImplementingUnit)]
    public class WorkRequirementsClassificationController : Controller
    {
        WorkRequirementsClassificationBDL classBDL = new WorkRequirementsClassificationBDL();

        [Route("")]
        [ActionName("list")]
        public ActionResult Index()
        {
            return View("Index", classBDL.GetClassifications(false));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureRequirementsClassification RequirementClass = classBDL.GetClassifications(id);
            if (RequirementClass == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            return PartialView(RequirementClass);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InfrastructureRequirementsClassification RequirementClass)
        {
            Validate(RequirementClass);
            if (ModelState.IsValid)
            {
                return Json(new { result = classBDL.AddClassifications(RequirementClass) });
            }
            return PartialView(RequirementClass);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureRequirementsClassification RequirementClass = classBDL.GetClassifications(id);
            if (RequirementClass == null)
            {
                return HttpNotFound();
            }
            return PartialView(RequirementClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InfrastructureRequirementsClassification RequirementClass)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = classBDL.UpdateClassifications(RequirementClass) });
            }
            return PartialView(RequirementClass);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureRequirementsClassification RequirementClass = classBDL.GetClassifications(id);
            if (RequirementClass == null)
            {
                return HttpNotFound();
            }
            return PartialView(RequirementClass);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return Json(new { result = classBDL.PurgeClassifications(id) });
        }

        [Route("restore-list")]
        [ActionName("restoreindex")]
        public ActionResult RestoreIndex()
        {
            return View(classBDL.GetClassifications(true));
        }

        public ActionResult Restore(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureRequirementsClassification RequirementClass = classBDL.GetClassifications(id);
            if (RequirementClass == null)
            {
                return HttpNotFound();
            }
            return PartialView(RequirementClass);
        }

        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public ActionResult RestorConfirmed(int id)
        {
            return Json(new { result = classBDL.RestoreClassifications(id) });
        }

        private void Validate(InfrastructureRequirementsClassification RequirementClass)
        {
            InfrastructureRequirementsClassificationValidator _validator = new InfrastructureRequirementsClassificationValidator();
            ValidationResult _validationResult = _validator.Validate(RequirementClass);
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