using PUPFMIS.BusinessAndDataLogic;
using PUPFMIS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System;

namespace PUPFMIS.Areas.EndUsers.Controllers
{
    [RouteArea("end-users")]
    [RoutePrefix("catalogue")]
    [Route("{action}")]
    [Authorize(Roles = SystemRoles.SuperUser + ", " + SystemRoles.EndUser)]
    public class CatalogueController : Controller
    {
        private CatalogueBL catalogueBL = new CatalogueBL();
        private CatalogueDAL catalogueDAL = new CatalogueDAL();
        private ItemDataAccess itemDataAccess = new ItemDataAccess();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();

        [HttpPost]
        [Route("")]
        [ActionName("view-item-catalogue")]
        public ActionResult CommonSuppliesCatalogue(string ProjectCode, bool FromCatalogue)
        {
            var departmentCode = hrisDataAccess.GetEmployee(User.Identity.Name).DepartmentCode;
            if(FromCatalogue == false)
            {
                Session["ProjectPlan"] = new ProjectPlanVM();
                Session["ProjectPlan"] = catalogueDAL.GetProject(ProjectCode);
                ((ProjectPlanVM)Session["ProjectPlan"]).ProjectPlanItems = new List<ProjectPlanItemsVM>();
                ((ProjectPlanVM)Session["ProjectPlan"]).NewItemProposals = new List<ProjectPlanItemsVM>();
            }
            ViewBag.Categories = catalogueDAL.GetItemCategories();
            if(ProjectCode.Substring(0, 4) == "CSPR")
            {
                ViewBag.ProjectCode = ProjectCode;
                return View("CommonSuppliesCatalogue", itemDataAccess.GetItemCatalogueView(false, "CUOS", departmentCode));
            }

            if (ProjectCode.Substring(0, 4) == "EUPR")
            {
                ViewBag.ProjectCode = ProjectCode;
                return View("CommonSuppliesCatalogue", itemDataAccess.GetItemCatalogueView(false, departmentCode));
            }
            return RedirectToAction("project-details", "ProjectPlans", new { Area = "end-user", ProjectCode = ProjectCode });
        }

        [Route("")]
        [ActionName("view-item-catalogue")]
        public ActionResult CommonSuppliesCatalogue()
        {
            var departmentCode = hrisDataAccess.GetEmployee(User.Identity.Name).DepartmentCode;
            var projectPlan = Session["ProjectPlan"] as ProjectPlanVM;
            if (projectPlan == null)
            {
                return RedirectToAction("list", "ProjectPlans", new { Area = "end-users" });
            }
            ViewBag.Categories = catalogueDAL.GetItemCategories();
            if (projectPlan.ProjectCode.Substring(0, 4) == "CSPR")
            {
                ViewBag.ProjectCode = projectPlan.ProjectCode;
                return View("CommonSuppliesCatalogue", itemDataAccess.GetItemCatalogueView(false, "CUOS", departmentCode));
            }

            if (projectPlan.ProjectCode.Substring(0, 4) == "EUPR")
            {
                ViewBag.ProjectCode = projectPlan.ProjectCode;
                return View("CommonSuppliesCatalogue", itemDataAccess.GetItemCatalogueView(false, departmentCode));
            }
            return RedirectToAction("project-details", "ProjectPlans", new { Area = "end-user", ProjectCode = projectPlan.ProjectCode });
        }

        [Route("view-suppliers")]
        [ActionName("view-suppliers")]
        public ActionResult ViewSuppliers(int SupplierNo)
        {
            ViewBag.SupplierNo = SupplierNo;
            return PartialView("SupplierView", catalogueDAL.GetSuppliers());
        }

