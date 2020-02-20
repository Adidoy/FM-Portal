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
//    [Route("offices-master/{action}")]
//    [Authorize]
//    public class OfficesMasterController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        public ActionResult Index()
//        {
//            var offices = db.Offices.Where(d => d.PurgeFlag == false).Include(o => o.FKHigherOffice).Include(o => o.FKSectors);
//            return View(offices.ToList());
//        }

//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            OfficesMaster Office = db.Offices.Find(id);
//            if (Office == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(Office);
//        }

//        public ActionResult Create()
//        {
//            List<SelectListItem> _sectors = new SelectList(db.Sectors.Where(d => d.PurgeFlag == false), "ID", "SectorName").ToList();
//            var _firstSectorItem = int.Parse(_sectors[0].Value);
//            List<SelectListItem> _higherOffices = new SelectList(db.Offices.Where(d => d.PurgeFlag == false), "ID", "OfficeName").ToList();
//            _higherOffices.Insert(0, new SelectListItem { Text = "None", Value = "0"});
//            ViewBag.Sector = _sectors;
//            ViewBag.HeadOffice = _higherOffices;
//            return PartialView();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,Sector,HeadOffice,OfficeCode,OfficeName,OfficeHead,Designation,PurgeFlag,CreatedAt")] OfficesMaster Office)
//        {
//            var _officeValidator = new OfficesValidators();
//            var _result = _officeValidator.Validate(Office);

//            if (!_result.IsValid)
//            {
//                foreach (var _error in _result.Errors)
//                {
//                    ModelState.AddModelError(_error.PropertyName, _error.ErrorMessage);
//                }
//            }

//            if (ModelState.IsValid)
//            {
//                Office.HeadOffice = (Office.HeadOffice == 0) ? null : Office.HeadOffice;
//                Office.PurgeFlag = false;
//                Office.CreatedAt = DateTime.Now;
//                db.Offices.Add(Office);
//                var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
//                db.SaveChanges();
//                OfficeAudit(AuditActions.AddRecord, _currentValues, null, Office.ID);
//                return Json(new {
//                    status = "success"
//                });
//            }

//            ViewBag.HeadOffice = new SelectList(db.Offices.Where(d => d.PurgeFlag == false), "ID", "OfficeName", Office.HeadOffice);
//            ViewBag.Sector = new SelectList(db.Sectors.Where(d => d.PurgeFlag == false), "ID", "SectorName", Office.Sector);
//            return PartialView(Office);
//        }

//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            OfficesMaster Office = db.Offices.Find(id);
//            if (Office == null)
//            {
//                return HttpNotFound();
//            }
//            List<SelectListItem> _sectors = new SelectList(db.Sectors.Where(d => d.PurgeFlag == false), "ID", "SectorName", Office.Sector).ToList();
//            List<SelectListItem> _higherOffices = new SelectList(db.Offices.Where(d => d.PurgeFlag == false), "ID", "OfficeName", Office.HeadOffice).ToList();
//            _higherOffices.Insert(0, new SelectListItem { Text = "None", Value = "0" });
//            ViewBag.Sector = _sectors;
//            ViewBag.HeadOffice = _higherOffices;
//            return PartialView(Office);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,Sector,HeadOffice,OfficeCode,OfficeName,OfficeHead,Designation,UpdatedAt")] OfficesMaster Office)
//        {
//            OfficesMaster _officeUpdate = db.Offices.Find(Office.ID);
//            _officeUpdate.OfficeCode = Office.OfficeCode;
//            _officeUpdate.OfficeName = Office.OfficeName;
//            _officeUpdate.OfficeHead = Office.OfficeHead;
//            _officeUpdate.Designation = Office.Designation;
//            _officeUpdate.Sector = (Office.Sector == 0) ? null : Office.Sector;
//            _officeUpdate.HeadOffice = (Office.HeadOffice == 0) ? null : Office.HeadOffice;
//            _officeUpdate.UpdatedAt = DateTime.Now;

//            var _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
//            var _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
//            var _officeValidators = new OfficesValidators();

//            foreach (var prop in _originalValues.PropertyNames)
//            {
//                if (!Equals(_currentValues[prop], _originalValues[prop]))
//                {
//                    var _result = _officeValidators.Validate(Office, prop);
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
//                OfficeAudit(AuditActions.UpdateRecord, _currentValues, _originalValues, Office.ID);
//                db.SaveChanges();
//                return Json(new {
//                    status = "success"
//                });
//            }
//            ViewBag.HeadOffice = new SelectList(db.Offices, "ID", "OfficeCode", Office.HeadOffice);
//            ViewBag.Sector = new SelectList(db.Sectors, "ID", "SectorCode", Office.Sector);
//            return PartialView(Office);
//        }

//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            OfficesMaster Office = db.Offices.Find(id);
//            if (Office == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(Office);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            OfficesMaster _officePurge = db.Offices.Find(id);
//            _officePurge.PurgeFlag = true;
//            _officePurge.DeletedAt = DateTime.Now;

//            var _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
//            var _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
//            OfficeAudit(AuditActions.DeleteRecord, _currentValues, _originalValues, _officePurge.ID);

//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        public ActionResult GetOfficesFromSector(int SectorID)
//        {
//            List<SelectListItem> _offices = new SelectList(db.Offices.Where(d => d.PurgeFlag == false && d.Sector == SectorID), "ID", "OfficeName").ToList();
//            _offices.Insert(0, (new SelectListItem { Text = "None", Value = "0", Selected = true }));
//            return Json(_offices, JsonRequestBehavior.AllowGet);
//        }

//        private void OfficeAudit(AuditActions Action, DbPropertyValues CurrentValues, DbPropertyValues OriginalValues, int ID)
//        {
//            MasterTablesAuditController _sectorsAudit = new MasterTablesAuditController();
//            _sectorsAudit.Action = Action;
//            _sectorsAudit.TableName = "Offices";
//            _sectorsAudit.OriginalValues = OriginalValues;
//            _sectorsAudit.CurrentValues = CurrentValues;
//            _sectorsAudit.AuditableKey = ID;
//            _sectorsAudit.UserID = (Session["UserID"] == null) ? null : Session["UserID"].ToString();
//            _sectorsAudit.Log();
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
