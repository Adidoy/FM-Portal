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
    public class ItemBusinessLogic : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private LogsMasterTables log = new LogsMasterTables();
        UnitOfMeasureBL unitOfMeasureBL = new UnitOfMeasureBL();
        ItemCategoriesBL itemCategoriesBL = new ItemCategoriesBL();
        InventoryTypeDataAccess inventoryTypesBL = new InventoryTypeDataAccess();
        ChartOfAccountsBL coaBL = new ChartOfAccountsBL();

        public List<Item> GetItems(bool DeleteFlag)
        {
            return db.Items.Where(d => d.PurgeFlag == DeleteFlag).ToList();
        }
        public Item GetItemsByID(int ID, bool DeleteFlag)
        {
            return db.Items.Where(d => d.PurgeFlag == DeleteFlag && d.ID == ID).OrderBy(d => d.ItemName).FirstOrDefault();
        }
        public Item GetItemsByCode(string ItemName, bool DeleteFlag)
        {
            return db.Items.Where(d => d.ItemName == ItemName && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }
        public bool AddItemRecord(Item itemModel)
        {
            Item item = db.Items.Find(itemModel.ID);
            ItemPrice itemPrice = new ItemPrice();
            DbPropertyValues currentValues;

            if (item == null)
            {
                itemModel.ItemCode = GenerateItemCode(itemModel.InventoryTypeReference, itemModel.ItemCategoryReference);
                itemModel.PurgeFlag = false;
                itemModel.CreatedAt = DateTime.Now;
                db.Items.Add(itemModel);

                currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;
                log.Action = "Add Record";

                if (db.SaveChanges() == 0)
                {
                    return false;
                }

                log.AuditableKey = itemModel.ID;
                log.ProcessedBy = null;
                log.TableName = "Items";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, null, currentValues);

                itemPrice.UnitPrice = Convert.ToDecimal("0.00");
                itemPrice.IsPrevailingPrice = true;
                itemPrice.Item = itemModel.ID;
                itemPrice.CreatedAt = DateTime.Now;
                db.ItemPrices.Add(itemPrice);

                if (db.SaveChanges() == 1)
                {
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

                    log.Action = "Update Record";
                }
                else
                {
                    _supplier.PurgeFlag = true;
                    _supplier.DeletedAt = DateTime.Now;

                    log.Action = "Purge Record";
                }

                _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
                _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
                log.AuditableKey = supplier.ID;
                log.ProcessedBy = null;
                log.TableName = "master_suppliers";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, _originalValues, _currentValues);

                if (db.SaveChanges() == 1)
                {
                    return true;
                }
            }
            return false;
        }
        private string GenerateItemCode(int InventoryTypeCode, int CategoryID)
        {
            string ItemCode = string.Empty;
            var category = (CategoryID.ToString().Length == 1) ? "00" + CategoryID.ToString() : (CategoryID.ToString().Length == 2) ? "0" + CategoryID.ToString() : CategoryID.ToString();
            var series = (db.Items.Where(d => d.InventoryTypeReference == InventoryTypeCode && d.ItemCategoryReference == CategoryID).Count() + 1).ToString();
            series = (series.Length == 1) ? "00" + series : (series.Length == 2) ? "0" + series : series;
            var inventoryTypeCode = (InventoryTypeCode.ToString().Length == 1) ? "0" + InventoryTypeCode.ToString() : InventoryTypeCode.ToString();
            ItemCode = inventoryTypeCode + "-" + category + "-" + series;
            return ItemCode;
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
    public class ItemDataAccess : Controller
    {
        FMISDbContext db = new FMISDbContext();
        ABDBContext abdb = new ABDBContext();

        public List<DBMCategories> GetCategories()
        {
            return db.DBMCategories.Where(d => d.PurgeFlag == false).OrderBy(d => d.ItemCategoryName).ToList();
        }
        public List<InventoryType> GetInventoryTypes()
        {
            return db.InventoryTypes.Where(d => d.IsTangible == true).OrderBy(d => d.InventoryTypeName).ToList();
        }
        public List<UnitOfMeasure> GetUnitsOfMeasure()
        {
            return db.UOM.Where(d => d.PurgeFlag == false).OrderBy(d => d.UnitName).ToList();
        }
        public List<ChartOfAccounts> GetChartOfAccounts()
        {
            return abdb.ChartOfAccounts.OrderBy(d => d.UACS).ToList();
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