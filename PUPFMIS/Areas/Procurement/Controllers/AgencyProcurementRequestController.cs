using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("procurement")]
    [RoutePrefix("agency-procurement-request")]
    [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin + ", " + SystemRoles.ProcurementAdministrator + ", " + SystemRoles.ProcurementPlanningChief + ", " + SystemRoles.ProcurementStaff + ", " + SystemRoles.ProjectCoordinator)]
    public class AgencyProcurementRequestController : Controller
    {
        private AgencyProcurementRequestDAL aprDAL = new AgencyProcurementRequestDAL();
        private AgencyProcurementRequestBL aprBL = new AgencyProcurementRequestBL();

        [Route("")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            var dashboard = new APRDashboardVM();
            dashboard.FiscalYears = aprDAL.GetFiscalYears();
            dashboard.NewProjects = aprDAL.GetContracts(User.Identity.Name);
            return View("Dashboard", dashboard);
        }

        [Route("{FiscalYear}")]
        [ActionName("index")]
        public ActionResult Index(int FiscalYear)
        {
            return View(aprDAL.GetAgencyProcurementRequests(FiscalYear));
        }

        [Route("{ContractCode}/create")]
        [ActionName("create")]
        public ActionResult Create(string ContractCode)
        {
            var aprVM = aprDAL.A2AContractSetup(ContractCode, User.Identity.Name);
            return View("Create", aprVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{ContractCode}/create")]
        [ActionName("create")]
        public ActionResult Create(AgencyProcurementRequestVM AgencyProcurementRequestVM)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = aprDAL.PostAgencyProcurementRequest(AgencyProcurementRequestVM, User.Identity.Name) });
            }
            return View("Create", AgencyProcurementRequestVM);
        }

        [ActionName("print")]
        [Route("{AgencyControlNo}/print")]
        public ActionResult PrintPurchaseRequest(string AgencyControlNo)
        {
            var stream = aprBL.GenerateAgencyProcurementRequest(AgencyControlNo, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
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
    }
}