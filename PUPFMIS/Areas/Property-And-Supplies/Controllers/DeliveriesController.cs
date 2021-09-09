using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Areas.Property_And_Supplies.Controllers
{
    [Route("{action}")]
    [RouteArea("property-and-supplies")]
    [RoutePrefix("deliveries")]
    [Authorize(Roles = SystemRoles.PropertyDirector + ", " + SystemRoles.SuppliesChief + ", " + SystemRoles.SuppliesStaff)]
    public class DeliveriesController : Controller
    {
        private HRISDataAccess hris = new HRISDataAccess();
        private ContractsDAL contractsDAL = new ContractsDAL();
        private DeliveriesBL deliveriesBL = new DeliveriesBL();
        private DeliveriesDAL deliveriesDAL = new DeliveriesDAL();
        private UnitOfMeasureDataAccess unitsDAL = new UnitOfMeasureDataAccess();

        [Route("")]
        [ActionName("dashboard")]
        public ActionResult Dashboard()
        {
            var dashboard = new DeliveriesDashboard();
            dashboard.ContractsForDelivery = contractsDAL.GetContractsForDelivery();
            return View(dashboard);
        }

        [ActionName("details")]
        [Route("{ReferenceNo}/details")]
        public ActionResult Details(string ReferenceNo)
        {
            ViewBag.ReferenceNo = ReferenceNo;
            var unregisteredItems = new UnregisteredItemsVM();
            unregisteredItems.Supplies = deliveriesDAL.GetUnregisteredSupplies(ReferenceNo);
            unregisteredItems.PPE = deliveriesDAL.GetUnregisteredPPE(ReferenceNo);
            if(unregisteredItems.Supplies.Count >= 1 || unregisteredItems.PPE.Count >= 1)
            {
                return View("RegisterItems", unregisteredItems);
            }
            else
            {
                var deliverySetup = deliveriesDAL.GetDeliverySetup(ReferenceNo);
                ViewBag.DeliveryUnit = new SelectList(unitsDAL.GetUOMs(false), "ID", "UnitName");
                ViewBag.ReceivedBy = new SelectList(hris.GetEmployees("PSMO"), "EmployeeCode", "EmployeeName");
                return View("Details", deliverySetup);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("register-items")]
        public ActionResult PostUnregisteredItems(UnregisteredItemsVM UnregisteredItems)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = deliveriesDAL.PostUnregisteredItems(UnregisteredItems, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            return PartialView("_RegisterItems", UnregisteredItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("post-delivery")]
        public ActionResult PostDelivery(DeliveryVM Delivery)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = deliveriesDAL.PostDelivery(Delivery, User.Identity.Name), JsonRequestBehavior.AllowGet });
            }
            ViewBag.DeliveryUnit = new SelectList(unitsDAL.GetUOMs(false), "ID", "UnitName");
            ViewBag.ReceivedBy = new SelectList(hris.GetEmployees("PSMO"), "EmployeeCode", "EmployeeName");
            return PartialView("_Details", Delivery);
        }

        [Route("list")]
        [ActionName("deliveries-list")]
        public ActionResult Index()
        {
            var deliveries = deliveriesDAL.GetDeliveryList();
            return View("Index", deliveries);
        }

        [Route("{ReferenceNo}/print")]
        [ActionName("print-delivery")]
        public ActionResult PrintPurchaseOrder(string ReferenceNo)
        {
            var stream = deliveriesBL.GenerateDeliveryAcceptanceReport(ReferenceNo, Server.MapPath("~/Content/imgs/PUPLogo.png"), User.Identity.Name);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-length", stream.Length.ToString());
            Response.AppendHeader("Content-Disposition", "inline; filename=Delivery Receipt Report - " + ReferenceNo + ".pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(stream.ToArray());
            stream.Close();
            Response.End();

            return RedirectToAction("dashboard");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                hris.Dispose();
                unitsDAL.Dispose();
                deliveriesBL.Dispose();
                contractsDAL.Dispose();
                deliveriesDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}