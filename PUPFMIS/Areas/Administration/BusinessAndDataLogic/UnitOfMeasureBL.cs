using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;


namespace PUPFMIS.BusinessAndDataLogic
{
    public class UnitOfMeasureDataAccess : Controller
    {
        private FMISDbContext db = new FMISDbContext();

        public List<UnitOfMeasure> GetUOMs(bool DeleteFlag)
        {
            return db.UOM.Where(d => d.PurgeFlag == DeleteFlag).OrderBy(d => d.UnitName).ToList();
        }
        public UnitOfMeasure GetUOM(int? ID)
        {
            return db.UOM.Find(ID);
        }
        public bool AddUOM(UnitOfMeasure UOM)
        {
            UOM.PurgeFlag = false;
            UOM.CreatedAt = DateTime.Now;
            db.UOM.Add(UOM);
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = UOM.ID;
            _log.ProcessedBy = null;
            _log.Action = "Add Record";
            _log.TableName = "PROC_MSTR_UnitsOfMeasure";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, null, _currentValues);

            return true;
        }
        public bool UpdateUOM(UnitOfMeasure UOM)
        {
            UnitOfMeasure _unitOfMeasureUpdate = GetUOM(UOM.ID);
            _unitOfMeasureUpdate.UnitName = UOM.UnitName;
            _unitOfMeasureUpdate.Abbreviation = UOM.Abbreviation;
            _unitOfMeasureUpdate.UpdatedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;

            if(db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = UOM.ID;
            _log.ProcessedBy = null;
            _log.Action = "Update Record";
            _log.TableName = "PROC_MSTR_UnitsOfMeasure";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            return true;
        }
        public bool PurgeUOM(int? ID)
        {
            UnitOfMeasure _unitOfMeasureDelete = GetUOM(ID);
            _unitOfMeasureDelete.PurgeFlag = true;
            _unitOfMeasureDelete.DeletedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;

            if(db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = (int)ID;
            _log.ProcessedBy = null;
            _log.Action = "Purge Record";
            _log.TableName = "PROC_MSTR_UnitsOfMeasure";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            return true;
        }
        public bool RestoreUOM(int? ID)
        {
            UnitOfMeasure _unitOfMeasureDelete = GetUOM(ID);
            _unitOfMeasureDelete.PurgeFlag = false;
            _unitOfMeasureDelete.DeletedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;

            if(db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = (int)ID;
            _log.ProcessedBy = null;
            _log.Action = "Record Restored";
            _log.TableName = "PROC_MSTR_UnitsOfMeasure";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            return true;
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