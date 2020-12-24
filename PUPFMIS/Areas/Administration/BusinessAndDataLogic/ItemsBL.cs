using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ItemDataAccess : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private LogsMasterTables log = new LogsMasterTables();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private ABISDataAccess abisDataAccess = new ABISDataAccess();
        private AccountsManagementBL accountsBL = new AccountsManagementBL();
        private InventoryTypeDataAccess inventoryTypesBL = new InventoryTypeDataAccess();
        private ItemAllowedUsersDataAccess itemAllowedUsersDataAccess = new ItemAllowedUsersDataAccess();

        public List<Item> GetItems(bool DeleteFlag)
        {
            return db.Items.Where(d => d.PurgeFlag == DeleteFlag).OrderBy(d => d.ItemFullName).ToList();
        }
        public List<ItemVM> GetItemCatalogueView(bool DeleteFlag, string InventoryTypeCode, string DepartmentCode)
        {
            var itemViewModel = new List<ItemVM>();
            var UACS = abisDataAccess.GetChartOfAccounts();
            var unitPrices = db.ItemPrices.Where(d => d.IsPrevailingPrice == true).ToList();
            var exceptionItems = db.ItemAllowedUsers.GroupBy(d => d.ItemReference).Select(d => d.Key).ToList();
            var itemAllowedUsers = db.ItemAllowedUsers.Where(d => d.DepartmentReference == DepartmentCode).GroupBy(d => d.ItemReference).Select(d => d.Key).ToList();
            var allowedItems = db.Items.Where(d => d.PurgeFlag == false && d.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == InventoryTypeCode && itemAllowedUsers.Contains(d.ID)).ToList();
            var itemList = db.Items.Where(d => d.PurgeFlag == false && d.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == InventoryTypeCode && !exceptionItems.Contains(d.ID)).ToList();
            var allItems = itemList.Union(allowedItems).ToList();

            foreach (var item in allItems)
            {
                itemViewModel.Add(new ItemVM
                {
                    ID = item.ID,
                    ItemImage = item.ItemImage,
                    ItemCode = item.ItemCode,
                    ItemFullName = item.ItemFullName,
                    ItemType = item.FKItemTypeReference.ItemTypeName,
                    ItemShortSpecifications = item.ItemShortSpecifications,
                    ItemSpecifications = item.ItemSpecifications == null ? "Not Applicable" : item.ItemSpecifications,
                    IsTangible = item.FKItemTypeReference.FKInventoryTypeReference.IsTangible ? "Yes" : "No",
                    AccountClass = UACS.Where(d => d.UACS_Code == item.FKItemTypeReference.UACSObjectClass).FirstOrDefault().AcctName,
                    ProcurementSource = item.ProcurementSource,
                    Category = item.FKCategoryReference.ItemCategoryName,
                    IndividualUOMReference = item.IndividualUOMReference == null ? "Not Applicable" : item.FKIndividualUnitReference.UnitName,
                    PackagingUOMReference = item.PackagingUOMReference == null ? "Not Applicable" : item.FKPackagingUnitReference.UnitName,
                    InventoryType = item.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    QuantityPerPackage = item.QuantityPerPackage,
                    MinimumIssuanceQty = item.MinimumIssuanceQty,
                    UnitPrice = unitPrices.Where(d => d.Item == item.ID).FirstOrDefault().UnitPrice,
                    ResponsibilityCenter = item.ResponsibilityCenter == null ? "None" : hrisDataAccess.GetDepartmentDetails(item.ResponsibilityCenter).Department,
                    PurchaseRequestOffice = item.PurchaseRequestOffice == null ? "Requesting Office" : hrisDataAccess.GetDepartmentDetails(item.PurchaseRequestOffice).Department,
                    PurgeFlag = item.PurgeFlag ? "Yes" : "No",
                    CreatedAt = item.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"),
                    UpdatedAt = item.UpdatedAt == null ? "Not Applicable" : ((DateTime)item.UpdatedAt).ToString("dd MMMM yyyy hh:mm tt"),
                    DeletedAt = item.DeletedAt == null ? "Not Applicable" : ((DateTime)item.DeletedAt).ToString("dd MMMM yyyy hh:mm tt")
                });
            }

            return itemViewModel;
        }
        public List<ItemVM> GetItemCatalogueView(bool DeleteFlag, string DepartmentCode)
        {
            var itemViewModel = new List<ItemVM>();
            var UACS = abisDataAccess.GetChartOfAccounts();
            var unitPrices = db.ItemPrices.Where(d => d.IsPrevailingPrice == true).ToList();
            var exceptionItems = db.ItemAllowedUsers.GroupBy(d => d.ItemReference).Select(d => d.Key).ToList();
            var itemAllowedUsers = db.ItemAllowedUsers.Where(d => d.DepartmentReference == DepartmentCode).GroupBy(d => d.ItemReference).Select(d => d.Key).ToList();
            var allowedItems = db.Items.Where(d => d.PurgeFlag == false && itemAllowedUsers.Contains(d.ID)).ToList();
            var itemList = db.Items.Where(d => d.PurgeFlag == false && !exceptionItems.Contains(d.ID)).ToList();
            var allItems = itemList.Union(allowedItems).ToList();
            var offices = hrisDataAccess.GetAllDepartments();
            var unitsOfMeasure = db.UOM.ToList();

            foreach (var item in allItems)
            {
                itemViewModel.Add(new ItemVM
                {
                    ID = item.ID,
                    ItemImage = item.ItemImage,
                    ItemCode = item.ItemCode,
                    ItemFullName = item.ItemFullName,
                    ItemType = item.FKItemTypeReference.ItemTypeName,
                    ItemShortSpecifications = item.ItemShortSpecifications,
                    ItemSpecifications = item.ItemSpecifications == null ? "Not Applicable" : item.ItemSpecifications,
                    IsTangible = item.FKItemTypeReference.FKInventoryTypeReference.IsTangible ? "Yes" : "No",
                    AccountClass = UACS.Where(d => d.UACS_Code == item.FKItemTypeReference.UACSObjectClass).FirstOrDefault().AcctName,
                    ProcurementSource = item.ProcurementSource,
                    Category = item.FKCategoryReference.ItemCategoryName,
                    IndividualUOMReference = item.IndividualUOMReference == null ? "Not Applicable" : unitsOfMeasure.Where(d => d.ID == item.IndividualUOMReference).FirstOrDefault().UnitName,
                    PackagingUOMReference = item.PackagingUOMReference == null ? "Not Applicable" : unitsOfMeasure.Where(d => d.ID == item.PackagingUOMReference).FirstOrDefault().UnitName,
                    InventoryType = item.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    QuantityPerPackage = item.QuantityPerPackage,
                    MinimumIssuanceQty = item.MinimumIssuanceQty,
                    UnitPrice = item.FKItemTypeReference.FKInventoryTypeReference.IsTangible ? (decimal?)unitPrices.Where(d => d.Item == item.ID).FirstOrDefault().UnitPrice : null,
                    ResponsibilityCenter = item.ResponsibilityCenter == null ? "None" : offices.Where(d => d.DepartmentCode == item.ResponsibilityCenter).FirstOrDefault().Department,
                    PurchaseRequestOffice = item.PurchaseRequestOffice == null ? "Requesting Office" : offices.Where(d => d.DepartmentCode == item.PurchaseRequestOffice).FirstOrDefault().Department,
                    PurgeFlag = item.PurgeFlag ? "Yes" : "No",
                    CreatedAt = item.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"),
                    UpdatedAt = item.UpdatedAt == null ? "Not Applicable" : ((DateTime)item.UpdatedAt).ToString("dd MMMM yyyy hh:mm tt"),
                    DeletedAt = item.DeletedAt == null ? "Not Applicable" : ((DateTime)item.DeletedAt).ToString("dd MMMM yyyy hh:mm tt")
                });
            }

            return itemViewModel;
        }
        public List<ItemVM> GetItemView(bool DeleteFlag, string InventoryTypeCode)
        {
            var items = db.Items.Include(o => o.FKItemTypeReference).Where(d => d.PurgeFlag == DeleteFlag && d.FKItemTypeReference.FKInventoryTypeReference.InventoryCode == InventoryTypeCode).ToList();
            var UACS = abisDataAccess.GetChartOfAccounts();
            var unitPrices = db.ItemPrices.Where(d => d.IsPrevailingPrice == true).ToList();
            var itemViewModel = new List<ItemVM>();

            foreach (var item in items)
            {
                itemViewModel.Add(new ItemVM
                {
                    ID = item.ID,
                    ItemImage = item.ItemImage,
                    ItemCode = item.ItemCode,
                    ItemFullName = item.ItemFullName,
                    ItemType = item.FKItemTypeReference.ItemTypeName,
                    ItemShortSpecifications = item.ItemShortSpecifications,
                    ItemSpecifications = item.ItemSpecifications == null ? "Not Applicable" : item.ItemSpecifications,
                    IsTangible = item.FKItemTypeReference.FKInventoryTypeReference.IsTangible ? "Yes" : "No",
                    AccountClass = UACS.Where(d => d.UACS_Code == item.FKItemTypeReference.UACSObjectClass).FirstOrDefault().AcctName,
                    ProcurementSource = item.ProcurementSource,
                    Category = item.FKCategoryReference.ItemCategoryName,
                    IndividualUOMReference = item.IndividualUOMReference == null ? "Not Applicable" : item.FKIndividualUnitReference.UnitName,
                    PackagingUOMReference = item.PackagingUOMReference == null ? "Not Applicable" : item.FKPackagingUnitReference.UnitName,
                    InventoryType = item.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    QuantityPerPackage = item.QuantityPerPackage,
                    MinimumIssuanceQty = item.MinimumIssuanceQty,
                    UnitPrice = unitPrices.Where(d => d.Item == item.ID).FirstOrDefault().UnitPrice,
                    ResponsibilityCenter = item.ResponsibilityCenter == null ? "None" : hrisDataAccess.GetDepartmentDetails(item.ResponsibilityCenter).Department,
                    PurchaseRequestOffice = item.PurchaseRequestOffice == null ? "Requesting Office" : hrisDataAccess.GetDepartmentDetails(item.PurchaseRequestOffice).Department,
                    PurgeFlag = item.PurgeFlag ? "Yes" : "No",
                    CreatedAt = item.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"),
                    UpdatedAt = item.UpdatedAt == null ? "Not Applicable" : ((DateTime)item.UpdatedAt).ToString("dd MMMM yyyy hh:mm tt"),
                    DeletedAt = item.DeletedAt == null ? "Not Applicable" : ((DateTime)item.DeletedAt).ToString("dd MMMM yyyy hh:mm tt")
                });
            }

            return itemViewModel;
        }
        public ItemVM GetItemDetails(string ItemCode)
        {
            var item = db.Items.Include(o => o.FKItemTypeReference).Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            var UACS = abisDataAccess.GetChartOfAccounts(item.FKItemTypeReference.UACSObjectClass);
            var unitPriceRecord = db.ItemPrices.Where(d => d.FKItemReference.ItemCode == ItemCode && d.IsPrevailingPrice == true).FirstOrDefault();
            var unitPrice = unitPriceRecord == null ? null : (decimal?)unitPriceRecord.UnitPrice;

            return new ItemVM
            {
                ID = item.ID,
                ItemImage = item.ItemImage,
                ItemCode = item.ItemCode,
                ItemFullName = item.ItemFullName,
                ItemType = item.FKItemTypeReference.ItemTypeName,
                ItemShortSpecifications = item.ItemShortSpecifications,
                ItemSpecifications = item.ItemSpecifications == null ? "Not Applicable" : item.ItemSpecifications,
                IsTangible = item.FKItemTypeReference.FKInventoryTypeReference.IsTangible ? "Yes" : "No",
                AccountClass = UACS.AcctName,
                ProcurementSource = item.ProcurementSource,
                Category = item.FKCategoryReference.ItemCategoryName,
                IndividualUOMReference = item.IndividualUOMReference == null ? "Not Applicable" : item.FKIndividualUnitReference.UnitName,
                PackagingUOMReference = item.PackagingUOMReference == null ? "Not Applicable" : item.FKPackagingUnitReference.UnitName,
                InventoryType = item.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                QuantityPerPackage = item.QuantityPerPackage,
                MinimumIssuanceQty = item.MinimumIssuanceQty,
                UnitPrice = unitPrice,
                ResponsibilityCenter = item.ResponsibilityCenter == null ? "None" : hrisDataAccess.GetDepartmentDetails(item.ResponsibilityCenter).Department,
                PurchaseRequestOffice = item.PurchaseRequestOffice == null ? "Requesting Office" : hrisDataAccess.GetDepartmentDetails(item.PurchaseRequestOffice).Department,
                PurgeFlag = item.PurgeFlag ? "Yes" : "No",
                CreatedAt = item.CreatedAt.ToString("dd MMMM yyyy hh:mm tt"),
                UpdatedAt = item.UpdatedAt == null ? "Not Applicable" : ((DateTime)item.UpdatedAt).ToString("dd MMMM yyyy hh:mm tt"),
                DeletedAt = item.DeletedAt == null ? "Not Applicable" : ((DateTime)item.DeletedAt).ToString("dd MMMM yyyy hh:mm tt")
            };
        }
        public bool AddItemRecord(ItemVM itemViewModel, string UserEmail)
        {
            DbPropertyValues currentValues;
            ItemType itemType = db.ItemTypes.Where(d => d.ItemTypeCode == itemViewModel.ItemType).FirstOrDefault();

            if (itemType == null)
            {
                return false;
            }

            var item = new Item
            {
                ItemCode = GenerateItemCode(itemType.ItemTypeCode),
                ItemFullName = itemType.ItemTypeName.ToUpper() + "; " + itemViewModel.ItemShortSpecifications.ToUpper(),
                ItemImage = itemViewModel.ItemImage,
                ItemShortSpecifications = itemViewModel.ItemShortSpecifications,
                ItemSpecifications = itemViewModel.ItemSpecifications,
                ItemType = itemType.ID,
                CategoryReference = Convert.ToInt32(itemViewModel.Category),
                ResponsibilityCenter = itemViewModel.ResponsibilityCenter == "None" ? null : itemViewModel.ResponsibilityCenter,
                PurchaseRequestOffice = itemViewModel.PurchaseRequestOffice == "Requesting Office" ? null : itemViewModel.PurchaseRequestOffice,
                ProcurementSource = itemViewModel.ProcurementSource,
                PackagingUOMReference = itemViewModel.PackagingUOMReference == null ? null : (int?)Convert.ToUInt32(itemViewModel.PackagingUOMReference),
                QuantityPerPackage = itemViewModel.QuantityPerPackage,
                IndividualUOMReference = itemViewModel.IndividualUOMReference == null ? null : (int?)Convert.ToUInt32(itemViewModel.IndividualUOMReference),
                MinimumIssuanceQty = itemViewModel.MinimumIssuanceQty,
                CreatedAt = DateTime.Now,
                PurgeFlag = false
            };

            db.Items.Add(item);
            currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            log.Action = "Add Record";
            log.AuditableKey = item.ID;
            log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
            log.TableName = "MASTER_ITEM";
            MasterTablesLogger logger = new MasterTablesLogger();
            logger.Log(log, null, currentValues);

            if (itemType.FKInventoryTypeReference.IsTangible == true)
            {
                ItemPrice itemPrice = new ItemPrice
                {
                    UnitPrice = Convert.ToDecimal(itemViewModel.UnitPrice),
                    IsPrevailingPrice = true,
                    Item = item.ID,
                    CreatedAt = DateTime.Now
                };
                db.ItemPrices.Add(itemPrice);

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            if(itemViewModel.AllowedUsers != null)
            {
                foreach (var dept in itemViewModel.AllowedUsers)
                {
                    ItemAllowedUsers allowedUsers = new ItemAllowedUsers()
                    {
                        DepartmentReference = dept,
                        ItemReference = item.ID
                    };
                    db.ItemAllowedUsers.Add(allowedUsers);
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public bool UpdateItemRecord(ItemVM itemViewModel, string UserEmail)
        {
            Item item = db.Items.Where(d => d.ItemCode == itemViewModel.ItemCode).FirstOrDefault();

            item.ItemImage = itemViewModel.ItemImage;
            item.ItemFullName = item.FKItemTypeReference.ItemTypeName.ToUpper() + "; " + itemViewModel.ItemShortSpecifications.ToUpper();
            item.ItemShortSpecifications = itemViewModel.ItemShortSpecifications;
            item.ItemSpecifications = itemViewModel.ItemSpecifications;
            item.ProcurementSource = itemViewModel.ProcurementSource;
            item.CategoryReference = Convert.ToInt32(itemViewModel.Category);
            item.ResponsibilityCenter = itemViewModel.ResponsibilityCenter == "None" ? null : itemViewModel.ResponsibilityCenter;
            item.PurchaseRequestOffice = itemViewModel.PurchaseRequestOffice == "Requesting Office" ? null : itemViewModel.PurchaseRequestOffice;
            item.PackagingUOMReference = itemViewModel.PackagingUOMReference == null ? null : (int?)Convert.ToUInt32(itemViewModel.PackagingUOMReference);
            item.QuantityPerPackage = itemViewModel.QuantityPerPackage;
            item.IndividualUOMReference = itemViewModel.IndividualUOMReference == null ? null : (int?)Convert.ToUInt32(itemViewModel.IndividualUOMReference);
            item.MinimumIssuanceQty = itemViewModel.MinimumIssuanceQty;
            item.UpdatedAt = DateTime.Now;

            DbPropertyValues currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
            DbPropertyValues originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
            log.Action = "Update Record";
            log.AuditableKey = item.ID;
            log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
            log.TableName = "MASTER_ITEM_TYPES";
            MasterTablesLogger logger = new MasterTablesLogger();
            logger.Log(log, originalValues, currentValues);

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            ItemPrice previousPrice = db.ItemPrices.Where(d => d.Item == item.ID).OrderByDescending(d => d.UpdatedAt).FirstOrDefault();
            if (item.FKItemTypeReference.FKInventoryTypeReference.IsTangible == true && previousPrice.UnitPrice != itemViewModel.UnitPrice)
            {
                previousPrice.IsPrevailingPrice = false;
                previousPrice.UpdatedAt = DateTime.Now;
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                ItemPrice itemPrice = new ItemPrice
                {
                    UnitPrice = Convert.ToDecimal(itemViewModel.UnitPrice),
                    IsPrevailingPrice = true,
                    Item = item.ID,
                    CreatedAt = DateTime.Now
                };

                db.ItemPrices.Add(itemPrice);

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            if(db.ItemAllowedUsers.Where(d => d.FKItemReference.ItemCode == itemViewModel.ItemCode).Count() != 0)
            {
                db.ItemAllowedUsers.RemoveRange(db.ItemAllowedUsers.Where(d => d.FKItemReference.ItemCode == itemViewModel.ItemCode));
                db.SaveChanges();
            }

            if (itemViewModel.AllowedUsers != null)
            {
                foreach (var dept in itemViewModel.AllowedUsers)
                {
                    ItemAllowedUsers allowedUsers = new ItemAllowedUsers()
                    {
                        DepartmentReference = dept,
                        ItemReference = item.ID
                    };
                    db.ItemAllowedUsers.Add(allowedUsers);
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public bool DeleteItemRecord(string ItemCode, string UserEmail)
        {
            Item item = db.Items.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            DbPropertyValues currentValues;
            DbPropertyValues originalValues;

            if (item != null)
            {
                item.PurgeFlag = true;
                item.DeletedAt = DateTime.Now;

                log.Action = "Purge Record";

                currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                log.AuditableKey = item.ID;
                log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
                log.TableName = "MASTER_ITEMS";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, originalValues, currentValues);

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            return true;
        }
        public bool RestoreItemRecord(string ItemCode, string UserEmail)
        {
            Item item = db.Items.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            DbPropertyValues currentValues;
            DbPropertyValues originalValues;

            if (item != null)
            {
                item.PurgeFlag = false;
                item.DeletedAt = null;
                item.UpdatedAt = DateTime.Now;

                log.Action = "Restore Record";

                currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                log.AuditableKey = item.ID;
                log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
                log.TableName = "master_items";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, originalValues, currentValues);

                if (db.SaveChanges() == 1)
                {
                    return true;
                }
            }
            return false;
        }
        private string GenerateItemCode(string ItemTypeCode)
        {
            var series = (db.Items.Where(d => d.FKItemTypeReference.ItemTypeCode == ItemTypeCode).Count() + 1).ToString();
            series = series.Length == 2 ? series : "0" + series;
            var ItemCode = ItemTypeCode + "-" + series;
            return ItemCode;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                accountsBL.Dispose();
                hrisDataAccess.Dispose();
                abisDataAccess.Dispose();
                inventoryTypesBL.Dispose();
                itemAllowedUsersDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}