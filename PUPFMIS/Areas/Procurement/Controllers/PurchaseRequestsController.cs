using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.Areas.Procurement.Controllers
{

    [Route("{action}")]
    [RouteArea("procurement")]
    [RoutePrefix("purchase-requests")]
    [Authorize(Roles = SystemRoles.ProcurementPlanningChief + "," + SystemRoles.ProcurementStaff + "," + SystemRoles.ProcurementAdministrator)]
    public class ProcurementPurchaseRequestsController : Controller
    {
        ProcurementPurchaseRequestDAL prDAL = new ProcurementPurchaseRequestDAL();
        ProcurementPurchaseRequestBL prBL = new ProcurementPurchaseRequestBL();
        ProcurementProjectsDAL contractDAL = new ProcurementProjectsDAL();

        [Route("dashboard")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            var dashboard = new PurchaseRequestCoordinatorDashboard();
            dashboard.NewProcurementProjects = contractDAL.GetContracts(User.Identity.Name, ProcurementProjectStages.ContractOpened);
            dashboard.OpenPRSubmissions = contractDAL.GetContracts(User.Identity.Name, ProcurementProjectStages.PurchaseRequestSubmissionOpening);
            dashboard.ForReceiving = prDAL.GetPendingSubmissions();
            dashboard.PRFiscalYears = prDAL.GetFiscalYears();
            return View("Dashboard", dashboard);
        }

        [Route("{FiscalYear}")]
        [ActionName("index")]
        public ActionResult Index(int FiscalYear)
        {
            var purchaseRequests = prDAL.GetPurchaseRequests(FiscalYear).ToList();
            ViewBag.FiscalYear = FiscalYear;
            return View("Index", purchaseRequests);
        }

        [ActionName("details")]
        [Route("{ContractCode}/details")]
        public ActionResult Details(string ContractCode)
        {
            var procurementProjectStage = contractDAL.GetProcurementProjectStage(ContractCode);
            var contractStrategy = contractDAL.GetContractStrategy(ContractCode);
            if(procurementProjectStage == ProcurementProjectStages.ContractOpened)
            {
                if (contractStrategy == ContractStrategies.LotBidding)
                {
                    var contract = contractDAL.GetLotContractDetails(ContractCode);
                    return View("ContractLotOpened", contract);
                }
                else
                {
                    if(ContractCode.Contains("ATA"))
                    {
                        var contract = contractDAL.GetA2AContractDetails(ContractCode);
                        return View("ContractA2AOpened", contract);
                    }
                    else
                    {
                        var contract = contractDAL.GetSingleContractDetails(ContractCode);
                        return View("ContractOpened", contract);
                    }
                }
            }
            else
            {
                if (contractStrategy == ContractStrategies.LotBidding)
                {
                    var contract = contractDAL.GetLotContractDetails(ContractCode);
                    return View("PurchaseRequestLotSubmissionOpening", contract);
                }
                else
                {
                    var contract = contractDAL.GetSingleContractDetails(ContractCode);
                    return View("PurchaseRequestSingleSubmissionOpening", contract);
                }
            }
        }

        [ActionName("open-pr-submission")]
        [Route("{ContractCode}/open-pr-submission")]
        public ActionResult OpenPRSubmission(string ContractCode)
        {
            return Json(new { result = contractDAL.OpenPRSubmission(ContractCode, User.Identity.Name) }, JsonRequestBehavior.AllowGet);
        }

        [Route("open-submissions")]
        [ActionName("open-submissions")]
        public ActionResult OpenSubmissionsIndex()
        {
            var contracts = contractDAL.GetContracts(User.Identity.Name, ProcurementProjectStages.PurchaseRequestSubmissionOpening);
            return View("OpenSubmissionsIndex", contracts);
        }

        [Route("{ContractCode}/close-pr-submission")]
        [ActionName("close-pr-submission")]
        public ActionResult ClosePRSubmission(string ContractCode)
        {
            return Json(new { result = contractDAL.ClosePRSubmission(ContractCode, User.Identity.Name) }, JsonRequestBehavior.AllowGet);
        }

        [Route("open-submissions/{ContractCode}/memo/print")]
        [ActionName("print-purchase-request-memo")]
        public ActionResult PrintSubmissionMemo(string ContractCode)
        {
            var stream = prBL.GeneratePurchaseRequestMemorandum(ContractCode, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        [Route("{PRNumber}/receive")]
        [ActionName("receive")]
        public ActionResult ReceivePurchaseRequest(string PRNumber)
        {
            return Json(new { result = prDAL.ReceivePurchaseRequest(PRNumber, User.Identity.Name) });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                prBL.Dispose();
                prDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}