//using PUPFMIS.Models;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web.Mvc;

//namespace PUPFMIS.Controllers
//{
//    [Route("ops/property/inventory-adjustment/{type}/{action}")]
//    public class InventoryAdjustmentsController : Controller
//    {
//        private FMISDbContext db = new FMISDbContext();

//        // GET: InventoryAdjustments
//        public ActionResult Index(string type)
//        {
//            ViewBag.Type = type;
//            return View(db.InventoryAdjustment.ToList());
//        }

//        // GET: InventoryAdjustments/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            InventoryAdjustment inventoryAdjustment = db.InventoryAdjustment.Find(id);
//            if (inventoryAdjustment == null)
//            {
//                return HttpNotFound();
//            }
//            return View(inventoryAdjustment);
//        }

//        // GET: InventoryAdjustments/Create
//        public ActionResult Create(string type)
//        {
//            ViewBag.Type = type;
//            return View();
//        }

//        // POST: InventoryAdjustments/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "ID,Date,OverallRemarks,ProcessedBy,VerifiedBy")] InventoryAdjustment inventoryAdjustment)
//        {
//            if (ModelState.IsValid)
//            {
//                db.InventoryAdjustment.Add(inventoryAdjustment);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(inventoryAdjustment);
//        }

//        // GET: InventoryAdjustments/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            InventoryAdjustment inventoryAdjustment = db.InventoryAdjustment.Find(id);
//            if (inventoryAdjustment == null)
//            {
//                return HttpNotFound();
//            }
//            return View(inventoryAdjustment);
//        }

//        // POST: InventoryAdjustments/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "ID,Date,OverallRemarks,ProcessedBy,VerifiedBy")] InventoryAdjustment inventoryAdjustment)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(inventoryAdjustment).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(inventoryAdjustment);
//        }

//        // GET: InventoryAdjustments/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            InventoryAdjustment inventoryAdjustment = db.InventoryAdjustment.Find(id);
//            if (inventoryAdjustment == null)
//            {
//                return HttpNotFound();
//            }
//            return View(inventoryAdjustment);
//        }

//        // POST: InventoryAdjustments/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            InventoryAdjustment inventoryAdjustment = db.InventoryAdjustment.Find(id);
//            db.InventoryAdjustment.Remove(inventoryAdjustment);
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
