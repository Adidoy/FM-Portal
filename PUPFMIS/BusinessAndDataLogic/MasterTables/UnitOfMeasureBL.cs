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
    public class UnitOfMeasureDataAccess : Controller
    {
        private FMISDbContext db = new FMISDbContext();

        public List<UnitOfMeasure> GetUOMs(bool DeleteFlag)
        {
            return db.UOM.Where(d => d.PurgeFlag == DeleteFlag).OrderBy(d => d.UnitName).ToList();
        }

        public UnitOfMeasure GetUOMs(int? ID)
        {
            return db.UOM.Where(d => d.PurgeFlag == false && d.ID == ID).FirstOrDefault();
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