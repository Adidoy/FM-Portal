//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;
//using PUPFMIS.BusinessAndDataLogic;

//namespace PUPFMIS.Controllers
//{
//    [Authorize]
//    [RoutePrefix("master-tables/services")]
//    public class ServicesController : Controller
//    {
//        private ServicesBL serviceBL = new ServicesBL();
//        private ServiceDAL serviceDAL = new ServiceDAL();
        
//        [Route("")]
//        [Route("list")]
//        [ActionName("list")]
//        public ActionResult Index()
//        {
//            var services = serviceDAL.GetServices(false);
//            return View("index", services);
//        }

//        [Route("{ServiceCode}/details")]
//        [ActionName("view-service-details")]
//        public ActionResult Details(string ServiceCode)
//        {
//            if (ServiceCode == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ServicesVM service = serviceDAL.GetServiceDetails(ServiceCode);
//            if (service == null)
//            {
//                return HttpNotFound();
//            }
//            return View("details", service);
//        }

//        [Route("create")]
//        [ActionName("add-service")]
//        public ActionResult Create()
//        {
//            ViewBag.ServiceTypeReference = new SelectList(serviceDAL.GetInventoryTypes(), "ID", "InventoryTypeName");
//            ViewBag.ServiceCategoryReference = new SelectList(serviceDAL.GetCategories(ProcurementSources.PS_DBM), "ID", "ItemCategoryName");
//            ViewBag.AccountClass = new SelectList(serviceDAL.GetAccounts(), "UACS_Code", "AcctName");
//            return View("create");
//        }

//        [HttpPost]
//        [Route("create")]
//        [ValidateAntiForgeryToken]
//        [ActionName("add-service")]
//        public ActionResult Create(Services service)
//        {
//            if (ModelState.IsValid)
//            {
//                var Message = string.Empty;
//                if(serviceDAL.AddServiceRecord(service, User.Identity.Name, out Message))
//                {
//                    return RedirectToAction("list", new { Area = "" });
//                }
//            }

//            ViewBag.ServiceTypeReference = new SelectList(serviceDAL.GetInventoryTypes(), "ID", "InventoryTypeName", service.ServiceTypeReference);
//            ViewBag.ServiceCategoryReference = new SelectList(serviceDAL.GetCategories(ProcurementSources.PS_DBM), "ID", "ItemCategoryName", service.ServiceCategoryReference);
//            ViewBag.AccountClass = new SelectList(serviceDAL.GetAccounts(), "UACS_Code", "AcctName", service.AccountClass);
//            return View("create", service);
//        }

//        [ActionName("update-service")]
//        [Route("{ServiceCode}/update")]
//        public ActionResult Edit(string ServiceCode)
//        {
//            if (ServiceCode == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Services service = serviceDAL.GetServiceByCode(ServiceCode, false);
//            if (service == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.ServiceTypeReference = new SelectList(serviceDAL.GetInventoryTypes(), "ID", "InventoryTypeName", service.ServiceTypeReference);
//            ViewBag.ServiceCategoryReference = new SelectList(serviceDAL.GetCategories(ProcurementSources.PS_DBM), "ID", "ItemCategoryName", service.ServiceCategoryReference);
//            ViewBag.AccountClass = new SelectList(serviceDAL.GetAccounts(), "UACS_Code", "AcctName", service.AccountClass);
//            return View("edit", service);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [ActionName("update-service")]
//        [Route("{ServiceCode}/update")]
//        public ActionResult Edit(Services service)
//        {
//            if (ModelState.IsValid)
//            {
//                string Message = string.Empty;
//                if(serviceDAL.UpdateServiceRecord(service, User.Identity.Name, out Message))
//                {
//                    return RedirectToAction("list", "Services", new { Area = "" });
//                }
//                return RedirectToAction("Index");
//            }
//            ViewBag.ServiceTypeReference = new SelectList(serviceDAL.GetInventoryTypes(), "ID", "InventoryTypeName", service.ServiceTypeReference);
//            ViewBag.ServiceCategoryReference = new SelectList(serviceDAL.GetCategories(ProcurementSources.PS_DBM), "ID", "ItemCategoryName", service.ServiceCategoryReference);
//            ViewBag.AccountClass = new SelectList(serviceDAL.GetAccounts(), "UACS_Code", "AcctName", service.AccountClass);
//            return View(service);
//        }

//        [ActionName("purge-service")]
//        [Route("{ServiceCode}/delete")]
//        public ActionResult Delete(string ServiceCode)
//        {
//            if (ServiceCode == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ServicesVM services = serviceDAL.GetServiceDetails(ServiceCode);
//            if (services == null)
//            {
//                return HttpNotFound();
//            }
//            return View("delete", services);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        [ActionName("purge-service")]
//        [Route("{ServiceCode}/delete")]
//        public ActionResult DeleteConfirmed(string ServiceCode)
//        {
//            if (ServiceCode == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            string Message = string.Empty;
//            if (serviceDAL.DeleteServiceRecord(ServiceCode, User.Identity.Name, out Message))
//            {
//                return RedirectToAction("list", "Services", new { Area = "" });
//            }
//            return RedirectToAction("list", new { Area = "" });
//        }

//        [Route("deleted-list")]
//        [ActionName("view-deleted-services")]
//        public ActionResult DeletedIndex()
//        {
//            var services = serviceDAL.GetServices(true);
//            return View("indexDeleted", services);
//        }

//        [ActionName("restore-service-confirmed")]
//        [Route("{ServiceCode}/restore")]
//        public ActionResult RestoreConfirmed(string ServiceCode)
//        {
//            if (ServiceCode == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Services service = serviceDAL.GetServiceByCode(ServiceCode, true);
//            if (service == null)
//            {
//                return HttpNotFound();
//            }
//            string Message = string.Empty;
//            if (serviceDAL.RestoreService(service.ServiceCode, User.Identity.Name, out Message))
//            {
//                return RedirectToAction("list", "Services", new { Area = "" });
//            }
//            return RedirectToAction("list", new { Area = "" });
//        }

//        [ActionName("get-categories")]
//        public JsonResult GetCategories(ProcurementSources ProcurementSource)
//        {
//            return Json(serviceDAL.GetCategories(ProcurementSource), JsonRequestBehavior.AllowGet);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                serviceBL.Dispose();
//                serviceDAL.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
