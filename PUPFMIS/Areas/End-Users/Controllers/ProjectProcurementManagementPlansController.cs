using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RoutePrefix("ppmp")]
    [RouteArea("end-users")]
    [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
    public class ProjectProcurementManagementPlansController : Controller
    {
        private ProjectProcurementManagementPlanDAL ppmpDAL = new ProjectProcurementManagementPlanDAL();

        [Route("dashboard")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            return View("Dashboard", ppmpDAL.GetDashboardValues((DateTime.Now.Year), User.Identity.Name));
        }

        [Route("{FiscalYear}/list")]
        [ActionName("list")]
        public ActionResult Index(int FiscalYear)
        {
            var ppmpList = ppmpDAL.GetPPMPs(FiscalYear, User.Identity.Name);
            return View("Index", ppmpList);
        }

        [Route("{ReferenceNo}/details")]
        [ActionName("details")]
        public ActionResult Details(string ReferenceNo)
        {
            if (ReferenceNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ppmpVM = ppmpDAL.GetPPMPDetail(ReferenceNo);
            if (ppmpVM == null)
            {
                return RedirectToAction("record-not-found", "Errors", new { Area = "" });
            }
            return View("details", ppmpVM);
        }

        [ActionName("print-ppmp")]
        [Route("{ReferenceNo}/print")]
        public ActionResult PrintPPMP(string ReferenceNo)
        {
            var ppmpBL = new ProjectProcurementManagementPlanBL();
            var stream = ppmpBL.PrintPPMP(Server.MapPath("~/Content/imgs/PUPLogo.png"), ReferenceNo);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            //Response.AddHeader("content-disposition", "attachment; filename=" + ReferenceNo + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("list", new { Area = "end-users" });
        }

        [HttpPost]
        [ActionName("submit")]
        [Route("{FiscalYear}/{PPMPType}/submit")]
        public ActionResult SubmitPPMP(int FiscalYear, PPMPTypes PPMPType)
        {
            return Json(new { result = ppmpDAL.SubmitPPMP(FiscalYear, PPMPType, User.Identity.Name) });
        }

        [ActionName("item-details")]
        [Route("{ReferenceNo}/{ItemCode}/details")]
        public ActionResult ItemDetails(string ReferenceNo, string ItemCode)
        {
            var ppmpItemDetails = ppmpDAL.GetPPMPItemDetails(ReferenceNo, ItemCode);
            if (ppmpItemDetails == null)
            {
                return RedirectToAction("record-not-found", "Errors", new { Area = "" });
            }
            ViewBag.ReferenceNo = ReferenceNo;
            return View("ItemDetails", ppmpItemDetails);
        }

        [HttpPost]
        [ActionName("update-item-details")]
        [Route("{ReferenceNo}/{ItemCode}/details")]
        public ActionResult ItemDetails(PPMPItemDetailsVM ItemDetails)
        {
            if (ItemDetails == null)
            {
                return RedirectToAction("not-found", "Errors", new { Area = "" });
            }
            return Json(new { result = ppmpDAL.UpdatePPMPItem(ItemDetails) });
        }

        //[Route("{FiscalYear}")]
        //[ActionName("list")]
        //public ActionResult Index(int FiscalYear)
        //{
        //    BudgetPropsalVM budgetProposal = ppmpDAL.GetBudgetProposalDetails(User.Identity.Name, FiscalYear);
        //    if (budgetProposal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View("index", budgetProposal);
        //}

        //[ActionName("print-budget-proposal")]
        //[Route("{FiscalYear}/budget-proposal/print")]
        //public ActionResult PrintBudgetProposal(int FiscalYear)
        //{
        //    var stream = ppmpBL.GenerateBudgetProposalReport(Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name, FiscalYear);
        //    Response.Clear();
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.AddHeader("content-length", stream.Length.ToString());
        //    //Response.AddHeader("content-disposition", "attachment; filename=" + ReferenceNo + ".pdf");
        //    Response.ContentType = "application/pdf";
        //    Response.BinaryWrite(stream.ToArray());
        //    stream.Close();
        //    Response.End();

        //    return RedirectToAction("list", new { Area = "end-users" });
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ppmpDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
