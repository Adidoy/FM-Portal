using FluentValidation.Results;
using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("administration")]
    [RoutePrefix("item-types")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin)]
    public class ItemTypesController : Controller
    {
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private ABISDataAccess abisDataAccess = new ABISDataAccess();
        private ItemTypeDataAccess itemTypesDAL = new ItemTypeDataAccess();
        private InventoryTypeDataAccess inventoryTypeDataAccess = new InventoryTypeDataAccess();
        private ItemCategoriesDataAccess itemCategoriesDataAccess = new ItemCategoriesDataAccess();

        [Route("")]
        [Route("list")]
        [ActionName("list")]
        public ActionResult Index()
        {
            var itemTypes = itemTypesDAL.GetItemTypes(false);
            return View("Index", itemTypes.ToList());
        }

        [Route("restore-list")]
        [ActionName("restore-list")]
        public ActionResult RestoreIndex()
        {
            var itemTypes = itemTypesDAL.GetItemTypes(true);
            return View("RestoreIndex", itemTypes.ToList());
        }

        [ActionName("details")]
        [Route("{ItemTypeCode}/details")]
        public ActionResult Details(string ItemTypeCode)
        {
            if (ItemTypeCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypeVM itemTypeVM = itemTypesDAL.GetItemTypeDetails(ItemTypeCode, false);
            if (itemTypeVM == null)
            {
                return HttpNotFound();
            }
            return View("Details", itemTypeVM);
        }

        [Route("create")]
        [ActionName("create")]
        public ActionResult Create()
        {
            var itemType = new ItemTypeVM();
            var InventoryTypes = inventoryTypeDataAccess.GetInventoryTypes().OrderBy(d => d.InventoryTypeName).ToList();
            ViewBag.InventoryTypeReference = new SelectList(InventoryTypes, "ID", "InventoryTypeName");
            ViewBag.AccountClass = new SelectList(abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.GenAcctName == "Maintenance and Other Operating Expenses" || d.GenAcctName == "Inventories" || d.GenAcctName == "Property, Plant and Equipment" || d.GenAcctName == "Intangible Assets").ToList(), "UACS_Code", "AcctName");
            return View(itemType);
        }

        [HttpPost]
        [Route("create")]
        [ActionName("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemTypeVM ItemType)
        {
            ModelState.Remove("ItemTypeCode");
            if (ModelState.IsValid)
            {
                ItemTypeValidator validator = new ItemTypeValidator();
                ValidationResult result = validator.Validate(ItemType);
                if (!result.IsValid)
                {
                    foreach (ValidationFailure error in result.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    return View(ItemType);
                }
                return Json(new { result = itemTypesDAL.AddItemTypeRecord(ItemType, User.Identity.Name) });
            }

            var InventoryTypes = inventoryTypeDataAccess.GetInventoryTypes().OrderBy(d => d.InventoryTypeName).ToList();
            ViewBag.InventoryTypeReference = new SelectList(InventoryTypes, "ID", "InventoryTypeName", ItemType.InventoryTypeReference);
            ViewBag.AccountClass = new SelectList(abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.GenAcctName == "Maintenance and Other Operating Expenses").ToList(), "UACS_Code", "AcctName");
            return View(ItemType);
        }

        [ActionName("edit")]
        [Route("{ItemTypeCode}/edit")]
        public ActionResult Edit(string ItemTypeCode)
        {
            if (ItemTypeCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypeVM itemType = itemTypesDAL.GetItemType(ItemTypeCode, false);
            if (itemType == null)
            {
                return HttpNotFound();
            }
            var InventoryTypes = inventoryTypeDataAccess.GetInventoryTypes().OrderBy(d => d.InventoryTypeName).ToList();
            ViewBag.InventoryTypeReference = new SelectList(InventoryTypes, "ID", "InventoryTypeName", itemType.InventoryTypeReference);
            ViewBag.AccountClass = new SelectList(abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.GenAcctName == "Maintenance and Other Operating Expenses").ToList(), "UACS_Code", "AcctName", itemType.UACSObjectClass);
            return View(itemType);
        }

        [HttpPost]
        [ActionName("edit")]
        [ValidateAntiForgeryToken]
        [Route("{ItemTypeCode}/edit")]
        public ActionResult Edit(ItemType ItemType)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = itemTypesDAL.UpdateItemTypeRecord(ItemType, User.Identity.Name) });
            }
            var InventoryTypes = inventoryTypeDataAccess.GetInventoryTypes().OrderBy(d => d.InventoryTypeName).ToList();
            ViewBag.InventoryTypeReference = new SelectList(InventoryTypes, "ID", "InventoryTypeName", ItemType.InventoryTypeReference);
            ViewBag.AccountClass = new SelectList(abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.GenAcctName == "Maintenance and Other Operating Expenses").ToList(), "UACS_Code", "AcctName", ItemType.UACSObjectClass);
            return View(ItemType);
        }

        [ActionName("delete")]
        [Route("{ItemTypeCode}/delete")]
        public ActionResult Delete(string ItemTypeCode)
        {
            if (ItemTypeCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypeVM itemTypeVM = itemTypesDAL.GetItemTypeDetails(ItemTypeCode, false);
            if (itemTypeVM == null)
            {
                return HttpNotFound();
            }
            return View(itemTypeVM);
        }

        [HttpPost]
        [ActionName("delete")]
        [ValidateAntiForgeryToken]
        [Route("{ItemTypeCode}/delete")]
        public ActionResult DeleteConfirmed(string ItemTypeCode)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = itemTypesDAL.PurgeItemTypeRecord(ItemTypeCode, User.Identity.Name) });
            }
            return RedirectToAction("Index");
        }

        [ActionName("restore")]
        [Route("{ItemTypeCode}/restore")]
        public ActionResult Restore(string ItemTypeCode)
        {
            if (ItemTypeCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypeVM itemTypeVM = itemTypesDAL.GetItemTypeDetails(ItemTypeCode, true);
            if (itemTypeVM == null)
            {
                return HttpNotFound();
            }
            return View(itemTypeVM);
        }

        [HttpPost]
        [ActionName("restore")]
        [ValidateAntiForgeryToken]
        [Route("{ItemTypeCode}/restore")]
        public ActionResult RestoreConfirmed(string ItemTypeCode)
        {
            if (ModelState.IsValid)
            {
                return Json(new { result = itemTypesDAL.RestoreItemTypeRecord(ItemTypeCode, User.Identity.Name) });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                itemTypesDAL.Dispose();
                abisDataAccess.Dispose();
                hrisDataAccess.Dispose();
                inventoryTypeDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
