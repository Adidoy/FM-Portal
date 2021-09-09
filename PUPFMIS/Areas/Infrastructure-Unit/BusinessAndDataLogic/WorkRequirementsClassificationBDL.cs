using FluentValidation.Results;
using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class WorkRequirementsClassificationBDL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private LogsMasterTables log = new LogsMasterTables();

        public List<InfrastructureRequirementsClassification> GetClassifications(bool DeleteFlag)
        {
            return db.InfrastructureRequirementsClass.Where(d => d.PurgeFlag == DeleteFlag).ToList();
        }
        public InfrastructureRequirementsClassification GetClassifications(int? ID)
        {
            return db.InfrastructureRequirementsClass.Find(ID);
        }
        public bool AddClassifications(InfrastructureRequirementsClassification RequirementClass)
        {
            RequirementClass.ClassificationName = RequirementClass.ClassificationName.ToUpper();
            RequirementClass.PurgeFlag = false;
            RequirementClass.CreatedAt = DateTime.Now;
            db.InfrastructureRequirementsClass.Add(RequirementClass);
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = RequirementClass.ID;
            _log.ProcessedBy = null;
            _log.Action = "Add Record";
            _log.TableName = "PROC_MSTR_Infrastructure_Classification";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, null, _currentValues);

            return true;
        }
        public bool UpdateClassifications(InfrastructureRequirementsClassification RequirementClass)
        {
            InfrastructureRequirementsClassification _requirementClass = GetClassifications(RequirementClass.ID);
            _requirementClass.ClassificationName = RequirementClass.ClassificationName.ToUpper();
            _requirementClass.UpdatedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = RequirementClass.ID;
            _log.ProcessedBy = null;
            _log.Action = "Update Record";
            _log.TableName = "PROC_MSTR_Infrastructure_Classification";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            return true;
        }
        public bool PurgeClassifications(int? ID)
        {
            InfrastructureRequirementsClassification _requirementClass = GetClassifications(ID);
            _requirementClass.PurgeFlag = true;
            _requirementClass.DeletedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = (int)ID;
            _log.ProcessedBy = null;
            _log.Action = "Purge Record";
            _log.TableName = "PROC_MSTR_Infrastructure_Classification";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            return true;
        }
        public bool RestoreClassifications(int? ID)
        {
            InfrastructureRequirementsClassification _requirementClass = GetClassifications(ID);
            _requirementClass.PurgeFlag = false;
            _requirementClass.DeletedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = (int)ID;
            _log.ProcessedBy = null;
            _log.Action = "Record Restored";
            _log.TableName = "PROC_MSTR_Infrastructure_Classification";
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