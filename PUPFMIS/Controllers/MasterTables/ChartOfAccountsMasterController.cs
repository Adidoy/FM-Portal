//using FluentValidation;
//using PUPFMIS.Models;
//using PUPFMIS.Validators;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Web.Mvc;

//namespace PUPFMIS.Controllers.MasterTables
//{
//    [Route("chart-accounts-master/{action}")]
//    [Authorize]
//    public class ChartOfAccountsMasterController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        public ActionResult Index()
//        {
//            var Accounts = db.ChartOfAccounts.Where(d => d.PurgeFlag == false).Include(c => c.FKClass).Include(c => c.FKGenAcct).Include(c => c.FKSubAcct).OrderBy(o => o.UACS);
//            return View(Accounts.ToList());
//        }

//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ChartOfAccountsMaster Account = db.ChartOfAccounts.Find(id);
//            Account.FKClass = (Account.Class == null) ? null : db.ChartOfAccounts.Find(Account.Class);
//            Account.FKGenAcct = (Account.GenAcct == null) ? null : db.ChartOfAccounts.Find(Account.GenAcct);
//            Account.FKSubAcct = (Account.SubAcct == null) ? null : db.ChartOfAccounts.Find(Account.SubAcct);
//            if (Account == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(Account);
//        }

//        public ActionResult Create()
//        {
//            List<SelectListItem> _class = new SelectList(db.ChartOfAccounts.Where(d => d.PurgeFlag == false && d.Class == null), "ID", "AccountTitle").ToList();
//            _class.Insert(0, new SelectListItem { Text = "None", Value = "0" });
//            ViewBag.Class = _class;
//            List<SelectListItem> _genAcct = new SelectList(db.ChartOfAccounts.Where(d => d.PurgeFlag == false && d.GenAcct == null), "ID", "AccountTitle").ToList();
//            _genAcct.Insert(0, new SelectListItem { Text = "None", Value = "0"});
//            ViewBag.GenAcct = _genAcct;
//            List<SelectListItem> _subAcct = new SelectList(db.ChartOfAccounts.Where(d => d.PurgeFlag == false && d.SubAcct == null), "ID", "AccountTitle").ToList();
//            _subAcct.Insert(0, new SelectListItem { Text = "None", Value = "0"});
//            ViewBag.SubAcct = _subAcct;
//            return PartialView();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,UACS,AccountTitle,Class,GenAcct,SubAcct,PurgeFlag,CreatedAt")] ChartOfAccountsMaster Account)
//        {
//            var _accountsValidator = new ChartOfAccountsValidators();
//            var _result = _accountsValidator.Validate(Account);

//            if(!_result.IsValid)
//            {
//                foreach(var _error in _result.Errors)
//                {
//                    ModelState.AddModelError(_error.PropertyName, _error.ErrorMessage);
//                }
//            }

//            if (ModelState.IsValid)
//            {
//                Account.PurgeFlag = false;
//                Account.CreatedAt = DateTime.Now;
//                Account.Class = (Account.Class == 0) ? null : Account.Class;
//                Account.GenAcct = (Account.GenAcct == 0) ? null : Account.GenAcct;
//                Account.SubAcct = (Account.SubAcct == 0) ? null : Account.SubAcct;

//                db.ChartOfAccounts.Add(Account);
//                var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
//                db.SaveChanges();
//                Audit(AuditActions.AddRecord, _currentValues, null, Account.ID);
//                return Json(new
//                {
//                    status = "success"
//                });
//            }

//            ViewBag.Class = new SelectList(db.ChartOfAccounts, "ID", "AccountTitle", Account.Class);
//            ViewBag.GenAcct = new SelectList(db.ChartOfAccounts, "ID", "AccountTitle", Account.GenAcct);
//            ViewBag.SubAcct = new SelectList(db.ChartOfAccounts, "ID", "AccountTitle", Account.SubAcct);
//            return PartialView(Account);
//        }

