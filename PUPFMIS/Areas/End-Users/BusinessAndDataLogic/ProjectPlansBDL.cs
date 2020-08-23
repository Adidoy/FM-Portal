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
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();

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
        public List<ProjectPlanListVM> GetProjects(string UserEmail, int FiscalYear)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrisDataAccess.GetDepartmentDetails(user.DepartmentCode);
            var projects = db.ProjectPlans.Where(d => d.Department == office.DepartmentCode && d.FiscalYear == FiscalYear).ToList();
            List<ProjectPlanListVM> projectPlans = new List<ProjectPlanListVM>();
            
            foreach (var project in projects)
            {
                decimal estimatedBudget = 0.00m;

                if (db.ProjectPlanItems.Where(d => d.ProjectReference == project.ID).Count() != 0)
                {
                    estimatedBudget += (decimal)db.ProjectPlanItems.Where(d => d.ProjectReference == project.ID).Sum(d => d.ProjectEstimatedBudget);
                }

                if (db.ProjectPlanServices.Where(d => d.ProjectReference == project.ID).Count() != 0)
                {
                    estimatedBudget += (decimal)db.ProjectPlanServices.Where(d => d.ProjectReference == project.ID).Sum(d => d.ProjectEstimatedBudget);
                }

                ProjectPlanListVM plan = new ProjectPlanListVM() {
                    ProjectCode = project.ProjectCode,
                    ProjectName = project.ProjectName,
                    Office = hrisDataAccess.GetDepartmentDetails(project.Unit).Department,
                    ProjectStatus = project.ProjectStatus,
                    EstimatedBudget = estimatedBudget
                };
                projectPlans.Add(plan);
            }
            return projectPlans;
        }
        public ProjectPlanVM GetProjectDetails(string ProjectCode, string UserEmail)
        {
            var projectPlanHeader = db.ProjectPlans.Where(d => d.ProjectCode == ProjectCode).FirstOrDefault();
            var currentUser = hrisDataAccess.GetEmployee(UserEmail);
            var employee = hrisDataAccess.GetEmployee(UserEmail);
            var office = hrisDataAccess.GetFullDepartmentDetails(projectPlanHeader.Unit == null ? projectPlanHeader.Department : projectPlanHeader.Unit);

            ProjectPlanVM projectPlan = new ProjectPlanVM
            {
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
                PreparedByEmpCode = employee.EmployeeCode,
                SubmittedBy = office.DepartmentHead,
                ProjectMonthStart = systemBDL.GetMonthName(projectPlanHeader.ProjectMonthStart),
                TotalEstimatedBudget = projectPlanHeader.TotalEstimatedBudget,
                ProjectStatus = projectPlanHeader.ProjectStatus,
                NewItemProposals =  new List<ProjectPlanItemsVM>(),
                ProjectPlanItems = new List<ProjectPlanItemsVM>()
            };

            var obligationItems = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.ProposalType == BudgetProposalType.Actual).ToList();
            foreach (var item in obligationItems)
            {
                ProjectPlanItemsVM items = new ProjectPlanItemsVM()
                {
                    ProjectCode = ProjectCode,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    ItemImage = item.FKItemReference.ItemImage,
                    ProcurementSource = item.FKItemReference.ProcurementSource,
                    PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.UnitName,
                    IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.UnitName,
                    JanQty = (int)item.ProjectJanQty,
                    FebQty = (int)item.ProjectFebQty,
                    MarQty = (int)item.ProjectMarQty,
                    AprQty = (int)item.ProjectAprQty,
                    MayQty = (int)item.ProjectMayQty,
                    JunQty = (int)item.ProjectJunQty,
                    JulQty = (int)item.ProjectJulQty,
                    AugQty = (int)item.ProjectAugQty,
                    SepQty = (int)item.ProjectSepQty,
                    OctQty = (int)item.ProjectOctQty,
                    NovQty = (int)item.ProjectNovQty,
                    DecQty = (int)item.ProjectDecQty,
                    TotalQty = (int)item.ProjectTotalQty,
                    UnitCost = item.UnitCost,
                    Supplier1ID = item.Supplier1,
                    Supplier1UnitCost = (decimal)item.Supplier1UnitCost,
                    Supplier2ID = item.Supplier2,
                    Supplier2UnitCost = item.Supplier2UnitCost,
                    Supplier3ID = item.Supplier3,
                    Supplier3UnitCost = item.Supplier3UnitCost,
                    EstimatedBudget = item.ProjectEstimatedBudget,
                    Remarks = item.Justification
                };
                projectPlan.ProjectPlanItems.Add(items);
            }

            var obligationServices = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.ProposalType == BudgetProposalType.Actual).ToList();
            foreach (var item in obligationServices)
            {
                ProjectPlanItemsVM items = new ProjectPlanItemsVM()
                {
                    ProjectCode = ProjectCode,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemShortSpecifications,
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    ItemImage = null,
                    ProcurementSource = ProcurementSources.Non_DBM,
                    PackagingUOMReference = null,
                    IndividualUOMReference = null,
                    TotalQty = item.ProjectQuantity,
                    UnitCost = item.UnitCost,
                    Supplier1ID = item.Supplier1,
                    Supplier1UnitCost = (decimal)item.Supplier1UnitCost,
                    Supplier2ID = item.Supplier2,
                    Supplier2UnitCost = item.Supplier2UnitCost,
                    Supplier3ID = item.Supplier3,
                    Supplier3UnitCost = item.Supplier3UnitCost,
                    EstimatedBudget = item.ProjectEstimatedBudget,
                    Remarks = item.Justification
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
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.FKItemReference.ItemSpecifications,
                    ItemImage = item.FKItemReference.ItemImage,
                    ProcurementSource = item.FKItemReference.ProcurementSource,
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.UnitName,
                    IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.UnitName,
                    JanQty = (item.ProjectJanQty == 0) ? null : item.ProjectJanQty,
                    FebQty = (item.ProjectFebQty == 0) ? null : item.ProjectFebQty,
                    MarQty = (item.ProjectMarQty == 0) ? null : item.ProjectMarQty,
                    AprQty = (item.ProjectAprQty == 0) ? null : item.ProjectAprQty,
                    MayQty = (item.ProjectMayQty == 0) ? null : item.ProjectMayQty,
                    JunQty = (item.ProjectJunQty == 0) ? null : item.ProjectJunQty,
                    JulQty = (item.ProjectJulQty == 0) ? null : item.ProjectJulQty,
                    AugQty = (item.ProjectAugQty == 0) ? null : item.ProjectAugQty,
                    SepQty = (item.ProjectSepQty == 0) ? null : item.ProjectSepQty,
                    OctQty = (item.ProjectOctQty == 0) ? null : item.ProjectOctQty,
                    NovQty = (item.ProjectNovQty == 0) ? null : item.ProjectNovQty,
                    DecQty = (item.ProjectDecQty == 0) ? null : item.ProjectDecQty,
                    TotalQty = item.ProjectTotalQty,
                    UnitCost = item.UnitCost,
                    Supplier1ID = item.Supplier1,
                    Supplier1UnitCost = item.Supplier1UnitCost,
                    Supplier2ID = item.Supplier2,
                    Supplier2UnitCost = item.Supplier2UnitCost,
                    Supplier3ID = item.Supplier3,
                    Supplier3UnitCost = item.Supplier3UnitCost,
                    EstimatedBudget = item.ProjectEstimatedBudget,
                    Remarks = item.Justification
                };
                projectPlan.NewItemProposals.Add(items);
            }

            var newSpendingServices = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.ProposalType == BudgetProposalType.NewProposal).ToList();
            foreach (var item in newSpendingServices)
            {
                ProjectPlanItemsVM items = new ProjectPlanItemsVM()
                {
                    ProjectCode = ProjectCode,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.ItemSpecifications,
                    ItemImage = null,
                    ProcurementSource = ProcurementSources.Non_DBM,
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    PackagingUOMReference = null,
                    IndividualUOMReference = null,
                    TotalQty = (int)item.ProjectQuantity,
                    UnitCost = item.UnitCost,
                    Supplier1ID = item.Supplier1,
                    Supplier1UnitCost = (decimal)item.Supplier1UnitCost,
                    Supplier2ID = item.Supplier2,
                    Supplier2UnitCost = item.Supplier2UnitCost,
                    Supplier3ID = item.Supplier3,
                    Supplier3UnitCost = item.Supplier3UnitCost,
                    EstimatedBudget = item.ProjectEstimatedBudget,
                    Remarks = item.Justification
                };
                projectPlan.NewItemProposals.Add(items);
            }

            return projectPlan;
        }
        public List<ActualSuppliesObligation> GetActualObligation(string OfficeCode, int FiscalYear)
        {
            var officeInfo = hrdb.HRISDepartments.Where(d => d.DepartmentCode == OfficeCode).FirstOrDefault();
            var fiscalYear = db.ProjectPlans.OrderByDescending(d => d.FiscalYear).Select(d => d.FiscalYear).First() - 1;
            var actualSuppliesObligation = db.SuppliesIssueDetails.Where(d => d.FKRequestHeader.Office == officeInfo.DepartmentID && d.FKRequestHeader.ReleasedAt.Value.Year == fiscalYear)
                                           .Select(d => new
                                           {
                                               ItemID = d.FKSuppliesMaster.FKItem.ID,
                                               ItemCode = d.FKSuppliesMaster.FKItem.ItemCode,
                                               ItemName = d.FKSuppliesMaster.FKItem.FKItemTypeReference.ItemTypeName.ToUpper(),
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
                                               ItemID = d.ItemID,
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
                                               ItemID = d.Key.ItemID,
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
                                               ActualObligation = d.Sum(x => x.QtyIssued),
                                               UnitCost = db.ItemPrices.Where(x => x.FKItemReference.ItemCode == d.Key.ItemCode && x.IsPrevailingPrice == true).FirstOrDefault().UnitPrice
                                           }).ToList();
            return actualSuppliesObligation;
        }
        public ActualSuppliesObligation GetActualObligation(string Office, int FiscalYear, string ItemCode)
        {
            var officeInfo = hrdb.OfficeModel.Where(d => d.OfficeName == Office).FirstOrDefault();
            var fiscalYear = Convert.ToInt32(FiscalYear) - 1;
            var actualSuppliesObligation = db.SuppliesIssueDetails.Where(d => d.FKRequestHeader.Office == officeInfo.ID && d.FKRequestHeader.ReleasedAt.Value.Year == fiscalYear && d.FKSuppliesMaster.FKItem.ItemCode == ItemCode)
                                           .Select(d => new
                                           {
                                               ItemID = d.FKSuppliesMaster.FKItem.ID,
                                               ItemCode = d.FKSuppliesMaster.FKItem.ItemCode,
                                               ItemName = d.FKSuppliesMaster.FKItem.FKItemTypeReference.ItemTypeName.ToUpper(),
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
                                               ItemID = d.ItemID,
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
                                               ItemID = d.Key.ItemID,
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
                                               ActualObligation = d.Sum(x => x.QtyIssued),
                                               UnitCost = db.ItemPrices.Where(x => x.FKItemReference.ItemCode == d.Key.ItemCode && x.IsPrevailingPrice == true).FirstOrDefault().UnitPrice
                                           }).FirstOrDefault();
            return actualSuppliesObligation;
        }
        public bool ValidateProjectPlan(ProjectPlans projectPlan, string UserEmail, out string ErrorMessage)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrisDataAccess.GetDepartmentDetails(projectPlan.Unit);
            if (projectPlan.ProjectCode.Substring(0,4) == "CSPR")
            {
                //var prefix = "CSPR-" + (office.DepartmentCode.Contains("-") ? office.DepartmentCode.Replace("-", "") : office.DepartmentCode) + "-";
                if(db.ProjectPlans.Where(d => d.Unit == office.DepartmentCode && d.FiscalYear == projectPlan.FiscalYear).Count() >= 1)
                {
                    ErrorMessage = "Common-use Supplies Project for " + office.Department.ToUpper() +" already exists. Only one Common-use Office Supplies Project is allowed per Fiscal Year";
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
            var office = hrisDataAccess.GetFullDepartmentDetails(projectPlan.Unit);
            var employee = hrisDataAccess.GetEmployee(user.Email);
            projectPlan.Department = office.DepartmentCode;
            projectPlan.PreparedBy = user.EmpCode;
            projectPlan.SubmittedBy = office.DepartmentHead;
            projectPlan.ProjectStatus = "New Project";
            projectPlan.ProjectCode = GenerateProjectCode(projectPlan.FiscalYear, projectPlan.Unit, projectPlan.ProjectCode.Substring(0,4));
            db.ProjectPlans.Add(projectPlan);
            if(db.SaveChanges() == 0)
            {
                ProjectCode = null;
                return false;
            }

            var newProjectSwitch = new SwitchBoard
            {
                DepartmentCode = projectPlan.Department,
                MessageType = "Project Plan",
                Reference = projectPlan.ProjectCode,
                Subject = projectPlan.ProjectCode + " - " + projectPlan.ProjectName
            };

            db.SwitchBoard.Add(newProjectSwitch);
            if(db.SaveChanges() == 0)
            {
                ProjectCode = null;
                return false;
            }

            var newProjectSwitchBody = new SwitchBoardBody
            {
                SwitchBoardReference = newProjectSwitch.ID,
                ActionBy = employee.EmployeeCode,
                Remarks = projectPlan.ProjectCode + " - " + projectPlan.ProjectName + " has been created by "+ employee.EmployeeName + ", " + employee.Designation +". (" + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt") +")",
                DepartmentCode = employee.DepartmentCode,
                UpdatedAt = DateTime.Now
            };

            db.SwitchBoardBody.Add(newProjectSwitchBody);
            if (db.SaveChanges() == 0)
            {
                ProjectCode = null;
                return false;
            }

            if (projectPlan.ProjectCode.Substring(0, 4) == "CSPR")
            {
                var actualObligations = GetActualObligation(office.DepartmentCode, projectPlan.FiscalYear);
                if (actualObligations.Count == 0)
                {
                    ProjectCode = db.ProjectPlans.Find(projectPlan.ID).ProjectCode;
                    return true;
                }
                List<ProjectPlanItems> projectItems = new List<ProjectPlanItems>();
                foreach (var item in actualObligations)
                {
                    projectItems.Add(new ProjectPlanItems {
                        ProjectReference = projectPlan.ID,
                        ItemReference = item.ItemID,
                        ProposalType = BudgetProposalType.Actual,
                        ProjectJanQty = item.JANActualObligation == null ? 0 : item.JANActualObligation,
                        ProjectFebQty = item.FEBActualObligation == null ? 0 : item.FEBActualObligation,
                        ProjectMarQty = item.MARActualObligation == null ? 0 : item.MARActualObligation,
                        ProjectAprQty = item.APRActualObligation == null ? 0 : item.APRActualObligation,
                        ProjectMayQty = item.MAYActualObligation == null ? 0 : item.MAYActualObligation,
                        ProjectJunQty = item.JUNActualObligation == null ? 0 : item.JUNActualObligation,
                        ProjectJulQty = item.JULActualObligation == null ? 0 : item.JULActualObligation,
                        ProjectAugQty = item.AUGActualObligation == null ? 0 : item.AUGActualObligation,
                        ProjectSepQty = item.SEPActualObligation == null ? 0 : item.SEPActualObligation,
                        ProjectOctQty = item.OCTActualObligation == null ? 0 : item.OCTActualObligation,
                        ProjectNovQty = item.NOVActualObligation == null ? 0 : item.NOVActualObligation,
                        ProjectDecQty = item.DECActualObligation == null ? 0 : item.DECActualObligation,
                        ProjectTotalQty = (int)item.ActualObligation,
                        ProjectEstimatedBudget = item.UnitCost * (int)item.ActualObligation,
                        UnitCost = item.UnitCost,
                        Justification = "Actual Obligation",
                        Supplier1 = db.Supplies.Find(1).ID,
                        Supplier1UnitCost = item.UnitCost
                    });
                }

                db.ProjectPlanItems.AddRange(projectItems);
                if (db.SaveChanges() == 0)
                {
                    ProjectCode = null;
                    return false;
                }

                newProjectSwitchBody = new SwitchBoardBody
                {
                    SwitchBoardReference = newProjectSwitch.ID,
                    ActionBy = "SYSTEM",
                    Remarks = "Items based on Actual Obligation from the previous fiscal year are added.",
                    DepartmentCode = employee.DepartmentCode,
                    UpdatedAt = DateTime.Now
                };

                db.SwitchBoardBody.Add(newProjectSwitchBody);
                if (db.SaveChanges() == 0)
                {
                    ProjectCode = null;
                    return false;
                }
            }
            
            ProjectCode = db.ProjectPlans.Find(projectPlan.ID).ProjectCode;
            return true;
        }
        public bool IsItemTangible(string ItemCode)
        {
            var item = db.Items.Where(d => d.ItemCode == ItemCode).FirstOrDefault();
            if (item == null)
            {
                return db.Items.Where(d => d.ItemFullName == ItemCode).FirstOrDefault().FKItemTypeReference.FKInventoryTypeReference.IsTangible;
            }
            else
            {
                return db.Items.Where(d => d.ItemCode == ItemCode).FirstOrDefault().FKItemTypeReference.FKInventoryTypeReference.IsTangible;
            }
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
                    ItemName = projectItem.FKItemReference.ItemFullName,
                    ItemSpecifications = projectItem.FKItemReference.ItemSpecifications,
                    ItemCategory = projectItem.FKItemReference.FKCategoryReference.ItemCategoryName,
                    InventoryType = projectItem.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemImage = projectItem.FKItemReference.ItemImage,
                    IndividualUOMReference = projectItem.FKItemReference.FKIndividualUnitReference.UnitName,
                    PackagingUOMReference = projectItem.FKItemReference.FKPackagingUnitReference.UnitName,
                    ProcurementSource = projectItem.FKItemReference.ProcurementSource,
                    MinimumIssuanceQty = projectItem.FKItemReference.MinimumIssuanceQty,
                    DistributionQtyPerPack = projectItem.FKItemReference.QuantityPerPackage,
                    JanQty = projectItem.ProjectJanQty,
                    FebQty = projectItem.ProjectFebQty,
                    MarQty = projectItem.ProjectMarQty,
                    AprQty = projectItem.ProjectAprQty,
                    MayQty = projectItem.ProjectMayQty,
                    JunQty = projectItem.ProjectJunQty,
                    JulQty = projectItem.ProjectJulQty,
                    AugQty = projectItem.ProjectAugQty,
                    SepQty = projectItem.ProjectSepQty,
                    OctQty = projectItem.ProjectOctQty,
                    NovQty = projectItem.ProjectNovQty,
                    DecQty = projectItem.ProjectDecQty,
                    UnitCost = projectItem.UnitCost,
                    TotalQty = projectItem.ProjectTotalQty,
                    Remarks = projectItem.Justification,
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
                projectService = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == ProjectCode && d.FKItemReference.ItemCode == ItemCode).FirstOrDefault();
                var supplier1 = db.Suppliers.Find(projectService.Supplier1);
                Supplier supplier2 = (projectService.Supplier2 == null) ? new Supplier() : db.Suppliers.Find(projectService.Supplier2);
                Supplier supplier3 = (projectService.Supplier3 == null) ? new Supplier() : db.Suppliers.Find(projectService.Supplier3);
                return new ProjectPlanItemsVM()
                {
                    ProjectCode = ProjectCode,
                    ItemCode = ItemCode,
                    ProposalType = projectService.ProposalType,
                    ItemName = projectService.FKItemReference.ItemFullName,
                    ItemSpecifications = projectService.ItemSpecifications,
                    ItemImage = null,
                    InventoryType = projectService.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = projectService.FKItemReference.FKCategoryReference.ItemCategoryName,
                    IndividualUOMReference = null,
                    PackagingUOMReference = null,
                    ProcurementSource = projectService.FKItemReference.ProcurementSource,
                    MinimumIssuanceQty = null,
                    DistributionQtyPerPack = null,
                    UnitCost = projectService.UnitCost,
                    TotalQty = projectService.ProjectQuantity,
                    Remarks = projectService.Justification,
                    Supplier1ID = supplier1.ID,
                    Supplier1Name = supplier1.SupplierName,
                    Supplier1Address = supplier1.Address,
                    Supplier1ContactNo = supplier1.ContactNumber,
                    Supplier1EmailAddress = supplier1.EmailAddress == null ? "Not Provided" : supplier1.EmailAddress,
                    Supplier1UnitCost = (decimal)projectService.Supplier1UnitCost,
                    Supplier2ID = supplier2.ID,
                    Supplier2Name = supplier2.SupplierName,
                    Supplier2Address = supplier2.Address,
                    Supplier2ContactNo = supplier2.ContactNumber,
                    Supplier2EmailAddress = supplier2.EmailAddress == null ? "Not Provided" : supplier2.EmailAddress,
                    Supplier2UnitCost = projectService.Supplier2UnitCost,
                    Supplier3ID = supplier3.ID,
                    Supplier3Name = supplier3.SupplierName,
                    Supplier3Address = supplier3.Address,
                    Supplier3ContactNo = supplier3.ContactNumber,
                    Supplier3EmailAddress = supplier3.EmailAddress == null ? "Not Provided" : supplier3.EmailAddress,
                    Supplier3UnitCost = projectService.Supplier3UnitCost
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
            var officeName = hrisDataAccess.GetFullDepartmentDetails(projetItem.FKProjectReference.Department).Department;
            var actualObligation = GetActualObligation(officeName, projetItem.FKProjectReference.FiscalYear, ItemCode);
            
            return new ProjectPlanItemsVM()
            {
                ProjectCode = ProjectCode,
                ItemCode = ItemCode,
                //ItemName = projetItem.FKItemReference.FKItemTypeReference.ItemTypeName.ToUpper() + ((projetItem.FKItemReference.ItemShortSpecifications == null) ? "" : ", " + projetItem.FKItemReference.ItemShortSpecifications),
                ItemSpecifications = projetItem.FKItemReference.ItemSpecifications,
                ItemImage = projetItem.FKItemReference.ItemImage,
                InventoryType = projetItem.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                IndividualUOMReference = projetItem.FKItemReference.FKIndividualUnitReference.UnitName,
                PackagingUOMReference = projetItem.FKItemReference.FKPackagingUnitReference.UnitName,
                ProcurementSource = projetItem.FKItemReference.ProcurementSource,
                MinimumIssuanceQty = projetItem.FKItemReference.MinimumIssuanceQty,
                DistributionQtyPerPack = projetItem.FKItemReference.QuantityPerPackage,
                ActualObligation = (actualObligation == null) ? null : actualObligation.ActualObligation,
                JanQty = projetItem.ProjectJanQty,
                FebQty = projetItem.ProjectFebQty,
                MarQty = projetItem.ProjectMayQty,
                AprQty = projetItem.ProjectAprQty,
                MayQty = projetItem.ProjectMayQty,
                JunQty = projetItem.ProjectJunQty,
                JulQty = projetItem.ProjectJulQty,
                AugQty = projetItem.ProjectAugQty,
                SepQty = projetItem.ProjectSepQty,
                OctQty = projetItem.ProjectOctQty,
                NovQty = projetItem.ProjectNovQty,
                DecQty = projetItem.ProjectDecQty,
                TotalQty = projetItem.ProjectTotalQty,
                Remarks = projetItem.Justification,
            };
        }
        private string GenerateProjectCode(int FiscalYear, string DepartmentCode, string Type)
        {
            var prefix = Type + "-" + (DepartmentCode.Contains("-") ? DepartmentCode.Replace("-", "").ToString().ToUpper() : DepartmentCode.ToUpper());
            var series = db.ProjectPlans.Where(d => d.ProjectCode.StartsWith(prefix) && d.FiscalYear == FiscalYear).Count() + 1;
            var seriesStr = (series.ToString().Length == 1) ? "00" + series.ToString() : (series.ToString().Length == 2) ? "0" + series.ToString() : series.ToString();
            return prefix + "-" + seriesStr + "-" + FiscalYear.ToString();
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
            var projectService = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == Item.ProjectCode && d.FKItemReference.ItemCode == Item.ItemCode).FirstOrDefault();

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
            if(projectItem != null)
            {
                projectItem.ItemReference = db.Items.Where(d => d.ItemCode == Item.ItemCode).FirstOrDefault().ID;
                projectItem.ProjectReference = db.ProjectPlans.Where(d => d.ProjectCode == Item.ProjectCode).FirstOrDefault().ID;
                projectItem.ProposalType = Item.ProposalType;
                projectItem.ProjectJanQty = Item.JanQty;
                projectItem.ProjectFebQty = Item.FebQty;
                projectItem.ProjectMarQty = Item.MarQty;
                projectItem.ProjectAprQty = Item.AprQty;
                projectItem.ProjectMayQty = Item.MayQty;
                projectItem.ProjectJunQty = Item.JunQty;
                projectItem.ProjectJulQty = Item.JulQty;
                projectItem.ProjectAugQty = Item.AugQty;
                projectItem.ProjectSepQty = Item.SepQty;
                projectItem.ProjectOctQty = Item.OctQty;
                projectItem.ProjectNovQty = Item.NovQty;
                projectItem.ProjectDecQty = Item.DecQty;
                projectItem.ProjectTotalQty = Item.TotalQty;
                projectItem.UnitCost = (decimal)Item.UnitCost;
                projectItem.Supplier1 = Item.Supplier1ID;
                projectItem.Supplier1UnitCost = Item.Supplier1UnitCost;
                projectItem.Supplier2 = Item.Supplier2ID == 0 ? null : Item.Supplier2ID;
                projectItem.Supplier2UnitCost = Item.Supplier2UnitCost;
                projectItem.Supplier3 = Item.Supplier3ID == 0 ? null : Item.Supplier3ID;
                projectItem.Supplier3UnitCost = Item.Supplier3UnitCost;
                projectItem.ProjectEstimatedBudget = Item.EstimatedBudget;
                projectItem.Justification = Item.Remarks;

                if(db.SaveChanges() == 0)
                {
                    Message = "Error";
                    return false;
                }
            }
            
            if(projectService != null)
            {
                projectService.ItemReference = db.Items.Where(d => d.ItemCode == Item.ItemCode).FirstOrDefault().ID;
                projectService.ProjectReference = db.ProjectPlans.Where(d => d.ProjectCode == Item.ProjectCode).FirstOrDefault().ID;
                projectService.ProposalType = Item.ProposalType;
                projectService.ProjectQuantity = Item.TotalQty;
                projectService.UnitCost = (decimal)Item.UnitCost;
                projectService.Supplier1 = Item.Supplier1ID;
                projectService.Supplier1UnitCost = Item.Supplier1UnitCost;
                projectService.Supplier2 = Item.Supplier2ID == 0 ? null : Item.Supplier2ID;
                projectService.Supplier2UnitCost = Item.Supplier2UnitCost;
                projectService.Supplier3 = Item.Supplier3ID == 0 ? null : Item.Supplier3ID;
                projectService.Supplier3UnitCost = Item.Supplier3UnitCost;
                projectService.ProjectEstimatedBudget = Item.EstimatedBudget;
                projectService.Justification = Item.Remarks;

                if (db.SaveChanges() == 0)
                {
                    Message = "Error";
                    return false;
                }
            }

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
            if(ProjectPlan.NewItemProposals.Count == 0)
            {
                return false;
            }

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
                hrisDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}