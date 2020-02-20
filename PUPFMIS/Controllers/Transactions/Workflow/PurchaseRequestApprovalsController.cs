//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using PUPFMIS.Models;

//namespace PUPFMIS.Controllers.Transactions.Workflow
//{
//    [Route("ops/procurement/puchase-request-approval/{action}")]
//    [Authorize]
//    public class PurchaseRequestApprovalsController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: PurchaseRequestApprovals
//        public ActionResult Index()
//        {
//            return View(db.PRApproval.ToList());
//        }

//        // GET: PurchaseRequestApprovals/Details/5
//        //public ActionResult Details(int? id)
//        //{
//        //    if (id == null)
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    PurchaseRequestApproval purchaseRequestApproval = db.PRApproval.Find(id);
//        //    if (purchaseRequestApproval == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(purchaseRequestApproval);
//        //}

//        public ActionResult Details()
//        {
//            return View();
//        }

//        public ActionResult Review()
//        {
//            return View();
//        }

//        // GET: PurchaseRequestApprovals/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: PurchaseRequestApprovals/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,Action,Remarks,ActionTakenBy,CreatedAt")] PurchaseRequestApproval purchaseRequestApproval)
//        {
//            if (ModelState.IsValid)
//            {
//                db.PRApproval.Add(purchaseRequestApproval);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(purchaseRequestApproval);
//        }

//        // GET: PurchaseRequestApprovals/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PurchaseRequestApproval purchaseRequestApproval = db.PRApproval.Find(id);
//            if (purchaseRequestApproval == null)
//            {
//                return HttpNotFound();
//            }
//            return View(purchaseRequestApproval);
//        }

//        // POST: PurchaseRequestApprovals/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,Action,Remarks,ActionTakenBy,CreatedAt")] PurchaseRequestApproval purchaseRequestApproval)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(purchaseRequestApproval).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(purchaseRequestApproval);
//        }

//        // GET: PurchaseRequestApprovals/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            PurchaseRequestApproval purchaseRequestApproval = db.PRApproval.Find(id);
//            if (purchaseRequestApproval == null)
//            {
//                return HttpNotFound();
//            }
//            return View(purchaseRequestApproval);
//        }

//        // POST: PurchaseRequestApprovals/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            PurchaseRequestApproval purchaseRequestApproval = db.PRApproval.Find(id);
//            db.PRApproval.Remove(purchaseRequestApproval);
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
