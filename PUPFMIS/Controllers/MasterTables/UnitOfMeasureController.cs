using FluentValidation;
using FluentValidation.Results;
using PUPFMIS.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Authorize]
    [Route("master-tables/units-measure/{action}")]
    public class UnitOfMeasureController : Controller
    {
        private FMISDbContext db = new FMISDbContext();

        public ActionResult Index()
        {
            return View(db.UOM.Where(d => d.PurgeFlag == false).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitOfMeasure UOM = db.UOM.Find(id);
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
                UOM.PurgeFlag = false;
                UOM.CreatedAt = DateTime.Now;
                db.UOM.Add(UOM);
                var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
                db.SaveChanges();
                LogsMasterTables _log = new LogsMasterTables();
                _log.AuditableKey = UOM.ID;
                _log.ProcessedBy = null;
                _log.Action = "Add Record";
                _log.TableName = "master_UOM";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(_log, null, _currentValues);
                return Json(new { status = "success" });
            }
            return PartialView(UOM);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitOfMeasure _uom = db.UOM.Find(id);
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
            UnitOfMeasure _unitOfMeasureUpdate = db.UOM.Find(UOM.ID);
            _unitOfMeasureUpdate.UnitName = UOM.UnitName;
            _unitOfMeasureUpdate.Abbreviation = UOM.Abbreviation;
            _unitOfMeasureUpdate.UpdatedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;

            if (ModelState.IsValid)
            {
                LogsMasterTables _log = new LogsMasterTables();
                _log.AuditableKey = UOM.ID;
                _log.ProcessedBy = null;
                _log.Action = "Update Record";
                _log.TableName = "master_UOM";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(_log, _originalValues, _currentValues);
                db.SaveChanges();
                return Json(new { status = "success" });
            }
            return PartialView(UOM);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitOfMeasure UOM = db.UOM.Find(id);
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
            UnitOfMeasure _unitOfMeasureDelete = db.UOM.Find(id);
            _unitOfMeasureDelete.PurgeFlag = true;
            _unitOfMeasureDelete.DeletedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;
            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = id;
            _log.ProcessedBy = null;
            _log.Action = "Purge Record";
            _log.TableName = "master_UOM";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);
            db.SaveChanges();
            return Json(new { status = "success" });
        }

        public ActionResult RestoreIndex()
        {
            return View(db.UOM.Where(d => d.PurgeFlag == true).ToList());
        }

        public ActionResult Restore(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnitOfMeasure UOM = db.UOM.Find(id);
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
            UnitOfMeasure _unitOfMeasureDelete = db.UOM.Find(id);
            _unitOfMeasureDelete.PurgeFlag = false;
            _unitOfMeasureDelete.DeletedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;
            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = id;
            _log.ProcessedBy = null;
            _log.Action = "Record Restored";
            _log.TableName = "master_UOM";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);
            db.SaveChanges();
            return Json(new { status = "success" });
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
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
