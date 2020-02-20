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
}