using FluentValidation;
using FluentValidation.Results;
using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Authorize]
    [Route("master-tables/item-categories/{action}")]
    public class ItemCategoriesController : Controller
    {
        private FMISDbContext db = new FMISDbContext();

        public ActionResult Index()
        {
            return View(db.ItemCategories.Where(d => d.PurgeFlag == false).ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemCategory itemCategory = db.ItemCategories.Find(id);
            if (itemCategory == null)
            {
                return HttpNotFound();
            }
            return PartialView(itemCategory);
        }

        public ActionResult Create()
        {
            List<string> categoryFor = new List<string>();
            categoryFor.Add("Items");
            categoryFor.Add("Services");

            ViewBag.CategoryFor = new SelectList(categoryFor);
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemCategory itemCategory)
        {
            Validate(itemCategory);
            if (ModelState.IsValid)
            {
                itemCategory.PurgeFlag = false;
                itemCategory.CreatedAt = DateTime.Now;
                db.ItemCategories.Add(itemCategory);
                var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
                LogsMasterTables _log = new LogsMasterTables();
                db.SaveChanges();
                _log.AuditableKey = itemCategory.ID;
                _log.ProcessedBy = null;
                _log.Action = "Add Record";
                _log.TableName = "master_itemCategory";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(_log, null, _currentValues);
                return Json(new { status = "success" });
            }
            return PartialView(itemCategory);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemCategory _itemCategory = db.ItemCategories.Find(id);
            if (_itemCategory == null)
            {
                return HttpNotFound();
            }
            List<string> categoryFor = new List<string>();
            categoryFor.Add("Items");
            categoryFor.Add("Services");

            ViewBag.CategoryFor = new SelectList(categoryFor);
            return PartialView(_itemCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ItemCategory itemCategory)
        {
            ItemCategory _itemCategoryUpdate = db.ItemCategories.Find(itemCategory.ID);
            _itemCategoryUpdate.ItemCategoryName = itemCategory.ItemCategoryName;
            _itemCategoryUpdate.UpdatedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;

            if (ModelState.IsValid)
            {
                LogsMasterTables _log = new LogsMasterTables();
                _log.AuditableKey = itemCategory.ID;
                _log.ProcessedBy = null;
                _log.Action = "Update Record";
                _log.TableName = "master_itemCategory";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(_log, _originalValues, _currentValues);
                db.SaveChanges();
                return Json(new { status = "success" });
            }
            return PartialView(itemCategory);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemCategory _itemCategory = db.ItemCategories.Find(id);
            if (_itemCategory == null)
            {
                return HttpNotFound();
            }
            return PartialView(_itemCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemCategory _itemCategoryDelete = db.ItemCategories.Find(id);
            _itemCategoryDelete.PurgeFlag = true;
            _itemCategoryDelete.DeletedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;
            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = id;
            _log.ProcessedBy = null;
            _log.Action = "Purge Record";
            _log.TableName = "master_itemCategory";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);
            db.SaveChanges();
            return Json(new { status = "success" });
        }
        
        [Route("master-tables/item-categories/restore-index")]
        public ActionResult RestoreIndex()
        {
            return View(db.ItemCategories.Where(d => d.PurgeFlag == true).ToList());
        }

        public ActionResult Restore(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemCategory _itemCategory = db.ItemCategories.Find(id);
            if (_itemCategory == null)
            {
                return HttpNotFound();
            }
            return PartialView(_itemCategory);
        }

        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public ActionResult RestorConfirmed(int id)
        {
            ItemCategory _itemCategoryRestore = db.ItemCategories.Find(id);
            _itemCategoryRestore.PurgeFlag = false;
            _itemCategoryRestore.DeletedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;
            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = id;
            _log.ProcessedBy = null;
            _log.Action = "Record Restored";
            _log.TableName = "master_itemCategory";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);
            db.SaveChanges();
            return Json(new { status = "success" });
        }

        private void Validate(ItemCategory itemCategory)
        {
            ItemCategoryValidator _validator = new ItemCategoryValidator();
            ValidationResult _validationResult = _validator.Validate(itemCategory);
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
