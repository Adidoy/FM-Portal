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
    [RoutePrefix("items/types")]
    [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin)]
    public class ItemTypesController : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private ItemTypesDataAccess itemTypesDL = new ItemTypesDataAccess();

        [Route("")]
        [ActionName("list")]
        public ActionResult Index()
        {
            return View("Index", itemTypesDL.GetItemTypes().Where(d => d.PurgeFlag == false).ToList());
        }

        [Route("restore")]
        [ActionName("restore-list")]
        public ActionResult RestoreIndex()
        {
            return View("RestoreIndex", itemTypesDL.GetItemTypes().Where(d => d.PurgeFlag == true).ToList());
        }

        [ActionName("details")]
        [Route("{ItemTypeCode}/details")]
        public ActionResult Details(string ItemTypeCode)
        {
            if (ItemTypeCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ItemType = itemTypesDL.GetItemTypes(ItemTypeCode);
            if (ItemType == null)
            {
                return HttpNotFound();
            }
            return View("details", ItemType);
        }

        [ActionName("delete")]
        [Route("{ItemTypeCode}/delete")]
        public ActionResult Delete(string ItemTypeCode)
        {
            if (ItemTypeCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypes itemType = itemTypesDL.GetItemTypes(ItemTypeCode);
            if (itemType == null)
            {
                return HttpNotFound();
            }
            return View("delete", itemType);
        }

        [Route("create")]
        [ActionName("create")]
        public ActionResult Create()
        {
            ViewBag.ItemClassificationReference = new SelectList(itemTypesDL.GetItemClassifications(), "ID", "Classification");
            ViewBag.ResponsibilityCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department");
            ViewBag.PurchaseRequestCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department");
            return View();
        }

        [HttpPost]
        [Route("create")]
        [ActionName("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemTypes ItemTypeModel)
        {
            if (ItemTypeModel == null)
            {
                return Json(new { result = false });
            }

            ModelState.Remove("ID");
            ModelState.Remove("PurgeFlag");
            ModelState.Remove("CreatedAt");

            if (ModelState.IsValid)
            {
                var Key = string.Empty;
                var Message = new List<string>();
                itemTypesDL.Validate(ItemTypeModel, out Key, out Message);
                if(Key != null)
                {
                    foreach(var errorMessage in Message)
                    {
                        ModelState.AddModelError(Key, errorMessage);
                    }
                    ViewBag.ItemClassificationReference = new SelectList(itemTypesDL.GetItemClassifications(), "ID", "Classification", ItemTypeModel.ItemClassificationReference);
                    ViewBag.ResponsibilityCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department", ItemTypeModel.ResponsibilityCenter);
                    ViewBag.PurchaseRequestCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department", ItemTypeModel.PurchaseRequestCenter);
                    return PartialView("_Form", ItemTypeModel);
                }
                return Json(new { result = itemTypesDL.AddItemTypeRecord(ItemTypeModel, User.Identity.Name) });
            }

            ViewBag.ItemClassificationReference = new SelectList(itemTypesDL.GetItemClassifications(), "ID", "Classification", ItemTypeModel.ItemClassificationReference);
            ViewBag.ResponsibilityCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department", ItemTypeModel.ResponsibilityCenter);
            ViewBag.PurchaseRequestCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department", ItemTypeModel.PurchaseRequestCenter);
            return PartialView("_Form", ItemTypeModel);
        }

        [ActionName("edit")]
        [Route("{ItemTypeCode}/edit")]
        public ActionResult Edit(string ItemTypeCode)
        {
            if (ItemTypeCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypes itemType = itemTypesDL.GetItemTypes(ItemTypeCode);
            if (itemType == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemClassificationReference = new SelectList(itemTypesDL.GetItemClassifications(), "ID", "Classification", itemType.ItemClassificationReference);
            ViewBag.ResponsibilityCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department", itemType.ResponsibilityCenter);
            ViewBag.PurchaseRequestCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department", itemType.PurchaseRequestCenter);
            return View(itemType);
        }

        [HttpPost]
        [ActionName("edit")]
        [ValidateAntiForgeryToken]
        [Route("{ItemTypeCode}/edit")]
        public ActionResult Edit(ItemTypes ItemTypeModel)
        {
            if (ModelState.IsValid)
            {
                string Key = null;
                string Message = null;
                var result = itemTypesDL.UpdateItemTypeRecord(ItemTypeModel, User.Identity.Name, out Key, out Message);
                if(result == false && (Key != null && Message != null))
                {
                    ModelState.AddModelError(Key, Message);
                    ViewBag.ItemClassificationReference = new SelectList(itemTypesDL.GetItemClassifications(), "ID", "Classification", ItemTypeModel.ItemClassificationReference);
                    ViewBag.ResponsibilityCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department", ItemTypeModel.ResponsibilityCenter);
                    ViewBag.PurchaseRequestCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department", ItemTypeModel.PurchaseRequestCenter);
                    return PartialView("_Form", ItemTypeModel);
                }
                else
                {
                    return Json(new { result = result });
                }
            }

            ViewBag.ItemClassificationReference = new SelectList(itemTypesDL.GetItemClassifications(), "ID", "Classification", ItemTypeModel.ItemClassificationReference);
            ViewBag.ResponsibilityCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department", ItemTypeModel.ResponsibilityCenter);
            ViewBag.PurchaseRequestCenter = new SelectList(itemTypesDL.GetDepartments(), "DepartmentCode", "Department", ItemTypeModel.PurchaseRequestCenter);
            return PartialView("_Form", ItemTypeModel);
        }

        [ValidateAntiForgeryToken]
        [Route("{ItemTypeCode}/delete")]
        [HttpPost, ActionName("delete")]
        public ActionResult DeleteConfirmed(string ItemTypeCode)
        {
            if (ItemTypeCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var itemType = itemTypesDL.GetItemTypes(ItemTypeCode);
            return Json(new { result = itemTypesDL.DeleteItemTypeRecord(itemType, User.Identity.Name) });
        }

        [ActionName("restore")]
        [Route("{ItemTypeCode}/restore")]
        public ActionResult Restore(string ItemTypeCode)
        {
            if (ItemTypeCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTypes itemType = itemTypesDL.GetItemTypes(ItemTypeCode);
            if (itemType == null)
            {
                return HttpNotFound();
            }
            return View("restore", itemType);
        }

        [ValidateAntiForgeryToken]
        [Route("{ItemTypeCode}/restore")]
        [HttpPost, ActionName("restore-item")]
        public ActionResult RestoreConfirmed(string ItemTypeCode)
        {
            if (ItemTypeCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var itemType = itemTypesDL.GetItemTypes(ItemTypeCode);
            return Json(new { result =  itemTypesDL.RestoreItemTypeRecord(itemType, User.Identity.Name) });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hrisDataAccess.Dispose();
                itemTypesDL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}