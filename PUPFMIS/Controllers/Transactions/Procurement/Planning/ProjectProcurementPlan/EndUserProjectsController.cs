//using FluentValidation.Results;
//using PUPFMIS.BusinessLayer;
//using PUPFMIS.Models;
//using System.Net;
//using System.Web.Mvc;
//using System.Collections.Generic;

//namespace PUPFMIS.Controllers
//{
//    [Route("ops/procurement/planning/projects/{action}")]
//    public class EndUserProjectsController : Controller
//    {
//        EndUserProjectsBL endUserProjectBL = new EndUserProjectsBL();
//        static ProjectMarketSurveyVM projectMarketSurveyVM = new ProjectMarketSurveyVM();

//        [ActionName("view-projects")]
//        public ActionResult Index()
//        {
//            return View("Index", endUserProjectBL.GetActiveProjects());
//        }

//        [ActionName("create")]
//        public ActionResult Create()
//        {
//            ViewBag.FiscalYear = endUserProjectBL.GetFiscalYears();
//            return View("Create", endUserProjectBL.AddEndUserProjectRecord());
//        }



//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                endUserProjectBL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
