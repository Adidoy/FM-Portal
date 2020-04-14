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
    public class SuppliersBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        LogsMasterTables _log = new LogsMasterTables();

        public List<Supplier> GetActiveSuppliers()
        {
            return db.Suppliers.Where(d => d.PurgeFlag == false).ToList();
        }

        public List<Supplier> GetPurgedSuppliers()
        {
            return db.Suppliers.Where(d => d.PurgeFlag == true).ToList();
        }

        public Supplier GetSupplierDetails(int? SupplierID)
        {
            return db.Suppliers.Find(SupplierID);
        }

        public bool AddSupplierRecord(Supplier supplier)
        {
            Supplier _supplier = db.Suppliers.Find(supplier.ID);
            DbPropertyValues _currentValues;

            if (_supplier == null)
            {
                supplier.PurgeFlag = false;
                supplier.CreatedAt = DateTime.Now;
                db.Suppliers.Add(supplier);

                _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
                _log.Action = "Add Record";

                if (db.SaveChanges() == 1)
                {
                    _log.AuditableKey = supplier.ID;
                    _log.ProcessedBy = null;
                    _log.TableName = "master_suppliers";
                    MasterTablesLogger _logger = new MasterTablesLogger();
                    _logger.Log(_log, null, _currentValues);
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool UpdateSupplierRecord(Supplier supplier, bool deleteFlag)
        {
            Supplier _supplier = db.Suppliers.Find(supplier.ID);
            DbPropertyValues _currentValues;
            DbPropertyValues _originalValues;

            if (_supplier != null)
            {
                if (deleteFlag == false)
                {
                    _supplier.SupplierName = supplier.SupplierName;
                    _supplier.Address = supplier.Address;
                    _supplier.ContactNumber = supplier.ContactNumber;
                    _supplier.AlternateContactNumber = supplier.AlternateContactNumber;
                    _supplier.EmailAddress = supplier.EmailAddress;
                    _supplier.Website = supplier.Website;
                    _supplier.UpdatedAt = DateTime.Now;

                    _log.Action = "Update Record";
                }
                else
                {
                    _supplier.PurgeFlag = true;
                    _supplier.DeletedAt = DateTime.Now;

                    _log.Action = "Purge Record";
                }

                _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                _log.AuditableKey = supplier.ID;
                _log.ProcessedBy = null;
                _log.TableName = "master_suppliers";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(_log, _originalValues, _currentValues);

                if (db.SaveChanges() == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool RestoreSupplierRecord(Supplier supplier)
        {
            Supplier _supplier = db.Suppliers.Find(supplier.ID);
            DbPropertyValues _currentValues;
            DbPropertyValues _originalValues;

            if (_supplier != null)
            {
                _supplier.PurgeFlag = false;
                _supplier.UpdatedAt = DateTime.Now;
                _supplier.DeletedAt = null;
                _log.Action = "Restore Record";
                _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                _log.AuditableKey = supplier.ID;
                _log.ProcessedBy = null;
                _log.TableName = "master_suppliers";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(_log, _originalValues, _currentValues);

                if (db.SaveChanges() == 1)
                {
                    return true;
                }
            }
            return false;
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