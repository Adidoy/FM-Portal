using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class CatalogueBL : Controller
    {
        FMISDbContext db = new FMISDbContext();
        CatalogueDAL catalogueDAL = new CatalogueDAL();

        public bool IsItemTangible(string ItemCode)
        {
            var item = db.Items.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            if(item == null)
            {
                return db.Services.Where(d => d.ServiceCode == ItemCode).FirstOrDefault().FKInventoryTypeReference.IsTangible;
            }
            else
            {
                return db.Items.Where(d => d.ItemCode == ItemCode).FirstOrDefault().FKInventoryTypeReference.IsTangible;
            }
        }
        public bool ItemBelongsToProject(string ProjectCode, string ItemCode)
        {
            return (db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.FKItemReference.ItemCode == ItemCode && d.ProposalType == BudgetProposalType.NewProposal).Count() == 1) ? true : false;
        }
        public bool ItemBelongsToProjectActual(string ProjectCode, string ItemCode)
        {
            return db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.FKItemReference.ItemCode == ItemCode && d.ProposalType == BudgetProposalType.Actual).Count() == 1 ? true : false;
        }
        public bool ValidateProjectPlan(ProjectPlanVM projectPlan, out string Message)
        {
            if (projectPlan == null)
            {
                Message = "Session Expired";
                return false;
            }
            if (projectPlan.NewItemProposals == null)
            {
                Message = "There are no items in the basket. Please go to catalogue to add items.";
                return false;
            }
            Message = string.Empty;
            return true;
        }
        public bool ValidateAddItem(CatalogueBasketItemVM item, string ProjectCode, out string Message)
        {
            if (ProjectCode.Substring(0, 4) == "CSPR")
            {
                item.JanQty = (item.JanQty == null) ? 0 : item.JanQty;
                item.FebQty = (item.FebQty == null) ? 0 : item.FebQty;
                item.MarQty = (item.MarQty == null) ? 0 : item.MarQty;
                item.AprQty = (item.AprQty == null) ? 0 : item.AprQty;
                item.MayQty = (item.MayQty == null) ? 0 : item.MayQty;
                item.JunQty = (item.JunQty == null) ? 0 : item.JunQty;
                item.JulQty = (item.JulQty == null) ? 0 : item.JulQty;
                item.AugQty = (item.AugQty == null) ? 0 : item.AugQty;
                item.SepQty = (item.SepQty == null) ? 0 : item.SepQty;
                item.OctQty = (item.OctQty == null) ? 0 : item.OctQty;
                item.NovQty = (item.NovQty == null) ? 0 : item.NovQty;
                item.DecQty = (item.DecQty == null) ? 0 : item.DecQty;
                item.TotalQty = (int)(item.JanQty + item.FebQty + item.MarQty + item.AprQty + item.MayQty + item.JunQty + item.JulQty + item.AugQty + item.OctQty + item.NovQty + item.DecQty);
            }
            else
            {
                var StartMonth = db.ProjectPlans.Where(d => d.ProjectCode == ProjectCode).FirstOrDefault().ProjectMonthStart;
                switch (StartMonth)
                {
                    case 1:
                        {
                            item.JanQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                    case 2:
                        {
                            item.FebQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                    case 3:
                        {
                            item.MarQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                    case 4:
                        {
                            item.AprQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                    case 5:
                        {
                            item.MayQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                    case 6:
                        {
                            item.JunQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                    case 7:
                        {
                            item.JulQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                    case 8:
                        {
                            item.AugQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                    case 9:
                        {
                            item.SepQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                    case 10:
                        {
                            item.OctQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                    case 11:
                        {
                            item.NovQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                    case 12:
                        {
                            item.DecQty = item.TotalQty;
                            item.TotalQty = item.TotalQty = item.TotalQty;
                        }
                        break;
                }
            }

            if (item.TotalQty <= 0)
            {
                Message = "Please enter quantity requirement for at least one (1) quarter.";
                return false;
            }
            if (item.Remarks == null || item.Remarks == string.Empty)
            {
                Message = "Please provide justification/remarks for the new spending proposal.";
                return false;
            }

            Message = string.Empty;
            return true;
        }
        public decimal ComputeUnitCost(decimal? Supplier1UnitCost, decimal? Supplier2UnitCost, decimal? Supplier3UnitCost)
        {
            decimal unitCost = 0.00m;
            int count = 0;
            if(Supplier1UnitCost != null)
            {
                unitCost += (decimal)Supplier1UnitCost;
                count++;
            }
            if (Supplier2UnitCost != null)
            {
                unitCost += (decimal)Supplier2UnitCost;
                count++;
            }
            if (Supplier3UnitCost != null)
            {
                unitCost += (decimal)Supplier3UnitCost;
                count++;
            }
            unitCost = unitCost / count;
            return unitCost;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class CatalogueDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDbContext hrdb = new HRISDbContext();
        private SystemBDL systemBDL = new SystemBDL();
        private ProjectPlansDAL projectPlanDAL = new ProjectPlansDAL();

        public List<Supplier> GetSuppliers()
        {
            return db.Suppliers.Where(d => d.PurgeFlag == false).ToList();
        }
        public List<CatalogueVM> GetCatalogue()
        {
            List<CatalogueVM> catalogue = new List<CatalogueVM>();
            var items = db.Items.Select(d => new CatalogueVM
            {
                ItemCode = d.ItemCode,
                ItemName = d.ItemName.ToUpper() + ", " + d.ItemShortSpecifications,
                ItemSpecifications = d.ItemSpecifications,
                ItemImage = d.ItemImage,
                ItemInventoryType = d.FKInventoryTypeReference.InventoryTypeName,
                ItemCategory = d.FKItemCategoryReference.ItemCategoryName,
                ProcurementSource = d.ProcurementSource,
                IndividualUOMReference = d.FKIndividualUnitReference.UnitName,
                PackagingUOMReference = d.FKPackagingUnitReference.UnitName,
                DistributionQtyPerPack = d.DistributionQtyPerPack,
                MinimumIssuanceQty = d.MinimumIssuanceQty
            }).ToList();
            var services = db.Services.Select(d => new CatalogueVM
            {
                ItemCode = d.ServiceCode,
                ItemName = d.ServiceName.ToUpper(),
                ItemSpecifications = d.ItemShortSpecifications,
                ItemImage = null,
                ItemInventoryType = d.FKInventoryTypeReference.InventoryTypeName,
                ItemCategory = d.FKCategoryReference.ItemCategoryName,
                ProcurementSource = ProcurementSources.Non_DBM,
                IndividualUOMReference = null,
                PackagingUOMReference = null,
                DistributionQtyPerPack = null,
                MinimumIssuanceQty = null,
            }).ToList();
            catalogue.AddRange(items);
            catalogue.AddRange(services);
            return catalogue;
        }
        public List<string> GetItemCategories()
        {
            return db.ItemCategories.Select(d => d.ItemCategoryName).ToList();
        }
        public Supplier GetSupplier(int? SupplierID)
        {
            return (SupplierID == null) ? new Supplier() : db.Suppliers.Find(SupplierID);
        }
        public ProjectPlanVM GetProject(string ProjectCode)
        {
            ProjectPlanVM projectPlan = new ProjectPlanVM();

            var projectPlanHeader = db.ProjectPlans.Where(d => d.ProjectCode == ProjectCode).FirstOrDefault();
            var user = db.UserAccounts.Find(projectPlanHeader.PreparedBy);
            var office = hrdb.OfficeModel.Find(projectPlanHeader.Office);
            projectPlan.ProjectCode = projectPlanHeader.ProjectCode;
            projectPlan.ProjectName = projectPlanHeader.ProjectName;
            projectPlan.Description = projectPlanHeader.Description;
            projectPlan.FiscalYear = projectPlanHeader.FiscalYear;
            projectPlan.Office = office.OfficeName;
            projectPlan.PreparedBy = user.FKUserInformationReference.FirstName.ToUpper() + " " + user.FKUserInformationReference.LastName.ToUpper() + ", " + user.FKUserInformationReference.Designation;
            projectPlan.SubmittedBy = office.OfficeHead;
            projectPlan.ProjectMonthStart = systemBDL.GetMonthName(projectPlanHeader.ProjectMonthStart);
            projectPlan.TotalEstimatedBudget = projectPlanHeader.TotalEstimatedBudget;
            projectPlan.NewItemProposals = new List<ProjectPlanItemsVM>();

            return projectPlan;
        }
        public List<CatalogueVM> GetCommonSuppliesCatalogue()
        {
            return db.Items.Where(d => d.FKInventoryTypeReference.InventoryCode == "CUOS")
                   .Select(d => new CatalogueVM
                   {
                       ItemCode = d.ItemCode,
                       ItemName = d.ItemName.ToUpper() + ", " + d.ItemShortSpecifications,
                       ItemSpecifications = d.ItemSpecifications,
                       ItemImage = d.ItemImage,
                       ItemInventoryType = d.FKInventoryTypeReference.InventoryTypeName,
                       ItemCategory = d.FKItemCategoryReference.ItemCategoryName,
                       ProcurementSource = d.ProcurementSource,
                       IndividualUOMReference = d.FKIndividualUnitReference.UnitName,
                       PackagingUOMReference = d.FKPackagingUnitReference.UnitName,
                       DistributionQtyPerPack = d.DistributionQtyPerPack,
                       MinimumIssuanceQty = d.MinimumIssuanceQty
                   }).ToList();
        }
        public decimal GetEstimatedBudget(int TotalQty, decimal UnitCost)
        {
            var estimatedBudget = (TotalQty * UnitCost);
            var inflation = Convert.ToDecimal(db.SystemVariables.Where(d => d.VariableName == "Inflation Rate").FirstOrDefault().Value);
            estimatedBudget = estimatedBudget + (estimatedBudget * (inflation / 100));
            return estimatedBudget;
        }
        public bool SaveBasket(ProjectPlanVM projectPlan, out string Message)
        {
            if (projectPlan == null)
            {
                Message = "An error occurred. Please try again.";
                return false;
            }

            var project = db.ProjectPlans.Where(d => d.ProjectCode == projectPlan.ProjectCode).FirstOrDefault();
            //var projectDetails = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == projectPlan.ProjectCode).ToList();

            List<ProjectPlanItems> projectPlanItems = new List<ProjectPlanItems>();
            List<ProjectPlanServices> projectPlanServices = new List<ProjectPlanServices>();
            if (projectPlan.NewItemProposals != null)
            {
                foreach (var items in projectPlan.NewItemProposals)
                {
                    var IsTangible = db.InventoryTypes.Where(d => d.InventoryTypeName == items.InventoryType).FirstOrDefault().IsTangible;
                    if (IsTangible)
                    {
                        var itemInfo = db.Items.Where(d => d.ItemCode == items.ItemCode).FirstOrDefault();
                        ProjectPlanItems projectItem = new ProjectPlanItems()
                        {
                            ItemReference = itemInfo.ID,
                            ProjectReference = project.ID,
                            ProposalType = BudgetProposalType.NewProposal,
                            JanQty = items.JanQty,
                            FebQty = items.FebQty,
                            MarQty = items.MarQty,
                            AprQty = items.AprQty,
                            MayQty = items.MayQty,
                            JunQty = items.JunQty,
                            JulQty = items.JulQty,
                            AugQty = items.AugQty,
                            SepQty = items.SepQty,
                            OctQty = items.OctQty,
                            NovQty = items.NovQty,
                            DecQty = items.DecQty,
                            TotalQty = items.TotalQty,
                            UnitCost = (decimal)items.UnitCost,
                            Supplier1 = items.Supplier1ID,
                            Supplier1UnitCost = items.Supplier1UnitCost,
                            Supplier2 = items.Supplier2ID,
                            Supplier2UnitCost = items.Supplier2UnitCost,
                            Supplier3 = items.Supplier3ID,
                            Supplier3UnitCost = items.Supplier3UnitCost,
                            EstimatedBudget = items.EstimatedBudget,
                            Remarks = items.Remarks
                        };
                        projectPlanItems.Add(projectItem);
                    }
                    else
                    {
                        var service = db.Services.Where(d => d.ServiceCode == items.ItemCode).FirstOrDefault();
                        ProjectPlanServices projectService = new ProjectPlanServices()
                        {
                            ProjectReference = project.ID,
                            ServiceReference = service.ID,
                            ProposalType = items.ProposalType,
                            ItemSpecifications = items.ItemSpecifications,
                            Quantity = items.TotalQty,
                            UnitCost = (decimal)items.UnitCost,
                            Supplier1 = items.Supplier1ID,
                            Supplier1UnitCost = items.Supplier1UnitCost,
                            Supplier2 = items.Supplier2ID,
                            Supplier2UnitCost = items.Supplier2UnitCost,
                            Supplier3 = items.Supplier3ID,
                            Supplier3UnitCost = items.Supplier3UnitCost,
                            EstimatedBudget = items.EstimatedBudget,
                            Remarks = items.Remarks
                        };
                        projectPlanServices.Add(projectService);
                    }
                }
            }

            if (projectPlanItems.Count > 0)
            {
                db.ProjectPlanItems.AddRange(projectPlanItems);
            }
            if (projectPlanServices.Count > 0)
            {
                db.ProjectPlanServices.AddRange(projectPlanServices);
            }

            if (db.SaveChanges() == 0)
            {
                Message = "An error occurred. Please try again.";
                return false;
            }

            Message = string.Empty;
            return true;
        }
        public CatalogueBasketItemVM GetItem(string ItemCode, string InventoryType)
        {
            var inventoryType = db.InventoryTypes.Where(d => d.InventoryTypeName.ToUpper() == InventoryType.ToUpper()).FirstOrDefault();
            if(inventoryType.IsTangible)
            {
                return db.Items.Where(d => d.ItemCode == ItemCode)
                   .Select(d => new CatalogueBasketItemVM
                   {
                       ItemCode = d.ItemCode,
                       ItemName = d.ItemName.ToUpper() + ", " + d.ItemShortSpecifications,
                       ItemSpecifications = d.ItemSpecifications,
                       ItemImage = d.ItemImage,
                       ItemCategory = d.FKItemCategoryReference.ItemCategoryName,
                       InventoryType = d.FKInventoryTypeReference.InventoryTypeName,
                       ProcurementSource = d.ProcurementSource,
                       IndividualUOMReference = d.FKIndividualUnitReference.UnitName,
                       PackagingUOMReference = d.FKPackagingUnitReference.UnitName,
                       DistributionQtyPerPack = d.DistributionQtyPerPack,
                       MinimumIssuanceQty = d.MinimumIssuanceQty,
                       UnitCost = db.ItemPrices.Where(x => x.Item == d.ID && x.IsPrevailingPrice == true).FirstOrDefault().UnitPrice
                   }).FirstOrDefault();
            }
            if(!inventoryType.IsTangible)
            {
                return db.Services.Where(d => d.ServiceCode == ItemCode)
                   .Select(d => new CatalogueBasketItemVM
                   {
                       ItemCode = d.ServiceCode,
                       ItemName = d.ServiceName.ToUpper() + ", " + d.ItemShortSpecifications,
                       ProcurementSource = d.ProcurementSource,
                       ItemImage = null,
                       ItemCategory = d.FKCategoryReference.ItemCategoryName,
                       InventoryType = d.FKInventoryTypeReference.InventoryTypeName,
                   }).FirstOrDefault();
            }
            return null;
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hrdb.Dispose();
                systemBDL.Dispose();
                projectPlanDAL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}