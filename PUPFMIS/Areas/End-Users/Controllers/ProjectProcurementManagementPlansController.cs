using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Areas.EndUsers.Controllers
{
    [Route("{action}")]
    [RoutePrefix("ppmp")]
    [RouteArea("end-users")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
    public class ProjectProcurementManagementPlansController : Controller
    {
        private ProjectProcurementManagementPlanBL ppmpBL = new ProjectProcurementManagementPlanBL();
        private ProjectProcurementManagementPlanDAL ppmpDAL = new ProjectProcurementManagementPlanDAL();

        [Route("{FiscalYear}")]
        [ActionName("list")]
        public ActionResult Index(int FiscalYear)
        {
            BudgetPropsalVM budgetProposal = ppmpDAL.GetBudgetProposalDetails(User.Identity.Name, FiscalYear);
            if(budgetProposal == null)
            {
                return HttpNotFound();
            }
            return View("index", budgetProposal);
        }

        [Route("{ReferenceNo}/details")]
        [ActionName("view-ppmp-details")]
        public ActionResult Details(string ReferenceNo)
        {
            if (ReferenceNo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ppmpVM = ppmpDAL.GetPPMPDetails(ReferenceNo, User.Identity.Name);
            if (ppmpVM == null)
            {
                return HttpNotFound();
            }
            return View("details", ppmpVM);
        }

        [ActionName("print-ppmp")]
        [Route("{ReferenceNo}/print")]
        public ActionResult PrintPPMP(string ReferenceNo)
        { 
            var stream = ppmpBL.PrintPPMP(ReferenceNo, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
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

        [ActionName("print-budget-proposal")]
        [Route("{FiscalYear}/budget-proposal/print")]
        public ActionResult PrintBudgetProposal(int FiscalYear)
        {
            var stream = ppmpBL.GenerateBudgetProposalReport(Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name, FiscalYear);
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
        [Route("submit-ppmp")]
        [ValidateAntiForgeryToken]
        [ActionName("submit-ppmp")]
        public ActionResult SubmitPPMP(BudgetPropsalVM BudgetProposal)
        {
            foreach(var item in BudgetProposal.PPMPList)
            {
                if(item.IsSelected == true)
                {
                    if(!ppmpDAL.SubmitPPMP(item.ReferenceNo, User.Identity.Name))
                    {
                        return Json(false);
                    }
                }
            }
            return Json(true);
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ppmpDAL.Dispose();
                ppmpBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
