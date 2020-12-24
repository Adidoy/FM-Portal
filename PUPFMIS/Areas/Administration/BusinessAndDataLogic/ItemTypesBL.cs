using FluentValidation.Results;
using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ItemTypeBusinessLogic : Controller
    {
        FMISDbContext db = new FMISDbContext();
        public void Validate(ItemTypeVM ItemType)
        {
            ItemTypeValidator validator = new ItemTypeValidator();
            ValidationResult result = validator.Validate(ItemType);
            if (!result.IsValid)
            {
                foreach (ValidationFailure error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
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
    public class ItemTypeDataAccess : Controller
    {
        FMISDbContext db = new FMISDbContext();
        ABISDataAccess abis = new ABISDataAccess();
        HRISDataAccess hris = new HRISDataAccess();

        public List<ItemTypeVM> GetItemTypes(bool DeleteFlag)
        {
            var itemTypes = db.ItemTypes.Include(i => i.FKInventoryTypeReference).Where(d => d.PurgeFlag == DeleteFlag).OrderBy(d => d.ItemTypeName).ToList();
            return itemTypes.Select(d => new ItemTypeVM
            {
                ID = d.ID,
                ItemTypeCode = d.ItemTypeCode,
                ItemTypeName = d.ItemTypeName,
                InventoryTypeName = d.FKInventoryTypeReference.InventoryTypeName,
                UACSObjectClass = abis.GetChartOfAccounts(d.UACSObjectClass).AcctName,
                IsTangible = d.FKInventoryTypeReference.IsTangible ? "Yes" : "No",
                PurgeFlag = d.PurgeFlag,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
                DeletedAt = d.DeletedAt
            }).ToList();
        }
        public ItemTypeVM GetItemType(string ItemTypeCode, bool DeleteFlag)
        {
            var itemType = db.ItemTypes.Where(d => d.ItemTypeCode == ItemTypeCode && d.PurgeFlag == DeleteFlag).FirstOrDefault();
            return new ItemTypeVM
            {
                ID = itemType.ID,
                ItemTypeCode = itemType.ItemTypeCode,
                ItemTypeName = itemType.ItemTypeName,
                UACSObjectClass = itemType.UACSObjectClass,
                InventoryTypeName = itemType.FKInventoryTypeReference.InventoryTypeName,
                InventoryTypeReference = itemType.InventoryTypeReference,
                IsSpecificationUserDefined = itemType.IsSpecificationUserDefined,
                IsTangible = itemType.FKInventoryTypeReference.IsTangible ? "Yes" : "No",
                ResponsibilityCenter = itemType.FKInventoryTypeReference.ResponsibilityCenter
            };
        }
        public ItemTypeVM GetItemTypeDetails(string ItemTypeCode, bool DeleteFlag)
        {
            var itemTypes = db.ItemTypes.Where(d => d.ItemTypeCode == ItemTypeCode && d.PurgeFlag == DeleteFlag).FirstOrDefault();
            return new ItemTypeVM
            {
                ID = itemTypes.ID,
                ItemTypeCode = itemTypes.ItemTypeCode,
                ItemTypeName = itemTypes.ItemTypeName,
                InventoryTypeName = itemTypes.FKInventoryTypeReference.InventoryTypeName,
                UACSObjectClass = abis.GetChartOfAccounts(itemTypes.UACSObjectClass).AcctName,
                IsTangible = itemTypes.FKInventoryTypeReference.IsTangible ? "Yes" : "No",
                ResponsibilityCenter = itemTypes.FKInventoryTypeReference.ResponsibilityCenter == null ? "None" : itemTypes.FKInventoryTypeReference.ResponsibilityCenter,
                PurgeFlag = itemTypes.PurgeFlag,
                CreatedAt = itemTypes.CreatedAt,
                UpdatedAt = itemTypes.UpdatedAt,
                DeletedAt = itemTypes.DeletedAt
            };
        }
        public bool AddItemTypeRecord(ItemTypeVM ItemType, string UserEmail)
        {
            DbPropertyValues currentValues;
            ItemType itemType = db.ItemTypes.Find(ItemType.ID);
            LogsMasterTables log = new LogsMasterTables();
            AccountsManagementBL accountsBL = new AccountsManagementBL();

            if (itemType == null)
            {
                itemType = db.ItemTypes.Add(new ItemType {
                    ItemTypeCode = GenerateItemCode(ItemType.InventoryTypeReference),
                    ItemTypeName = ItemType.ItemTypeName,
                    UACSObjectClass = ItemType.UACSObjectClass,
                    InventoryTypeReference = ItemType.InventoryTypeReference,
                    IsSpecificationUserDefined = ItemType.IsSpecificationUserDefined,
                    PurgeFlag = false,
                    CreatedAt = DateTime.Now
                });

                currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
                log.Action = "Add Record";

                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                log.AuditableKey = ItemType.ID;
                log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
                log.TableName = "PROC_Master_ItemTypes";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, null, currentValues);

                return true;
            }
            return false;
        }
        public bool UpdateItemTypeRecord(ItemType ItemType, string UserEmail)
        {
            LogsMasterTables log = new LogsMasterTables();
            AccountsManagementBL accountsBL = new AccountsManagementBL();
            ItemType itemType = db.ItemTypes.Where(d => d.ItemTypeCode == ItemType.ItemTypeCode).FirstOrDefault();

            if (itemType != null)
            {
                if (itemType.InventoryTypeReference != ItemType.InventoryTypeReference || itemType.UACSObjectClass != ItemType.UACSObjectClass)
                {
                    ItemType.ItemTypeCode = GenerateItemCode(ItemType.InventoryTypeReference);
                    ItemType.PurgeFlag = false;
                    ItemType.CreatedAt = DateTime.Now;
                    db.ItemTypes.Add(ItemType);

                    DbPropertyValues currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
                    log.Action = "Add Record";

                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }

                    log.AuditableKey = ItemType.ID;
                    log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
                    log.TableName = "MASTER_ITEM_TYPES";
                    MasterTablesLogger logger = new MasterTablesLogger();
                    logger.Log(log, null, currentValues);

                    itemType.PurgeFlag = true;
                    itemType.DeletedAt = DateTime.Now;

                    currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                    DbPropertyValues originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                    log.Action = "Update Record";
                    log.AuditableKey = itemType.ID;
                    log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
                    log.TableName = "MASTER_ITEM_TYPES";
                    logger = new MasterTablesLogger();
                    logger.Log(log, originalValues, currentValues);

                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    itemType.ItemTypeName = ItemType.ItemTypeName;
                    itemType.InventoryTypeReference = ItemType.InventoryTypeReference;
                    itemType.UACSObjectClass = ItemType.UACSObjectClass;
                    itemType.IsSpecificationUserDefined = ItemType.IsSpecificationUserDefined;
                    itemType.UpdatedAt = DateTime.Now;

                    DbPropertyValues currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                    DbPropertyValues originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                    log.Action = "Update Record";
                    log.AuditableKey = itemType.ID;
                    log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
                    log.TableName = "MASTER_ITEM_TYPES";
                    MasterTablesLogger logger = new MasterTablesLogger();
                    logger.Log(log, originalValues, currentValues);

                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }

                    return true;
                }
            }
            return false;
        }
        public bool PurgeItemTypeRecord(string ItemTypeCode, string UserEmail)
        {
            LogsMasterTables log = new LogsMasterTables();
            AccountsManagementBL accountsBL = new AccountsManagementBL();
            ItemType itemType = db.ItemTypes.Where(d => d.ItemTypeCode == ItemTypeCode).FirstOrDefault();

            if (itemType != null)
            {
                itemType.PurgeFlag = true;
                itemType.DeletedAt = DateTime.Now;
                DbPropertyValues currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                DbPropertyValues originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                log.Action = "Purge Record";
                log.AuditableKey = itemType.ID;
                log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID; 
                log.TableName = "MASTER_ITEM_TYPES";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, originalValues, currentValues);

                if (db.SaveChanges() == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool RestoreItemTypeRecord(string ItemTypeCode, string UserEmail)
        {
            LogsMasterTables log = new LogsMasterTables();
            AccountsManagementBL accountsBL = new AccountsManagementBL();
            ItemType itemType = db.ItemTypes.Where(d => d.ItemTypeCode == ItemTypeCode).FirstOrDefault();

            if (itemType != null)
            {
                itemType.PurgeFlag = false;
                itemType.DeletedAt = null;
                itemType.UpdatedAt = DateTime.Now;
                DbPropertyValues currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                DbPropertyValues originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                log.Action = "Purge Record";
                log.AuditableKey = itemType.ID;
                log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
                log.TableName = "MASTER_ITEM_TYPES";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, originalValues, currentValues);

                if (db.SaveChanges() == 1)
                {
                    return true;
                }
            }
            return false;
        }
        private string GenerateItemCode(int InventoryTypeID)
        {
            string ItemCode = string.Empty;
            var itemTypeCount = (db.ItemTypes.Where(d => d.FKInventoryTypeReference.ID == InventoryTypeID).Count() + 1).ToString();
            ItemCode = InventoryTypeID + (itemTypeCount.Length == 1 ? "00" + itemTypeCount : itemTypeCount.Length == 2 ? "0" + itemTypeCount : itemTypeCount);
            return ItemCode;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                abis.Dispose();
                hris.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}