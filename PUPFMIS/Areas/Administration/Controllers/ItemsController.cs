using FluentValidation.Results;
using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("administration")]
    [RoutePrefix("items/master")]
    [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.SystemAdmin)]
    public class ItemsController : Controller
    {
        ItemDataAccess itemDataAccess = new ItemDataAccess();

        [Route("")]
        [ActionName("list")]
        public ActionResult Index()
        {
            return View("index", itemDataAccess.GetItems().Where(d => d.PurgeFlag == false).ToList());
        }

        [Route("restore")]
        [ActionName("restore-list")]
        public ActionResult RestoreIndex()
        {
            return View("RestoreIndex", itemDataAccess.GetItems().Where(d => d.PurgeFlag == true).ToList());
        }

        [ActionName("details")]
        [Route("{ItemCode}/details")]
        public ActionResult Details(string ItemCode)
        {
            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = itemDataAccess.GetItemDetails(ItemCode);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View("details", item);
        }

        [ActionName("restore")]
        [Route("{ItemCode}/restore")]
        public ActionResult Restore(string ItemCode)
        {
            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemVM item = itemDataAccess.GetItemDetails(ItemCode);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View("restore", item);
        }

        [ActionName("delete")]
        [Route("{ItemCode}/delete")]
        public ActionResult Delete(string ItemCode)
        {
            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item = itemDataAccess.GetItemDetails(ItemCode);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View("Delete", item);
        }

        [Route("create")]
        [ActionName("create")]
        public ActionResult Create()
        {
            ViewBag.PackagingUOMReference = new SelectList(itemDataAccess.GetUOMs(), "ID", "UnitName");
            ViewBag.IndividualUOMReference = new SelectList(itemDataAccess.GetUOMs(), "ID", "UnitName");
            ViewBag.Article = new SelectList(itemDataAccess.GetItemArticles(), "Value", "Text");
            ViewBag.Category = new SelectList(itemDataAccess.GetCategories(), "ID", "ItemCategoryName");
            ViewBag.AllowedUsers = new SelectList(itemDataAccess.GetDepartments(), "DepartmentCode", "Department");
            return View();
        }

        [HttpPost]
        [Route("create")]
        [ActionName("create")]
        public ActionResult Create(ItemVM item, HttpPostedFileBase upload)
        {
            ModelState.Remove("ID");
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var _reader = new System.IO.BinaryReader(upload.InputStream);
                    item.ItemImage = _reader.ReadBytes(upload.ContentLength);
                }
                ItemValidator validator = new ItemValidator();
                ValidationResult validationResult = validator.Validate(item);
                if (!validationResult.IsValid)
                {
                    foreach (ValidationFailure error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }

                    return PartialView("_Form", item);
                }
                return Json(new { result = itemDataAccess.AddItemRecord(item, User.Identity.Name) });
            }
            ViewBag.PackagingUOMReference = new SelectList(itemDataAccess.GetUOMs(), "ID", "UnitName", item.PackagingUOMReference);
            ViewBag.IndividualUOMReference = new SelectList(itemDataAccess.GetUOMs(), "ID", "UnitName", item.IndividualUOMReference);
            ViewBag.Article = new SelectList(itemDataAccess.GetItemArticles(), "Value", "Text", item.Article);
            ViewBag.Category = new SelectList(itemDataAccess.GetCategories(), "ID", "ItemCategoryName", item.Category);
            ViewBag.AllowedUsers = new SelectList(itemDataAccess.GetDepartments(), "DepartmentCode", "Department", item.AllowedUsers);
            return PartialView("_Form", item);
        }

        [Route("{ItemCode}/update")]
        [ActionName("update")]
        public ActionResult Edit(string ItemCode)
        {
            if(ItemCode == null)
            {
                return HttpNotFound();
            }
            var item = itemDataAccess.GetItemDetails(ItemCode);
            ViewBag.PackagingUOMReference = new SelectList(itemDataAccess.GetUOMs(), "ID", "UnitName", item.PackagingUOMReference);
            ViewBag.IndividualUOMReference = new SelectList(itemDataAccess.GetUOMs(), "ID", "UnitName", item.IndividualUOMReference);
            ViewBag.Article = new SelectList(itemDataAccess.GetItemArticles(), "Value", "Text", item.Article);
            ViewBag.Category = new SelectList(itemDataAccess.GetCategories(), "ID", "ItemCategoryName", item.Category);
            ViewBag.AllowedUsers = new SelectList(itemDataAccess.GetDepartments(), "DepartmentCode", "Department");
            return View("Edit", item);
        }

        [ValidateAntiForgeryToken]
        [Route("{ItemCode}/delete")]
        [HttpPost, ActionName("delete")]
        public ActionResult DeleteConfirmed(string ItemCode)
        {
            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return Json(new { result = itemDataAccess.DeleteItemRecord(ItemCode, User.Identity.Name) });
        }

        //        [ActionName("update-item")]
        //        [Route("{ItemCode}/update")]
        //        public ActionResult Edit(string ItemCode)
        //        {
        //            if (ItemCode == null)
        //            {
        //                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //            }
        //            ItemVM item = itemDataAccess.GetItemDetails(ItemCode);
        //            if (item == null)
        //            {
        //                return HttpNotFound();
        //            }
        //            var allowedUsers = itemAllowedUsersDataAccess.GetAllowedUsers(item.ID);
        //            var Offices = hrisDataAccess.GetAllDepartments().Where(d => d.Lvl == 1).OrderBy(d => d.Department).ToList();
        //            //var ItemTypes = itemTypeDAL.GetItemTypes(false).OrderBy(d => d.ItemTypeName).ToList();
        //            //var InventoryTypes = inventoryTypeDataAccess.GetInventoryTypes();
        //            var CategoryReferences = itemCategoriesDataAccess.GetItemCategories(false);
        //            var UnitsOfMeasure = uomDataAccess.GetUOMs(false);

        //            var ResponsibilityCenters = new SelectList(Offices, "DepartmentCode", "Department").ToList();
        //            ResponsibilityCenters.Add(new SelectListItem() { Text = "None", Value = "None" });
        //            ResponsibilityCenters.Where(d => d.Value == (item.ResponsibilityCenter != "None" ? Offices.Where(o => o.Department == item.ResponsibilityCenter).FirstOrDefault().DepartmentCode : "None")).FirstOrDefault().Selected = true;

        //            var PurchaseRequestOffices = new SelectList(Offices, "DepartmentCode", "Department").ToList();
        //            PurchaseRequestOffices.Add(new SelectListItem() { Text = "Requesting Office", Value = "Requesting Office" });
        //            PurchaseRequestOffices.Where(d => d.Value == (item.PurchaseRequestOffice != "Requesting Office" ? Offices.Where(o => o.Department == item.PurchaseRequestOffice).FirstOrDefault().DepartmentCode : "Requesting Office")).FirstOrDefault().Selected = true;

        //            var PackagingUnits = new SelectList(UnitsOfMeasure, "ID", "UnitName").ToList();
        //            PackagingUnits.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });
        //            var IndividualUnits = new SelectList(UnitsOfMeasure, "ID", "UnitName").ToList();
        //            IndividualUnits.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });


        //            PackagingUnits.Where(d => d.Value == (item.PackagingUOMReference != "Not Applicable" ? UnitsOfMeasure.Where(o => o.UnitName == item.PackagingUOMReference).FirstOrDefault().ID.ToString() : "Not Applicable" )).FirstOrDefault().Selected = true;
        //            IndividualUnits.Where(d => d.Value == (item.IndividualUOMReference != "Not Applicable" ? UnitsOfMeasure.Where(o => o.UnitName == item.IndividualUOMReference).FirstOrDefault().ID.ToString() : "Not Applicable")).FirstOrDefault().Selected = true;


        //            ViewBag.PackagingUOMReference = PackagingUnits;
        //            ViewBag.IndividualUOMReference = IndividualUnits;
        //            ViewBag.ResponsibilityCenter = ResponsibilityCenters;
        //            ViewBag.PurchaseRequestOffice = PurchaseRequestOffices;
        //            //ViewBag.ItemType = new SelectList(ItemTypes, "ItemTypeCode", "ItemTypeName", ItemTypes.Where(d => d.ItemTypeName == item.ItemType).FirstOrDefault().ItemTypeCode);
        //            ViewBag.Category = new SelectList(CategoryReferences, "ID", "ItemCategoryName", CategoryReferences.Where(d => d.ItemCategoryName == item.Category).FirstOrDefault().ID);
        //            //ViewBag.InventoryType = new SelectList(InventoryTypes, "ID", "InventoryTypeName", InventoryTypes.Where(d => d.InventoryTypeName == item.InventoryType).FirstOrDefault().ID);
        //            var users = new SelectList(Offices, "DepartmentCode", "Department").ToList();


        //            foreach (var dept in allowedUsers)
        //            {
        //                users.Where(d => d.Value == dept).FirstOrDefault().Selected = true;
        //            }
        //            ViewBag.AllowedUsers = users;
        //            ViewBag.Action = "Edit";
        //            return View("edit", item);
        //        }

        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        [ActionName("update-item")]
        //        [Route("{ItemCode}/update")]
        //        public ActionResult Edit(ItemVM item, HttpPostedFileBase upload)
        //        {
        //            var Offices = hrisDataAccess.GetAllDepartments().Where(d => d.Lvl == 1).OrderBy(d => d.Department).ToList();
        //            //var ItemTypes = itemTypeDAL.GetItemTypes(false).OrderBy(d => d.ItemTypeName).ToList();
        //            //var InventoryTypes = inventoryTypeDataAccess.GetInventoryTypes();
        //            var CategoryReferences = itemCategoriesDataAccess.GetItemCategories(false);

        //            ViewBag.Category = new SelectList(CategoryReferences, "ID", "ItemCategoryName", item.Category);
        //            var ResponsibilityCenters = item.ResponsibilityCenter == null ? new SelectList(Offices, "DepartmentCode", "Department").ToList() : new SelectList(Offices, "DepartmentCode", "Department", item.ResponsibilityCenter).ToList();
        //            ResponsibilityCenters.Add(new SelectListItem() { Text = "None", Value = "None", Selected = item.ResponsibilityCenter == null ? true : false });
        //            ViewBag.ResponsibilityCenter = ResponsibilityCenters;

        //            var PurchaseRequestOffices = item.PurchaseRequestOffice == null ? new SelectList(Offices, "DepartmentCode", "Department").ToList() : new SelectList(Offices, "DepartmentCode", "Department", item.PurchaseRequestOffice).ToList();
        //            PurchaseRequestOffices.Add(new SelectListItem() { Text = "Requesting Office", Value = "Requesting Office", Selected = item.PurchaseRequestOffice == null ? true : false });
        //            ViewBag.PurchaseRequestOffice = PurchaseRequestOffices;
        //            ViewBag.PackagingUOMReference = new SelectList(uomDataAccess.GetUOMs(false), "ID", "UnitName", item.PackagingUOMReference);
        //            ViewBag.IndividualUOMReference = new SelectList(uomDataAccess.GetUOMs(false), "ID", "UnitName", item.IndividualUOMReference);
        //            //ViewBag.InventoryType = new SelectList(InventoryTypes, "ID", "InventoryTypeName", item.InventoryType);
        //            //ViewBag.ItemType = new SelectList(ItemTypes, "ItemTypeCode", "ItemTypeName", item.ItemType);

        //            if (ModelState.IsValid)
        //            {
        //                if (upload != null && upload.ContentLength > 0)
        //                {
        //                    var _reader = new System.IO.BinaryReader(upload.InputStream);
        //                    item.ItemImage = _reader.ReadBytes(upload.ContentLength);
        //                }
        //                if (item.IsTangible == "Yes")
        //                {
        //                    ItemValidator validator = new ItemValidator();
        //                    ValidationResult validationResult = validator.Validate(item);
        //                    if (!validationResult.IsValid)
        //                    {
        //                        foreach (ValidationFailure error in validationResult.Errors)
        //                        {
        //                            ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
        //                        }

        //                        return View("edit", item);
        //                    }
        //                }
        //                return Json(new { result = itemDataAccess.UpdateItemRecord(item, User.Identity.Name) });
        //            }
        //            return View("edit", item);
        //        }

        //        [Route("{ItemCode}/delete")]
        //        [ActionName("delete-item")]
        //        public ActionResult Delete(string ItemCode)
        //        {
        //            if (ItemCode == null)
        //            {
        //                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //            }
        //            ItemVM item = itemDataAccess.GetItemDetails(ItemCode);
        //            if (item == null)
        //            {
        //                return HttpNotFound();
        //            }
        //            return View("delete", item);
        //        }

        [ValidateAntiForgeryToken]
        [Route("{ItemCode}/restore")]
        [HttpPost, ActionName("restore-item")]
        public ActionResult RestoreConfirmed(string ItemCode)
        {
            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return Json(new { result = itemDataAccess.RestoreItemRecord(ItemCode, User.Identity.Name) });
        }

        [Route("{ArticleID}/get-article-details")]
        [ActionName("get-article-details")]
        public ActionResult GetArticleDetails(int? ArticleID)
        {
            if(ArticleID == null)
            {
                return HttpNotFound();
            }
            return Json(itemDataAccess.GetArticleDetails((int)ArticleID), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                itemDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}