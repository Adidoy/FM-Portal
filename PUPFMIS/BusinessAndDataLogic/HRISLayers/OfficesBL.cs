using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models.HRIS;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class OfficesBL : Controller
    {
        private HRISDbContext db = new HRISDbContext();

        public List<Offices> GetOffices()
        {
            return db.OfficeModel.ToList();
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