//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ChartOfAccountsMaster Account = db.ChartOfAccounts.Find(id);
//            if (Account == null)
//            {
//                return HttpNotFound();
//            }
//            List<SelectListItem> _class = new SelectList(db.ChartOfAccounts.Where(d => d.PurgeFlag == false), "ID", "AccountTitle", Account.Class).ToList();
//            _class.Insert(0, new SelectListItem { Text = "None", Value = "0" });
//            ViewBag.Class = _class;
//            List<SelectListItem> _genAcct = new SelectList(db.ChartOfAccounts.Where(d => d.PurgeFlag == false), "ID", "AccountTitle",Account.GenAcct).ToList();
//            _genAcct.Insert(0, new SelectListItem { Text = "None", Value = "0" });
//            ViewBag.GenAcct = _genAcct;
//            List<SelectListItem> _subAcct = new SelectList(db.ChartOfAccounts.Where(d => d.PurgeFlag == false), "ID", "AccountTitle",Account.SubAcct).ToList();
//            _subAcct.Insert(0, new SelectListItem { Text = "None", Value = "0" });
//            ViewBag.SubAcct = _subAcct;
//            return PartialView(Account);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,UACS,AccountTitle,Class,GenAcct,SubAcct,UpdatedAt")] ChartOfAccountsMaster Account)
//        {
//            ChartOfAccountsMaster _accountUpdate = db.ChartOfAccounts.Find(Account.ID);
//            _accountUpdate.UACS = Account.UACS;
//            _accountUpdate.AccountTitle = Account.AccountTitle;
//            _accountUpdate.Class = (Account.Class == 0) ? null : Account.Class;
//            _accountUpdate.GenAcct = (Account.GenAcct == 0 ) ? null : Account.GenAcct;
//            _accountUpdate.SubAcct = (Account.SubAcct == 0) ? null : Account.SubAcct;
//            _accountUpdate.UpdatedAt = DateTime.Now;

//            var _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
//            var _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;

//            if (ModelState.IsValid)
//            {
//                Audit(AuditActions.UpdateRecord, _currentValues, _originalValues, Account.ID);
//                db.SaveChanges();
//                return Json(new {
//                    status = "success"
//                });
//            }
//            ViewBag.Class = new SelectList(db.ChartOfAccounts, "ID", "AccountTitle", Account.Class);
//            ViewBag.GenAcct = new SelectList(db.ChartOfAccounts, "ID", "AccountTitle", Account.GenAcct);
//            ViewBag.SubAcct = new SelectList(db.ChartOfAccounts, "ID", "AccountTitle", Account.SubAcct);
//            return View(Account);
//        }

//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ChartOfAccountsMaster Accounts = db.ChartOfAccounts.Find(id);
//            if (Accounts == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(Accounts);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            ChartOfAccountsMaster _accountPurge = db.ChartOfAccounts.Find(id);
//            _accountPurge.PurgeFlag = true;
//            _accountPurge.DeletedAt = DateTime.Now;

//            var _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
//            var _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
//            Audit(AuditActions.DeleteRecord, _currentValues, _originalValues, _accountPurge.ID);

//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        public ActionResult GetGeneralAccounts(int ClassID)
//        {
//            List<SelectListItem> _generalAccounts = new SelectList(db.ChartOfAccounts.Where(d => d.PurgeFlag == false && (d.Class == ClassID && d.GenAcct == null)), "ID", "AccountTitle").ToList();
//            _generalAccounts.Insert(0, (new SelectListItem { Text = "None", Value = "0" }));
//            return Json(_generalAccounts, JsonRequestBehavior.AllowGet);
//        }

//        public ActionResult GetSubAccounts(int GeneralAccountID)
//        {
//            List<SelectListItem> _subAccounts = new SelectList(db.ChartOfAccounts.Where(d => d.PurgeFlag == false && (d.GenAcct == GeneralAccountID && d.SubAcct == null)), "ID", "AccountTitle").ToList();
//            _subAccounts.Insert(0, (new SelectListItem { Text = "None", Value = "0" }));
//            return Json(_subAccounts, JsonRequestBehavior.AllowGet);
//        }

//        public void Audit(AuditActions Action, DbPropertyValues CurrentValues, DbPropertyValues OriginalValues, int ID)
//        {
//            MasterTablesAuditController _audit = new MasterTablesAuditController();
//            _audit.Action = Action;
//            _audit.TableName = "Chart of Accounts";
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
