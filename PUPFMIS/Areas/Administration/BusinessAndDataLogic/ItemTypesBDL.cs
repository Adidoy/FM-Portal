using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ItemTypesDataAccess : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private LogsMasterTables log = new LogsMasterTables();

        public List<HRISDepartment> GetDepartments()
        {
            var departments = hris.GetAllDepartments();
            departments.Add(new HRISDepartment {
                DepartmentID = 0,
                DepartmentCode = "_REQOFF",
                Department = "Requesting Office"
            });
            return departments.OrderBy(d => d.DepartmentID).ToList();
        }
        public void Validate(ItemTypes ItemType, out string Key, out List<string> Message)
        {
            Key = null;
            Message = new List<string>();

            ItemTypeValidator validator = new ItemTypeValidator();
            var validationResult = validator.Validate(ItemType);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    Key = validationResult.Errors.Count() > 1 ? "" : error.PropertyName;
                    Message.Add(error.ErrorMessage);
                }

            }
        }
        public List<ItemClassification> GetItemClassifications()
        {
            return db.ItemClassification.OrderBy(d => d.Classification).ToList();
        }
        public List<ItemTypes> GetItemTypes()
        {
            return (from types in db.ItemTypes.ToList()
                    join resp in hris.GetAllDepartments() on types.ResponsibilityCenter equals resp.DepartmentCode into responsibilityCenters
                    from m in responsibilityCenters.DefaultIfEmpty()
                    join purq in hris.GetAllDepartments() on types.PurchaseRequestCenter equals purq.DepartmentCode into purchaseRequestCenters
                    from x in purchaseRequestCenters.DefaultIfEmpty()
                    select new ItemTypes
                    {
                        ID = types.ID,
                        ItemTypeCode = types.ItemTypeCode,
                        ItemType = types.ItemType,
                        FKItemClassificationReference = types.FKItemClassificationReference,
                        ResponsibilityCenter = m == null ? "Requesting Office" : m.Department,
                        PurchaseRequestCenter = x == null ? "Requesting Office" : x.Department,
                        PurgeFlag = types.PurgeFlag
                    }).OrderBy(d => d.ItemType).ToList();
        }
        public ItemTypes GetItemTypes(string ItemTypeCode)
        {
            var itemType = db.ItemTypes.Where(d => d.ItemTypeCode == ItemTypeCode).FirstOrDefault();
            itemType.ResponsibilityCenter = itemType.ResponsibilityCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(itemType.ResponsibilityCenter).DepartmentCode;
            itemType.PurchaseRequestCenter = itemType.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(itemType.PurchaseRequestCenter).DepartmentCode;
            return itemType;
        }
        public bool AddItemTypeRecord(ItemTypes ItemType, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            DbPropertyValues currentValues;
            ItemTypes itemType = db.ItemTypes.Where(d => d.ItemTypeCode == ItemType.ItemTypeCode).FirstOrDefault();

            if (itemType != null)
            {
                return false;
            }

            itemType = new ItemTypes
            {
                ItemTypeCode = ItemType.ItemTypeCode,
                ItemType = ItemType.ItemType,
                ItemClassificationReference = ItemType.ItemClassificationReference,
                ResponsibilityCenter = ItemType.ResponsibilityCenter == "_REQOFF" ? null : ItemType.ResponsibilityCenter,
                PurchaseRequestCenter = ItemType.PurchaseRequestCenter == "_REQOFF" ? null : ItemType.PurchaseRequestCenter,
                CreatedAt = DateTime.Now,
                PurgeFlag = false
            };

            db.ItemTypes.Add(itemType);
            currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            log.Action = "Add Record";
            log.AuditableKey = itemType.ID;
            log.ProcessedBy = user.ID;
            log.TableName = "PROC_MSTR_Item_Types";
            MasterTablesLogger logger = new MasterTablesLogger();
            logger.Log(log, null, currentValues);

            return true;
        }
        public bool UpdateItemTypeRecord(ItemTypes ItemType, string UserEmail, out string Key, out string Message)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var itemType = db.ItemTypes.Find(ItemType.ID);
            DbPropertyValues currentValues;
            DbPropertyValues originalValues;

            if(itemType.ItemTypeCode != ItemType.ItemTypeCode && db.ItemTypes.Where(d => d.ItemTypeCode == ItemType.ItemTypeCode).Count() != 0)
            {
                Key = "ItemTypeCode";
                Message = "Item Type Code already exists in the database.";
                return false;
            }

            if (itemType.ItemType != ItemType.ItemType && db.ItemTypes.Where(d => d.ItemType == ItemType.ItemType).Count() != 0)
            {
                Key = "ItemType";
                Message = "Item Type already exists in the database.";
                return false;
            }

            if (itemType != null)
            {
                itemType.ItemTypeCode = ItemType.ItemTypeCode;
                itemType.ItemType = ItemType.ItemType;
                itemType.ItemClassificationReference = ItemType.ItemClassificationReference;
                itemType.ResponsibilityCenter = ItemType.ResponsibilityCenter == "_REQOFF" ? null : ItemType.ResponsibilityCenter;
                itemType.PurchaseRequestCenter = ItemType.PurchaseRequestCenter == "_REQOFF" ? null : ItemType.PurchaseRequestCenter;
                itemType.UpdatedAt = DateTime.Now;

                currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;

                if (db.SaveChanges() == 0)
                {
                    Key = null;
                    Message = null;
                    return false;
                }

                log.AuditableKey = itemType.ID;
                log.ProcessedBy = user.ID;
                log.Action = "Update Record";
                log.TableName = "PROC_MSTR_Item_Types";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(log, originalValues, currentValues);
            }

            Key = null;
            Message = null;
            return true;
        }
        public bool DeleteItemTypeRecord(ItemTypes ItemType, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var itemType = db.ItemTypes.Find(ItemType.ID);
            DbPropertyValues currentValues;
            DbPropertyValues originalValues;

            if (itemType != null)
            {
                itemType.ItemTypeCode = ItemType.ItemTypeCode;
                itemType.ItemType = ItemType.ItemType;
                itemType.ItemClassificationReference = ItemType.ItemClassificationReference;
                itemType.PurgeFlag = true;
                itemType.DeletedAt = DateTime.Now;

                currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;

                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                log.AuditableKey = itemType.ID;
                log.ProcessedBy = user.ID;
                log.Action = "Purge Record";
                log.TableName = "PROC_MSTR_Item_Types";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(log, originalValues, currentValues);
            }

            return true;
        }
        public bool RestoreItemTypeRecord(ItemTypes ItemType, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var itemType = db.ItemTypes.Find(ItemType.ID);
            DbPropertyValues currentValues;
            DbPropertyValues originalValues;

            if (itemType != null)
            {
                itemType.ItemTypeCode = ItemType.ItemTypeCode;
                itemType.ItemType = ItemType.ItemType;
                itemType.ItemClassificationReference = ItemType.ItemClassificationReference;
                itemType.PurgeFlag = false;
                itemType.UpdatedAt = DateTime.Now;

                currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;

                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                log.AuditableKey = itemType.ID;
                log.ProcessedBy = user.ID;
                log.Action = "Restore Record";
                log.TableName = "PROC_MSTR_Item_Types";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(log, originalValues, currentValues);
            }

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