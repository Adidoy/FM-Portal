using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PUPFMIS.Areas.Procurement.Controllers
{
    [Route("{action}")]
    [RouteArea("procurement")]
    [RoutePrefix("agency-procurement-request")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin + ", " + SystemRoles.ProcurementAdministrator + ", " + SystemRoles.ProcurementPlanningChief + ", " + SystemRoles.ProcurementStaff)]
    public class AgemcyProcurementRequestController : Controller
    {
        private AgencyProcurementRequestDAL aprDAL = new AgencyProcurementRequestDAL();
        private AgencyProcurementRequestBL aprBL = new AgencyProcurementRequestBL();

        [Route("")]
        [ActionName("index")]
        public ActionResult Index()
        {
            return View(aprDAL.GetAPR());
        }

        [Route("create")]
        [ActionName("create")]
        public ActionResult Create()
        {
            return View(aprDAL.GetPurchaseRequestDetails());
        }

        [HttpPost]
        [Route("create")]
        [ActionName("create")]
        public ActionResult Create(List<PurchaseRequestDetailsVM> PRDetails)
        {
            return Json(new { result = aprDAL.PostToAgencyProcurementRequest(PRDetails, User.Identity.Name) });
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