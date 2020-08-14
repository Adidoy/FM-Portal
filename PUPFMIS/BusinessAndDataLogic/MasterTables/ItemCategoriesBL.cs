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
    public class ItemCategoriesDataAccess : Controller
    {
        FMISDbContext db = new FMISDbContext();

        public List<ItemCategory> GetItemCategories(bool DeleteFlag)
        {
            return db.ItemCategories.Where(d => d.PurgeFlag == DeleteFlag).ToList();
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