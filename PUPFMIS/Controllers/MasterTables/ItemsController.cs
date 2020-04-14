using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Controllers
{
    [RoutePrefix("master-tables/items")]
    [Authorize(Roles = SystemRoles.SuperUser)]
    public class ItemsController : Controller
    {
        ItemBusinessLogic itemsBL = new ItemBusinessLogic();
        ItemDataAccess itemDA = new ItemDataAccess();

        [Route("")]
        [Route("list")]
        [ActionName("item-list")]
        public ActionResult Index()
        {
            return View("index", itemsBL.GetItems(false));
        }

        [ActionName("item-details")]
        [Route("details/{ItemCode}")]
        public ActionResult Details(string ItemCode)
        {
            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = itemsBL.GetItemsByCode(ItemCode, false);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View("details",item);
        }

        [Route("create")]
        [ActionName("create-item")]
        public ActionResult Create()
        {
            ViewBag.AccountClass = new SelectList(itemDA.GetChartOfAccounts(), "UACS", "AccountTitle");
            ViewBag.IndividualUOMReference = new SelectList(itemDA.GetUnitsOfMeasure(), "ID", "UnitName");
            ViewBag.InventoryTypeReference = new SelectList(itemDA.GetInventoryTypes(), "ID", "InventoryTypeName");
            ViewBag.ItemCategoryReference = new SelectList(itemDA.GetCategories(), "ID", "ItemCategoryName");
            ViewBag.PackagingUOMReference = new SelectList(itemDA.GetUnitsOfMeasure(), "ID", "UnitName");
            return View("create");
        }

        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        [ActionName("create-item")]
        public ActionResult Create(Item item, HttpPostedFileBase upload)
        {

            ModelState.Remove("ItemCode");
            ModelState.Remove("ItemImage");
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var _reader = new System.IO.BinaryReader(upload.InputStream);
                    item.ItemImage = _reader.ReadBytes(upload.ContentLength);
                }

                if (itemsBL.AddItemRecord(item))
                {
                    return RedirectToAction("item-list", "Items", new { Area = "" });
                }
                else
                {
                    ViewBag.IndividualUOMReference = new SelectList(itemDA.GetUnitsOfMeasure(), "ID", "UnitName", item.IndividualUOMReference);
                    ViewBag.InventoryTypeReference = new SelectList(itemDA.GetInventoryTypes(), "ID", "InventoryTypeName", item.InventoryTypeReference);
                    ViewBag.ItemCategoryReference = new SelectList(itemDA.GetCategories(), "ID", "ItemCategoryName", item.ItemCategoryReference);
                    ViewBag.PackagingUOMReference = new SelectList(itemDA.GetUnitsOfMeasure(), "ID", "UnitName", item.PackagingUOMReference);
                    return View("create", item);
                }
            }
            ViewBag.IndividualUOMReference = new SelectList(itemDA.GetUnitsOfMeasure(), "ID", "UnitName", item.IndividualUOMReference);
            ViewBag.InventoryTypeReference = new SelectList(itemDA.GetInventoryTypes(), "ID", "InventoryTypeName", item.InventoryTypeReference);
            ViewBag.ItemCategoryReference = new SelectList(itemDA.GetCategories(), "ID", "ItemCategoryName", item.ItemCategoryReference);
            ViewBag.PackagingUOMReference = new SelectList(itemDA.GetUnitsOfMeasure(), "ID", "UnitName", item.PackagingUOMReference);
            return View("create", item);
        }

        [ActionName("update-item")]
        [Route("update/{ItemCode}")]
        public ActionResult Edit(string ItemCode)
        {
            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = itemsBL.GetItemsByCode(ItemCode, false);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.IndividualUOMReference = new SelectList(itemDA.GetUnitsOfMeasure(), "ID", "UnitName", item.IndividualUOMReference);
            ViewBag.InventoryTypeReference = new SelectList(itemDA.GetInventoryTypes(), "ID", "InventoryTypeName", item.InventoryTypeReference);
            ViewBag.ItemCategoryReference = new SelectList(itemDA.GetCategories(), "ID", "ItemCategoryName", item.ItemCategoryReference);
            ViewBag.PackagingUOMReference = new SelectList(itemDA.GetUnitsOfMeasure(), "ID", "UnitName", item.PackagingUOMReference);
            return View("edit", item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("update-item")]
        [Route("update/{ItemCode}")]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            ViewBag.IndividualUOMReference = new SelectList(itemDA.GetUnitsOfMeasure(), "ID", "UnitName", item.IndividualUOMReference);
            ViewBag.InventoryTypeReference = new SelectList(itemDA.GetInventoryTypes(), "ID", "InventoryTypeName", item.InventoryTypeReference);
            ViewBag.ItemCategoryReference = new SelectList(itemDA.GetCategories(), "ID", "ItemCategoryName", item.ItemCategoryReference);
            ViewBag.PackagingUOMReference = new SelectList(itemDA.GetUnitsOfMeasure(), "ID", "UnitName", item.PackagingUOMReference);
            return View("edit", item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(string ItemCode)
        {
            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = itemsBL.GetItemsByCode(ItemCode, false);
            if (item == null)
            {
                return HttpNotFound();
            }
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
                itemDA.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
