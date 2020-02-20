//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;
//using System.Data.Entity.Infrastructure;
//using PUPFMIS.Validators;
//using FluentValidation;

//namespace PUPFMIS.Controllers
//{
//    [Route("procurement-categories-master/{action}")]
//    [Authorize]
//    public class ItemSubCategoryMasterController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        public ActionResult Index()
//        {
//            return View(db.ItemSubCategory.Where(d => d.PurgeFlag == false).ToList());
//        }

//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ItemSubCategoryMaster ItemSubCategory = db.ItemSubCategory.Find(id);
//            if (ItemSubCategory == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(ItemSubCategory);
//        }

//        public ActionResult Create()
//        {
//            return PartialView();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,ProcurementCategoryName,ProcurementSource,PurgeFlag,CreatedAt")] ItemSubCategoryMaster ItemSubCategory)
//        {
//            var _procurementCategoryValidator = new ProcurementCategoriesValidators();
//            var _result = _procurementCategoryValidator.Validate(ItemSubCategory);

//            if(!_result.IsValid)
//            {
//                foreach(var _error in _result.Errors)
//                {
//                    ModelState.AddModelError(_error.PropertyName, _error.ErrorMessage);
//                }
//            }

//            if (ModelState.IsValid)
//            {
//                ItemSubCategory.PurgeFlag = false;
//                ItemSubCategory.CreatedAt = DateTime.Now;

//                db.ItemSubCategory.Add(ItemSubCategory);
//                var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
//                db.SaveChanges();
//                Audit(AuditActions.AddRecord, _currentValues, null, ItemSubCategory.ID);

//                return Json(new {
//                    status = "success"
//                });
//            }

//            return PartialView(ItemSubCategory);
//        }

//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ItemSubCategoryMaster ItemSubCategory = db.ItemSubCategory.Find(id);
//            if (ItemSubCategory == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(ItemSubCategory);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,ProcurementCategoryName,ProcurementSource")] ItemSubCategoryMaster ItemSubCategory)
//        {
//            ItemSubCategoryMaster _itemSubCategoryUpdate = db.ItemSubCategory.Find(ItemSubCategory.ID);
//            _itemSubCategoryUpdate.ProcurementCategoryName = ItemSubCategory.ProcurementCategoryName;
//            _itemSubCategoryUpdate.UpdatedAt = DateTime.Now;

//            var _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
//            var _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
//            var _procurementCategoriesValidator = new ProcurementCategoriesValidators();

//            foreach (var prop in _originalValues.PropertyNames)
//            {
//                if (!Equals(_currentValues[prop], _originalValues[prop]))
//                {
//                    var _result = _procurementCategoriesValidator.Validate(ItemSubCategory, prop);
//                    if (!_result.IsValid)
//                    {
//                        foreach (var _error in _result.Errors)
//                        {
//                            ModelState.AddModelError(_error.PropertyName, _error.ErrorMessage);
//                        }
//                    }
//                }
//            }

//            if (ModelState.IsValid)
//            {
//                Audit(AuditActions.UpdateRecord, _currentValues, _originalValues, ItemSubCategory.ID);
//                db.SaveChanges();
//                return Json(new {
//                    status = "success"
//                });
//            }
//            return PartialView(ItemSubCategory);
//        }

//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ItemSubCategoryMaster ProcurementCategory = db.ItemSubCategory.Find(id);
//            if (ProcurementCategory == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(ProcurementCategory);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            ItemSubCategoryMaster _itemSubCategoryDelete = db.ItemSubCategory.Find(id);
//            _itemSubCategoryDelete.PurgeFlag = true;
//            _itemSubCategoryDelete.DeletedAt = DateTime.Now;

//            var _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
//            var _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;

//            Audit(AuditActions.DeleteRecord, _currentValues, _originalValues, _itemSubCategoryDelete.ID);
                
//            db.SaveChanges();
//            return RedirectToAction("index");
//        }

//        private void Audit(AuditActions Action, DbPropertyValues CurrentValues, DbPropertyValues OriginalValues, int ID)
//        {
//            MasterTablesAuditController _audit = new MasterTablesAuditController();
//            _audit.Action = Action;
//            _audit.TableName = "Procurement Categories";
//            _audit.OriginalValues = OriginalValues;
//            _audit.CurrentValues = CurrentValues;
//            _audit.AuditableKey = ID;
//            _audit.UserID = (Session["UserID"] == null) ? null : Session["UserID"].ToString();
//            _audit.Log();
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
