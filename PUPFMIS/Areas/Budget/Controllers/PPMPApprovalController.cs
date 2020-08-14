using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Areas.Budget.Controllers
{
    [RouteArea("budget")]
    [RoutePrefix("approval/ppmp")]
    [Route("{action}")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.BudgetOfficer + ", " + SystemRoles.BudgetAdmin)]
    public class PPMPApprovalController : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private PPMPApprovalDAL ppmpApprovalDAL = new PPMPApprovalDAL();
        private PPMPApprovalBL ppmpApprovalBL = new PPMPApprovalBL();

        [Route("{FiscalYear}")]
        [ActionName("offices-list")]
        public ActionResult ListPerOffice(int FiscalYear)
        {
            return View("IndexOfficeList", ppmpApprovalDAL.GetOffices(FiscalYear));
        }

        [Route("{FiscalYear}/{OfficeCode}/list")]
        [ActionName("office-ppmp-list")]
        public ActionResult OfficePPMPList(string FiscalYear, string OfficeCode)
        {
            ViewBag.OfficeCode = OfficeCode;
            ViewBag.FiscalYear = FiscalYear;
            return View("IndexLineItemsPerAccount", ppmpApprovalDAL.GetPPMPLineItemsPerAccount(OfficeCode));
        }

        [ActionName("view-line-items")]
        [Route("{FiscalYear}/{OfficeCode}/{UACS}/details")]
        public ActionResult Details(int FiscalYear, string OfficeCode, string UACS)
        {
            if (UACS == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PPMPEvaluationVM ppmpEvaluation = ppmpApprovalDAL.GetEvaluationDetails(UACS, OfficeCode, FiscalYear);
            if (ppmpEvaluation == null)
            {
                return HttpNotFound();
            }
            ppmpEvaluation.UACS = UACS;
            ViewBag.FiscalYear = FiscalYear;
            ViewBag.FundSource = new SelectList(ppmpApprovalDAL.GetFundSource(), "FUND_CLUSTER", "FUND_DESC");
            return View("DetailsAccountLineItems", ppmpEvaluation);
        }

        [HttpPost]
        [ActionName("update-details")]
        [Route("{FiscalYear}/{OfficeCode}/{UACS}/details")]
        public ActionResult UpdateDetails(PPMPEvaluationVM PPMPEvaluationVM, string FiscalYear, string OfficeCode, string UACS, string updateAction)
        {
            if(updateAction == "update")
            {
                return UpdateItems(PPMPEvaluationVM);
            }
            else
            {
                return ApproveBudget(PPMPEvaluationVM, FiscalYear);
            }
        }

        public ActionResult ApproveBudget(PPMPEvaluationVM PPMPEvaluationVM, string FiscalYear)
        {
            ppmpApprovalDAL.SaveApproval(PPMPEvaluationVM, User.Identity.Name);
            return RedirectToAction("office-ppmp-list", "PPMPApproval", new { Area = "budget", OfficeCode = PPMPEvaluationVM.OfficeCode, FiscalYear = FiscalYear });
        }

        public ActionResult UpdateItems(PPMPEvaluationVM PPMPEvaluationVM)
        {
            foreach (var item in PPMPEvaluationVM.NewSpendingItems)
            {
                if (item.ApprovalAction == "Approved")
                {
                    item.EstimatedCost = item.UnitCost * item.ReducedQuantity;
                }
                if (item.ApprovalAction == "Disapproved")
                {
                    item.EstimatedCost = 0.00m;
                }
            }
            PPMPEvaluationVM.ApprovedBudget = PPMPEvaluationVM.NewSpendingItems.Sum(d => d.EstimatedCost);
            ViewBag.FundSource = new SelectList(ppmpApprovalDAL.GetFundSource(), "FUND_CLUSTER", "FUND_DESC");
            return View("DetailsAccountLineItems", PPMPEvaluationVM);
        }

        [ActionName("print-ppmp")]
        [Route("{ReferenceNo}/view")]
        public ActionResult PrintPPMP(string ReferenceNo)
        {
            var stream = ppmpApprovalBL.ViewPPMP(ReferenceNo, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
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