        [ActionName("add-to-basket")]
        [Route("{InventoryType}/{ItemCode}/add-to-basket")]
        public ActionResult AddToBasket(string ItemCode, string InventoryType)
        {
            var projectPlan = Session["ProjectPlan"] as ProjectPlanVM;
            if (projectPlan == null)
            {
                return RedirectToAction("list", "ProjectPlans", new { Area = "end-users" });
            }

            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = catalogueDAL.GetItem(ItemCode, InventoryType);
            if (item == null)
            {
                return HttpNotFound();
            }

            if (projectPlan.ProjectCode.Substring(0, 4) == "CSPR")
            {
                return AddSupplies(item, projectPlan);
            }
            else
            {
                return AddProjectItem(item, projectPlan);
            }
        }

        private ActionResult AddSupplies(CatalogueBasketItemVM Item, ProjectPlanVM ProjectPlan)
        {
            if (Item.InventoryType == "Common Use Office Supplies" && Item.ProcurementSource == ProcurementSources.PS_DBM)
            {
                var supplierInfo = catalogueDAL.GetSupplier(1);
                Item.Supplier1ID = supplierInfo.ID;
                Item.Supplier1Name = supplierInfo.SupplierName;
                Item.Supplier1Address = supplierInfo.Address;
                Item.Supplier1ContactNo = supplierInfo.ContactNumber;
                Item.Supplier1EmailAddress = (supplierInfo.EmailAddress == null) ? "Email Address not provided." : supplierInfo.EmailAddress;
                Item.Supplier1UnitCost = Item.UnitCost == null ? 0.00m : (decimal)Item.UnitCost;
                Item.UnitCost = Item.UnitCost == null ? 0.00m : (decimal)Item.UnitCost;
            }

            if (catalogueBL.ItemBelongsToProject(ProjectPlan.ProjectCode, Item.ItemCode))
            {
                ModelState.AddModelError("", "Item is included in the Project.");
                ViewBag.EnableElement = false;
                return View("AddCommonSuppliesToBasket", Item);
            }

            if (catalogueBL.ItemBelongsToProjectActual(ProjectPlan.ProjectCode, Item.ItemCode))
            {
                ModelState.AddModelError("", "Item is already included in the Project as your Actual Obligation for the previous year.");
                ModelState.AddModelError("", "By continuing, item will be added to your New Spending Proposal (Tier 2).");
                ViewBag.EnableElement = true;
                Item.ProposalType = BudgetProposalType.NewProposal;
                return View("AddCommonSuppliesToBasket", Item);
            }

            var newProposalItem = ProjectPlan.NewItemProposals.Where(d => d.ItemCode == Item.ItemCode).FirstOrDefault();
            if (newProposalItem != null)
            {
                ModelState.AddModelError("", "Item is already in the Basket.");
                ViewBag.EnableElement = true;
                return View("AddCommonSuppliesToBasket", Item);
            }

            Item.ProposalType = BudgetProposalType.NewProposal;
            ViewBag.EnableElement = true;
            return View("AddCommonSuppliesToBasket", Item);
        }

