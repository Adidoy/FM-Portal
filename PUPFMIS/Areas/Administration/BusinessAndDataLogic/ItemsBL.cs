using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using PUPFMIS.Models.HRIS;
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
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();

        public List<Item> GetItems()
        {
            return db.Items.ToList().OrderBy(d => d.ItemFullName).OrderBy(d => d.ItemFullName).ToList();
        }
        public List<UnitOfMeasure> GetUOMs()
        {
            return db.UOM.Where(d => d.PurgeFlag == false).OrderBy(d => d.UnitName).ToList();
        }
        public List<SelectListItem> GetItemArticles()
        {
            return db.ItemArticles
                   .Where(d => d.PurgeFlag == false)
                   .OrderBy(d => d.ArticleName)
                   .Select(d => new SelectListItem {
                       Value = d.ID.ToString(),
                       Text =  d.ArticleName + " (" + d.ArticleCode + ") (" + d.FKItemTypeReference.ItemTypeCode + ")" 
                   }).ToList();
        }
        public List<ItemCategory> GetCategories()
        {
            return db.ItemCategories.Where(d => d.PurgeFlag == false).OrderBy(d => d.ItemCategoryName).ToList();
        }
        public ItemArticles GetArticleDetails(int ID)
        {
            var itemArticle = db.ItemArticles.Find(ID);
            itemArticle.FKItemTypeReference.PurchaseRequestCenter = itemArticle.FKItemTypeReference.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(itemArticle.FKItemTypeReference.PurchaseRequestCenter).Department;
            itemArticle.FKItemTypeReference.ResponsibilityCenter = itemArticle.FKItemTypeReference.ResponsibilityCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(itemArticle.FKItemTypeReference.ResponsibilityCenter).Department;
            var UACSObject = abis.GetChartOfAccounts(itemArticle.UACSObjectClass);
            itemArticle.UACSObjectClass = UACSObject.SubAcctName + " - " + UACSObject.AcctName;
            return itemArticle;
        }
        public List<HRISDepartment> GetDepartments()
        {
            return hris.GetAllDepartments().Where(d => !d.Department.Contains("Inactive") && d.Lvl == 1).OrderBy(d => d.DepartmentID).ToList();
        }
        public ItemVM GetItemDetails(string ItemCode)
        {
            var item = db.Items.ToList().Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            var UACS = abis.GetChartOfAccounts(item.FKArticleReference.UACSObjectClass);
            var unitPriceRecord = db.ItemPrices.ToList().Where(d => d.FKItemReference.ItemCode == ItemCode && d.IsPrevailingPrice == true).FirstOrDefault();
            var unitPrice = unitPriceRecord == null ? null : (decimal?)unitPriceRecord.UnitPrice;

            return new ItemVM
            {
                ID = item.ID,
                Article = item.ArticleReference,
                ItemImage = item.ItemImage,
                ItemCode = item.ItemCode,
                ItemFullName = item.ItemFullName,
                ItemShortSpecifications = item.ItemShortSpecifications,
                IsSpecsUserDefined = item.IsSpecsUserDefined,
                ItemSpecifications = item.ItemSpecifications == null ? "Not Applicable" : item.ItemSpecifications,
                AccountClass = UACS.SubAcctName + " >> " + UACS.AcctName,
                ProcurementSource = item.ProcurementSource,
                Category = item.FKCategoryReference.ID,
                Classification = item.FKArticleReference.FKItemTypeReference.FKItemClassificationReference.Classification,
                ItemType = item.FKArticleReference.FKItemTypeReference.ItemType,
                IndividualUOMReference = item.FKIndividualUnitReference.ID,
                PackagingUOMReference = item.FKPackagingUnitReference.ID,
                QuantityPerPackage = item.QuantityPerPackage,
                MinimumIssuanceQty = item.MinimumIssuanceQty,
                UnitPrice = unitPrice,
                ResponsibilityCenter = item.FKArticleReference.FKItemTypeReference.ResponsibilityCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(item.FKArticleReference.FKItemTypeReference.ResponsibilityCenter).Department,
                PurchaseRequestOffice = item.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter == null ? "Requesting Office" : hris.GetDepartmentDetails(item.FKArticleReference.FKItemTypeReference.PurchaseRequestCenter).Department,
                ArticleInformation = item.FKArticleReference,
                CategoryInfomation = item.FKCategoryReference,
                IndividualUnitInformation = item.FKIndividualUnitReference,
                PackagingUnitInformation = item.FKPackagingUnitReference,
                CreatedAt = item.CreatedAt.ToString("MMMM dd yyyy hh:mm tt"),
                UpdatedAt = item.UpdatedAt == null ? "Not Yet Applicable" : ((DateTime)item.UpdatedAt).ToString("MMMM dd yyyy hh:mm tt"),
                DeletedAt = item.DeletedAt == null ? "Not Yet Applicable" : ((DateTime)item.DeletedAt).ToString("MMMM dd yyyy hh:mm tt"),
                PurgeFlag = item.PurgeFlag == true ? "Yes" : "No"
            };
        }
        public bool AddItemRecord(ItemVM ItemModel, string UserEmail)
        {
            DbPropertyValues currentValues;
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var Sequence = (db.Items.Where(d => d.ArticleReference == ItemModel.Article).Count() + 1).ToString();
            var Article = db.ItemArticles.Find(ItemModel.Article);
            var item = new Item
            {
                Sequence = Sequence.Length == 1 ? "0" + Sequence : Sequence,
                ArticleReference = ItemModel.Article,
                ItemFullName = Article.ArticleName + "; " + ItemModel.ItemShortSpecifications,
                ItemImage = ItemModel.ItemImage,
                ItemShortSpecifications = ItemModel.ItemShortSpecifications,
                IsSpecsUserDefined = ItemModel.IsSpecsUserDefined,
                ItemSpecifications = ItemModel.ItemSpecifications,
                CategoryReference = Convert.ToInt32(ItemModel.Category),
                ProcurementSource = ItemModel.ProcurementSource,
                PackagingUOMReference = ItemModel.PackagingUOMReference,
                QuantityPerPackage = ItemModel.QuantityPerPackage,
                IndividualUOMReference = ItemModel.IndividualUOMReference,
                MinimumIssuanceQty = ItemModel.MinimumIssuanceQty,
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
            log.ProcessedBy = user.ID;
            log.TableName = "PROC_MSTR_Item";
            MasterTablesLogger logger = new MasterTablesLogger();
            logger.Log(log, null, currentValues);
            
            if(ItemModel.ProcurementSource == ProcurementSources.AgencyToAgency)
            {
                ItemPrice itemPrice = new ItemPrice
                {
                    UnitPrice = Convert.ToDecimal(ItemModel.UnitPrice),
                    IsPrevailingPrice = true,
                    Item = item.ID,
                    EffectivityDate = DateTime.Now
                };
                db.ItemPrices.Add(itemPrice);

                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            if (ItemModel.AllowedUsers != null)
            {
                foreach (var dept in ItemModel.AllowedUsers)
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
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            Item item = db.Items.ToList().Where(d => d.ItemCode == ItemCode).FirstOrDefault();
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
                log.ProcessedBy = user.ID;
                log.TableName = "PROC_MSTR_Item";
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
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            Item item = db.Items.ToList().Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            DbPropertyValues currentValues;
            DbPropertyValues originalValues;

            if (item != null)
            {
                item.PurgeFlag = false;
                item.DeletedAt = DateTime.Now;
                item.UpdatedAt = DateTime.Now;

                log.Action = "Restore Record";

                currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                log.AuditableKey = item.ID;
                log.ProcessedBy = user.ID;
                log.TableName = "PROC_MSTR_Item";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, originalValues, currentValues);

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
                hris.Dispose();
                abis.Dispose();
            }
            base.Dispose(disposing);
        }
        //public bool UpdateItemRecord(ItemVM itemViewModel, string UserEmail)
        //{
        //    Item item = db.Items.Where(d => d.ItemCode == itemViewModel.ItemCode).FirstOrDefault();

        //    item.ItemImage = itemViewModel.ItemImage;
        //    item.ItemFullName = item.FKItemTypeReference.ItemTypeName.ToUpper() + "; " + itemViewModel.ItemShortSpecifications.ToUpper();
        //    item.ItemShortSpecifications = itemViewModel.ItemShortSpecifications;
        //    item.ItemSpecifications = itemViewModel.ItemSpecifications;
        //    item.ProcurementSource = itemViewModel.ProcurementSource;
        //    item.CategoryReference = Convert.ToInt32(itemViewModel.Category);
        //    item.ResponsibilityCenter = itemViewModel.ResponsibilityCenter == "None" ? null : itemViewModel.ResponsibilityCenter;
        //    item.PurchaseRequestOffice = itemViewModel.PurchaseRequestOffice == "Requesting Office" ? null : itemViewModel.PurchaseRequestOffice;
        //    item.PackagingUOMReference = itemViewModel.PackagingUOMReference == null ? null : (int?)Convert.ToUInt32(itemViewModel.PackagingUOMReference);
        //    item.QuantityPerPackage = itemViewModel.QuantityPerPackage;
        //    item.IndividualUOMReference = itemViewModel.IndividualUOMReference == null ? null : (int?)Convert.ToUInt32(itemViewModel.IndividualUOMReference);
        //    item.MinimumIssuanceQty = itemViewModel.MinimumIssuanceQty;
        //    item.UpdatedAt = DateTime.Now;

        //    DbPropertyValues currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
        //    DbPropertyValues originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
        //    log.Action = "Update Record";
        //    log.AuditableKey = item.ID;
        //    log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
        //    log.TableName = "MASTER_ITEM_TYPES";
        //    MasterTablesLogger logger = new MasterTablesLogger();
        //    logger.Log(log, originalValues, currentValues);

        //    if (db.SaveChanges() == 0)
        //    {
        //        return false;
        //    }

        //    ItemPrice previousPrice = db.ItemPrices.Where(d => d.Item == item.ID).OrderByDescending(d => d.UpdatedAt).FirstOrDefault();
        //    if (item.FKItemTypeReference.FKInventoryTypeReference.IsTangible == true && previousPrice.UnitPrice != itemViewModel.UnitPrice)
        //    {
        //        previousPrice.IsPrevailingPrice = false;
        //        previousPrice.UpdatedAt = DateTime.Now;
        //        if (db.SaveChanges() == 0)
        //        {
        //            return false;
        //        }

        //        ItemPrice itemPrice = new ItemPrice
        //        {
        //            UnitPrice = Convert.ToDecimal(itemViewModel.UnitPrice),
        //            IsPrevailingPrice = true,
        //            Item = item.ID,
        //            CreatedAt = DateTime.Now
        //        };

        //        db.ItemPrices.Add(itemPrice);

        //        if (db.SaveChanges() == 0)
        //        {
        //            return false;
        //        }
        //    }

        //    if (db.ItemAllowedUsers.Where(d => d.FKItemReference.ItemCode == itemViewModel.ItemCode).Count() != 0)
        //    {
        //        db.ItemAllowedUsers.RemoveRange(db.ItemAllowedUsers.Where(d => d.FKItemReference.ItemCode == itemViewModel.ItemCode));
        //        db.SaveChanges();
        //    }

        //    if (itemViewModel.AllowedUsers != null)
        //    {
        //        foreach (var dept in itemViewModel.AllowedUsers)
        //        {
        //            ItemAllowedUsers allowedUsers = new ItemAllowedUsers()
        //            {
        //                DepartmentReference = dept,
        //                ItemReference = item.ID
        //            };
        //            db.ItemAllowedUsers.Add(allowedUsers);
        //            if (db.SaveChanges() == 0)
        //            {
        //                return false;
        //            }
        //        }
        //    }

        //    return true;
        //}
    }
}