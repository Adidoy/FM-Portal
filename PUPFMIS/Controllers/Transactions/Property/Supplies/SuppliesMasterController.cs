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
//    [Route("ops/property/supplies/{action}")]
//    public class SuppliesMasterController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: SuppliesMaster
//        public ActionResult Index()
//        {
//            var SuppliesMaster = db.SuppliesMaster.Include(s => s.FKItem).Include(s => s.FKUOMDelivery).Include(s => s.FKUOMDistribution);
//            return View(SuppliesMaster.ToList());
//        }

//        // GET: StockCards/Details/5
//        //public ActionResult StockCards(int? id)
//        //{
//        //    if (id == null)
//        //    {
//        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//        //    }
//        //    StockCard stockCard = db.StockCard.Find(id);
//        //    if (stockCard == null)
//        //    {
//        //        return HttpNotFound();
//        //    }
//        //    return View(stockCard);
//        //}

//        [Route("ops/property/supplies/{id}/stock-cards")]
//        public ActionResult StockCards(int? id)
//        {
//            //if (id == null)
//            //    {
//            //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            //    }
//            ViewBag.FundCluster = new List<SelectListItem>(){
//                 new SelectListItem { Text = "General Fund", Value = "GF"},
//                 new SelectListItem { Text = "Regular Trust Fund", Value = "RTF" },
//                 new SelectListItem { Text = "Special Trust Fund", Value = "STF" }
//                };
//            StockCard stockCard = db.StockCard.Find(id);
//            //    if (stockCard == null)
//            //    {
//            //        return HttpNotFound();
//            //    }
//            return View();
//        }

//        // GET: SuppliesMaster/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SuppliesMaster SuppliesMaster = db.SuppliesMaster.Find(id);
//            if (SuppliesMaster == null)
//            {
//                return HttpNotFound();
//            }
//            return View(SuppliesMaster);
//        }

//        // GET: SuppliesMaster/Create
//        public ActionResult Create()
//        {
//            ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName");
//            ViewBag.UOMDelivery = new SelectList(db.UOM, "ID", "UnitName");
//            ViewBag.UOMDistribution = new SelectList(db.UOM, "ID", "UnitName");
//            return View();
//        }

//        // POST: SuppliesMaster/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,StockNo,UOMDelivery,UOMDistribution,ItemID")] SuppliesMaster SuppliesMaster)
//        {
//            if (ModelState.IsValid)
//            {
//                db.SuppliesMaster.Add(SuppliesMaster);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName", SuppliesMaster.ItemID);
//            ViewBag.UOMDelivery = new SelectList(db.UOM, "ID", "UnitName", SuppliesMaster.UOMDelivery);
//            ViewBag.UOMDistribution = new SelectList(db.UOM, "ID", "UnitName", SuppliesMaster.UOMDistribution);
//            return View(SuppliesMaster);
//        }

//        // GET: SuppliesMaster/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SuppliesMaster SuppliesMaster = db.SuppliesMaster.Find(id);
//            if (SuppliesMaster == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName", SuppliesMaster.ItemID);
//            ViewBag.UOMDelivery = new SelectList(db.UOM, "ID", "UnitName", SuppliesMaster.UOMDelivery);
//            ViewBag.UOMDistribution = new SelectList(db.UOM, "ID", "UnitName", SuppliesMaster.UOMDistribution);
//            return View(SuppliesMaster);
//        }

//        // POST: SuppliesMaster/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,StockNo,UOMDelivery,UOMDistribution,ItemID")] SuppliesMaster SuppliesMaster)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(SuppliesMaster).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.ItemID = new SelectList(db.ItemsMaster, "ID", "ItemName", SuppliesMaster.ItemID);
//            ViewBag.UOMDelivery = new SelectList(db.UOM, "ID", "UnitName", SuppliesMaster.UOMDelivery);
//            ViewBag.UOMDistribution = new SelectList(db.UOM, "ID", "UnitName", SuppliesMaster.UOMDistribution);
//            return View(SuppliesMaster);
//        }

//        // GET: SuppliesMaster/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            SuppliesMaster SuppliesMaster = db.SuppliesMaster.Find(id);
//            if (SuppliesMaster == null)
//            {
//                return HttpNotFound();
//            }
//            return View(SuppliesMaster);
//        }

//        // POST: SuppliesMaster/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            SuppliesMaster SuppliesMaster = db.SuppliesMaster.Find(id);
//            db.SuppliesMaster.Remove(SuppliesMaster);
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
