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
//    [Route("ops/property/warehouse/{action}")]
//    public class ItemPullOutsController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: ItemPullOuts
//        public ActionResult Index()
//        {
//            List<PullOutVM> pullOut = new List<PullOutVM>();
//            return View(pullOut);
//        }

//        // GET: ItemPullOuts/Details/5
//        public ActionResult Details(int? id)
//        {
//            //if (id == null)
//            //{
//            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            //}
//            ItemPullOutHeader itemPullOutHeader = db.ItemPullOutHeader.Find(id);
//            //if (itemPullOutHeader == null)
//            //{
//            //    return HttpNotFound();
//            //}
//            return View(itemPullOutHeader);
//        }

//        // GET: ItemPullOuts/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: ItemPullOuts/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,TrxDate,Staff,Reference,Purpose")] ItemPullOutHeader itemPullOutHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.ItemPullOutHeader.Add(itemPullOutHeader);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(itemPullOutHeader);
//        }

//        // GET: ItemPullOuts/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ItemPullOutHeader itemPullOutHeader = db.ItemPullOutHeader.Find(id);
//            if (itemPullOutHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(itemPullOutHeader);
//        }

//        // POST: ItemPullOuts/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,TrxDate,Staff,Reference,Purpose")] ItemPullOutHeader itemPullOutHeader)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(itemPullOutHeader).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(itemPullOutHeader);
//        }

//        // GET: ItemPullOuts/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ItemPullOutHeader itemPullOutHeader = db.ItemPullOutHeader.Find(id);
//            if (itemPullOutHeader == null)
//            {
//                return HttpNotFound();
//            }
//            return View(itemPullOutHeader);
//        }

//        // POST: ItemPullOuts/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            ItemPullOutHeader itemPullOutHeader = db.ItemPullOutHeader.Find(id);
//            db.ItemPullOutHeader.Remove(itemPullOutHeader);
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
