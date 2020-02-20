//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;

//namespace PUPFMIS.Controllers.Transactions.Property.Delivery
//{
//    [Route("ops/property/deliveries/{action}")]
//    public class DeliveriesController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: Deliveries
//        public ActionResult Index()
//        {
//            var deliveryHeader = db.DeliveryHeader;
//            return View(deliveryHeader.ToList());
//        }

//        // GET: Deliveries/Details/5
//        //public ActionResult Details(int? id)
//        //{
//        //    if (id == null)
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    DeliveryHeader deliveryHeader = db.DeliveryHeader.Find(id);
//        //    if (deliveryHeader == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(deliveryHeader);
//        //}

//        public ActionResult Details()
//        {
//            return View();
//        }

//        // GET: Deliveries/Create
//        public ActionResult Create()
//        {
            
//            return View();
//        }

//        [Route("ops/property/deliveries/getreferences/{type}")]
//        public ActionResult GetReferences(string type)
//        {
//            if(type == "APR")
//            {
//                return PartialView("ModalAPR");
//            }
//            else if(type == "PO")
//            {
//                return PartialView("ModalPO");
//            }
//            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        }

//        // POST: Deliveries/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,DeliveryAcceptanceNumber,DateProcessed,ProcessedBy,ReceivedBy,PONumber,PODate,InvoiceNumber,InvoiceDate,DRNumber,DeliveryDate,Supplier")] DeliveryHeader deliveryHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.DeliveryHeader.Add(deliveryHeader);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(deliveryHeader);
//        }

//        // GET: Deliveries/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            DeliveryHeader deliveryHeader = db.DeliveryHeader.Find(id);
//            if (deliveryHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(deliveryHeader);
//        }

//        // POST: Deliveries/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,DeliveryAcceptanceNumber,DateProcessed,ProcessedBy,ReceivedBy,PONumber,PODate,InvoiceNumber,InvoiceDate,DRNumber,DeliveryDate,Supplier")] DeliveryHeader deliveryHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(deliveryHeader).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(deliveryHeader);
//        }

//        // GET: Deliveries/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            DeliveryHeader deliveryHeader = db.DeliveryHeader.Find(id);
//            if (deliveryHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(deliveryHeader);
//        }

//        // POST: Deliveries/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            DeliveryHeader deliveryHeader = db.DeliveryHeader.Find(id);
//            db.DeliveryHeader.Remove(deliveryHeader);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
