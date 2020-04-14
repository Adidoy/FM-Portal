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
    public class InventoryTypeDataAccess : Controller
    {
        private FMISDbContext db = new FMISDbContext();

        public List<InventoryType> GetInventoryTypes(bool IsTangible)
        {
            return db.InventoryTypes.Where(d => d.IsTangible == IsTangible).ToList();
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