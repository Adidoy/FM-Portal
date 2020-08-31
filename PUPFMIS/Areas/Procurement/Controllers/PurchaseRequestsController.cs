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
    [RoutePrefix("purchase-requests")]
    [Authorize(Roles = SystemRoles.ProjectCoordinator + ", " + SystemRoles.ProcurementStaff)]
    public class ProcurementPurchaseRequestsController : Controller
    {
        ProcurementPurchaseRequestDAL prDAL = new ProcurementPurchaseRequestDAL();
        ProcurementPurchaseRequestBL prBL = new ProcurementPurchaseRequestBL();

        [Route("pending-submissions")]
        [ActionName("pending-submissions")]
        public ActionResult Index()
        {
            return View("Index", prDAL.GetPurchaseRequests(User.Identity.Name));
        }

        [ActionName("details")]
        [Route("{PRNumber}/details")]
        public ActionResult Details(string PRNumber)
        {
            return View(prDAL.GetPurchaseRequest(PRNumber));
        }

        [HttpPost]
        [ActionName("details")]
        [ValidateAntiForgeryToken()]
        [Route("{PRNumber}/details")]
        public ActionResult Details(PurchaseRequestVM PurchaseRequest)
        {
            return Json(new { result = prDAL.PostPRReceive(PurchaseRequest.PRNumber, User.Identity.Name) });
        }

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
    }
}