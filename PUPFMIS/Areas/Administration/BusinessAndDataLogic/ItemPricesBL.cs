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
    public class ItemPricesBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        LogsMasterTables _log = new LogsMasterTables();

        public List<ItemPrice> GetPrevailingPrices()
        {
            return db.ItemPrices.Where(d => d.IsPrevailingPrice == true && d.FKItemReference.ProcurementSource == ProcurementSources.AgencyToAgency).ToList();
        }
        public List<ItemPrice> GetPriceHistory(string ItemCode)
        {
            return db.ItemPrices.OrderByDescending(d => d.UpdatedAt).ToList().Where(d => d.FKItemReference.ItemCode == ItemCode).ToList();
        }
        public ItemPrice GetPriceDetails(int? PriceID)
        {
            return db.ItemPrices.Find(PriceID);
        }
        public bool AddPriceRecord(ItemPrice price)
        {
            DbPropertyValues _currentValues;
            DbPropertyValues _originalValues;

            ItemPrice newPriceRecord = new ItemPrice();
            newPriceRecord.Item = price.Item;
            newPriceRecord.UnitPrice = price.UnitPrice;
            newPriceRecord.IsPrevailingPrice = true;
            newPriceRecord.EffectivityDate = DateTime.Now;
            db.ItemPrices.Add(newPriceRecord);

            ItemPrice oldPriceRecord = db.ItemPrices.Find(price.ID);
            oldPriceRecord.IsPrevailingPrice = false;
            oldPriceRecord.UpdatedAt = DateTime.Now;

            _currentValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().CurrentValues;
            _originalValues = db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).First().OriginalValues;
            _log.AuditableKey = price.ID;
            _log.ProcessedBy = null;
            _log.TableName = "master_itemPrices";
            MasterTablesLogger _logger = new MasterTablesLogger();
            _logger.Log(_log, _originalValues, _currentValues);

            if (db.SaveChanges() == 2)
            {
                return true;
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