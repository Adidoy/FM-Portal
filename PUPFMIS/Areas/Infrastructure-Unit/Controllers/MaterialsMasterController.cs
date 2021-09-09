using FluentValidation.Results;
using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("infrastructure-unit")]
    [RoutePrefix("materials-master")]
    [UserAuthorization(Roles = SystemRoles.InfrastructureImplementingUnit)]
    public class MaterialsMasterController : Controller
    {
        MaterialsMasterBDL materialsBDL = new MaterialsMasterBDL();

        [Route("")]
        [ActionName("list")]
        public ActionResult Index()
        {
            return View("Index", materialsBDL.GetMaterials(false));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureMaterials Material = materialsBDL.GetMaterials(id);
            if (Material == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            return PartialView(Material);
        }

        public ActionResult Create()
        {
            ViewBag.UOMReference = new SelectList(materialsBDL.GetUOM(), "ID", "UnitName");
            var workClass = materialsBDL.GetWorkClassifications();
            ViewBag.WorkClassificationReference = new SelectList(workClass, "ID", "ClassificationName");
            ViewBag.WorkRequirementReference = new SelectList(materialsBDL.GetWorkRequirements(workClass.First().ID), "ID", "Requirement");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InfrastructureMaterials MaterialModel)
        {
            Validate(MaterialModel);
            if (ModelState.IsValid)
            {
                return Json(new { result = materialsBDL.AddMaterial(MaterialModel) });
            }
            ViewBag.UOMReference = new SelectList(materialsBDL.GetUOM(), "ID", "UnitName", MaterialModel.UOMReference);
            var workClass = materialsBDL.GetWorkClassifications();
            ViewBag.WorkClassificationReference = new SelectList(workClass, "ID", "ClassificationName", MaterialModel.WorkClassificationReference);
            ViewBag.WorkRequirementReference = new SelectList(materialsBDL.GetWorkRequirements(MaterialModel.WorkClassificationReference == null ? (int)MaterialModel.WorkClassificationReference : 0), "ID", "Requirement", MaterialModel.WorkRequirementReference);
            return PartialView(MaterialModel);
        }

        [ActionName("get-work-requirements")]
        public ActionResult GetWorkRequirements(int WorkClassID)
        {
            return Json(materialsBDL.GetWorkRequirements(WorkClassID), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureMaterials Material = materialsBDL.GetMaterials(id);
            if (Material == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            ViewBag.UOMReference = new SelectList(materialsBDL.GetUOM(), "ID", "UnitName", Material.UOMReference);
            var workClass = materialsBDL.GetWorkClassifications();
            ViewBag.WorkClassificationReference = new SelectList(workClass, "ID", "ClassificationName", Material.WorkClassificationReference);
            ViewBag.WorkRequirementReference = new SelectList(materialsBDL.GetWorkRequirements(Material.WorkClassificationReference != null ? (int)Material.WorkClassificationReference : 0), "ID", "Requirement", Material.WorkRequirementReference);
            return PartialView(Material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InfrastructureMaterials MaterialModel)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = materialsBDL.UpdateMaterial(MaterialModel) });
            }
            ViewBag.UOMReference = new SelectList(materialsBDL.GetUOM(), "ID", "UnitName", MaterialModel.UOMReference);
            var workClass = materialsBDL.GetWorkClassifications();
            ViewBag.WorkClassificationReference = new SelectList(workClass, "ID", "ClassificationName", MaterialModel.WorkClassificationReference);
            ViewBag.WorkRequirementReference = new SelectList(materialsBDL.GetWorkRequirements(MaterialModel.WorkClassificationReference == null ? (int)MaterialModel.WorkClassificationReference : 0), "ID", "Requirement", MaterialModel.WorkRequirementReference);
            return PartialView(MaterialModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureMaterials Material = materialsBDL.GetMaterials(id);
            if (Material == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            return PartialView(Material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return Json(new { result = materialsBDL.PurgeMaterials(id) });
        }

        [Route("restore-list")]
        [ActionName("restoreindex")]
        public ActionResult RestoreIndex()
        {
            return View(materialsBDL.GetMaterials(true));
        }

        public ActionResult Restore(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            InfrastructureMaterials Material = materialsBDL.GetMaterials(id);
            if (Material == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            return PartialView(Material);
        }

        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public ActionResult RestorConfirmed(int id)
        {
            return Json(new { result = materialsBDL.RestoreMaterials(id) });
        }

        private void Validate(InfrastructureMaterials Material)
        {
            InfrastructureMaterialsValidator _validator = new InfrastructureMaterialsValidator();
            ValidationResult _validationResult = _validator.Validate(Material);
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