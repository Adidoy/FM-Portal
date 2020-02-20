//using FluentValidation;
//using PUPFMIS.Models;
//using PUPFMIS.Validators;
//using System;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Web.Mvc;

//namespace PUPFMIS.Controllers.MasterTables
//{
//    [Route("sectors-master/{action}")]
//    [Authorize]
//    public class SectorsMasterController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        public ActionResult Index()
//        {
//            return View(db.Sectors.Where(d => d.PurgeFlag == false).ToList());
//        }

//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SectorsMaster Sector = db.Sectors.Find(id);
//            if (Sector == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(Sector);
//        }

//        public ActionResult Create()
//        {
//            return PartialView();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,SectorCode,SectorName,Head,HeadDesignation,AssistantHead,AssistantHeadDesignation,PurgeFlag,CreatedAt")] SectorsMaster Sector)
//        {
//            var _sectorValidator = new SectorsMasterValidators();
//            var _result = _sectorValidator.Validate(Sector);

//            if (!_result.IsValid)
//            {
//                foreach (var _error in _result.Errors)
//                {
//                    ModelState.AddModelError(_error.PropertyName, _error.ErrorMessage);
//                }
//            }

//            if (ModelState.IsValid)
//            {
//                Sector.PurgeFlag = false;
//                Sector.CreatedAt = DateTime.Now;

//                db.Sectors.Add(Sector);
//                var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
//                db.SaveChanges();
//                SectorAudit(AuditActions.AddRecord, _currentValues, null, Sector.ID);
//                return Json(new {
//                    status = "success"
//                });
//            }
//            return PartialView(Sector);
//        }

//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SectorsMaster Sector = db.Sectors.Find(id);
//            if (Sector == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(Sector);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,SectorCode,SectorName,Head,HeadDesignation,AssistantHead,AssistantHeadDesignation,UpdatedAt")] SectorsMaster Sector)
//        {
//            SectorsMaster _sectorUpdate = db.Sectors.Find(Sector.ID);
//            _sectorUpdate.SectorCode = Sector.SectorCode;
//            _sectorUpdate.SectorName = Sector.SectorName;
//            _sectorUpdate.Head = Sector.Head;
//            _sectorUpdate.HeadDesignation = Sector.HeadDesignation;
//            _sectorUpdate.AssistantHead = Sector.AssistantHead;
//            _sectorUpdate.AssistantHeadDesignation = Sector.AssistantHeadDesignation;
//            _sectorUpdate.UpdatedAt = DateTime.Now;

//            var _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
//            var _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
//            var _sectorValidators = new SectorsMasterValidators();

//            foreach (var prop in _originalValues.PropertyNames)
//            {
//                if (!Equals(_currentValues[prop], _originalValues[prop]))
//                {
//                    var _result = _sectorValidators.Validate(Sector, prop);
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
//                SectorAudit(AuditActions.UpdateRecord, _currentValues, _originalValues, Sector.ID);
//                db.SaveChanges();
//                return Json(new
//                {
//                    status = "success"
//                });
//            }
//            return PartialView(Sector);
//        }

//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SectorsMaster Sector = db.Sectors.Find(id);
//            if (Sector == null)
//            {
//                return HttpNotFound();
//            }
//            return PartialView(Sector);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            SectorsMaster _sectorPurge = db.Sectors.Find(id);
//            _sectorPurge.PurgeFlag = true;
//            _sectorPurge.DeletedAt = DateTime.Now;
//            var _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
//            var _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
//            SectorAudit(AuditActions.DeleteRecord, _currentValues, _originalValues, _sectorPurge.ID);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        private void SectorAudit(AuditActions Action, DbPropertyValues CurrentValues, DbPropertyValues OriginalValues, int ID)
//        {
//            MasterTablesAuditController _sectorsAudit = new MasterTablesAuditController();
//            _sectorsAudit.Action = Action;
//            _sectorsAudit.TableName = "Sectors";
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
