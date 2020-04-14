//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.BusinessAndDataLogic;
//using PUPFMIS.Models;

//namespace PUPFMIS.Controllers
//{
//    [Authorize]
//    [Route("ops/procurement/planning/{action}")]
//    public class PPMPController : Controller
//    {
//        private PPMPBL ppmpBL = new PPMPBL();

//        [ActionName("dashboard")]
//        public ActionResult Dashboard()
//        {
//            PPMPClientDashboard dashboard = new PPMPClientDashboard();
//            return View("Dashboard", dashboard);
//        }

//        [ActionName("view-ppmp")]
//        public ActionResult ViewPPMP()
//        {
//            return View("Index", ppmpBL.GetMyPPMP());
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                ppmpBL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}