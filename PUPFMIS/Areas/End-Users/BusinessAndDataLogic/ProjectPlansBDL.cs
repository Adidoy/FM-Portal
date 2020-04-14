using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ProjectPlansDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDbContext hrdb = new HRISDbContext();
        private ProjectProcurementManagementPlanDAL ppmpDAL = new ProjectProcurementManagementPlanDAL();
        private SystemBDL systemBDL = new SystemBDL();

        public SelectList GetMonths()
        {
            return systemBDL.GetMonths();
        }
        public SelectList GetFiscalYears()
        {
            return systemBDL.GetFiscalYears();
        }
        private decimal ComputeUnitCost(decimal? Supplier1UnitCost, decimal? Supplier2UnitCost, decimal? Supplier3UnitCost)
        {
            decimal unitCost = 0.00m;
            int count = 0;
            if (Supplier1UnitCost != null)
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
        public List<ProjectPlanListVM> GetProjects(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrdb.OfficeModel.Find(user.FKUserInformationReference.Office);
            var projects = db.ProjectPlans.Where(d => d.Office == office.ID).ToList();
            List<ProjectPlanListVM> projectPlans = new List<ProjectPlanListVM>();
            
            foreach (var project in projects)
            {
                decimal estimatedBudget = 0.00m;

                if (db.ProjectPlanItems.Where(d => d.ProjectReference == project.ID).Count() != 0)
                {
                    estimatedBudget += (decimal)db.ProjectPlanItems.Where(d => d.ProjectReference == project.ID).Sum(d => d.EstimatedBudget);
                }

                if (db.ProjectPlanServices.Where(d => d.ProjectReference == project.ID).Count() != 0)
                {
                    estimatedBudget += (decimal)db.ProjectPlanServices.Where(d => d.ProjectReference == project.ID).Sum(d => d.EstimatedBudget);
                }

                ProjectPlanListVM plan = new ProjectPlanListVM() {
                    ProjectCode = project.ProjectCode,
                    ProjectName = project.ProjectName,
                    Office = office.OfficeName,
                    ProjectStatus = project.ProjectStatus,
                    EstimatedBudget = estimatedBudget
                };
                projectPlans.Add(plan);
            }
            return projectPlans;
        }
        public ProjectPlanVM GetProjectDetails(string ProjectCode)
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
            projectPlan.ProjectStatus = projectPlanHeader.ProjectStatus;
            projectPlan.ProjectPlanItems = new List<ProjectPlanItemsVM>();
            projectPlan.NewItemProposals = new List<ProjectPlanItemsVM>();

            var obligationItems = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.ProposalType == BudgetProposalType.Actual).ToList();
            foreach (var item in obligationItems)
            {
                ProjectPlanItemsVM items = new ProjectPlanItemsVM()
                {
                    ProjectCode = ProjectCode,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemName.ToUpper() + ", " + item.FKItemReference.ItemShortSpecifications,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    InventoryType = item.FKItemReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKItemCategoryReference.ItemCategoryName,
                    ItemImage = item.FKItemReference.ItemImage,
                    ProcurementSource = item.FKItemReference.ProcurementSource,
                    PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.UnitName,
                    IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.UnitName,
                    JanQty = (int)item.JanQty,
                    FebQty = (int)item.FebQty,
                    MarQty = (int)item.MarQty,
                    AprQty = (int)item.AprQty,
                    MayQty = (int)item.MayQty,
                    JunQty = (int)item.JunQty,
                    JulQty = (int)item.JulQty,
                    AugQty = (int)item.AugQty,
                    SepQty = (int)item.SepQty,
                    OctQty = (int)item.OctQty,
                    NovQty = (int)item.NovQty,
                    DecQty = (int)item.DecQty,
                    TotalQty = (int)item.TotalQty,
                    Supplier1ID = item.Supplier1,
                    Supplier1UnitCost = (decimal)item.Supplier1UnitCost,
                    Supplier2ID = item.Supplier2,
                    Supplier2UnitCost = item.Supplier2UnitCost,
                    Supplier3ID = item.Supplier3,
                    Supplier3UnitCost = item.Supplier3UnitCost,
                    EstimatedBudget = item.EstimatedBudget,
                    Remarks = item.Remarks
                };
                projectPlan.ProjectPlanItems.Add(items);
            }

            var obligationServices = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.ProposalType == BudgetProposalType.Actual).ToList();
            foreach (var item in obligationServices)
            {
                ProjectPlanItemsVM items = new ProjectPlanItemsVM()
                {
                    ProjectCode = ProjectCode,
                    ItemCode = item.FKServiceReference.ServiceCode,
                    ItemName = item.FKServiceReference.ServiceName.ToUpper(),
                    ItemSpecifications = item.FKServiceReference.ItemShortSpecifications,
                    InventoryType = item.FKServiceReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKServiceReference.FKCategoryReference.ItemCategoryName,
                    ItemImage = null,
                    ProcurementSource = ProcurementSources.Non_DBM,
                    PackagingUOMReference = null,
                    IndividualUOMReference = null,
                    TotalQty = item.Quantity,
                    Supplier1ID = item.Supplier1,
                    Supplier1UnitCost = (decimal)item.Supplier1UnitCost,
                    Supplier2ID = item.Supplier2,
                    Supplier2UnitCost = item.Supplier2UnitCost,
                    Supplier3ID = item.Supplier3,
                    Supplier3UnitCost = item.Supplier3UnitCost,
                    EstimatedBudget = item.EstimatedBudget,
                    Remarks = item.Remarks
                };
                projectPlan.ProjectPlanItems.Add(items);
            }

            var newSpendingItems = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.ProposalType == BudgetProposalType.NewProposal).ToList();
            foreach (var item in newSpendingItems)
            {
                ProjectPlanItemsVM items = new ProjectPlanItemsVM()
                {
                    ProjectCode = ProjectCode,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemName.ToUpper() + ", " + item.FKItemReference.ItemShortSpecifications,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    ItemImage = item.FKItemReference.ItemImage,
                    ProcurementSource = item.FKItemReference.ProcurementSource,
                    InventoryType = item.FKItemReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKItemCategoryReference.ItemCategoryName,
                    PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.UnitName,
                    IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.UnitName,
                    JanQty = (item.JanQty == 0) ? null : item.JanQty,
                    FebQty = (item.FebQty == 0) ? null : item.FebQty,
                    MarQty = (item.MarQty == 0) ? null : item.MarQty,
                    AprQty = (item.AprQty == 0) ? null : item.AprQty,
                    MayQty = (item.MayQty == 0) ? null : item.MayQty,
                    JunQty = (item.JunQty == 0) ? null : item.JunQty,
                    JulQty = (item.JulQty == 0) ? null : item.JulQty,
                    AugQty = (item.AugQty == 0) ? null : item.AugQty,
                    SepQty = (item.SepQty == 0) ? null : item.SepQty,
                    OctQty = (item.OctQty == 0) ? null : item.OctQty,
                    NovQty = (item.NovQty == 0) ? null : item.NovQty,
                    DecQty = (item.DecQty == 0) ? null : item.DecQty,
                    TotalQty = item.TotalQty,
                    Supplier1ID = item.Supplier1,
                    Supplier1UnitCost = item.Supplier1UnitCost,
                    Supplier2ID = item.Supplier2,
                    Supplier2UnitCost = item.Supplier2UnitCost,
                    Supplier3ID = item.Supplier3,
                    Supplier3UnitCost = item.Supplier3UnitCost,
                    EstimatedBudget = item.EstimatedBudget,
                    Remarks = item.Remarks
                };
                projectPlan.NewItemProposals.Add(items);
            }

            var newSpendingServices = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.ProposalType == BudgetProposalType.NewProposal).ToList();
            foreach (var item in newSpendingServices)
            {
                ProjectPlanItemsVM items = new ProjectPlanItemsVM()
                {
                    ProjectCode = ProjectCode,
                    ItemCode = item.FKServiceReference.ServiceCode,
                    ItemName = item.FKServiceReference.ServiceName.ToUpper() + ", " + item.FKServiceReference.ItemShortSpecifications,
                    ItemSpecifications = item.ItemSpecifications,
                    ItemImage = null,
                    ProcurementSource = ProcurementSources.Non_DBM,
                    InventoryType = item.FKServiceReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKServiceReference.FKCategoryReference.ItemCategoryName,
                    PackagingUOMReference = null,
                    IndividualUOMReference = null,
                    TotalQty = (int)item.Quantity,
                    Supplier1ID = item.Supplier1,
                    Supplier1UnitCost = (decimal)item.Supplier1UnitCost,
                    Supplier2ID = item.Supplier2,
                    Supplier2UnitCost = item.Supplier2UnitCost,
                    Supplier3ID = item.Supplier3,
                    Supplier3UnitCost = item.Supplier3UnitCost,
                    EstimatedBudget = item.EstimatedBudget,
                    Remarks = item.Remarks
                };
                projectPlan.NewItemProposals.Add(items);
            }

            return projectPlan;
        }
        public List<ActualSuppliesObligation> GetActualObligation(string Office, string FiscalYear)
        {
            var officeInfo = hrdb.OfficeModel.Where(d => d.OfficeName == Office).FirstOrDefault();
            var fiscalYear = Convert.ToInt32(FiscalYear) - 1;
            var actualSuppliesObligation = db.SuppliesIssueDetails.Where(d => d.FKRequestHeader.Office == officeInfo.ID && d.FKRequestHeader.ReleasedAt.Value.Year == fiscalYear)
                                           .Select(d => new
                                           {
                                               ItemCode = d.FKSuppliesMaster.FKItem.ItemCode,
                                               ItemName = d.FKSuppliesMaster.FKItem.ItemName.ToUpper() + ", " + d.FKSuppliesMaster.FKItem.ItemShortSpecifications,
                                               ItemSpecifications = d.FKSuppliesMaster.FKItem.ItemSpecifications,
                                               ItemImage = d.FKSuppliesMaster.FKItem.ItemImage,
                                               ProcurementSource = d.FKSuppliesMaster.FKItem.ProcurementSource,
                                               PackagingUOMReference = d.FKSuppliesMaster.FKItem.FKPackagingUnitReference.UnitName,
                                               IndividualUOMReference = d.FKSuppliesMaster.FKItem.FKIndividualUnitReference.UnitName,
                                               DateReleased = d.FKRequestHeader.ReleasedAt,
                                               QtyIssued = d.QtyIssued
                                           })
                                           .GroupBy(d => new
                                           {
                                               ItemCode = d.ItemCode,
                                               ItemName = d.ItemName,
                                               ItemSpecifications = d.ItemSpecifications,
                                               ItemImage = d.ItemImage,
                                               ProcurementSource = d.ProcurementSource,
                                               PackagingUOMReference = d.PackagingUOMReference,
                                               IndividualUOMReference = d.IndividualUOMReference
                                           })
                                           .Select(d => new ActualSuppliesObligation
                                           {
                                               ItemCode = d.Key.ItemCode,
                                               ItemName = d.Key.ItemName,
                                               ItemSpecifications = d.Key.ItemSpecifications,
                                               ItemImage = d.Key.ItemImage,
                                               ProcurementSource = d.Key.ProcurementSource,
                                               PackagingUOMReference = d.Key.PackagingUOMReference,
                                               IndividualUOMReference = d.Key.IndividualUOMReference,
                                               JANActualObligation = d.Where(x => x.DateReleased.Value.Month == 1).Sum(x => x.QtyIssued),
                                               FEBActualObligation = d.Where(x => x.DateReleased.Value.Month == 2).Sum(x => x.QtyIssued),
                                               MARActualObligation = d.Where(x => x.DateReleased.Value.Month == 3).Sum(x => x.QtyIssued),
                                               APRActualObligation = d.Where(x => x.DateReleased.Value.Month == 4).Sum(x => x.QtyIssued),
                                               MAYActualObligation = d.Where(x => x.DateReleased.Value.Month == 5).Sum(x => x.QtyIssued),
                                               JUNActualObligation = d.Where(x => x.DateReleased.Value.Month == 6).Sum(x => x.QtyIssued),
                                               JULActualObligation = d.Where(x => x.DateReleased.Value.Month == 7).Sum(x => x.QtyIssued),
                                               AUGActualObligation = d.Where(x => x.DateReleased.Value.Month == 8).Sum(x => x.QtyIssued),
                                               SEPActualObligation = d.Where(x => x.DateReleased.Value.Month == 9).Sum(x => x.QtyIssued),
                                               OCTActualObligation = d.Where(x => x.DateReleased.Value.Month == 10).Sum(x => x.QtyIssued),
                                               NOVActualObligation = d.Where(x => x.DateReleased.Value.Month == 11).Sum(x => x.QtyIssued),
                                               DECActualObligation = d.Where(x => x.DateReleased.Value.Month == 12).Sum(x => x.QtyIssued),
                                               ActualObligation = d.Sum(x => x.QtyIssued)
                                           }).ToList();
            return actualSuppliesObligation;
        }
        public ActualSuppliesObligation GetActualObligation(string Office, string FiscalYear, string ItemCode)
        {
            var officeInfo = hrdb.OfficeModel.Where(d => d.OfficeName == Office).FirstOrDefault();
            var fiscalYear = Convert.ToInt32(FiscalYear) - 1;
            var actualSuppliesObligation = db.SuppliesIssueDetails.Where(d => d.FKRequestHeader.Office == officeInfo.ID && d.FKRequestHeader.ReleasedAt.Value.Year == fiscalYear && d.FKSuppliesMaster.FKItem.ItemCode == ItemCode)
                                           .Select(d => new
                                           {
                                               ItemCode = d.FKSuppliesMaster.FKItem.ItemCode,
                                               ItemName = d.FKSuppliesMaster.FKItem.ItemName.ToUpper() + ", " + d.FKSuppliesMaster.FKItem.ItemShortSpecifications,
                                               ItemSpecifications = d.FKSuppliesMaster.FKItem.ItemSpecifications,
                                               ItemImage = d.FKSuppliesMaster.FKItem.ItemImage,
                                               ProcurementSource = d.FKSuppliesMaster.FKItem.ProcurementSource,
                                               PackagingUOMReference = d.FKSuppliesMaster.FKItem.FKPackagingUnitReference.UnitName,
                                               IndividualUOMReference = d.FKSuppliesMaster.FKItem.FKIndividualUnitReference.UnitName,
                                               DateReleased = d.FKRequestHeader.ReleasedAt,
                                               QtyIssued = d.QtyIssued
                                           })
                                           .GroupBy(d => new
                                           {
                                               ItemCode = d.ItemCode,
                                               ItemName = d.ItemName,
                                               ItemSpecifications = d.ItemSpecifications,
                                               ItemImage = d.ItemImage,
                                               ProcurementSource = d.ProcurementSource,
                                               PackagingUOMReference = d.PackagingUOMReference,
                                               IndividualUOMReference = d.IndividualUOMReference
                                           })
                                           .Select(d => new ActualSuppliesObligation
                                           {
                                               ItemCode = d.Key.ItemCode,
                                               ItemName = d.Key.ItemName,
                                               ItemSpecifications = d.Key.ItemSpecifications,
                                               ItemImage = d.Key.ItemImage,
                                               ProcurementSource = d.Key.ProcurementSource,
                                               PackagingUOMReference = d.Key.PackagingUOMReference,
                                               IndividualUOMReference = d.Key.IndividualUOMReference,
                                               JANActualObligation = d.Where(x => x.DateReleased.Value.Month == 1).Sum(x => x.QtyIssued),
                                               FEBActualObligation = d.Where(x => x.DateReleased.Value.Month == 2).Sum(x => x.QtyIssued),
                                               MARActualObligation = d.Where(x => x.DateReleased.Value.Month == 3).Sum(x => x.QtyIssued),
                                               APRActualObligation = d.Where(x => x.DateReleased.Value.Month == 4).Sum(x => x.QtyIssued),
                                               MAYActualObligation = d.Where(x => x.DateReleased.Value.Month == 5).Sum(x => x.QtyIssued),
                                               JUNActualObligation = d.Where(x => x.DateReleased.Value.Month == 6).Sum(x => x.QtyIssued),
                                               JULActualObligation = d.Where(x => x.DateReleased.Value.Month == 7).Sum(x => x.QtyIssued),
                                               AUGActualObligation = d.Where(x => x.DateReleased.Value.Month == 8).Sum(x => x.QtyIssued),
                                               SEPActualObligation = d.Where(x => x.DateReleased.Value.Month == 9).Sum(x => x.QtyIssued),
                                               OCTActualObligation = d.Where(x => x.DateReleased.Value.Month == 10).Sum(x => x.QtyIssued),
                                               NOVActualObligation = d.Where(x => x.DateReleased.Value.Month == 11).Sum(x => x.QtyIssued),
                                               DECActualObligation = d.Where(x => x.DateReleased.Value.Month == 12).Sum(x => x.QtyIssued),
                                               ActualObligation = d.Sum(x => x.QtyIssued)
                                           }).FirstOrDefault();
            return actualSuppliesObligation;
        }
        public bool ValidateProjectPlan(ProjectPlans projectPlan, out string ErrorMessage)
        {
            if(projectPlan.ProjectCode.Substring(0,4) == "CSPR")
            {
                if(db.ProjectPlans.Where(d => d.ProjectCode.Substring(0, 4) == "CSPR" && d.FiscalYear == projectPlan.FiscalYear).Count() == 1)
                {
                    ErrorMessage = "Project already exists. Only one Common-use Office Supplies Project is allowed per Fiscal Year";
                    return false;
                }
                ErrorMessage = string.Empty;
                return true;
            }
            ErrorMessage = string.Empty;
            return true;
        }
        public bool SaveProjectPlan(ProjectPlans projectPlan, string UserEmail, out string ProjectCode)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrdb.OfficeModel.Find(user.FKUserInformationReference.Office);
            projectPlan.Office = office.ID;
            projectPlan.PreparedBy = user.ID;
            projectPlan.SubmittedBy = null;
            projectPlan.ProjectStatus = "New Project";
            projectPlan.ProjectCode = GenerateProjectCode(projectPlan.FiscalYear, office.OfficeName, projectPlan.ProjectCode.Substring(0,4));
            db.ProjectPlans.Add(projectPlan);
            if(db.SaveChanges() == 0)
            {
                ProjectCode = null;
                return false;
            }
            
            if(projectPlan.ProjectCode.Substring(0, 4) == "CUOS")
            {
                var actualObligations = GetActualObligation(office.OfficeName, projectPlan.FiscalYear);
                if (actualObligations.Count == 0)
                {
                    ProjectCode = db.ProjectPlans.Find(projectPlan.ID).ProjectCode;
                    return true;
                }
                var inflation = Convert.ToDecimal(db.SystemVariables.Where(d => d.VariableName == "Inflation Rate").FirstOrDefault().Value);
                List<ProjectPlanItems> projectItems = new List<ProjectPlanItems>();
                foreach (var item in actualObligations)
                {
                    var itemInfo = db.Items.Where(d => d.ItemCode == item.ItemCode).FirstOrDefault();
                    var itemPrice = db.ItemPrices.Where(d => d.FKItemReference.ItemCode == item.ItemCode && d.IsPrevailingPrice == true).FirstOrDefault().UnitPrice;
                    itemPrice = itemPrice + (itemPrice * (inflation / 100));
                    var estimatedBudget = ((int)item.ActualObligation * itemPrice);
                    item.JANActualObligation = (item.JANActualObligation == null) ? 0 : (int)item.JANActualObligation;
                    item.FEBActualObligation = (item.FEBActualObligation == null) ? 0 : (int)item.FEBActualObligation;
                    item.MARActualObligation = (item.MARActualObligation == null) ? 0 : (int)item.MARActualObligation;
                    item.APRActualObligation = (item.APRActualObligation == null) ? 0 : (int)item.APRActualObligation;
                    item.MAYActualObligation = (item.MAYActualObligation == null) ? 0 : (int)item.MAYActualObligation;
                    item.JUNActualObligation = (item.JUNActualObligation == null) ? 0 : (int)item.JUNActualObligation;
                    item.JULActualObligation = (item.JULActualObligation == null) ? 0 : (int)item.JULActualObligation;
                    item.AUGActualObligation = (item.AUGActualObligation == null) ? 0 : (int)item.AUGActualObligation;
                    item.SEPActualObligation = (item.SEPActualObligation == null) ? 0 : (int)item.SEPActualObligation;
                    item.OCTActualObligation = (item.OCTActualObligation == null) ? 0 : (int)item.OCTActualObligation;
                    item.NOVActualObligation = (item.NOVActualObligation == null) ? 0 : (int)item.NOVActualObligation;
                    item.DECActualObligation = (item.DECActualObligation == null) ? 0 : (int)item.DECActualObligation;
                    var TotalQty = item.JANActualObligation + item.FEBActualObligation + item.MARActualObligation + item.APRActualObligation + item.MAYActualObligation + item.JUNActualObligation + item.JULActualObligation + item.AUGActualObligation + item.SEPActualObligation + item.OCTActualObligation + item.NOVActualObligation + item.DECActualObligation;
                    ProjectPlanItems projectItem = new ProjectPlanItems()
                    {
                        ItemReference = itemInfo.ID,
                        ProjectReference = projectPlan.ID,
                        ProposalType = BudgetProposalType.Actual,
                        JanQty = (int)item.JANActualObligation,
                        FebQty = (int)item.FEBActualObligation,
                        MarQty = (int)item.MARActualObligation,
                        AprQty = (int)item.APRActualObligation,
                        MayQty = (int)item.MAYActualObligation,
                        JunQty = (int)item.JUNActualObligation,
                        JulQty = (int)item.JULActualObligation,
                        AugQty = (int)item.AUGActualObligation,
                        SepQty = (int)item.SEPActualObligation,
                        OctQty = (int)item.OCTActualObligation,
                        NovQty = (int)item.NOVActualObligation,
                        DecQty = (int)item.DECActualObligation,
                        UnitCost = itemPrice,
                        TotalQty = (int)TotalQty,
                        Supplier1 = 1,
                        Supplier1UnitCost = itemPrice,
                        EstimatedBudget = estimatedBudget
                    };
                    projectItems.Add(projectItem);
                }

                db.ProjectPlanItems.AddRange(projectItems);
                if (db.SaveChanges() == 0)
                {
                    ProjectCode = null;
                    return false;
                }
            }
            
            ProjectCode = db.ProjectPlans.Find(projectPlan.ID).ProjectCode;
            return true;
        }
        public ProjectPlanItemsVM GetProjectItem(string ProjectCode, string InventoryType, string ItemCode)
        {
            var IsTangible = db.InventoryTypes.Where(d => d.InventoryTypeName == InventoryType).FirstOrDefault().IsTangible;
            var project = db.ProjectPlans.Where(d => d.ProjectCode == ProjectCode).FirstOrDefault();
            ProjectPlanItems projectItem = new ProjectPlanItems();
            ProjectPlanServices projectService = new ProjectPlanServices();
            if(IsTangible)
            {
                projectItem = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.FKItemReference.ItemCode == ItemCode).FirstOrDefault();
                var supplier1 = db.Suppliers.Find(projectItem.Supplier1);
                Supplier supplier2 = (projectItem.Supplier2 == null) ? new Supplier() : db.Suppliers.Find(projectItem.Supplier2);
                Supplier supplier3 = (projectItem.Supplier3 == null) ? new Supplier() : db.Suppliers.Find(projectItem.Supplier3);
                var test = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.FKItemReference.ItemCode == ItemCode);
                return new ProjectPlanItemsVM()
                {
                    ProjectCode = ProjectCode,
                    ItemCode = ItemCode,
                    ProposalType = projectItem.ProposalType,
                    ItemName = projectItem.FKItemReference.ItemName.ToUpper() + ((projectItem.FKItemReference.ItemShortSpecifications == null) ? "" : ", " + projectItem.FKItemReference.ItemShortSpecifications),
                    ItemSpecifications = projectItem.FKItemReference.ItemSpecifications,
                    InventoryType = projectItem.FKItemReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemImage = projectItem.FKItemReference.ItemImage,
                    ItemCategory = projectItem.FKItemReference.FKItemCategoryReference.ItemCategoryName,
                    IndividualUOMReference = projectItem.FKItemReference.FKIndividualUnitReference.UnitName,
                    PackagingUOMReference = projectItem.FKItemReference.FKPackagingUnitReference.UnitName,
                    ProcurementSource = projectItem.FKItemReference.ProcurementSource,
                    MinimumIssuanceQty = projectItem.FKItemReference.MinimumIssuanceQty,
                    DistributionQtyPerPack = projectItem.FKItemReference.DistributionQtyPerPack,
                    JanQty = projectItem.JanQty,
                    FebQty = projectItem.FebQty,
                    MarQty = projectItem.MarQty,
                    AprQty = projectItem.AprQty,
                    MayQty = projectItem.MayQty,
                    JunQty = projectItem.JunQty,
                    JulQty = projectItem.JulQty,
                    AugQty = projectItem.AugQty,
                    SepQty = projectItem.SepQty,
                    OctQty = projectItem.OctQty,
                    NovQty = projectItem.NovQty,
                    DecQty = projectItem.DecQty,
                    UnitCost = projectItem.UnitCost,
                    TotalQty = projectItem.TotalQty,
                    Remarks = projectItem.Remarks,
                    Supplier1ID = supplier1.ID,
                    Supplier1Name = supplier1.SupplierName,
                    Supplier1Address = supplier1.Address,
                    Supplier1ContactNo = supplier1.EmailAddress,
                    Supplier1EmailAddress = supplier1.EmailAddress,
                    Supplier1UnitCost = projectItem.Supplier1UnitCost,
                    Supplier2ID = supplier2.ID,
                    Supplier2Name = supplier2.SupplierName,
                    Supplier2Address = supplier2.Address,
                    Supplier2ContactNo = supplier2.EmailAddress,
                    Supplier2EmailAddress = supplier2.EmailAddress,
                    Supplier2UnitCost = projectItem.Supplier2UnitCost,
                    Supplier3ID = supplier3.ID,
                    Supplier3Name = supplier3.SupplierName,
                    Supplier3Address = supplier3.Address,
                    Supplier3ContactNo = supplier3.EmailAddress,
                    Supplier3EmailAddress = supplier3.EmailAddress,
                    Supplier3UnitCost = projectItem.Supplier3UnitCost
                };
            }
            else
            {
                projectService = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.FKServiceReference.ServiceCode == ItemCode).FirstOrDefault();
                var supplier1 = db.Suppliers.Find(projectService.Supplier1);
                Supplier supplier2 = (projectItem.Supplier2 == null) ? new Supplier() : db.Suppliers.Find(projectService.Supplier2);
                Supplier supplier3 = (projectItem.Supplier3 == null) ? new Supplier() : db.Suppliers.Find(projectService.Supplier3);
                return new ProjectPlanItemsVM()
                {
                    ProjectCode = ProjectCode,
                    ItemCode = ItemCode,
                    ProposalType = projectItem.ProposalType,
                    ItemName = projectService.FKServiceReference.ServiceName.ToUpper() + ((projectService.FKServiceReference.ItemShortSpecifications == null) ? "" : ", " + projectService.FKServiceReference.ItemShortSpecifications),
                    ItemSpecifications = projectService.ItemSpecifications,
                    ItemImage = null,
                    InventoryType = projectService.FKServiceReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = projectService.FKServiceReference.FKCategoryReference.ItemCategoryName,
                    IndividualUOMReference = null,
                    PackagingUOMReference = null,
                    ProcurementSource = projectService.FKServiceReference.ProcurementSource,
                    MinimumIssuanceQty = null,
                    DistributionQtyPerPack = null,
                    UnitCost = projectItem.UnitCost,
                    TotalQty = projectService.Quantity,
                    Remarks = projectService.Remarks,
                    Supplier1ID = supplier1.ID,
                    Supplier1Name = supplier1.SupplierName,
                    Supplier1Address = supplier1.Address,
                    Supplier1ContactNo = supplier1.EmailAddress,
                    Supplier1EmailAddress = supplier1.EmailAddress,
                    Supplier1UnitCost = projectItem.Supplier1UnitCost,
                    Supplier2ID = supplier2.ID,
                    Supplier2Name = supplier2.SupplierName,
                    Supplier2Address = supplier2.Address,
                    Supplier2ContactNo = supplier2.EmailAddress,
                    Supplier2EmailAddress = supplier2.EmailAddress,
                    Supplier2UnitCost = projectItem.Supplier2UnitCost,
                    Supplier3ID = supplier3.ID,
                    Supplier3Name = supplier3.SupplierName,
                    Supplier3Address = supplier3.Address,
                    Supplier3ContactNo = supplier3.EmailAddress,
                    Supplier3EmailAddress = supplier3.EmailAddress,
                    Supplier3UnitCost = projectItem.Supplier3UnitCost
                };
            }
        }
        public ProjectPlanItemsVM GetProjectItem(string ProjectCode, string ItemCode, BudgetProposalType ProposalType)
        {
            var projetItem = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.FKItemReference.ItemCode == ItemCode && d.ProposalType == ProposalType).FirstOrDefault();
            if(projetItem == null)
            {
                return null;
            }
            var officeName = hrdb.OfficeModel.Find(projetItem.FKProjectReference.Office).OfficeName;
            var actualObligation = GetActualObligation(officeName, projetItem.FKProjectReference.FiscalYear, ItemCode);
            
            return new ProjectPlanItemsVM()
            {
                ProjectCode = ProjectCode,
                ItemCode = ItemCode,
                ItemName = projetItem.FKItemReference.ItemName.ToUpper() + ((projetItem.FKItemReference.ItemShortSpecifications == null) ? "" : ", " + projetItem.FKItemReference.ItemShortSpecifications),
                ItemSpecifications = projetItem.FKItemReference.ItemSpecifications,
                ItemImage = projetItem.FKItemReference.ItemImage,
                InventoryType = projetItem.FKItemReference.FKInventoryTypeReference.InventoryTypeName, 
                ItemCategory = projetItem.FKItemReference.FKItemCategoryReference.ItemCategoryName,
                IndividualUOMReference = projetItem.FKItemReference.FKIndividualUnitReference.UnitName,
                PackagingUOMReference = projetItem.FKItemReference.FKPackagingUnitReference.UnitName,
                ProcurementSource = projetItem.FKItemReference.ProcurementSource,
                MinimumIssuanceQty = projetItem.FKItemReference.MinimumIssuanceQty,
                DistributionQtyPerPack = projetItem.FKItemReference.DistributionQtyPerPack,
                ActualObligation = (actualObligation == null) ? null : actualObligation.ActualObligation,
                JanQty = projetItem.JanQty,
                FebQty = projetItem.FebQty,
                MarQty = projetItem.MayQty,
                AprQty = projetItem.AprQty,
                MayQty = projetItem.MayQty,
                JunQty = projetItem.JunQty,
                JulQty = projetItem.JulQty,
                AugQty = projetItem.AugQty,
                SepQty = projetItem.SepQty,
                OctQty = projetItem.OctQty,
                NovQty = projetItem.NovQty,
                DecQty = projetItem.DecQty,
                TotalQty = projetItem.TotalQty,
                Remarks = projetItem.Remarks,
            };
        }
        private string GenerateProjectCode(string FiscalYear, string OfficeName, string Type)
        {
            var office = hrdb.OfficeModel.Where(d => d.OfficeName == OfficeName).FirstOrDefault();
            var series = db.ProjectPlans.Where(d => d.Office == office.ID && d.FiscalYear == FiscalYear && d.ProjectCode.Substring(0, 4) == Type).Count() + 1;
            var seriesStr = (series.ToString().Length == 1) ? "00" + series.ToString() : (series.ToString().Length == 2) ? "0" + series.ToString() : series.ToString();
            return Type + "-" + office.OfficeCode + "-" + seriesStr + "-" + FiscalYear;
        }
        public bool ValidateUpdateItem(ProjectPlanItemsVM item, string ProjectCode, out string Message)
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
        public bool UpdateItem(ProjectPlanItemsVM Item, out string Message)
        {
            if (Item.ProjectCode.Substring(0, 4) == "CSPR")
            {
                Item.JanQty = (Item.JanQty == null) ? 0 : Item.JanQty;
                Item.FebQty = (Item.FebQty == null) ? 0 : Item.FebQty;
                Item.MarQty = (Item.MarQty == null) ? 0 : Item.MarQty;
                Item.AprQty = (Item.AprQty == null) ? 0 : Item.AprQty;
                Item.MayQty = (Item.MayQty == null) ? 0 : Item.MayQty;
                Item.JunQty = (Item.JunQty == null) ? 0 : Item.JunQty;
                Item.JulQty = (Item.JulQty == null) ? 0 : Item.JulQty;
                Item.AugQty = (Item.AugQty == null) ? 0 : Item.AugQty;
                Item.SepQty = (Item.SepQty == null) ? 0 : Item.SepQty;
                Item.OctQty = (Item.OctQty == null) ? 0 : Item.OctQty;
                Item.NovQty = (Item.NovQty == null) ? 0 : Item.NovQty;
                Item.DecQty = (Item.DecQty == null) ? 0 : Item.DecQty;
                Item.TotalQty = (int)(Item.JanQty + Item.FebQty + Item.MarQty + Item.AprQty + Item.MayQty + Item.JunQty + Item.JulQty + Item.AugQty + Item.OctQty + Item.NovQty + Item.DecQty);
            }
            else
            {
                var StartMonth = db.ProjectPlans.Where(d => d.ProjectCode == Item.ProjectCode).FirstOrDefault().ProjectMonthStart;
                switch (StartMonth)
                {
                    case 1:
                        {
                            Item.JanQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                    case 2:
                        {
                            Item.FebQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                    case 3:
                        {
                            Item.MarQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                    case 4:
                        {
                            Item.AprQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                    case 5:
                        {
                            Item.MayQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                    case 6:
                        {
                            Item.JunQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                    case 7:
                        {
                            Item.JulQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                    case 8:
                        {
                            Item.AugQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                    case 9:
                        {
                            Item.SepQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                    case 10:
                        {
                            Item.OctQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                    case 11:
                        {
                            Item.NovQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                    case 12:
                        {
                            Item.DecQty = Item.TotalQty;
                            Item.TotalQty = Item.TotalQty = Item.TotalQty;
                        }
                        break;
                }
            }

            var projectItem = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == Item.ProjectCode && d.FKItemReference.ItemCode == Item.ItemCode).FirstOrDefault();

            if (Item.ProcurementSource == ProcurementSources.PS_DBM && Item.InventoryType == "Common Use Office Supplies")
            {
                Item.Supplier1ID = 1;
                Item.Supplier1UnitCost = (decimal)Item.UnitCost;
                Item.UnitCost = (decimal)Item.UnitCost;
                Item.EstimatedBudget = (decimal)Item.UnitCost * Item.TotalQty;
            }
            else
            {
                Item.UnitCost = ComputeUnitCost(Item.Supplier1UnitCost, Item.Supplier2UnitCost, Item.Supplier3UnitCost);
                Item.EstimatedBudget = (decimal)Item.UnitCost * Item.TotalQty;
            }

            projectItem.ItemReference = db.Items.Where(d => d.ItemCode == Item.ItemCode).FirstOrDefault().ID;
            projectItem.ProjectReference = db.ProjectPlans.Where(d => d.ProjectCode == Item.ProjectCode).FirstOrDefault().ID;
            projectItem.ProposalType = Item.ProposalType;
            projectItem.JanQty = Item.JanQty;
            projectItem.FebQty = Item.FebQty;
            projectItem.MarQty = Item.MarQty;
            projectItem.AprQty = Item.AprQty;
            projectItem.MayQty = Item.MayQty;
            projectItem.JunQty = Item.JunQty;
            projectItem.JulQty = Item.JulQty;
            projectItem.AugQty = Item.AugQty;
            projectItem.SepQty = Item.SepQty;
            projectItem.OctQty = Item.OctQty;
            projectItem.NovQty = Item.NovQty;
            projectItem.DecQty = Item.DecQty;
            projectItem.TotalQty = Item.TotalQty;
            projectItem.UnitCost = (decimal)Item.UnitCost;
            projectItem.Supplier1 = Item.Supplier1ID;
            projectItem.Supplier1UnitCost = Item.Supplier1UnitCost;
            projectItem.Supplier2 = Item.Supplier2ID;
            projectItem.Supplier2UnitCost = Item.Supplier2UnitCost;
            projectItem.Supplier3 = Item.Supplier3ID;
            projectItem.Supplier3UnitCost = Item.Supplier3UnitCost;
            projectItem.EstimatedBudget = Item.EstimatedBudget;
            projectItem.Remarks = Item.Remarks;

            db.SaveChanges();

            Message = string.Empty;
            return true;
        }
        public bool RemoveItem(CatalogueBasketItemVM item, out string Message)
        {
            var projectItem = db.ProjectPlanItems.Where(d => d.FKItemReference.ItemCode == item.ItemCode && d.FKProjectReference.ProjectCode == item.ProjectCode).FirstOrDefault();
            db.ProjectPlanItems.Remove(projectItem);
            if(db.SaveChanges() == 0)
            {
                Message = "An error occurred. The item was not removed from the project. Please try again.";
                return false;
            }
            Message = string.Empty;
            return true;
        }
        public bool PostToPPMP(ProjectPlanVM ProjectPlan, string UserEmail)
        {
            if(ppmpDAL.PostToPPMP(ProjectPlan, UserEmail))
            {
                var project = db.ProjectPlans.Where(d => d.ProjectCode == ProjectPlan.ProjectCode).FirstOrDefault();
                project.ProjectStatus = "Posted to PPMP";
                db.SaveChanges();
                return true;
            }
            return false;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hrdb.Dispose();
                ppmpDAL.Dispose();
                systemBDL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}