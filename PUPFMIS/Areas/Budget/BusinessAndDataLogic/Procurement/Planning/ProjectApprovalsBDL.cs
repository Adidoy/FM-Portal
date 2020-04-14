using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using PUPFMIS.Models.AIS;

namespace PUPFMIS.BusinessLayer
{
    public class ProjectApprovalsBDL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDbContext hrdb = new HRISDbContext();
        private ABDBContext abdb = new ABDBContext();

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