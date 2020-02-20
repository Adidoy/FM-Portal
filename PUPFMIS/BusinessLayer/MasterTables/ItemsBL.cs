using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessLayer
{
    public class ItemsBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private LogsMasterTables log = new LogsMasterTables();

        public List<Item> GetItems()
        {
            return db.Items.Where(d => d.PurgeFlag == false).OrderBy(d => d.ItemName).ToList();
        }

        public List<Item> GetItems(string CategoryName)
        {
            return db.Items.Where(d => d.PurgeFlag == false && d.FKItemCategoryReference.ItemCategoryName == CategoryName).OrderBy(d => d.ItemName).ToList();
        }

        public Item GetItems(int? ID)
        {
            return db.Items.Where(d => d.PurgeFlag == false && d.ID == ID).FirstOrDefault();
        }

        public List<Item> GetDeletedItems()
        {
            return db.Items.Where(d => d.PurgeFlag == true).OrderBy(d => d.ItemName).ToList();
        }

        public Item GetDeletedItems(int ID)
        {
            return db.Items.Where(d => d.PurgeFlag == true && d.ID == ID).FirstOrDefault();
        }

        public bool AddItemRecord(Item itemModel)
        {
            Item item = db.Items.Find(itemModel.ID);
            ItemPrice itemPrice = new ItemPrice();
            DbPropertyValues currentValues;

            if (item == null)
            {
                itemModel.ItemCode = GenerateItemCode(itemModel.ItemCategoryReference);
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

        private string GenerateItemCode(int ItemCategoryID)
        {
            string _categoryCode = (ItemCategoryID.ToString().Length == 1) ? "0" + ItemCategoryID.ToString() : ItemCategoryID.ToString();
            var _series = db.Items.Where(d => d.ItemCategoryReference == ItemCategoryID).Count() + 1;
            string _itemSeries = (_series.ToString().Length == 1) ? "00" + _series.ToString() : (_series.ToString().Length == 2) ? "0" + _series.ToString() : _series.ToString();
            return _categoryCode + "-" + _itemSeries;
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