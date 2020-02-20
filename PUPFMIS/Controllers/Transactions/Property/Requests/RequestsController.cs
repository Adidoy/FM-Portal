//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;

//namespace PUPFMIS.Controllers
//{
//    //[Route("ops/property/requests/action")]
//    public class RequestsController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        [Route("ops/property/requests/{type}/index")]
//        // GET: Requests
//        public ActionResult Index(string type)
//        {
//            if (type == "client")
//            {
//                return View("IndexClient", db.RequestHeader.ToList());
//            }
//            else if (type == "custodian")
//            {
//                return View("IndexCustodian", db.RequestHeader.ToList());
//            }
//            else if (type == "release")
//            {
//                return View("IndexRelease", db.RequestHeader.ToList());
//            }
//            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
//        }

//        // GET: Requests/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            RequestHeader requestHeader = db.RequestHeader.Find(id);
//            if (requestHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(requestHeader);
//        }

//        [Route("ops/property/requests/view-basket/client")]
//        public ActionResult RequestBasket()
//        {
//            var requestBasket = db.RequestDetails.ToList();
//            return View(requestBasket);
//        }

//        public ActionResult Create(string type)
//        {
//            if (type == "client")
//            {
//                return View("RequestCreate");
//            }
//            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
//        }

//        [Route("ops/property/requests/custodian/{id?}/{requestType}/review")]
//        public ActionResult ReviewRequest(int? id, string requestType)
//        {
//            if (requestType == "supplies")
//            {
//                //if (id == null)
//                //{
//                //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//                //}
//                RequestVM requestVM = new RequestVM();
//                RequestHeader requestHeader = db.RequestHeader.Find(id);
//                var requestDetail = db.RequestDetails.Where(d => d.RequestID == id).ToList();
//                requestVM.RequestHeader = requestHeader;
//                requestVM.RequestDetail = requestDetail;
//                //if (requestHeader == null)
//                //{
//                //    return HttpNotFound();
//                //}
//                return View("ReviewSuppliesRequest", requestVM);
//            }
//            else if (requestType == "semi-expendable")
//            {
//                //if (id == null)
//                //{
//                //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//                //}
//                ViewData["Request"] = db.RequestHeader.Find(id);
//                //if (requestHeader == null)
//                //{
//                //    return HttpNotFound();
//                //}
//                return View("ReviewSemiExpendableRequest");
//            }
//            else if (requestType == "ppe")
//            {
//                return View("ReviewPPERequest");
//            }
//            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
//        }

//        [Route("ops/property/requests/{type?}/{id?}")]
//        public ActionResult RequestRelease(string type, int? id)
//        {
//            if (type == "release")
//            {
//                if (id == null)
//                {
//                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//                }
//                RequestVM requestVM = new RequestVM();
//                RequestHeader requestHeader = db.RequestHeader.Find(id);
//                var requestDetail = db.RequestDetails.Where(d => d.RequestID == id).ToList();
//                requestVM.RequestHeader = requestHeader;
//                requestVM.RequestDetail = requestDetail;
//                //if (requestHeader == null)
//                //{
//                //    return HttpNotFound();
//                //}
//                return View("RequestRelease", requestVM);
//            }
//            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        }

//        [Route("ops/property/requests/custodian/{requestID?}/create-ics")]
//        public ActionResult CreateICS(int requestID)
//        {
//            ViewData["Reference"] = db.RequestHeader.Find(requestID);
//            return View();
//        }

//        [Route("ops/property/requests/custodian/{requestID?}/create-par")]
//        public ActionResult CreatePAR(int requestID)
//        {
//            ViewData["Reference"] = db.RequestHeader.Find(requestID);
//            return View();
//        }



//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,RISNo,Office,RequestDate,RequestStatus,DateReviewed,DateCancelled,DateIssued")] RequestHeader requestHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.RequestHeader.Add(requestHeader);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(requestHeader);
//        }

//        // GET: Requests/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            RequestHeader requestHeader = db.RequestHeader.Find(id);
//            if (requestHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(requestHeader);
//        }

//        // POST: Requests/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,RISNo,Office,RequestDate,RequestStatus,DateReviewed,DateCancelled,DateIssued")] RequestHeader requestHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(requestHeader).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(requestHeader);
//        }

//        // GET: Requests/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            RequestHeader requestHeader = db.RequestHeader.Find(id);
//            if (requestHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(requestHeader);
//        }

//        // POST: Requests/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            RequestHeader requestHeader = db.RequestHeader.Find(id);
//            db.RequestHeader.Remove(requestHeader);
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
