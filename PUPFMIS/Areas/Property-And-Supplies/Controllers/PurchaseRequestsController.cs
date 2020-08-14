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
    [RouteArea("property-and-supplies")]
    [RoutePrefix("purchase-requests")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SuppliesChief + ", " + SystemRoles.PropertyDirector)]
    public class PurchaseRequestsSuppliesController : Controller
    {
        private PurchaseRequestSuppliesDAL prDAL = new PurchaseRequestSuppliesDAL();
        private PurchaseRequestSuppliesBL prBL = new PurchaseRequestSuppliesBL();

        [Route("list")]
        [ActionName("index")]
        public ActionResult Index()
        {
            var procurementPrograms = prDAL.GetPurchaseRequests(User.Identity.Name);
            return View(procurementPrograms);
        }

        [ActionName("create")]
        [Route("create")]
        public ActionResult Create(string PAPCode)
        {
            ViewBag.FiscalYear = new SelectList(prDAL.GetFiscalYears());
            ViewBag.Period = new SelectList(prDAL.GetQuarters());
            return View();
        }

        [HttpPost]
        [Route("create")]
        [ActionName("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Details(int FiscalYear, string Period)
        {
            var purchaseRequestCSEDetails = new PurchaseRequestCSEVM();
            purchaseRequestCSEDetails.FiscalYear = FiscalYear;
            purchaseRequestCSEDetails.Period = Period;
            purchaseRequestCSEDetails.CSEItems = prDAL.GetCommonSupplies(Period, FiscalYear);
            return View("Details", purchaseRequestCSEDetails);
        }

        [HttpPost]
        [Route("post")]
        [ActionName("post-pr")]
        [ValidateAntiForgeryToken]
        public ActionResult PostPR(PurchaseRequestCSEVM PurchaseRequestCSE)
        {
            return Json(new { result = prDAL.PostPurchaseRequest(PurchaseRequestCSE, User.Identity.Name) });
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