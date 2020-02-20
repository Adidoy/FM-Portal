using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessLayer;

namespace PUPFMIS.Controllers
{
    [Authorize]
    [Route("master-tables/items/{action}")]
    public class ItemsController : Controller
    {
        ItemsBL itemsBL = new ItemsBL();
        UnitOfMeasureBL unitOfMeasureBL = new UnitOfMeasureBL();
        ItemCategoriesBL itemCategoriesBL = new ItemCategoriesBL();
        InventoryTypeBL inventoryTypesBL = new InventoryTypeBL();
        ChartOfAccountsBL coaBL = new ChartOfAccountsBL();

        public ActionResult Index()
        {
            return View(itemsBL.GetItems());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = itemsBL.GetItems(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        public ActionResult Create()
        {
            ViewBag.AccountClass = new SelectList(coaBL.GetChartOfAccounts(), "UACS", "AccountTitle");
            ViewBag.IndividualUOMReference = new SelectList(unitOfMeasureBL.GetUOMs(), "ID", "UnitName");
            ViewBag.InventoryTypeReference = new SelectList(inventoryTypesBL.GetInventoryTypes(), "ID", "InventoryTypeName");
            ViewBag.ItemCategoryReference = new SelectList(itemCategoriesBL.GetCategories(), "ID", "ItemCategoryName");
            ViewBag.PackagingUOMReference = new SelectList(unitOfMeasureBL.GetUOMs(), "ID", "UnitName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item, HttpPostedFileBase upload)
        {

            ModelState.Remove("ItemCode");
            ModelState.Remove("ItemImage");

            //ValidateModel(item);
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var _reader = new System.IO.BinaryReader(upload.InputStream);
                    item.ItemImage = _reader.ReadBytes(upload.ContentLength);
                }

                if (itemsBL.AddItemRecord(item))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.IndividualUOMReference = new SelectList(unitOfMeasureBL.GetUOMs(), "ID", "UnitName", item.IndividualUOMReference);
                    ViewBag.InventoryTypeReference = new SelectList(inventoryTypesBL.GetInventoryTypes(), "ID", "InventoryTypeName", item.InventoryTypeReference);
                    ViewBag.ItemCategoryReference = new SelectList(itemCategoriesBL.GetCategories(), "ID", "ItemCategoryName", item.ItemCategoryReference);
                    ViewBag.PackagingUOMReference = new SelectList(unitOfMeasureBL.GetUOMs(), "ID", "UnitName", item.PackagingUOMReference);
                    return View(item);
                }
            }

            ViewBag.IndividualUOMReference = new SelectList(unitOfMeasureBL.GetUOMs(), "ID", "UnitName", item.IndividualUOMReference);
            ViewBag.InventoryTypeReference = new SelectList(inventoryTypesBL.GetInventoryTypes(), "ID", "InventoryTypeName", item.InventoryTypeReference);
            ViewBag.ItemCategoryReference = new SelectList(itemCategoriesBL.GetCategories(), "ID", "ItemCategoryName", item.ItemCategoryReference);
            ViewBag.PackagingUOMReference = new SelectList(unitOfMeasureBL.GetUOMs(), "ID", "UnitName", item.PackagingUOMReference);
            return View(item);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = itemsBL.GetItems(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.IndividualUOMReference = new SelectList(unitOfMeasureBL.GetUOMs(), "ID", "UnitName", item.IndividualUOMReference);
            ViewBag.InventoryTypeReference = new SelectList(inventoryTypesBL.GetInventoryTypes(), "ID", "InventoryTypeName", item.InventoryTypeReference);
            ViewBag.ItemCategoryReference = new SelectList(itemCategoriesBL.GetCategories(), "ID", "ItemCategoryName", item.ItemCategoryReference);
            ViewBag.PackagingUOMReference = new SelectList(unitOfMeasureBL.GetUOMs(), "ID", "UnitName", item.PackagingUOMReference);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.IndividualUOMReference = new SelectList(db.UOM, "ID", "UnitName", item.IndividualUOMReference);
            //ViewBag.InventoryTypeReference = new SelectList(db.InventoryTypes, "ID", "InventoryTypeName", item.InventoryTypeReference);
            //ViewBag.ItemCategoryReference = new SelectList(db.ItemCategories, "ID", "ItemCategoryName", item.ItemCategoryReference);
            //ViewBag.PackagingUOMReference = new SelectList(db.UOM, "ID", "UnitName", item.PackagingUOMReference);
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Item item = itemsBL.GetItems(id);
            //if (item == null)
            //{
            //    return HttpNotFound();
            //}
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Item item = db.Items.Find(id);
            //db.Items.Remove(item);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                itemsBL.Dispose();
                unitOfMeasureBL.Dispose();
                itemCategoriesBL.Dispose();
                inventoryTypesBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
