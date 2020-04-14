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
    public class ItemCategoriesBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();

        public List<ItemCategory> GetCategories()
        {
            return db.ItemCategories.Where(d => d.PurgeFlag == false).ToList();
        }

        public ItemCategory GetCategories(int? ID)
        {
            return db.ItemCategories.Where(d => d.PurgeFlag == false && d.ID == ID).FirstOrDefault();
        }

        public List<string> GetCategoryNames()
        {
            return db.ItemCategories.Where(d => d.PurgeFlag == false).Select(d => d.ItemCategoryName).ToList();
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

   public class ItemCategoriesDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        public List<DBMCategories> GetDBMCategories(bool DeleteFlag)
        {
            return db.DBMCategories.Where(d => d.PurgeFlag == DeleteFlag).ToList();
        }
        public DBMCategories GetDBMCategoryByID(int ID, bool DeleteFlag)
        {
            return db.DBMCategories.Where(d => d.ID == ID && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }
        public DBMCategories GetDBMCategoryByName(string Name, bool DeleteFlag)
        {
            return db.DBMCategories.Where(d => d.ItemCategoryName == Name && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }
        public List<PhilGEPSCategories> GetPhilGEPSCategories(bool DeleteFlag)
        {
            return db.PhilGEPSCategories.Where(d => d.PurgeFlag == DeleteFlag).ToList();
        }
        public List<PhilGEPSCategories> GetPhilGEPSCategories(string CategoryFor, bool DeleteFlag)
        {
            return db.PhilGEPSCategories.Where(d => d.CategoryFor == CategoryFor && d.PurgeFlag == DeleteFlag).ToList();
        }
        public PhilGEPSCategories GetPhilGEPSCategoryByID(int ID, bool DeleteFlag)
        {
            return db.PhilGEPSCategories.Where(d => d.ID == ID && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }
        public PhilGEPSCategories GetPhilGEPSCategoryByName(string Name, bool DeleteFlag)
        {
            return db.PhilGEPSCategories.Where(d => d.ItemCategoryName == Name && d.PurgeFlag == DeleteFlag).FirstOrDefault();
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