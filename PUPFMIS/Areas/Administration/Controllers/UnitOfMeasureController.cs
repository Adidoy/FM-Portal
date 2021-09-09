using FluentValidation;
using FluentValidation.Results;
using PUPFMIS.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("administration")]
    [RoutePrefix("units-of-measure")]
    [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin)]
    public class UnitOfMeasureController : Controller
    {
        private UnitOfMeasureDataAccess uomDAL = new UnitOfMeasureDataAccess();

        [Route("")]
        [ActionName("list")]
        public ActionResult Index()
        {
            return View("Index", uomDAL.GetUOMs(false));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitOfMeasure UOM = uomDAL.GetUOM(id);
            if (UOM == null)
            {
                return HttpNotFound();
            }
            return PartialView(UOM);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UnitOfMeasure UOM)
        {
            Validate(UOM);
            if (ModelState.IsValid)
            {
                return Json(new { result = uomDAL.AddUOM(UOM) });
            }
            return PartialView(UOM);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitOfMeasure _uom = uomDAL.GetUOM(id);
            if (_uom == null)
            {
                return HttpNotFound();
            }
            return PartialView(_uom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UnitOfMeasure UOM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = uomDAL.UpdateUOM(UOM) });
            }
            return PartialView(UOM);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitOfMeasure UOM = uomDAL.GetUOM(id);
            if (UOM == null)
            {
                return HttpNotFound();
            }
            return PartialView(UOM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            return Json(new { result = uomDAL.PurgeUOM(id) });
        }

        [Route("restore-list")]
        [ActionName("restoreindex")]
        public ActionResult RestoreIndex()
        {
            return View(uomDAL.GetUOMs(true));
        }

        public ActionResult Restore(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitOfMeasure UOM = uomDAL.GetUOM(id);
            if (UOM == null)
            {
                return HttpNotFound();
            }
            return PartialView(UOM);
        }

        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public ActionResult RestorConfirmed(int id)
        {
            return Json(new { result = uomDAL.RestoreUOM(id) });
        }

        private void Validate(UnitOfMeasure UOM)
        {
            UnitValidator _validator = new UnitValidator();
            ValidationResult _validationResult = _validator.Validate(UOM);
            if (!_validationResult.IsValid)
            {
                foreach (ValidationFailure _result in _validationResult.Errors)
                {
                    ModelState.AddModelError(_result.PropertyName, _result.ErrorMessage);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uomDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
