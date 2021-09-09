using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("")]
    [RoutePrefix("purchase-requests")]
    public class PurchaseRequestsController : Controller
    {
        private EndUsersPurchaseRequestDAL prDAL = new EndUsersPurchaseRequestDAL();
        private EndUsersPurchaseRequestBL prBL = new EndUsersPurchaseRequestBL();
        private ProcurementProjectsDAL contractDAL = new ProcurementProjectsDAL();

        [Authorize(Roles = SystemRoles.SuppliesChief + ", " + SystemRoles.PropertyDirector + ", " + SystemRoles.EndUser + ", " + SystemRoles.InfrastructureImplementingUnit + ", " + SystemRoles.ResponsibilityCenterPlanner)]
        [Route("dashboard")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            PurchaseRequestDashboard PRDashboard = new PurchaseRequestDashboard();
            PRDashboard.OpenSubmissions = contractDAL.GetContractsOpenForSubmission(User.Identity.Name);
            PRDashboard.ForSubmission = prDAL.GetPurchaseRequests(User.Identity.Name).Where(d => d.PRStatus == PurchaseRequestStatus.PurchaseRequestCreated).ToList();
            PRDashboard.PRFiscalYears = prDAL.GetFiscalYears(User.Identity.Name);
            return View(PRDashboard);
        }

        [Authorize(Roles = SystemRoles.SuppliesChief + ", " + SystemRoles.PropertyDirector + ", " + SystemRoles.EndUser + ", " + SystemRoles.InfrastructureImplementingUnit + ", " + SystemRoles.ResponsibilityCenterPlanner)]
        [Route("{FiscalYear}")]
        [ActionName("index")]
        public ActionResult Index(int FiscalYear)
        {
            var procurementPrograms = prDAL.GetPurchaseRequests(FiscalYear, User.Identity.Name).ToList();
            ViewBag.FiscalYear = FiscalYear;
            return View(procurementPrograms);
        }

        [Authorize(Roles = SystemRoles.SuppliesChief + ", " + SystemRoles.PropertyDirector + ", " + SystemRoles.EndUser + ", " + SystemRoles.InfrastructureImplementingUnit + ", " + SystemRoles.ResponsibilityCenterPlanner)]
        [Route("{ContractCode}/create")]
        [ActionName("create")]
        public ActionResult Create(string ContractCode)
        {
            var purchaseRequestVM = prDAL.SetupPurchaseRequest(ContractCode, User.Identity.Name);
            return View("Create", purchaseRequestVM);
        }

        [Authorize(Roles = SystemRoles.SuppliesChief + ", " + SystemRoles.PropertyDirector + ", " + SystemRoles.EndUser + ", " + SystemRoles.InfrastructureImplementingUnit + ", " + SystemRoles.ResponsibilityCenterPlanner)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("create")]
        [Route("{ContractCode}/create")]
        public ActionResult Create(PurchaseRequestVM PurchaseRequest)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = prDAL.PostPurchaseRequest(User.Identity.Name, PurchaseRequest) });
            }
            return View("Create", PurchaseRequest);
        }

        [Authorize(Roles = SystemRoles.SuppliesChief + ", " + SystemRoles.PropertyDirector + ", " + SystemRoles.EndUser + ", " + SystemRoles.InfrastructureImplementingUnit + ", " + SystemRoles.ResponsibilityCenterPlanner)]
        [Route("{PRNumber}/submit")]
        [ActionName("submit")]
        public ActionResult Submit(string PRNumber)
        {
            return Json(new { result = prDAL.SubmitPurchaseRequest(PRNumber, User.Identity.Name) });
        }

        [Authorize(Roles = SystemRoles.SuppliesChief + ", " + SystemRoles.PropertyDirector + ", " + SystemRoles.EndUser + ", " + SystemRoles.InfrastructureImplementingUnit + ", " + SystemRoles.ResponsibilityCenterPlanner + ", " + SystemRoles.ProcurementAdministrator + ", " + SystemRoles.ProcurementPlanningChief + ", " + SystemRoles.ProjectCoordinator + ", " + SystemRoles.ProcurementStaff)]
        [ActionName("print")]
        [Route("{PRNumber}/print")]
        public ActionResult PrintPurchaseRequest(string PRNumber)
        {
            var stream = prBL.GeneratePurchaseRequest(PRNumber, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
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
                prBL.Dispose();
                prDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}