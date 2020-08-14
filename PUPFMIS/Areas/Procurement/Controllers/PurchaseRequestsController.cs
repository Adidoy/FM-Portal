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
    }
}