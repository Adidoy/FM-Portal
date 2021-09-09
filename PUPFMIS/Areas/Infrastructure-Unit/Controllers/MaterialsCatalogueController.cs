using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.BusinessAndDataLogic;

namespace PUPFMIS.Controllers
{
    [Route("{action}")]
    [RouteArea("infrastructure-unit")]
    [RoutePrefix("estimates/materials")]
    [UserAuthorization(Roles = SystemRoles.InfrastructureImplementingUnit)]
    public class MaterialsCatalogueController : Controller
    {
        private MaterialsCatalogueBDL materialsCatalogue = new MaterialsCatalogueBDL();

        [Route("view-catalogue/project/{InfraProjectCode}")]
        [ActionName("view-catalogue")]
        public ActionResult MaterialsCatalogue(string InfraProjectCode)
        {
            if (Session["Estimates"] == null)
            {
                var estimates = new MaterialsBasket
                {
                    InfraProjectCode = InfraProjectCode,
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            if (((MaterialsBasket)Session["Estimates"]).InfraProjectCode != InfraProjectCode)
            {
                var estimates = new MaterialsBasket
                {
                    InfraProjectCode = InfraProjectCode,
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            ViewBag.InfraProjectCode = InfraProjectCode;
            var materialsCatalogueList = materialsCatalogue.GetMaterialsCatalogue();
            return View("MaterialsCatalogue", materialsCatalogueList);
        }

        [Route("{ID}/add-material/project/{InfraProjectCode}")]
        [ActionName("add-material")]
        public ActionResult AddMaterial(int ID, string InfraProjectCode)
        {
            if (Session["Estimates"] == null)
            {
                var estimates = new MaterialsBasket
                {
                    InfraProjectCode = InfraProjectCode,
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            if (((MaterialsBasket)Session["Estimates"]).InfraProjectCode != InfraProjectCode)
            {
                var estimates = new MaterialsBasket
                {
                    InfraProjectCode = InfraProjectCode,
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            //var projectItem = catalogueDAL.GetProjectDetails(ProjectCode, ItemCode);
            //if (projectItem != null)
            //{
            //    return View("AddToBasket", projectItem);
            //}

            if (((MaterialsBasket)Session["Estimates"]).ItemList.Where(d => d.ID == ID).Count() >= 1)
            {
                var estimates = ((MaterialsBasket)Session["Estimates"]);
                var item = estimates.ItemList.Where(d => d.ID == ID).FirstOrDefault();
                return View("AddMaterial", item);
            }

            var material = materialsCatalogue.GetMaterial(ID);
            return View("AddMaterial", material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{ID}/add-material/project/{InfraProjectCode}")]
        [ActionName("add-material")]
        public ActionResult AddMaterial(InfrastructureDetailedEstimateVM MaterialItem, int ID, string InfraProjectCode)
        {
            if (Session["Estimates"] == null)
            {
                var estimates = new MaterialsBasket
                {
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            if (((MaterialsBasket)Session["Estimates"]).InfraProjectCode != InfraProjectCode)
            {
                var estimates = new MaterialsBasket
                {
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            if (MaterialItem.Quantity == 0)
            {
                ModelState.AddModelError("", "Quantity must be at least one (1).");
                return View("AddMaterial", MaterialItem);
            }

            if (MaterialItem.LaborUnitCost == 0)
            {
                ModelState.AddModelError("", "Labor Cost must be greater than " + (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph")) + ".");
                return View("AddMaterial", MaterialItem);
            }

            if (((MaterialsBasket)Session["Estimates"]).ItemList.Where(d => d.ID == ID).Count() >= 1)
            {
                var estimates = ((MaterialsBasket)Session["Estimates"]);
                var item = estimates.ItemList.Where(d => d.ID == ID).FirstOrDefault();
                estimates.ItemList.Remove(item);
                estimates.ItemList.Add(MaterialItem);
            }
            else
            {
                ((MaterialsBasket)Session["Estimates"]).ItemList.Add(MaterialItem);
            }

            MaterialItem.ItemTotalCost = MaterialItem.Quantity * MaterialItem.ItemUnitCost;
            MaterialItem.LaborTotalCost = MaterialItem.Quantity * MaterialItem.LaborUnitCost;
            MaterialItem.EstimatedDirectCost = MaterialItem.ItemTotalCost + MaterialItem.LaborTotalCost;
            MaterialItem.MobDemobilizationCost = (MaterialItem.EstimatedDirectCost * 0.01m);
            MaterialItem.OCMCost = (MaterialItem.EstimatedDirectCost * 0.12m);
            MaterialItem.ProfitCost = (MaterialItem.EstimatedDirectCost * 0.12m);
            MaterialItem.TotalMarkUp = MaterialItem.MobDemobilizationCost + MaterialItem.OCMCost + MaterialItem.ProfitCost;
            MaterialItem.VAT = (MaterialItem.EstimatedDirectCost + MaterialItem.TotalMarkUp) * 0.12m;
            MaterialItem.TotalIndirectCost = MaterialItem.TotalMarkUp + MaterialItem.VAT;
            MaterialItem.TotalAmount = MaterialItem.EstimatedDirectCost + MaterialItem.TotalIndirectCost;

            return RedirectToAction("view-catalogue");
        }

        [ActionName("view-estimates")]
        [Route("view-estimates/project/{InfraProjectCode}")]
        public ActionResult ViewEstimates(string InfraProjectCode)
        {
            if (Session["Estimates"] == null)
            {
                var estimates = new MaterialsBasket
                {
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            if (((MaterialsBasket)Session["Estimates"]).InfraProjectCode != InfraProjectCode)
            {
                var estimates = new MaterialsBasket
                {
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            var detailedEstimates = Session["Estimates"];
            return View("ViewEstimates", detailedEstimates);
        }

        [ActionName("remove-material")]
        [Route("{ItemID}/remove/project/{InfraProjectCode}")]
        public ActionResult RemoveBasketItem(int ItemID, string InfraProjectCode)
        {
            if (Session["Estimates"] == null)
            {
                var estimates = new MaterialsBasket
                {
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            if (((MaterialsBasket)Session["Estimates"]).InfraProjectCode != InfraProjectCode)
            {
                var estimates = new MaterialsBasket
                {
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            var estimate = Session["Estimates"] as MaterialsBasket;
            var itemToRemove = estimate.ItemList.Where(d => d.ID == ItemID).FirstOrDefault();
            estimate.ItemList.Remove(itemToRemove);
            return View("ViewEstimates", estimate);
        }

        [ActionName("clear-estimates")]
        [Route("{InfraProjectCode}/clear-estimates")]
        public ActionResult ClearBasket(string InfraProjectCode)
        {
            if (Session["Estimates"] == null)
            {
                var estimates = new MaterialsBasket
                {
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            if (((MaterialsBasket)Session["Estimates"]).InfraProjectCode != InfraProjectCode)
            {
                var estimates = new MaterialsBasket
                {
                    ItemList = new List<InfrastructureDetailedEstimateVM>()
                };
                Session["Estimates"] = estimates;
            }

            var materialsBasket = Session["Estimates"] as MaterialsBasket;
            materialsBasket.ItemList.Clear();
            return RedirectToAction("view-estimates");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("post-to-project")]
        [Route("{InfraProjectCode}/post-to-project")]
        public ActionResult PostItems()
        {
            var estimates = ((MaterialsBasket)Session["Estimates"]);
            Session.RemoveAll();
            return Json(new { result = materialsCatalogue.PostToProject(estimates, User.Identity.Name) });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                materialsCatalogue.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}