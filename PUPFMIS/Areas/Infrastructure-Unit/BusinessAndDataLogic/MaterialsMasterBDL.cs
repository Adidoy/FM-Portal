using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class MaterialsMasterBDL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private LogsMasterTables log = new LogsMasterTables();

        public List<UnitOfMeasure> GetUOM()
        {
            return db.UOM.Where(d => d.PurgeFlag == false).OrderBy(d => d.UnitName).ToList();
        }
        public List<InfrastructureRequirementsClassification> GetWorkClassifications()
        {
            return db.InfrastructureRequirementsClass.Where(d => d.PurgeFlag == false).ToList();
        }
        public List<InfrastructureRequirements> GetWorkRequirements(int WorkClassID)
        {
            return db.InfrastructureRequirements.Where(d => d.PurgeFlag == false && d.RequirementClassificationReference == WorkClassID).ToList();
        }
        public List<InfrastructureMaterials> GetMaterials()
        {
            return db.InfrastructureMaterials.Where(d => d.PurgeFlag == false).ToList();
        }
        public List<InfrastructureMaterials> GetMaterials(bool DeleteFlag)
        {
            return db.InfrastructureMaterials.Where(d => d.PurgeFlag == DeleteFlag).ToList();
        }
        public InfrastructureMaterials GetMaterials(int? ID)
        {
            return db.InfrastructureMaterials.Find(ID);
        }
        public bool AddMaterial(InfrastructureMaterials Material)
        {
            Material.WorkRequirementReference = Material.WorkRequirementReference >= 1 ? Material.WorkRequirementReference : null;
            Material.ItemName = Material.ItemName.ToUpper();
            Material.PurgeFlag = false;
            Material.CreatedAt = DateTime.Now;
            db.InfrastructureMaterials.Add(Material);
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
            if (db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = Material.ID;
            _log.ProcessedBy = null;
            _log.Action = "Add Record";
            _log.TableName = "PROC_MSTR_Infrastructure_Materials";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, null, _currentValues);

            return true;
        }
        public bool UpdateMaterial(InfrastructureMaterials Material)
        {
            InfrastructureMaterials _material = GetMaterials(Material.ID);
            _material.ItemName = Material.ItemName.ToUpper();
            _material.ItemSpecifications = Material.ItemSpecifications;
            _material.UOMReference = Material.UOMReference;
            _material.WorkClassificationReference = Material.WorkClassificationReference;
            _material.WorkRequirementReference = Material.WorkRequirementReference >= 1 ? Material.WorkRequirementReference : null;
            _material.UpdatedAt = DateTime.Now;
            var _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().CurrentValues;
            var _originalValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Modified).First().OriginalValues;

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            LogsMasterTables _log = new LogsMasterTables();
            _log.AuditableKey = Material.ID;
            _log.ProcessedBy = null;
            _log.Action = "Update Record";
            _log.TableName = "PROC_MSTR_Infrastructure_Materials";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            return true;
        }
        public bool PurgeMaterials(int? ID)
        {
            InfrastructureMaterials _material = GetMaterials(ID);
            _material.PurgeFlag = true;
            _material.DeletedAt = DateTime.Now;
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
            _log.TableName = "PROC_MSTR_Infrastructure_Material";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            return true;
        }
        public bool RestoreMaterials(int? ID)
        {
            InfrastructureMaterials _material = GetMaterials(ID);
            _material.PurgeFlag = false;
            _material.DeletedAt = DateTime.Now;
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
            _log.TableName = "PROC_MSTR_Infrastructure_Materials";
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