using FluentValidation.Results;
using PUPFMIS.BusinessLayer;
using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PUPFMIS.Controllers
{
    [Authorize]
    [Route("ops/procurement/planning/catalogue/{action}")]
    public class CatalogueController : Controller
    {

        private CatalogueBL catalogueBL = new CatalogueBL();

        [ActionName("common-use-supplies")]
        public ActionResult CatalogueCSE()
        {
            ViewBag.PageTitle = "Common Use Office Supplies";
            ViewBag.CategoryName = "All Categories";
            ViewBag.Categories = catalogueBL.GetCategories();
            return View("Catalogue", catalogueBL.GetCSEItems());
        }

        [ActionName("property-and-equipment")]
        public ActionResult CatalogueProperty()
        {
            ViewBag.PageTitle = "Property and Equipment";
            ViewBag.CategoryName = "All Categories";
            ViewBag.Categories = catalogueBL.GetCategories();
            return View("Catalogue", catalogueBL.GetPropertyItems());
        }

        [ActionName("semi-expandable-property")]
        public ActionResult CatalogueSemiExpandableProperty()
        {
            ViewBag.PageTitle = "Semi-Expendable Property and Equipment";
            ViewBag.CategoryName = "All Categories";
            ViewBag.Categories = catalogueBL.GetCategories();
            return View("Catalogue", catalogueBL.GetSemiExpandablePropertyItems());
        }

        [ActionName("view-by-category")]
        [Route("ops/procurement/planning/catalogue/{CategoryName}/view-by-category")]
        public ActionResult ViewByCategory(string CategoryName)
        {
            if(CategoryName == "All Categories")
            {
                return RedirectToAction("common-use-supplies");
            }
            if (CategoryName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Catalogue> catalogueByCategory = catalogueBL.GetCSEItemsByCategory(CategoryName);
            if (catalogueByCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryName = "All Categories";
            ViewBag.Categories = catalogueBL.GetCategories();
            return View("Catalogue", catalogueByCategory);
        }

        [ActionName("add-to-basket")]
        [Route("ops/procurement/planning/catalogue/{ItemCode}/add-to-basket")]
        public ActionResult AddToBasket(string ItemCode)
        {
            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket itemBasket;
            if((Session["BasketItems"] != null) && (((List<Basket>)Session["BasketItems"]).Where(d => d.ItemCode == ItemCode).Count() == 1))
            {
                itemBasket = ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemCode == ItemCode).FirstOrDefault();
                ViewBag.PageTitle = "Common Use Office Supplies";
                return View("AddToBasket", itemBasket);
            }
            itemBasket = catalogueBL.GetCSECItems(ItemCode, User.Identity.Name);
            if (itemBasket == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageTitle = "Common Use Office Supplies";
            return View("AddToBasket", itemBasket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("add-to-basket")]
        [Route("ops/procurement/planning/catalogue/{ItemCode}/add-to-basket")]
        public ActionResult AddToBasket(Basket itemBasket)
        {
            ValidateAddItem(itemBasket);
            if (ModelState.IsValid)
            {
                if (Session["BasketItems"] == null)
                {
                    Session["BasketItems"] = new List<Basket>();
                    //((List<Basket>)Session["BasketItems"]).Add(itemBasket);
                }

                itemBasket.Qtr1Qty = (String.IsNullOrEmpty(itemBasket.Qtr1Qty.ToString())) ? 0 : (int)itemBasket.Qtr1Qty;
                itemBasket.Qtr2Qty = (String.IsNullOrEmpty(itemBasket.Qtr2Qty.ToString())) ? 0 : (int)itemBasket.Qtr2Qty;
                itemBasket.Qtr3Qty = (String.IsNullOrEmpty(itemBasket.Qtr3Qty.ToString())) ? 0 : (int)itemBasket.Qtr3Qty;
                itemBasket.Qtr4Qty = (String.IsNullOrEmpty(itemBasket.Qtr4Qty.ToString())) ? 0 : (int)itemBasket.Qtr4Qty;
                itemBasket.TotalQty = ((int)itemBasket.Qtr1Qty + (int)itemBasket.Qtr2Qty + (int)itemBasket.Qtr3Qty + (int)itemBasket.Qtr4Qty);

                if (catalogueBL.ValidateConsumptionVSQuantity(itemBasket))
                {
                    if (String.IsNullOrEmpty(itemBasket.Remarks))
                    {
                        ModelState.AddModelError(string.Empty, "Total Quantity is greater than the Previous consumption. Please reduce the quantity requirement or provide a justification below.");
                        ViewBag.NeedsJustification = true;
                        return View("AddToBasket", itemBasket);
                    }
                }

                if (((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).Count() == 1)
                {
                    ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).FirstOrDefault().Qtr1Qty = itemBasket.Qtr1Qty;
                    ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).FirstOrDefault().Qtr2Qty = itemBasket.Qtr2Qty;
                    ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).FirstOrDefault().Qtr3Qty = itemBasket.Qtr3Qty;
                    ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).FirstOrDefault().Qtr4Qty = itemBasket.Qtr4Qty;
                    ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).FirstOrDefault().TotalQty = itemBasket.TotalQty;
                    ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).FirstOrDefault().Remarks = (String.IsNullOrEmpty(itemBasket.Remarks)) ? "Acceptable" : itemBasket.Remarks;
                }
                else
                {
                    itemBasket.Remarks = (String.IsNullOrEmpty(itemBasket.Remarks)) ? "Acceptable" : itemBasket.Remarks;
                    ((List<Basket>)Session["BasketItems"]).Add(itemBasket);
                }
                return RedirectToAction("common-use-supplies");
            }
            return View("AddToBasket", itemBasket);
        }

        [ActionName("update-basket-item")]
        [Route("ops/procurement/planning/catalogue/{ItemCode}/update-basket-item")]
        public ActionResult UpdateBasket(string ItemCode)
        {
            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Basket updateItem = ((List<Basket>)Session["BasketItems"]).Find(d => d.ItemCode == ItemCode);
            if (updateItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.PageTitle = "Common Use Office Supplies";
            return View("UpdateBasket", updateItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("update-basket")]
        [Route("ops/procurement/planning/catalogue/{ItemCode}/update-basket")]
        public ActionResult Update(Basket itemBasket)
        {
            ValidateAddItem(itemBasket);
            if (ModelState.IsValid)
            {
                itemBasket.Qtr1Qty = (String.IsNullOrEmpty(itemBasket.Qtr1Qty.ToString())) ? 0 : (int)itemBasket.Qtr1Qty;
                itemBasket.Qtr2Qty = (String.IsNullOrEmpty(itemBasket.Qtr2Qty.ToString())) ? 0 : (int)itemBasket.Qtr2Qty;
                itemBasket.Qtr3Qty = (String.IsNullOrEmpty(itemBasket.Qtr3Qty.ToString())) ? 0 : (int)itemBasket.Qtr3Qty;
                itemBasket.Qtr4Qty = (String.IsNullOrEmpty(itemBasket.Qtr4Qty.ToString())) ? 0 : (int)itemBasket.Qtr4Qty;
                itemBasket.TotalQty = ((int)itemBasket.Qtr1Qty + (int)itemBasket.Qtr2Qty + (int)itemBasket.Qtr3Qty + (int)itemBasket.Qtr4Qty);

                if (((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).Count() == 1)
                {
                    ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).First().Qtr1Qty = itemBasket.Qtr1Qty;
                    ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).First().Qtr2Qty = itemBasket.Qtr2Qty;
                    ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).First().Qtr3Qty = itemBasket.Qtr3Qty;
                    ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).First().Qtr4Qty = itemBasket.Qtr4Qty;
                    ((List<Basket>)Session["BasketItems"]).Where(d => d.ItemID == itemBasket.ItemID).First().TotalQty = itemBasket.TotalQty;
                }
                else
                {
                    ((List<Basket>)Session["BasketItems"]).Add(itemBasket);
                }
                return RedirectToAction("view-basket");
            }
            return View("update-basket", itemBasket);
        }

        [ActionName("remove-from-basket")]
        [Route("ops/procurement/planning/catalogue/{ItemCode}/remove-from-basket")]
        public ActionResult RemoveFromBasket(string ItemCode)
        {
            ((List<Basket>)Session["BasketItems"]).Remove(((List<Basket>)Session["BasketItems"]).Find(d => d.ItemCode == ItemCode));
            return RedirectToAction("view-basket");
        }

        [ActionName("view-basket")]
        public ActionResult ViewBasket()
        {
            ViewBag.PageTitle = "Common Use Office Supplies";
            var basketItems = ((List<Basket>)Session["BasketItems"]);
            return View("ViewBasket", basketItems);
        }

        private void ValidateAddItem(Basket BasketItem)
        {
            BasketValidator basketItemValidator = new BasketValidator();
            ValidationResult validationResult = basketItemValidator.Validate(BasketItem);
            if (!validationResult.IsValid)
            {
                foreach (ValidationFailure result in validationResult.Errors)
                {
                    ModelState.AddModelError(result.PropertyName, result.ErrorMessage);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                catalogueBL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
