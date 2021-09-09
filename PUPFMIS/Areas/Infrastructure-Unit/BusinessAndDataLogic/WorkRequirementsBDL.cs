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
    public class WorkRequirementsBDL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private LogsMasterTables log = new LogsMasterTables();

        public List<InfrastructureRequirementsClassification> GetClassifications()
        {
            return db.InfrastructureRequirementsClass.Where(d => d.PurgeFlag == false).ToList();
        }
        public List<InfrastructureRequirements> GetRequirements(bool DeleteFlag)
        {
            return db.InfrastructureRequirements.Where(d => d.PurgeFlag == DeleteFlag).ToList();
        }
        public InfrastructureRequirements GetRequirements(int? ID)
        {
            return db.InfrastructureRequirements.Find(ID);
        }
        public bool AddRequirements(InfrastructureRequirements Requirement)
        {
            Requirement.Requirement = Requirement.Requirement.ToUpper();
            Requirement.PurgeFlag = false;
            Requirement.CreatedAt = DateTime.Now;
            db.InfrastructureRequirements.Add(Requirement);
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = Requirement.ID;
            _log.ProcessedBy = null;
            _log.Action = "Add Record";
            _log.TableName = "PROC_MSTR_Infrastructure_Requirements";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, null, _currentValues);

            return true;
        }
        public bool UpdateRequirements(InfrastructureRequirements Requirement)
        {
            InfrastructureRequirements _requirement = GetRequirements(Requirement.ID);
            _requirement.Requirement = Requirement.Requirement.ToUpper();
            _requirement.RequirementClassificationReference = Requirement.RequirementClassificationReference;
            _requirement.UpdatedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = Requirement.ID;
            _log.ProcessedBy = null;
            _log.Action = "Update Record";
            _log.TableName = "PROC_MSTR_Infrastructure_Requirements";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            return true;
        }
        public bool PurgeRequirements(int? ID)
        {
            InfrastructureRequirements _requirement = GetRequirements(ID);
            _requirement.PurgeFlag = true;
            _requirement.DeletedAt = DateTime.Now;
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
            _log.TableName = "PROC_MSTR_Infrastructure_Requirements";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            return true;
        }
        public bool RestoreRequirements(int? ID)
        {
            InfrastructureRequirements _requirement = GetRequirements(ID);
            _requirement.PurgeFlag = false;
            _requirement.DeletedAt = DateTime.Now;
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
            _log.TableName = "PROC_MSTR_Infrastructure_Requirements";
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