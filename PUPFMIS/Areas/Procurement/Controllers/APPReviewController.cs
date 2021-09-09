//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.BusinessAndDataLogic;

//namespace PUPFMIS.Controllers
//{
//    [Route("{action}")]
//    [RouteArea("procurement")]
//    [RoutePrefix("annual-procurement-plan/review")]
//    public class APPReviewController : Controller
//    {
//        private APPReviewDAL appReviewDAL = new APPReviewDAL();

//        [ActionName("procurement-programs")]
//        [Route("{ReferenceNo}/procurement-programs")]
//        public ActionResult Index(string ReferenceNo)
//        {
//            var APPList = appReviewDAL.GetProcurementProgams(ReferenceNo);
//            return View("Index", APPList);
//        }

//        [ActionName("projects")]
//        [Route("{APPReferenceNo}/projects/{ReferenceNo}")]
//        public ActionResult Details(string ReferenceNo, string APPReferenceNo)
//        {
//            var APPList = appReviewDAL.GetProcurementProgamsByPAPCode(ReferenceNo, APPReferenceNo);
//            return View("Details", APPList);
//        }

//        //[ActionName("details")]
//        //public ActionResult Details()
//        //{
//        //    return View();
//        //}
//    }
//}