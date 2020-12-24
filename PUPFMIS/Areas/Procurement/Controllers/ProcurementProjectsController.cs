using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.Procurement.Controllers
{
    [Route("{action}")]
    [RouteArea("procurement")]
    [RoutePrefix("projects")]
    [Authorize(Roles = SystemRoles.ProcurementPlanningChief + ", " + SystemRoles.ProcurementAdministrator + ", " + SystemRoles.BACSECHead)]
    public class ProcurementProjectsController : Controller
    {
        private ProcurementProjectsDAL procurementProjectsDAL = new ProcurementProjectsDAL();

        [Route("dashboard")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            return View();
        }

        [Route("bidding/select-type")]
        [ActionName("select-bidding-type")]
        public ActionResult SelectBiddingType()
        {
            ViewBag.ProcurementProgram = new SelectList(procurementProjectsDAL.GetProcurementPrograms(BiddingTypes.ProgramBased).OrderBy(d => d.Code), "Code", "ProgramName");
            return View("SelectBiddingType");
        }

        [HttpPost]
        [Route("bidding/select-type")]
        [ActionName("select-bidding-type")]
        public ActionResult SelectBiddingType(SelectBiddingTypeVM SelectBiddingType)
        {
            TempData["BiddingType"] = SelectBiddingType.BiddingType;
            TempData["ProgramCode"] = SelectBiddingType.ProcurementProgram;
            TempData["BiddingStrategy"] = SelectBiddingType.BiddingStrategy;
            return RedirectToAction("create-bidding-project", new { Area = "procurement" });
        }

        [Route("bidding/create")]
        [ActionName("create-bidding-project")]
        public ActionResult CreateBiddingProject()
        {
            if (TempData["BiddingType"] == null && TempData["ProgramCode"] == null)
            {
                return RedirectToAction("select-bidding-type");
            }
            var BiddingType = (BiddingTypes)TempData["BiddingType"];
            var ProgramCode = (string)TempData["ProgramCode"];
            var BiddingStrategy = (BiddingStrategies)TempData["BiddingStrategy"];
            var BiddingProjectVM = BiddingType == BiddingTypes.ProjectBased ? procurementProjectsDAL.GetBiddingItemsProjectBased(BiddingType, ProgramCode, BiddingStrategy) : procurementProjectsDAL.GetBiddingItemsProgramBased(BiddingType, ProgramCode, BiddingStrategy);
            return View("CreateBiddingProject", BiddingProjectVM);
        }

        public ActionResult GetPrograms(BiddingTypes BiddingType)
        {
            var programs = procurementProjectsDAL.GetProcurementPrograms(BiddingType).OrderBy(d => d.Code).ToList();
            return Json(programs, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                procurementProjectsDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}