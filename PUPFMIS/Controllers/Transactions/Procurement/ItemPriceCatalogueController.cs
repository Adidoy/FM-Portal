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
//    [Route("ops/procurement/item-catalogue/{action}")]
//    [Authorize]
//    public class ItemsCatalogueController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: ItemsCatalogue
//        public ActionResult Index()
//        {
//            var itemsCatalogue = db.ItemsCatalogue.Include(i => i.FKItems);
//            return View(itemsCatalogue.ToList());
//        }

//        // GET: ItemsCatalogue/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ItemsCatalogue itemsCatalogue = db.ItemsCatalogue.Find(id);
//            if (itemsCatalogue == null)
//            {
//                return HttpNotFound();
//            }
//            return View(itemsCatalogue);
//        }

//        // GET: ItemsCatalogue/Create
//        public ActionResult Create()
//        {
//            ViewBag.Item = new SelectList(db.ItemsMaster, "ID", "ItemSpecifications");
//            return View();
//        }

//        // POST: ItemsCatalogue/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,Item,UnitPrice,EffectivityDate,CreatedAt,UpdatedAt,DeletedAt")] ItemsCatalogue itemsCatalogue)
//        {
//            if (ModelState.IsValid)
//            {
//                db.ItemsCatalogue.Add(itemsCatalogue);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.Item = new SelectList(db.ItemsMaster, "ID", "ItemSpecifications", itemsCatalogue.Item);
//            return View(itemsCatalogue);
//        }

//        // GET: ItemsCatalogue/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ItemsCatalogue itemsCatalogue = db.ItemsCatalogue.Find(id);
//            if (itemsCatalogue == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.Item = new SelectList(db.ItemsMaster, "ID", "ItemSpecifications", itemsCatalogue.Item);
//            return PartialView(itemsCatalogue);
//        }

//        // POST: ItemsCatalogue/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,Item,UnitPrice,EffectivityDate,CreatedAt,UpdatedAt,DeletedAt")] ItemsCatalogue itemsCatalogue)
//        {
//            ItemsCatalogue _updateCatalogue = db.ItemsCatalogue.Find(itemsCatalogue.ID);
//            _updateCatalogue.UnitPrice = itemsCatalogue.UnitPrice;
//            _updateCatalogue.EffectivityDate = itemsCatalogue.EffectivityDate;

//            if (ModelState.IsValid)
//            {
//                db.SaveChanges();
//                return Json(new
//                {
//                    status = "success"
//                });
//            }
//            ViewBag.Item = new SelectList(db.ItemsMaster, "ID", "ItemSpecifications", itemsCatalogue.Item);
//            return View(itemsCatalogue);
//        }

//        // GET: ItemsCatalogue/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            ItemsCatalogue itemsCatalogue = db.ItemsCatalogue.Find(id);
//            if (itemsCatalogue == null)
//            {
//                return HttpNotFound();
//            }
//            return View(itemsCatalogue);
//        }

//        // POST: ItemsCatalogue/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            ItemsCatalogue itemsCatalogue = db.ItemsCatalogue.Find(id);
//            db.ItemsCatalogue.Remove(itemsCatalogue);
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
