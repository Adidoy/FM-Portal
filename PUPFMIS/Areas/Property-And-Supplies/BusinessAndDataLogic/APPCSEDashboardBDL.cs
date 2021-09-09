//using PUPFMIS.Models;
//using PUPFMIS.Models.AIS;
//using PUPFMIS.Models.HRIS;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web.Mvc;

//namespace PUPFMIS.BusinessAndDataLogic
//{
//    public class APPCSEDashboardBL : Controller
//    {

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
                
//            }
//            base.Dispose(disposing);
//        }
//    }

//    public class APPCSEDashboardDAL : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();
//        private HRISDbContext hrdb = new HRISDbContext();
//        private TEMPAccounting abdb = new TEMPAccounting();

//        public List<int> GetPPMPFiscalYears()
//        {
//            var fiscalYears = db.PPMPHeader.Where(d => d.Status == "PPMP Evaluated" && d.FKPPMPTypeReference.InventoryCode == "CUOS").GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
//            return fiscalYears;
//        }

//        public List<int> GetAPPFiscalYears()
//        {
//            var fiscalYears = db.APPHeader.Where(d => d.APPType == "CSE").GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
//            return fiscalYears;
//        }

//        public List<string> GetAPPReferences()
//        {
//            return db.APPHeader.Where(d => d.APPType == "CSE").Select(d => d.ReferenceNo).ToList();
//        }

//        public APPCSEDashboard GetAPPCSEDashboard()
//        {
//            APPCSEDashboard dashboard = new APPCSEDashboard();
//            dashboard.PPMPFiscalYears = GetPPMPFiscalYears();
//            dashboard.APPCSEReferences = GetAPPReferences();
//            return dashboard;
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//                hrdb.Dispose();
//                abdb.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}