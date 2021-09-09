using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("end-users")]

    [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser + ", " + SystemRoles.ResponsibilityCenterPlanner + ", " + SystemRoles.SuppliesChief + ", " + SystemRoles.ResponsibilityCenterPlanner)]
    public class CatalogueController : Controller
    {
        private CatalogueDAL catalogueDAL = new CatalogueDAL();
        private SuppliersBL suppliersDataAccess = new SuppliersBL();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();

        [ActionName("view-catalogue")]
        [Route("{ProjectCode}/catalogue")]
        [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
        public ActionResult ViewCatalogue(string ProjectCode)
        {
            if (Session["Basket"] == null)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket
                {
                    DeliveryMonth = project.DeliveryMonth,
                    ProjectCode = project.ProjectCode,
                    BasketItems = new List<BasketItems>()
                };
                Session["Basket"] = basket;
            }

            if (((Basket)Session["Basket"]).ProjectCode != ProjectCode)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket
                {
                    DeliveryMonth = project.DeliveryMonth,
                    ProjectCode = project.ProjectCode,
                    BasketItems = new List<BasketItems>()
                };
                Session["Basket"] = basket;
            }

            ViewBag.ProjectCode = ProjectCode;
            ViewBag.Categories = new SelectList(catalogueDAL.GetItemCategories(), "ID", "ItemCategoryName");
            ViewBag.ItemTypes = new SelectList(catalogueDAL.GetItemTypes(), "ID", "ItemType");
            return View("ViewCatalogue", catalogueDAL.GetCatalogue(User.Identity.Name, ProjectCode));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{ProjectCode}/catalogue")]
        [ActionName("view-catalogue-search")]
        [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
        public ActionResult ViewCatalogueFromSearch(string ProjectCode, string SearchByItemName, int? Categories, int? ItemTypes)
        {
            if (Session["Basket"] == null)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket();
                basket.DeliveryMonth = project.DeliveryMonth;
                basket.ProjectCode = project.ProjectCode;
                basket.BasketItems = new List<BasketItems>();
                Session["Basket"] = basket;
            }

            if (((Basket)Session["Basket"]).ProjectCode != ProjectCode)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket();
                basket.DeliveryMonth = project.DeliveryMonth;
                basket.ProjectCode = project.ProjectCode;
                basket.BasketItems = new List<BasketItems>();
                Session["Basket"] = basket;
            }

            ModelState.Clear();
            ViewBag.ProjectCode = ProjectCode;
            ViewBag.Categories = new SelectList(catalogueDAL.GetItemCategories(), "ID", "ItemCategoryName");
            ViewBag.ItemTypes = new SelectList(catalogueDAL.GetItemTypes(), "ID", "ItemType");
            return View("ViewCatalogue", catalogueDAL.GetCatalogueFromSearch(User.Identity.Name, ProjectCode, Categories, ItemTypes, SearchByItemName));
        }

        [ActionName("add-to-basket")]
        [Route("{ProjectCode}/catalogue/{ItemCode}")]
        [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
        public ActionResult AddToBasket(string ProjectCode, string ItemCode)
        {
            if (Session["Basket"] == null)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket
                {
                    DeliveryMonth = project.DeliveryMonth,
                    ProjectCode = project.ProjectCode,
                    BasketItems = new List<BasketItems>()
                };
                Session["Basket"] = basket;
            }

            if (((Basket)Session["Basket"]).ProjectCode != ProjectCode)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket
                {
                    DeliveryMonth = project.DeliveryMonth,
                    ProjectCode = project.ProjectCode,
                    BasketItems = new List<BasketItems>()
                };
                Session["Basket"] = basket;
            }

            var projectItem = catalogueDAL.GetProjectDetails(ProjectCode, ItemCode);
            if (projectItem != null)
            {
                return View("AddToBasket", projectItem);
            }

            if (((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemCode == ItemCode).Count() >= 1)
            {
                var basket = ((Basket)Session["Basket"]);
                BasketItems basketItem = basket.BasketItems.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
                return View("AddToBasket", basketItem);
            }

            var item = catalogueDAL.GetItem(ItemCode);
            return View("AddToBasket", item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("add-to-basket")]
        [Route("{ProjectCode}/catalogue/{ItemCode}")]
        [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
        public ActionResult AddToBasket(BasketItems BasketItem, string ProjectCode, string ItemCode)
        {
            if (Session["Basket"] == null)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket();
                basket.DeliveryMonth = project.DeliveryMonth;
                basket.ProjectCode = project.ProjectCode;
                basket.BasketItems = new List<BasketItems>();
                Session["Basket"] = basket;
            }

            if (((Basket)Session["Basket"]).ProjectCode != ProjectCode)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket();
                basket.DeliveryMonth = project.DeliveryMonth;
                basket.ProjectCode = project.ProjectCode;
                basket.BasketItems = new List<BasketItems>();
                Session["Basket"] = basket;
            }

            if (ProjectCode.Contains("CSPR"))
            {
                BasketItem.TotalQty = BasketItem.JanQty + BasketItem.FebQty + BasketItem.MarQty + BasketItem.AprQty + BasketItem.MayQty + BasketItem.JunQty + BasketItem.JulQty + BasketItem.AugQty + BasketItem.SepQty + BasketItem.OctQty + BasketItem.NovQty + BasketItem.DecQty;
            }

            if(BasketItem.Classification == "Repair and Maintenance" || BasketItem.Classification == "Infrastructure")
            {
                BasketItem.TotalQty = 1;
            }

            if (BasketItem.TotalQty == 0)
            {

                ModelState.AddModelError("", "Quantity must be at least one (1).");
                return View("AddToBasket", BasketItem);
            }

            if (BasketItem.Justification == null || BasketItem.Justification == String.Empty)
            {
                ModelState.AddModelError("", "Please provide justification for this item.");
                return View("AddToBasket", BasketItem);
            }

            if (BasketItem.ProcurementSource == "External Suppliers" && BasketItem.ResponsibilityCenter == "Requesting Office")
            {
                var supplier1 = suppliersDataAccess.GetSupplierDetails(BasketItem.Supplier1ID);
                var supplier2 = suppliersDataAccess.GetSupplierDetails(BasketItem.Supplier2ID);
                var supplier3 = suppliersDataAccess.GetSupplierDetails(BasketItem.Supplier3ID);
                BasketItem.Supplier1Name = supplier1.SupplierName;
                BasketItem.Supplier1Address = supplier1.Address;
                BasketItem.Supplier1ContactNo = supplier1.ContactNumber;
                BasketItem.Supplier1EmailAddress = supplier1.EmailAddress;
                BasketItem.Supplier2Name = supplier2.SupplierName;
                BasketItem.Supplier2Address = supplier2.Address;
                BasketItem.Supplier2ContactNo = supplier2.ContactNumber;
                BasketItem.Supplier2EmailAddress = supplier2.EmailAddress;
                BasketItem.Supplier3Name = supplier3.SupplierName;
                BasketItem.Supplier3Address = supplier3.Address;
                BasketItem.Supplier3ContactNo = supplier3.ContactNumber;
                BasketItem.Supplier3EmailAddress = supplier3.EmailAddress;
                BasketItem.UnitCost = catalogueDAL.ComputeUnitCost(BasketItem.Supplier1UnitCost, BasketItem.Supplier2UnitCost, BasketItem.Supplier3UnitCost);
                BasketItem.UnitCost = Math.Round((decimal)BasketItem.UnitCost, 2, MidpointRounding.AwayFromZero);
                BasketItem.EstimatedBudget = BasketItem.UnitCost * BasketItem.TotalQty;
            }
            else if (BasketItem.ProcurementSource == "Department of Budget and Management - Procurement Service")
            {
                var supplier = suppliersDataAccess.GetSupplierDetails(1);
                BasketItem.Supplier1ID = 1;
                BasketItem.Supplier1Name = supplier.SupplierName;
                BasketItem.Supplier1Address = supplier.Address;
                BasketItem.Supplier1ContactNo = supplier.ContactNumber;
                BasketItem.Supplier1EmailAddress = supplier.EmailAddress;
                BasketItem.Supplier1UnitCost = (decimal)BasketItem.UnitCost;
                BasketItem.EstimatedBudget = BasketItem.UnitCost * BasketItem.TotalQty;
            }
            else
            {
                BasketItem.UnitCost = BasketItem.UnitCost == null ? null : BasketItem.UnitCost;
                BasketItem.EstimatedBudget = BasketItem.UnitCost == null ? null : BasketItem.UnitCost * BasketItem.TotalQty;
            }

            if (BasketItem.Supplier1ID == 0 && ((BasketItem.ProcurementSource == "External Suppliers" && BasketItem.ResponsibilityCenter == "Requesting Office") || BasketItem.ProcurementSource == "Department of Budget and Management - Procurement Service"))
            {
                ModelState.AddModelError("", "Please provide at least one (1) Supplier Information and Unit Cost.");
                return View("AddToBasket", BasketItem);
            }

            if (BasketItem.Supplier1UnitCost == 0 && ((BasketItem.ProcurementSource == "External Suppliers" && BasketItem.ResponsibilityCenter == "Requesting Office") || BasketItem.ProcurementSource == "Department of Budget and Management - Procurement Service"))
            {
                ModelState.AddModelError("", "Unit Cost for Supplier 1 must be greater than " + (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph")) + ".");
                return View("AddToBasket", BasketItem);
            }

            if ((BasketItem.Supplier2ID != null || BasketItem.Supplier2ID != 0) && BasketItem.Supplier2UnitCost == 0 && (BasketItem.ProcurementSource == "External Suppliers" && BasketItem.ResponsibilityCenter == "Requesting Office"))
            {
                ModelState.AddModelError("", "Unit Cost for Supplier 2 must be greater than " + (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph")) + ".");
                return View("AddToBasket", BasketItem);
            }

            if ((BasketItem.Supplier3ID != null || BasketItem.Supplier3ID != 0) && BasketItem.Supplier3UnitCost == 0 && (BasketItem.ProcurementSource == "External Suppliers" && BasketItem.ResponsibilityCenter == "Requesting Office"))
            {
                ModelState.AddModelError("", "Unit Cost for Supplier 3 must be greater than " + (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph")) + ".");
                return View("AddToBasket", BasketItem);
            }

            if (((Basket)Session["Basket"]).BasketItems.Where(d => d.ItemCode == ItemCode).Count() >= 1)
            {
                var basket = ((Basket)Session["Basket"]);
                BasketItems basketItem = basket.BasketItems.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
                basket.BasketItems.Remove(basketItem);
                basket.BasketItems.Add(BasketItem);
            }
            else
            {
                ((Basket)Session["Basket"]).BasketItems.Add(BasketItem);
            }
            return RedirectToAction("view-catalogue");
        }

        [ActionName("view-basket")]
        [Route("{ProjectCode}/view-basket")]
        [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
        public ActionResult ViewBasket(string ProjectCode)
        {
            if (Session["Basket"] == null)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket
                {
                    DeliveryMonth = project.DeliveryMonth,
                    ProjectCode = project.ProjectCode,
                    BasketItems = new List<BasketItems>()
                };
                Session["Basket"] = basket;
            }

            if (((Basket)Session["Basket"]).ProjectCode != ProjectCode)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket
                {
                    DeliveryMonth = project.DeliveryMonth,
                    ProjectCode = project.ProjectCode,
                    BasketItems = new List<BasketItems>()
                };
                Session["Basket"] = basket;
            }

            var basketWithItems = Session["Basket"];
            return View("ViewBasket", basketWithItems);
        }

        [ActionName("remove-basket-item")]
        [Route("{ProjectCode}/remove-basket-item/{ItemCode}")]
        [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
        public ActionResult RemoveBasketItem(string ProjectCode, string ItemCode)
        {
            if (Session["Basket"] == null)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket
                {
                    DeliveryMonth = project.DeliveryMonth,
                    ProjectCode = project.ProjectCode,
                    BasketItems = new List<BasketItems>()
                };
                Session["Basket"] = basket;
            }

            if (((Basket)Session["Basket"]).ProjectCode != ProjectCode)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket();
                basket.DeliveryMonth = project.DeliveryMonth;
                basket.ProjectCode = project.ProjectCode;
                basket.BasketItems = new List<BasketItems>();
                Session["Basket"] = basket;
            }

            var basketWithItems = Session["Basket"] as Basket;
            var itemToRemove = basketWithItems.BasketItems.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            basketWithItems.BasketItems.Remove(itemToRemove);
            return View("ViewBasket", basketWithItems);
        }

        [ActionName("clear-basket")]
        [Route("{ProjectCode}/clear-basket")]
        [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
        public ActionResult ClearBasket(string ProjectCode)
        {
            if (Session["Basket"] == null)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket
                {
                    DeliveryMonth = project.DeliveryMonth,
                    ProjectCode = project.ProjectCode,
                    BasketItems = new List<BasketItems>()
                };
                Session["Basket"] = basket;
            }

            if (((Basket)Session["Basket"]).ProjectCode != ProjectCode)
            {
                var projectPlanDAL = new ProjectPlansDAL();
                var project = projectPlanDAL.GetProjectPlan(ProjectCode);
                var basket = new Basket
                {
                    DeliveryMonth = project.DeliveryMonth,
                    ProjectCode = project.ProjectCode,
                    BasketItems = new List<BasketItems>()
                };
                Session["Basket"] = basket;
            }

            var basketWithItems = Session["Basket"] as Basket;
            basketWithItems.BasketItems.Clear();
            return RedirectToAction("view-basket");
        }

        [Route("view-suppliers")]
        [ActionName("view-suppliers")]
        [UserAuthorization(Roles = SystemRoles.ResponsibilityCenterPlanner + ", " + SystemRoles.SuppliesChief + ", " + SystemRoles.ResponsibilityCenterPlanner)]
        public ActionResult ViewSuppliers(int SupplierNo, int Supplier1ID, int? Supplier2ID, int? Supplier3ID, string ItemCode)
        {
            ViewBag.SupplierNo = SupplierNo;
            return PartialView("SupplierView", catalogueDAL.GetSuppliers(Supplier1ID, Supplier2ID, Supplier3ID, ItemCode));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("view-suppliers")]
        [ActionName("post-to-project")]
        [UserAuthorization(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
        public ActionResult PostItems(string ProjectCode)
        {
            var basket = ((Basket)Session["Basket"]);
            Session.RemoveAll();
            return Json(new { result = catalogueDAL.PostToProject(basket, User.Identity.Name) });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                catalogueDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}