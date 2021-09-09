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
    public class ItemArticlesBDL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private ABISDataAccess abis = new ABISDataAccess();
        private HRISDataAccess hris = new HRISDataAccess();
        private LogsMasterTables log = new LogsMasterTables();

        public List<ItemArticles> GetArticles()
        {
            return db.ItemArticles.OrderBy(d => d.ArticleCode).ThenBy(d => d.ArticleName).ToList();
        }
        public ItemArticles GetItemArticle(string ArticleCode)
        {
            return db.ItemArticles.Where(d => d.ArticleCode == ArticleCode).FirstOrDefault();
        }
        public List<SelectListItem> GetChartOfAccounts()
        {
            return abis.GetChartOfAccounts()
                   .Where(d => (d.ClassCode == "1" && d.GenAcctCode == "4") || (d.ClassCode == "1" && d.GenAcctCode == "5") || (d.ClassCode == "1" && d.GenAcctCode == "6") || (d.ClassCode == "1" && d.GenAcctCode == "8") || (d.ClassCode == "5" && d.GenAcctCode == "2"))
                   .Select(d => new SelectListItem {
                       Text = d.Class + " : " + d.SubAcctName + " >> " + d.AcctName,
                       Value = d.UACS_Code
                   }).ToList();
        }
        public List<ItemTypes> GetItemTypes()
        {
            return db.ItemTypes.Where(d => d.PurgeFlag == false).ToList();
        }
        public void Validate(ItemArticles ItemArticle, out string Key, out List<string> Message)
        {
            Key = null;
            Message = new List<string>();

            ItemArticleValidator validator = new ItemArticleValidator();
            var validationResult = validator.Validate(ItemArticle);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    Key = validationResult.Errors.Count() > 1 ? "" : error.PropertyName;
                    Message.Add(error.ErrorMessage);
                }

            }
        }
        public bool AddItemArticleRecord(ItemArticles ItemArticle, string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            DbPropertyValues currentValues;
            var itemArticle = db.ItemArticles.Where(d => d.ArticleCode == ItemArticle.ArticleCode).FirstOrDefault();

            if (itemArticle != null)
            {
                return false;
            }

            itemArticle = new ItemArticles
            {
                ArticleCode = GenerateArticleCode(ItemArticle.ItemTypeReference),
                ArticleName = ItemArticle.ArticleName,
                ItemTypeReference = ItemArticle.ItemTypeReference,
                UACSObjectClass = ItemArticle.UACSObjectClass,
                GLAccount = ItemArticle.GLAccount,
                CreatedAt = DateTime.Now,
                PurgeFlag = false
            };

            db.ItemArticles.Add(itemArticle);
            currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;

            if (db.SaveChanges() == 0)
            {
                return false;
            }

            log.Action = "Add Record";
            log.AuditableKey = itemArticle.ID;
            log.ProcessedBy = user.ID;
            log.TableName = "PROC_MSTR_Item_Articles";
            MasterTablesLogger logger = new MasterTablesLogger();
            logger.Log(log, null, currentValues);

            return true;
        }
        public bool UpdateItemArticleRecord(ItemArticles ItemArticle, string UserEmail, out string Key, out string Message)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var itemArticle = db.ItemArticles.Find(ItemArticle.ID);
            DbPropertyValues currentValues;
            DbPropertyValues originalValues;

            if (itemArticle.ItemTypeReference != ItemArticle.ItemTypeReference)
            {
                ItemArticle.ArticleCode = GenerateArticleCode(ItemArticle.ItemTypeReference);
                ItemArticle.CreatedAt = DateTime.Now;
                ItemArticle.PurgeFlag = false;
                db.ItemArticles.Add(ItemArticle);
                currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;

                if (db.SaveChanges() == 0)
                {
                    Key = null;
                    Message = null;
                    return false;
                }

                log.Action = "Add Record";
                log.AuditableKey = ItemArticle.ID;
                log.ProcessedBy = user.ID;
                log.TableName = "PROC_MSTR_Item_Articles";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, null, currentValues);

                itemArticle.PurgeFlag = true;
                itemArticle.DeletedAt = DateTime.Now;

                currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;

                if (db.SaveChanges() == 0)
                {
                    Key = null;
                    Message = null;
                    return false;
                }

                log.AuditableKey = itemArticle.ID;
                log.ProcessedBy = user.ID;
                log.Action = "Purge Record";
                log.TableName = "PROC_MSTR_Item_Articles";
                MasterTablesLogger _logger = new MasterTablesLogger();
                _logger.Log(log, originalValues, currentValues);
            }
            else
            {
                if (itemArticle.ArticleName != ItemArticle.ArticleName && db.ItemArticles.Where(d => d.ArticleName == ItemArticle.ArticleName).Count() != 0)
                {
                    Key = "ArticleName";
                    Message = "Item Article Name already exists in the database.";
                    return false;
                }

                if (itemArticle != null)
                {
                    itemArticle.ArticleCode = ItemArticle.ArticleCode;
                    itemArticle.ArticleName = ItemArticle.ArticleName;
                    itemArticle.UACSObjectClass = ItemArticle.UACSObjectClass;
                    itemArticle.GLAccount = ItemArticle.GLAccount;
                    itemArticle.ItemTypeReference = ItemArticle.ItemTypeReference;
                    itemArticle.UpdatedAt = DateTime.Now;

                    currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                    originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;

                    if (db.SaveChanges() == 0)
                    {
                        Key = null;
                        Message = null;
                        return false;
                    }

                    log.AuditableKey = itemArticle.ID;
                    log.ProcessedBy = user.ID;
                    log.Action = "Update Record";
                    log.TableName = "PROC_MSTR_Item_Articles";
                    MasterTablesLogger _logger = new MasterTablesLogger();
                    _logger.Log(log, originalValues, currentValues);
                }
            }

            Key = null;
            Message = null;
            return true;
        }
        public bool PurgeItemArticleRecord(string ArticleCode, string UserEmail)
        {
            LogsMasterTables log = new LogsMasterTables();
            AccountsManagementBL accountsBL = new AccountsManagementBL();
            var itemArticle = db.ItemArticles.Where(d => d.ArticleCode == ArticleCode).FirstOrDefault();

            if (itemArticle != null)
            {
                itemArticle.PurgeFlag = true;
                itemArticle.DeletedAt = DateTime.Now;
                DbPropertyValues currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                DbPropertyValues originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                log.Action = "Purge Record";
                log.AuditableKey = itemArticle.ID;
                log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
                log.TableName = "PROC_MSTR_Item_Articles";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, originalValues, currentValues);

                if (db.SaveChanges() == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public string GenerateArticleCode(int ItemTypeReference)
        {
            var count = (db.ItemArticles.Where(d => d.ItemTypeReference == ItemTypeReference).Count() + 1).ToString();
            var series = (ItemTypeReference.ToString().Length == 1 ? "0" + ItemTypeReference.ToString() : ItemTypeReference.ToString()) + (count.Length == 1 ? "00" + count : count.Length == 2 ? "0" + count : count);
            return series;
        }
        public bool RestoreItemArticleRecord(string ArticleCode, string UserEmail)
        {
            LogsMasterTables log = new LogsMasterTables();
            AccountsManagementBL accountsBL = new AccountsManagementBL();
            var itemArticle = db.ItemArticles.Where(d => d.ArticleCode == ArticleCode).FirstOrDefault();

            if (itemArticle != null)
            {
                itemArticle.PurgeFlag = false;
                itemArticle.DeletedAt = null;
                itemArticle.UpdatedAt = DateTime.Now;
                DbPropertyValues currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                DbPropertyValues originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                log.Action = "Restore Record";
                log.AuditableKey = itemArticle.ID;
                log.ProcessedBy = accountsBL.GetUser(UserEmail).UserID;
                log.TableName = "PROC_MSTR_Item_Articles";
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
                abis.Dispose();
                hris.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}