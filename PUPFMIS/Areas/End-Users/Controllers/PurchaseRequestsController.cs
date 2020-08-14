using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.End_Users.Controllers
{

    [Route("{action}")]
    [RouteArea("end-users")]
    [RoutePrefix("purchase-requests")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
    public class PurchaseRequestsController : Controller
    {
        private PurchaseRequestDAL prDAL = new PurchaseRequestDAL();
        private PurchaseRequestBL prBL = new PurchaseRequestBL();

        [Route("list")]
        [ActionName("index")]
        public ActionResult Index()
        {
            var procurementPrograms = prDAL.GetPurchaseRequests(User.Identity.Name);
            return View(procurementPrograms);
        }

        [ActionName("details")]
        [Route("{PAPCode}/details")]
        public ActionResult Details(string PAPCode)
        {
            var procurementProgram = prDAL.GetProcurementProgramDetailsByPAPCode(PAPCode, User.Identity.Name);
            return View(procurementProgram);
        }

        [HttpPost]
        [ActionName("details")]
        [Route("{PAPCode}/details")]
        public ActionResult Details(ProcurementProjectsVM ProcurementProgram)
        {
            return Json(new { result = prDAL.PostPurchaseRequest(ProcurementProgram, User.Identity.Name) });
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                prDAL.Dispose();
                prBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}