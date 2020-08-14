using FluentValidation.Results;
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
    public class ItemTypeBusinessLogic : Controller
    {
        FMISDbContext db = new FMISDbContext();
        public void Validate(ItemType ItemType)
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

        public List<ItemCategory> GetCategories()
        {
            return db.ItemCategories.Where(d => d.PurgeFlag == false).OrderBy(d => d.ItemCategoryName).ToList();
        }
        public List<ItemTypeVM> GetItemTypes(bool DeleteFlag)
        {
            var itemTypes = db.ItemTypes.Include(i => i.FKInventoryTypeReference).Where(d => d.PurgeFlag == DeleteFlag).OrderBy(d => d.ItemTypeName).ToList();
            return itemTypes.Select(d => new ItemTypeVM
            {
                ID = d.ID,
                ItemTypeCode = d.ItemTypeCode,
                ItemTypeName = d.ItemTypeName,
                InventoryTypeReference = d.FKInventoryTypeReference.InventoryTypeName,
                AccountClass = abis.GetChartOfAccounts(d.AccountClass).AcctName,
                IsTangible = d.FKInventoryTypeReference.IsTangible ? "Yes" : "No",
                PurgeFlag = d.PurgeFlag ? "Yes" : "No",
                CreatedAt = d.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"),
                UpdatedAt = d.UpdatedAt != null ? ((DateTime)d.UpdatedAt).ToString("dd MMMM yyyy hh:mm tt") : "Not Applicable",
                DeletedAt = d.DeletedAt != null ? ((DateTime)d.DeletedAt).ToString("dd MMMM yyyy hh:mm tt") : "Not Applicable"
            }).ToList();
        }
        public ItemType GetItemType(string ItemTypeCode, bool DeleteFlag)
        {
            return db.ItemTypes.Where(d => d.ItemTypeCode == ItemTypeCode && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }
        public ItemTypeVM GetItemTypeDetails(string ItemTypeCode, bool DeleteFlag)
        {
            var itemTypes = db.ItemTypes.Where(d => d.ItemTypeCode == ItemTypeCode && d.PurgeFlag == DeleteFlag).FirstOrDefault();
            return new ItemTypeVM
            {
                ID = itemTypes.ID,
                ItemTypeCode = itemTypes.ItemTypeCode,
                ItemTypeName = itemTypes.ItemTypeName,
                InventoryTypeReference = itemTypes.FKInventoryTypeReference.InventoryTypeName,
                AccountClass = abis.GetChartOfAccounts(itemTypes.AccountClass).AcctName,
                IsTangible = itemTypes.FKInventoryTypeReference.IsTangible ? "Yes" : "No",
                ResponsibilityCenter = itemTypes.FKInventoryTypeReference.ResponsibilityCenter == null ? "None" : itemTypes.FKInventoryTypeReference.ResponsibilityCenter,
                PurgeFlag = itemTypes.PurgeFlag ? "Yes" : "No",
                CreatedAt = itemTypes.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"),
                UpdatedAt = itemTypes.UpdatedAt != null ? ((DateTime)itemTypes.UpdatedAt).ToString("dd MMMM yyyy hh:mm tt") : "Not Applicable",
                DeletedAt = itemTypes.DeletedAt != null ? ((DateTime)itemTypes.DeletedAt).ToString("dd MMMM yyyy hh:mm tt") : "Not Applicable"
            };
        }
        public bool AddItemTypeRecord(ItemType ItemType, string UserEmail)
        {
            DbPropertyValues currentValues;
            Item item = db.Items.Find(ItemType.ID);
            LogsMasterTables log = new LogsMasterTables();
            AccountsManagementBL accountsBL = new AccountsManagementBL();

            if (item == null)
            {
                ItemType.ItemTypeCode = GenerateItemCode(ItemType.InventoryTypeReference);
                ItemType.PurgeFlag = false;
                ItemType.CreatedAt = DateTime.Now;
                db.ItemTypes.Add(ItemType);

                currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
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
                if (itemType.InventoryTypeReference != ItemType.InventoryTypeReference || itemType.AccountClass != ItemType.AccountClass)
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
                    itemType.AccountClass = ItemType.AccountClass;
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
            ItemCode = InventoryTypeID + "-" + (itemTypeCount.Length == 1 ? "00" + itemTypeCount : itemTypeCount.Length == 2 ? "0" + itemTypeCount : itemTypeCount);
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