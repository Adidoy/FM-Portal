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
                return db.Items.Where(d => d.ItemFullName == ItemCode).FirstOrDefault().FKItemTypeReference.FKInventoryTypeReference.IsTangible;
            }
            else
            {
                return db.Items.Where(d => d.ItemCode == ItemCode).FirstOrDefault().FKItemTypeReference.FKInventoryTypeReference.IsTangible;
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
                item.TotalQty = (int)(item.JanQty + item.FebQty + item.MarQty + item.AprQty + item.MayQty + item.JunQty + item.JulQty + item.AugQty + item.SepQty + item.OctQty + item.NovQty + item.DecQty);
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
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();

        public List<Supplier> GetSuppliers()
        {
            return db.Suppliers.Where(d => d.PurgeFlag == false).ToList();
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
            var projectPlanHeader = db.ProjectPlans.Where(d => d.ProjectCode == ProjectCode).FirstOrDefault();
            var user = db.UserAccounts.Where(d => d.EmpCode == projectPlanHeader.PreparedBy).FirstOrDefault();
            var employee = hrisDataAccess.GetEmployee(user.Email);
            var office = hrisDataAccess.GetFullDepartmentDetails(user.DepartmentCode);
            return new ProjectPlanVM {
                ProjectCode = projectPlanHeader.ProjectCode,
                ProjectName = projectPlanHeader.ProjectName,
                Description = projectPlanHeader.Description,
                FiscalYear = projectPlanHeader.FiscalYear,
                SectorCode = office.SectorCode,
                Sector = office.Sector,
                DepartmentCode = office.DepartmentCode,
                Department = office.Department,
                UnitCode = office.SectionCode,
                Unit = office.Section,
                PreparedBy = employee.EmployeeName,
                ProjectMonthStart = systemBDL.GetMonthName(projectPlanHeader.ProjectMonthStart),
                TotalEstimatedBudget = projectPlanHeader.TotalEstimatedBudget,
                NewItemProposals = new List<ProjectPlanItemsVM>() 
            };
        }
        public bool SaveBasket(ProjectPlanVM projectPlan, out string Message)
        {
            if (projectPlan == null)
            {
                Message = "An error occurred. Please try again.";
                return false;
            }

            var project = db.ProjectPlans.Where(d => d.ProjectCode == projectPlan.ProjectCode).FirstOrDefault();

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
                            ProjectJanQty = items.JanQty,
                            ProjectFebQty = items.FebQty,
                            ProjectMarQty = items.MarQty,
                            ProjectAprQty = items.AprQty,
                            ProjectMayQty = items.MayQty,
                            ProjectJunQty = items.JunQty,
                            ProjectJulQty = items.JulQty,
                            ProjectAugQty = items.AugQty,
                            ProjectSepQty = items.SepQty,
                            ProjectOctQty = items.OctQty,
                            ProjectNovQty = items.NovQty,
                            ProjectDecQty = items.DecQty,
                            ProjectTotalQty = items.TotalQty,
                            UnitCost = (decimal)items.UnitCost,
                            Supplier1 = items.Supplier1ID,
                            Supplier1UnitCost = items.Supplier1UnitCost,
                            Supplier2 = items.Supplier2ID,
                            Supplier2UnitCost = items.Supplier2UnitCost,
                            Supplier3 = items.Supplier3ID,
                            Supplier3UnitCost = items.Supplier3UnitCost,
                            ProjectEstimatedBudget = items.EstimatedBudget,
                            Justification = items.Remarks
                        };
                        projectPlanItems.Add(projectItem);
                    }
                    else
                    {
                        var service = db.Items.Where(d => d.ItemCode == items.ItemCode).FirstOrDefault();
                        ProjectPlanServices projectService = new ProjectPlanServices()
                        {
                            ProjectReference = project.ID,
                            ItemReference = service.ID,
                            ProposalType = items.ProposalType,
                            ItemSpecifications = items.ItemSpecifications,
                            ProjectQuantity = items.TotalQty,
                            UnitCost = (decimal)items.UnitCost,
                            Supplier1 = items.Supplier1ID,
                            Supplier1UnitCost = items.Supplier1UnitCost,
                            Supplier2 = items.Supplier2ID,
                            Supplier2UnitCost = items.Supplier2UnitCost,
                            Supplier3 = items.Supplier3ID,
                            Supplier3UnitCost = items.Supplier3UnitCost,
                            ProjectEstimatedBudget = items.EstimatedBudget,
                            Justification = items.Remarks
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

            var switchBoard = db.SwitchBoard.Where(d => d.Reference == project.ProjectCode).FirstOrDefault();
            var employee = hrisDataAccess.GetEmployeeByCode(project.PreparedBy);
            var newProjectSwitchBody = new SwitchBoardBody
            {
                SwitchBoardReference = switchBoard.ID,
                ActionBy = employee.EmployeeCode,
                Remarks = projectPlan.ProjectCode + " has been updated by " + employee.EmployeeName + ", " + employee.Designation + ". (Added Items) (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") + ")",
                DepartmentCode = employee.DepartmentCode,
                UpdatedAt = DateTime.Now
            };
            db.SwitchBoardBody.Add(newProjectSwitchBody);
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
                       ItemName = d.ItemFullName,
                       ItemSpecifications = d.ItemSpecifications,
                       ItemImage = d.ItemImage,
                       InventoryType = d.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                       ItemCategory = d.FKCategoryReference.ItemCategoryName,
                       ProcurementSource = d.ProcurementSource,
                       IndividualUOMReference = d.FKIndividualUnitReference.UnitName,
                       PackagingUOMReference = d.FKPackagingUnitReference.UnitName,
                       DistributionQtyPerPack = (int)d.QuantityPerPackage,
                       MinimumIssuanceQty = (int)d.MinimumIssuanceQty,
                       UnitCost = db.ItemPrices.Where(x => x.Item == d.ID && x.IsPrevailingPrice == true).FirstOrDefault().UnitPrice
                   }).FirstOrDefault();
            }
            if(!inventoryType.IsTangible)
            {
                return db.Items.Where(d => d.ItemCode == ItemCode)
                   .Select(d => new CatalogueBasketItemVM
                   {
                       ItemCode = d.ItemCode,
                       ItemName = d.ItemFullName,
                       ProcurementSource = d.ProcurementSource,
                       ItemImage = null,
                       ItemCategory = d.FKCategoryReference.ItemCategoryName,
                       InventoryType = d.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
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