        private ActionResult AddProjectItem(CatalogueBasketItemVM Item, ProjectPlanVM ProjectPlan)
        {
            if (catalogueBL.ItemBelongsToProject(ProjectPlan.ProjectCode, Item.ItemCode))
            {
                ModelState.AddModelError("", "Item is included in the Project.");
                ViewBag.EnableElement = false;
                return View("AddToBasket", Item);
            }

            if (catalogueBL.ItemBelongsToProjectActual(ProjectPlan.ProjectCode, Item.ItemCode))
            {
                ModelState.AddModelError("", "Item is already included in the Project as your Actual Obligation for the previous year.");
                ModelState.AddModelError("", "By continuing, item will be added to your New Spending Proposal (Tier 2).");
                ViewBag.EnableElement = true;
                Item.ProposalType = BudgetProposalType.NewProposal;
                return View("AddToBasket", Item);
            }

            if (Item.ProcurementSource == ProcurementSources.PS_DBM)
            {
                var supplierInfo = catalogueDAL.GetSupplier(1);
                Item.Supplier1ID = supplierInfo.ID;
                Item.Supplier1Name = supplierInfo.SupplierName;
                Item.Supplier1Address = supplierInfo.Address;
                Item.Supplier1ContactNo = supplierInfo.ContactNumber;
                Item.Supplier1EmailAddress = (supplierInfo.EmailAddress == null) ? "Email Address not provided." : supplierInfo.EmailAddress;
                Item.Supplier1UnitCost = (decimal)Item.UnitCost;
                Item.UnitCost = (decimal)Item.UnitCost;
            }

            var newProposalItem = ProjectPlan.NewItemProposals.Where(d => d.ItemCode == Item.ItemCode).FirstOrDefault();
            if (newProposalItem != null)
            {
                ModelState.AddModelError("", "Item is included in the basket.");
                ModelState.AddModelError("", "You may update the details of the item.");
                Item.ProposalType = newProposalItem.ProposalType;
                Item.TotalQty = newProposalItem.TotalQty;
                Item.Remarks = newProposalItem.Remarks;
                Item.UnitCost = newProposalItem.UnitCost;
                Item.Supplier1ID = newProposalItem.Supplier1ID;
                Item.Supplier1UnitCost = newProposalItem.Supplier1UnitCost;
                Item.Supplier2ID = newProposalItem.Supplier2ID;
                Item.Supplier2UnitCost = newProposalItem.Supplier2UnitCost;
                Item.Supplier3ID = newProposalItem.Supplier3ID;
                Item.Supplier3UnitCost = newProposalItem.Supplier3UnitCost;
                ViewBag.IsTangible = catalogueBL.IsItemTangible(Item.ItemCode);
                ViewBag.EnableElement = true;
                return View("AddToBasket", Item);
            }

            ViewBag.IsTangible = catalogueBL.IsItemTangible(Item.ItemCode);
            Item.ProposalType = BudgetProposalType.NewProposal;
            ViewBag.EnableElement = true;
            return View("AddToBasket", Item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("add-to-basket")]
        [Route("{InventoryType}/{ItemCode}/add-to-basket")]
        public ActionResult AddToBasket(CatalogueBasketItemVM itemBasket)
        {
            var projectPlan = Session["ProjectPlan"] as ProjectPlanVM;
            if (projectPlan == null)
            {
                return RedirectToAction("list", "ProjectPlans", new { Area = "end-users" });
            }

            string Message = string.Empty;
            if (!catalogueBL.ValidateAddItem(itemBasket, projectPlan.ProjectCode, out Message))
            {
                ModelState.AddModelError("", Message);
                ViewBag.IsTangible = catalogueBL.IsItemTangible(itemBasket.ItemCode);
                ViewBag.EnableElement = true;
                return View("AddToBasket", itemBasket);
            }

            if (itemBasket.ProcurementSource == ProcurementSources.PS_DBM)
            {
                var supplierInfo = catalogueDAL.GetSupplier(1);
                itemBasket.Supplier1ID = supplierInfo.ID;
                itemBasket.Supplier1Name = supplierInfo.SupplierName;
                itemBasket.Supplier1Address = supplierInfo.Address;
                itemBasket.Supplier1ContactNo = supplierInfo.ContactNumber;
                itemBasket.Supplier1EmailAddress = (supplierInfo.EmailAddress == null) ? "Email Address not provided." : supplierInfo.EmailAddress;
                itemBasket.Supplier1UnitCost = (decimal)itemBasket.UnitCost;
                itemBasket.UnitCost = (decimal)itemBasket.UnitCost;
            }

            var newItemProposal = projectPlan.NewItemProposals.Where(d => d.ItemCode == itemBasket.ItemCode).FirstOrDefault();
            if(newItemProposal != null)
            {
                projectPlan.NewItemProposals.Remove(newItemProposal);
            }

            if(itemBasket.ProcurementSource == ProcurementSources.PS_DBM && itemBasket.InventoryType == "Common Use Office Supplies")
            {
                itemBasket.Supplier1ID = 1;
                itemBasket.Supplier1UnitCost = (decimal)itemBasket.UnitCost;
                itemBasket.UnitCost = (decimal)itemBasket.UnitCost;
            }
            else
            {
                itemBasket.UnitCost = catalogueBL.ComputeUnitCost(itemBasket.Supplier1UnitCost, itemBasket.Supplier2UnitCost, itemBasket.Supplier3UnitCost);
                itemBasket.UnitCost = Math.Round((decimal)itemBasket.UnitCost, 2, MidpointRounding.AwayFromZero);
            }

            if (ModelState.IsValid)
            {
                projectPlan.NewItemProposals.Add(new ProjectPlanItemsVM
                {
                    ProjectCode = projectPlan.ProjectCode,
                    ItemCode = itemBasket.ItemCode,
                    ItemName = itemBasket.ItemName,
                    ItemImage = itemBasket.ItemImage,
                    ItemSpecifications = itemBasket.ItemSpecifications,
                    InventoryType = itemBasket.InventoryType,
                    ItemCategory = itemBasket.ItemCategory,
                    ProcurementSource = itemBasket.ProcurementSource,
                    IndividualUOMReference = itemBasket.IndividualUOMReference,
                    PackagingUOMReference = itemBasket.PackagingUOMReference,
                    ProposalType = BudgetProposalType.NewProposal,
                    JanQty = itemBasket.JanQty,
                    FebQty = itemBasket.FebQty,
                    MarQty = itemBasket.MarQty,
                    AprQty = itemBasket.AprQty,
                    MayQty = itemBasket.MayQty,
                    JunQty = itemBasket.JunQty,
                    JulQty = itemBasket.JulQty,
                    AugQty = itemBasket.AugQty,
                    SepQty = itemBasket.SepQty,
                    OctQty = itemBasket.OctQty,
                    NovQty = itemBasket.NovQty,
                    DecQty = itemBasket.DecQty,
                    TotalQty = itemBasket.TotalQty,
                    UnitCost = (decimal)itemBasket.UnitCost,
                    Supplier1ID = itemBasket.Supplier1ID,
                    Supplier1UnitCost = itemBasket.Supplier1UnitCost,
                    Supplier2ID = itemBasket.Supplier2ID,
                    Supplier2UnitCost = itemBasket.Supplier2UnitCost,
                    Supplier3ID = itemBasket.Supplier3ID,
                    Supplier3UnitCost = itemBasket.Supplier3UnitCost,
                    EstimatedBudget = (int)itemBasket.TotalQty * (decimal)itemBasket.UnitCost,
                    Remarks = itemBasket.Remarks
                });
                Session["ProjectPlan"] = projectPlan;
                ViewBag.StartMonth = projectPlan.ProjectMonthStart;
                return RedirectToAction("view-item-catalogue", "Catalogue", new { Area = "end-users" });
            }
            
            ViewBag.EnableElement = true;
            ViewBag.IsTangible = catalogueBL.IsItemTangible(itemBasket.ItemCode);
            return View("AddToBasket", itemBasket);
        }

        [ActionName("update-basket-item")]
        [Route("{InventoryType}/{ItemCode}/update-basket-item")]
        public ActionResult UpdateBasketItem(string ItemCode, string InventoryType)
        {
            var projectPlan = Session["ProjectPlan"] as ProjectPlanVM;
            if (projectPlan == null)
            {
                return RedirectToAction("list", "ProjectPlans", new { Area = "end-users" });
            }

            if (ItemCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = catalogueDAL.GetItem(ItemCode, InventoryType);
            if (item == null)
            {
                return HttpNotFound();
            }

            var basketItem = projectPlan.NewItemProposals.Where(d => d.ItemCode == item.ItemCode).FirstOrDefault();
            if (basketItem == null)
            {
                return HttpNotFound();
            }

            var supplier1 = catalogueDAL.GetSupplier(basketItem.Supplier1ID);
            var supplier2 = catalogueDAL.GetSupplier(basketItem.Supplier2ID);
            var supplier3 = catalogueDAL.GetSupplier(basketItem.Supplier3ID);

            item.JanQty = basketItem.JanQty;
            item.FebQty = basketItem.FebQty;
            item.MarQty = basketItem.MarQty;
            item.AprQty = basketItem.AprQty;
            item.MayQty = basketItem.MayQty;
            item.JunQty = basketItem.JunQty;
            item.JulQty = basketItem.JulQty;
            item.AugQty = basketItem.AugQty;
            item.SepQty = basketItem.SepQty;
            item.OctQty = basketItem.OctQty;
            item.NovQty = basketItem.NovQty;
            item.DecQty = basketItem.DecQty;
            item.UnitCost = basketItem.UnitCost;
            item.TotalQty = basketItem.TotalQty;
            item.Remarks = basketItem.Remarks;
            item.UnitCost = basketItem.UnitCost;
            item.Supplier1ID = supplier1.ID;
            item.Supplier1Name = supplier1.SupplierName;
            item.Supplier1Address = supplier1.Address;
            item.Supplier1ContactNo = supplier1.ContactNumber;
            item.Supplier1EmailAddress = (supplier1.EmailAddress == null) ? "No data provided." : supplier1.EmailAddress;
            item.Supplier1UnitCost = basketItem.Supplier1UnitCost;
            item.Supplier2ID = supplier2.ID;
            item.Supplier2Name = supplier2.SupplierName;
            item.Supplier2Address = supplier2.Address;
            item.Supplier2ContactNo = supplier2.ContactNumber;
            item.Supplier2EmailAddress = (supplier2.EmailAddress == null) ? "No data provided." : supplier2.EmailAddress;
            item.Supplier2UnitCost = basketItem.Supplier2UnitCost;
            item.Supplier3ID = supplier3.ID;
            item.Supplier3Name = supplier3.SupplierName;
            item.Supplier3Address = supplier3.Address;
            item.Supplier3ContactNo = supplier3.ContactNumber;
            item.Supplier3EmailAddress = (supplier3.EmailAddress == null) ? "No data provided." : supplier3.EmailAddress;
            item.Supplier3UnitCost = basketItem.Supplier3UnitCost;

            if (projectPlan.ProjectCode.Substring(0, 4) == "CSPR")
            {
                ViewBag.EnableElement = true;
                return View("UpdateCommonSuppliesBasket", item);
            }
            else
            {
                ViewBag.EnableElement = true;
                return View("UpdateBasket", item);
            }
        }

        [HttpPost]
        [ActionName("update-basket-item")]
        [Route("{InventoryType}/{ItemCode}/update-basket-item")]
        public ActionResult UpdateBasketItem(CatalogueBasketItemVM BasketItem)
        {
            var projectPlan = Session["ProjectPlan"] as ProjectPlanVM;
            if (projectPlan == null)
            {
                return RedirectToAction("list", "ProjectPlans", new { Area = "end-users" });
            }
            var item = projectPlan.NewItemProposals.Where(d => d.ItemCode == BasketItem.ItemCode).FirstOrDefault();
            if(item == null)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                return View("UpdateCommonSuppliesBasket", BasketItem);
            }

            if (BasketItem.InventoryType == "Common Use Office Supplies")
            {
                var supplierInfo = catalogueDAL.GetSupplier(1);
                BasketItem.Supplier1ID = supplierInfo.ID;
                BasketItem.Supplier1Name = supplierInfo.SupplierName;
                BasketItem.Supplier1Address = supplierInfo.Address;
                BasketItem.Supplier1ContactNo = supplierInfo.ContactNumber;
                BasketItem.Supplier1EmailAddress = (supplierInfo.EmailAddress == null) ? "Email Address not provided." : supplierInfo.EmailAddress;
                BasketItem.Supplier1UnitCost = (decimal)BasketItem.UnitCost;
                BasketItem.UnitCost = (decimal)BasketItem.UnitCost;
            }

            string Message = string.Empty;
            if (!catalogueBL.ValidateAddItem(BasketItem, projectPlan.ProjectCode, out Message))
            {
                ModelState.AddModelError("", Message);
                ViewBag.EnableElement = true;
                ViewBag.IsTangible = catalogueBL.IsItemTangible(BasketItem.ItemCode);
                return View("UpdateCommonSuppliesBasket", BasketItem);
            }

            projectPlan.NewItemProposals.Remove(item);
            projectPlan.NewItemProposals.Add(new ProjectPlanItemsVM
            {
                ProjectCode = projectPlan.ProjectCode,
                ItemCode = BasketItem.ItemCode,
                ItemName = BasketItem.ItemName,
                ItemImage = BasketItem.ItemImage,
                ItemSpecifications = BasketItem.ItemSpecifications,
                InventoryType = BasketItem.InventoryType,
                ItemCategory = BasketItem.ItemCategory,
                ProcurementSource = BasketItem.ProcurementSource,
                IndividualUOMReference = BasketItem.IndividualUOMReference,
                PackagingUOMReference = BasketItem.PackagingUOMReference,
                ProposalType = BudgetProposalType.NewProposal,
                JanQty = BasketItem.JanQty,
                FebQty = BasketItem.FebQty,
                MarQty = BasketItem.MarQty,
                AprQty = BasketItem.AprQty,
                MayQty = BasketItem.MayQty,
                JunQty = BasketItem.JunQty,
                JulQty = BasketItem.JulQty,
                AugQty = BasketItem.AugQty,
                SepQty = BasketItem.SepQty,
                OctQty = BasketItem.OctQty,
                NovQty = BasketItem.NovQty,
                DecQty = BasketItem.DecQty,
                TotalQty = BasketItem.TotalQty,
                UnitCost = (decimal)BasketItem.UnitCost,
                Supplier1ID = BasketItem.Supplier1ID,
                Supplier1UnitCost = BasketItem.Supplier1UnitCost,
                Supplier2ID = BasketItem.Supplier2ID,
                Supplier2UnitCost = BasketItem.Supplier2UnitCost,
                Supplier3ID = BasketItem.Supplier3ID,
                Supplier3UnitCost = BasketItem.Supplier3UnitCost,
                EstimatedBudget = (int)BasketItem.TotalQty * (decimal)BasketItem.UnitCost,
                Remarks = BasketItem.Remarks
            });
            Session["ProjectPlan"] = projectPlan;
            return RedirectToAction("common-supplies-basket", "Catalogue", new { Area = "end-users" });
        }
        
        [ActionName("remove-basket-item")]
        [Route("{ItemCode}/remove-basket-item")]
        public ActionResult RemoveBasketItem(string ItemCode)
        {
            var projectPlan = Session["ProjectPlan"] as ProjectPlanVM;
            if (projectPlan == null)
            {
                return RedirectToAction("list", "ProjectPlans", new { Area = "end-users" });
            }
            var item = projectPlan.NewItemProposals.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            if (item == null)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                return RedirectToAction("common-supplies-basket", "Catalogue", new { Area = "end-users" });
            }
            projectPlan.NewItemProposals.Remove(item);
            return RedirectToAction("common-supplies-basket", "Catalogue", new { Area = "end-users" });
        }

        [ActionName("common-supplies-basket")]
        [Route("view-basket")]
        public ActionResult ViewCommonSuppliesBasket()
        {
            var projectPlan = Session["ProjectPlan"] as ProjectPlanVM;
            if(projectPlan == null)
            {
                return RedirectToAction("list", "ProjectPlans", new { Area = "end-users" });
            }
            return View("ViewBasket", projectPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("save-common-supplies")]
        [Route("view-basket")]
        public ActionResult SaveCommonSupplies(ProjectPlanVM projectPlan)
        {
            string Message = string.Empty;
            if (!catalogueBL.ValidateProjectPlan(projectPlan, out Message))
            {
                ModelState.AddModelError("", Message);
                return View("ViewBasket", projectPlan);
            }
            if (ModelState.IsValid)
            {
                if(catalogueDAL.SaveBasket(projectPlan, out Message))
                { 
                    return RedirectToAction("project-details", "ProjectPlans", new { Area = "end-users", ProjectCode = projectPlan.ProjectCode });
                }
                ModelState.AddModelError("", "Something went wrong. Please try again.");
            }
            return View("common-supplies-basket", projectPlan);
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