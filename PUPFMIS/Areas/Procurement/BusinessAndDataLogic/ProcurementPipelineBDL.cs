using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ProcurementPipelineDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL system = new SystemBDL();

        public List<HRISEmployeeDetailsVM> GetProcurementEmployees()
        {
            var procurementEmployeees = new List<HRISEmployeeDetailsVM>();
            var bac = db.AgencyDetails.FirstOrDefault().BACOfficeReference;
            var pmo = db.AgencyDetails.FirstOrDefault().ProcurementOfficeReference;
            var pmoEmployees = hris.GetEmployees(pmo);
            procurementEmployeees.AddRange(pmoEmployees);
            return procurementEmployeees.OrderBy(d => d.EmployeeName).ToList();
        }
        public List<APPHeader> GetAPPs()
        {
            return db.APPHeader.Where(d => d.APPType != "CSE").ToList();
        }
        public List<ProcurementProjectsVM> GetProcurementProgramDetails(string ReferenceNo)
        {
            var procurementProjectList = new List<ProcurementProjectsVM>();
            var institutionalProjectList = db.APPInstitutionalItems.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo && d.PPMPReferences != null && !d.PPMPReferences.Contains("CUOS")).ToList();
            var projectList = db.APPProjectItems.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo).ToList();

            foreach(var institutionalItem in institutionalProjectList)
            {
                var projectItems = new List<ProcurementProjectItemsVM>();
                var PPMPList = institutionalItem.PPMPReferences.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                foreach(var ppmpReference in PPMPList)
                {
                    var items = db.ProjectPlanItems
                             .Where(d => d.FKAPPReference.ID == institutionalItem.FKAPPHeaderReference.ID
                                    && d.FKPPMPReference.ReferenceNo == ppmpReference
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == institutionalItem.ObjectClassification)
                             .ToList();
                    foreach(var item in items)
                    {
                        projectItems.Add(new ProcurementProjectItemsVM
                        {
                            ProjectCode = item.FKProjectReference.ProjectCode,
                            ItemCode = item.FKItemReference.ItemCode,
                            ItemName = item.FKItemReference.ItemFullName,
                            ItemSpecifications = item.FKItemReference.ItemShortSpecifications,
                            ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                            InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                            ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                            PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.Abbreviation,
                            IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.Abbreviation,
                            TotalQty = item.PPMPTotalQty,
                            EstimatedBudget = item.PPMPEstimatedBudget,
                            UnitCost = item.UnitCost
                        });
                    }
                }
                var procurementModes = institutionalItem.ModeOfProcurementReference.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                string procurementModeList = string.Empty;
                for(int i = 0; i < procurementModes.Count(); i++)
                {
                    if(i == procurementModes.Count() - 1)
                    {
                        procurementModeList += procurementModes[i];
                    }
                    else
                    {
                        procurementModeList += procurementModes[i] + "\n";
                    }
                }
                procurementProjectList.Add(new ProcurementProjectsVM
                {
                    Month = institutionalItem.Month,
                    PAPCode = institutionalItem.PAPCode,
                    UACS = institutionalItem.ObjectClassification,
                    ProcurementProgram = institutionalItem.ProcurementProgram,
                    ApprovedBudget = institutionalItem.Total,
                    ObjectClassification = abis.GetChartOfAccounts(institutionalItem.ObjectClassification).AcctName,
                    FundCluster = institutionalItem.FundSourceReference,
                    FundSource = abis.GetFundSources(institutionalItem.FundSourceReference).FUND_DESC,
                    StartMonth = institutionalItem.StartMonth,
                    ModeOfProcurement = procurementModeList,
                    Items = projectItems
                });
            }

            return procurementProjectList;
        }
        public ProcurementProjectsVM GetProcurementProgramDetailsByPAPCode(string PAPCode)
        {
            var procurementProject = new ProcurementProjectsVM();
            var institutionalProject = db.APPInstitutionalItems.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var project = db.APPProjectItems.Where(d => d.PAPCode == PAPCode).FirstOrDefault();

            if(institutionalProject != null)
            {
                var projectItems = new List<ProcurementProjectItemsVM>();
                var PPMPList = institutionalProject.PPMPReferences.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                foreach (var ppmpReference in PPMPList)
                {
                    var items = db.ProjectPlanItems
                             .Where(d => d.FKAPPReference.ID == institutionalProject.FKAPPHeaderReference.ID
                                    && d.FKPPMPReference.ReferenceNo == ppmpReference
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == institutionalProject.ObjectClassification
                                    && d.FundSource == institutionalProject.FundSourceReference
                                    && (institutionalProject.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m))
                             .ToList();

                    var services = db.ProjectPlanServices
                                 .Where(d => d.FKAPPHeaderReference.ID == institutionalProject.FKAPPHeaderReference.ID
                                        && d.FKPPMPReference.ReferenceNo == ppmpReference
                                        && d.FKItemReference.FKItemTypeReference.AccountClass == institutionalProject.ObjectClassification
                                        && d.FundSource == institutionalProject.FundSourceReference
                                        && (institutionalProject.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m))
                                 .ToList();

                    foreach (var item in items)
                    {
                        projectItems.Add(new ProcurementProjectItemsVM
                        {
                            ProjectCode = item.FKProjectReference.ProjectCode,
                            ProjectName = item.FKProjectReference.ProjectName,
                            DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                            EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                            ItemCode = item.FKItemReference.ItemCode,
                            ItemName = item.FKItemReference.ItemFullName,
                            ItemSpecifications = item.FKItemReference.ItemShortSpecifications,
                            ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                            InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                            ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                            PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.Abbreviation,
                            IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.Abbreviation,
                            TotalQty = item.PPMPTotalQty,
                            EstimatedBudget = item.PPMPEstimatedBudget,
                            UnitCost = item.UnitCost
                        });
                    }

                    foreach (var item in services)
                    {
                        projectItems.Add(new ProcurementProjectItemsVM
                        {
                            ProjectCode = item.FKProjectReference.ProjectCode,
                            ProjectName = item.FKProjectReference.ProjectName,
                            DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                            EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                            ItemCode = item.FKItemReference.ItemCode,
                            ItemName = item.FKItemReference.ItemFullName,
                            ItemSpecifications = item.FKItemReference.ItemShortSpecifications,
                            ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                            InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                            ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                            EstimatedBudget = item.PPMPEstimatedBudget,
                            UnitCost = item.UnitCost,
                            TotalQty = item.PPMPQuantity
                        });
                    }
                }
                var procurementModes = institutionalProject.ModeOfProcurementReference.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                string procurementModeList = string.Empty;
                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName;
                    }
                    else
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "\n";
                    }
                }

                var procurementTimeline = db.ProcurementTimeline
                    .Where(d => d.PAPCode == institutionalProject.PAPCode)
                    .FirstOrDefault();

                var schedule = new ProcurementProjectScheduleVM
                {
                    PurchaseRequestSubmission = procurementTimeline == null ? null : procurementTimeline.PurchaseRequestSubmission.Value.ToString("MMMM dd, yyyy"),
                    PreProcurementConference = procurementTimeline == null ? null : procurementTimeline.PreProcurementConference.Value.ToString("MMMM dd, yyyy"),
                    PostingOfIB = procurementTimeline == null ? null : procurementTimeline.PostingOfIB.Value.ToString("MMMM dd, yyyy"),
                    PreBidConference = procurementTimeline == null ? null : procurementTimeline.PreBidConference.Value.ToString("MMMM dd, yyyy"),
                    SubmissionOfBids = procurementTimeline == null ? null : procurementTimeline.SubmissionOfBids.Value.ToString("MMMM dd, yyyy"),
                    BidEvaluation = procurementTimeline == null ? null : procurementTimeline.BidEvaluation.Value.ToString("MMMM dd, yyyy"),
                    PostQualification = procurementTimeline == null ? null : procurementTimeline.PostQualification.Value.ToString("MMMM dd, yyyy"),
                    NOAIssuance = procurementTimeline == null ? null : procurementTimeline.NOAIssuance.Value.ToString("MMMM dd, yyyy"),
                    ContractSigning = procurementTimeline == null ? null : procurementTimeline.ContractSigning.Value.ToString("MMMM dd, yyyy"),
                    Approval = procurementTimeline == null ? null : procurementTimeline.Approval.Value.ToString("MMMM dd, yyyy"),
                    NTPIssuance = procurementTimeline == null ? null : procurementTimeline.NTPIssuance.Value.ToString("MMMM dd, yyyy"),
                    POReceived = procurementTimeline == null ? null : procurementTimeline.POReceived.Value.ToString("MMMM dd, yyyy")
                };

                procurementProject = new ProcurementProjectsVM()
                {
                    APPReference = institutionalProject.FKAPPHeaderReference.ReferenceNo,
                    Month = institutionalProject.Month,
                    PAPCode = institutionalProject.PAPCode,
                    UACS = institutionalProject.ObjectClassification,
                    ProcurementProgram = institutionalProject.ProcurementProgram,
                    ApprovedBudget = institutionalProject.Total,
                    ObjectClassification = abis.GetChartOfAccounts(institutionalProject.ObjectClassification).AcctName,
                    FundCluster = institutionalProject.FundSourceReference,
                    FundSource = abis.GetFundSources(institutionalProject.FundSourceReference).FUND_DESC,
                    EndUser = hris.GetDepartmentDetails(institutionalProject.EndUser).Department,
                    StartMonth = institutionalProject.StartMonth,
                    EndMonth = institutionalProject.EndMonth,
                    ModeOfProcurement = procurementModeList,
                    MOOETotal = institutionalProject.MOOEAmount,
                    CapitalOutlayTotal = institutionalProject.COAmount,
                    TotalEstimatedBudget = institutionalProject.Total,
                    Remarks = institutionalProject.Remarks,
                    ProjectCoordinator = institutionalProject.ProjectCoordinator == null ? null : hris.GetEmployeeDetailByCode(institutionalProject.ProjectCoordinator).EmployeeName,
                    ProjectSupport = institutionalProject.ProjectSupport == null ? null : hris.GetEmployeeDetailByCode(institutionalProject.ProjectSupport).EmployeeName,
                    Items = projectItems,
                    Schedule = schedule
                };
            }

            if (project != null)
            {
                var projectItems = new List<ProcurementProjectItemsVM>();
                var items = db.ProjectPlanItems
                             .Where(d => d.FKAPPReference.ReferenceNo == project.FKAPPHeaderReference.ReferenceNo
                                    && d.FKProjectReference.ProjectCode == project.PAPCode
                                    && d.FKItemReference.FKItemTypeReference.AccountClass ==project.ObjectClassification
                                    && d.FundSource == project.FundSourceReference
                                    && (project.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m))
                             .ToList();

                var services = db.ProjectPlanServices
                             .Where(d => d.FKAPPHeaderReference.ReferenceNo == project.FKAPPHeaderReference.ReferenceNo
                                    && d.FKProjectReference.ProjectCode == project.PAPCode
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == project.ObjectClassification
                                    && d.FundSource == project.FundSourceReference
                                    && (project.MOOEAmount != 0.00m ? d.UnitCost < 15000.00m : d.UnitCost >= 15000.00m))
                             .ToList();

                foreach (var item in items)
                {
                    projectItems.Add(new ProcurementProjectItemsVM
                    {
                        ProjectCode = item.FKProjectReference.ProjectCode,
                        ProjectName = item.FKProjectReference.ProjectName,
                        DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                        EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                        ItemCode = item.FKItemReference.ItemCode,
                        ItemName = item.FKItemReference.ItemFullName,
                        ItemSpecifications = item.FKItemReference.ItemShortSpecifications,
                        ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                        InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                        ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                        PackagingUOMReference = item.FKItemReference.FKPackagingUnitReference.Abbreviation,
                        IndividualUOMReference = item.FKItemReference.FKIndividualUnitReference.Abbreviation,
                        TotalQty = item.PPMPTotalQty,
                        EstimatedBudget = item.PPMPEstimatedBudget,
                        UnitCost = item.UnitCost
                    });
                }

                foreach (var item in services)
                {
                    projectItems.Add(new ProcurementProjectItemsVM
                    {
                        ProjectCode = item.FKProjectReference.ProjectCode,
                        ProjectName = item.FKProjectReference.ProjectName,
                        DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart),
                        EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department,
                        ItemCode = item.FKItemReference.ItemCode,
                        ItemName = item.FKItemReference.ItemFullName,
                        ItemSpecifications = item.FKItemReference.ItemShortSpecifications,
                        ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                        InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                        ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                        EstimatedBudget = item.PPMPEstimatedBudget,
                        UnitCost = item.UnitCost,
                        TotalQty = item.PPMPQuantity
                    });
                }

                var procurementModes = project.ModeOfProcurementReference.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                string procurementModeList = string.Empty;
                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName;
                    }
                    else
                    {
                        procurementModeList += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "\n";
                    }
                }
                procurementProject = new ProcurementProjectsVM()
                {
                    APPReference = project.FKAPPHeaderReference.ReferenceNo,
                    Month = project.Month,
                    PAPCode = project.PAPCode,
                    UACS = project.ObjectClassification,
                    ProcurementProgram = project.ProcurementProgram,
                    ApprovedBudget = project.Total,
                    ObjectClassification = abis.GetChartOfAccounts(project.ObjectClassification).AcctName,
                    FundCluster = project.FundSourceReference,
                    FundSource = abis.GetFundSources(project.FundSourceReference).FUND_DESC,
                    EndUser = hris.GetDepartmentDetails(project.EndUser).Department,
                    StartMonth = project.StartMonth,
                    EndMonth = project.EndMonth,
                    ModeOfProcurement = procurementModeList,
                    MOOETotal = project.MOOEAmount,
                    CapitalOutlayTotal = project.COAmount,
                    TotalEstimatedBudget = project.Total,
                    Remarks = project.Remarks,
                    ProjectCoordinator = project.ProjectCoordinator == null ? null : hris.GetEmployeeDetailByCode(project.ProjectCoordinator).EmployeeName,
                    ProjectSupport = project.ProjectSupport == null ? null : hris.GetEmployeeDetailByCode(project.ProjectSupport).EmployeeName,
                    Items = projectItems
                };
            }

            return procurementProject;
        }
        public bool SetSchedule(ProcurementProjectsVM ProcurementProject)
        {
            var procurementTimeline = new ProcurementTimeline
            {
                PAPCode = ProcurementProject.PAPCode,
                PurchaseRequestSubmission = Convert.ToDateTime(ProcurementProject.Schedule.PurchaseRequestSubmission),
                PreProcurementConference = Convert.ToDateTime(ProcurementProject.Schedule.PreProcurementConference),
                PostingOfIB = Convert.ToDateTime(ProcurementProject.Schedule.PostingOfIB),
                PreBidConference = Convert.ToDateTime(ProcurementProject.Schedule.PreBidConference),
                SubmissionOfBids = Convert.ToDateTime(ProcurementProject.Schedule.SubmissionOfBids),
                BidEvaluation = Convert.ToDateTime(ProcurementProject.Schedule.BidEvaluation),
                PostQualification = Convert.ToDateTime(ProcurementProject.Schedule.PostQualification),
                NOAIssuance = Convert.ToDateTime(ProcurementProject.Schedule.NOAIssuance),
                ContractSigning = Convert.ToDateTime(ProcurementProject.Schedule.ContractSigning),
                Approval = Convert.ToDateTime(ProcurementProject.Schedule.Approval),
                NTPIssuance = Convert.ToDateTime(ProcurementProject.Schedule.NTPIssuance),
                POReceived = Convert.ToDateTime(ProcurementProject.Schedule.POReceived)
            };
            db.ProcurementTimeline.Add(procurementTimeline);
            if(db.SaveChanges() == 0)
            {
                return false;
            }
            return true;
        }
        public List<ProcurementProgramsVM> GetUnassignedProcurementProgams(string ReferenceNo)
        {
            var procurementPrograms = new List<ProcurementProgramsVM>();
            var institutionalPrograms = db.APPInstitutionalItems.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo 
                                                                        && !d.PPMPReferences.Contains("CUOS") 
                                                                        && (d.ProjectCoordinator == null && d.ProjectSupport == null))
                                                                .ToList();
            var projectPrograms = db.APPProjectItems.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo && (d.ProjectCoordinator == null && d.ProjectSupport == null)).ToList();
            foreach(var program in institutionalPrograms)
            {
                var procurementModes = program.ModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();
                var procurementModeList = string.Empty;

                for(int i = 0; i < procurementModes.Count(); i++)
                {
                    if(i == procurementModes.Count() - 1)
                    {
                        procurementModeList += procurementModes[i];
                    }
                    else
                    {
                        procurementModeList += procurementModes[i] + "\n";
                    }
                }

                procurementPrograms.Add(new ProcurementProgramsVM {
                    Month = program.Month,
                    PAPCode = program.PAPCode,
                    UACS = program.ObjectClassification,
                    ProcurementProgram = program.ProcurementProgram,
                    ApprovedBudget = program.Total,
                    ObjectClassification = abis.GetChartOfAccounts(program.ObjectClassification).AcctName,
                    FundCluster = program.FundSourceReference.Replace("\r\n",""),
                    FundSource = abis.GetFundSources(program.FundSourceReference).FUND_DESC,
                    StartMonth = program.StartMonth,
                    EndMonth = program.EndMonth,
                    ProjectCoordinator = program.ProjectCoordinator,
                    ModeOfProcurement = procurementModeList,
                    ProjectSupport = program.ProjectSupport
                });
            }

            foreach (var program in projectPrograms)
            {
                var procurementModes = program.ModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();
                var procurementModeList = string.Empty;

                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += procurementModes[i];
                    }
                    else
                    {
                        procurementModeList += procurementModes[i] + "\n";
                    }
                }

                procurementPrograms.Add(new ProcurementProgramsVM
                {
                    Month = program.Month,
                    PAPCode = program.PAPCode,
                    UACS = program.ObjectClassification,
                    ProcurementProgram = program.ProcurementProgram,
                    ApprovedBudget = program.Total,
                    ObjectClassification = abis.GetChartOfAccounts(program.ObjectClassification).AcctName,
                    FundCluster = program.FundSourceReference.Replace("\r\n", ""),
                    FundSource = abis.GetFundSources(program.FundSourceReference).FUND_DESC,
                    StartMonth = program.StartMonth,
                    EndMonth = program.EndMonth,
                    ProjectCoordinator = program.ProjectCoordinator,
                    ModeOfProcurement = procurementModeList,
                    ProjectSupport = program.ProjectSupport
                });
            }

            return procurementPrograms;
        }
        public List<ProcurementProgramsVM> GetProcurementProgams(string ReferenceNo)
        {
            var procurementPrograms = new List<ProcurementProgramsVM>();
            var institutionalPrograms = db.APPInstitutionalItems.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo
                                                                        && !d.PPMPReferences.Contains("CUOS")
                                                                        && (d.ProjectCoordinator != null && d.ProjectSupport != null))
                                                                .ToList();
            var projectPrograms = db.APPProjectItems.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo).ToList();
            foreach (var program in institutionalPrograms)
            {
                var procurementModes = program.ModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();
                var procurementModeList = string.Empty;

                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += procurementModes[i];
                    }
                    else
                    {
                        procurementModeList += procurementModes[i] + "\n";
                    }
                }

                var projectCoordinator = hris.GetEmployeeDetailByCode(program.ProjectCoordinator).EmployeeName;
                var projectSupport = hris.GetEmployeeDetailByCode(program.ProjectSupport).EmployeeName;
                var procurementTimeline = db.ProcurementTimeline.Where(d => d.PAPCode == program.PAPCode).FirstOrDefault();

                procurementPrograms.Add(new ProcurementProgramsVM
                {
                    Month = program.Month,
                    PAPCode = program.PAPCode,
                    UACS = program.ObjectClassification,
                    ProcurementProgram = program.ProcurementProgram,
                    ApprovedBudget = program.Total,
                    ObjectClassification = abis.GetChartOfAccounts(program.ObjectClassification).AcctName,
                    FundCluster = program.FundSourceReference.Replace("\r\n", ""),
                    FundSource = abis.GetFundSources(program.FundSourceReference).FUND_DESC,
                    StartMonth = program.StartMonth,
                    EndMonth = program.EndMonth,
                    ProjectCoordinator = projectCoordinator,
                    ModeOfProcurement = procurementModeList,
                    ProjectSupport = projectSupport,
                    HasSchedule = procurementTimeline == null ? false : true
                });
            }

            foreach (var program in projectPrograms)
            {
                var procurementModes = program.ModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();
                var procurementModeList = string.Empty;

                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += procurementModes[i];
                    }
                    else
                    {
                        procurementModeList += procurementModes[i] + "\n";
                    }
                }

                var projectCoordinator = hris.GetEmployeeDetailByCode(program.ProjectCoordinator).EmployeeName;
                var projectSupport = hris.GetEmployeeDetailByCode(program.ProjectSupport).EmployeeName;

                procurementPrograms.Add(new ProcurementProgramsVM
                {
                    Month = program.Month,
                    PAPCode = program.PAPCode,
                    UACS = program.ObjectClassification,
                    ProcurementProgram = program.ProcurementProgram,
                    ApprovedBudget = program.Total,
                    ObjectClassification = abis.GetChartOfAccounts(program.ObjectClassification).AcctName,
                    FundCluster = program.FundSourceReference.Replace("\r\n", ""),
                    FundSource = abis.GetFundSources(program.FundSourceReference).FUND_DESC,
                    StartMonth = program.StartMonth,
                    EndMonth = program.EndMonth,
                    ProjectCoordinator = projectCoordinator,
                    ModeOfProcurement = procurementModeList,
                    ProjectSupport = projectSupport
                });
            }

            return procurementPrograms;
        }
        public List<ProcurementProgramsVM> GetProcurementProgams(string ReferenceNo, string Email)
        {
            var empCode = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault().EmpCode;
            var procurementPrograms = new List<ProcurementProgramsVM>();
            var institutionalPrograms = db.APPInstitutionalItems
                .Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo
                    && !d.PPMPReferences.Contains("CUOS")
                    && (d.ProjectCoordinator == empCode || d.ProjectSupport != empCode))
                .ToList();
            var projectPrograms = db.APPProjectItems
                .Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo
                    && (d.ProjectCoordinator == empCode || d.ProjectSupport != empCode))
                .ToList();
            foreach (var program in institutionalPrograms)
            {
                var procurementModes = program.ModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();
                var procurementModeList = string.Empty;

                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += procurementModes[i];
                    }
                    else
                    {
                        procurementModeList += procurementModes[i] + "\n";
                    }
                }

                var projectCoordinator = hris.GetEmployeeDetailByCode(program.ProjectCoordinator).EmployeeName;
                var projectSupport = hris.GetEmployeeDetailByCode(program.ProjectSupport).EmployeeName;
                var procurementTimeline = db.ProcurementTimeline.Where(d => d.PAPCode == program.PAPCode).FirstOrDefault();

                procurementPrograms.Add(new ProcurementProgramsVM
                {
                    Month = program.Month,
                    PAPCode = program.PAPCode,
                    UACS = program.ObjectClassification,
                    ProcurementProgram = program.ProcurementProgram,
                    ApprovedBudget = program.Total,
                    ObjectClassification = abis.GetChartOfAccounts(program.ObjectClassification).AcctName,
                    FundCluster = program.FundSourceReference.Replace("\r\n", ""),
                    FundSource = abis.GetFundSources(program.FundSourceReference).FUND_DESC,
                    StartMonth = program.StartMonth,
                    EndMonth = program.EndMonth,
                    ProjectCoordinator = projectCoordinator,
                    ModeOfProcurement = procurementModeList,
                    ProjectSupport = projectSupport,
                    HasSchedule = procurementTimeline == null ? false : true
                });
            }

            foreach (var program in projectPrograms)
            {
                var procurementModes = program.ModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToArray();
                var procurementModeList = string.Empty;

                for (int i = 0; i < procurementModes.Count(); i++)
                {
                    if (i == procurementModes.Count() - 1)
                    {
                        procurementModeList += procurementModes[i];
                    }
                    else
                    {
                        procurementModeList += procurementModes[i] + "\n";
                    }
                }

                var projectCoordinator = hris.GetEmployeeDetailByCode(program.ProjectCoordinator).EmployeeName;
                var projectSupport = hris.GetEmployeeDetailByCode(program.ProjectSupport).EmployeeName;

                procurementPrograms.Add(new ProcurementProgramsVM
                {
                    Month = program.Month,
                    PAPCode = program.PAPCode,
                    UACS = program.ObjectClassification,
                    ProcurementProgram = program.ProcurementProgram,
                    ApprovedBudget = program.Total,
                    ObjectClassification = abis.GetChartOfAccounts(program.ObjectClassification).AcctName,
                    FundCluster = program.FundSourceReference.Replace("\r\n", ""),
                    FundSource = abis.GetFundSources(program.FundSourceReference).FUND_DESC,
                    StartMonth = program.StartMonth,
                    EndMonth = program.EndMonth,
                    ProjectCoordinator = projectCoordinator,
                    ModeOfProcurement = procurementModeList,
                    ProjectSupport = projectSupport
                });
            }

            return procurementPrograms;
        }
        public bool AssignProject(ProcurementProjectsVM ProcurementProject)
        {
            var institutionalProject = db.APPInstitutionalItems.Where(d => d.PAPCode == ProcurementProject.PAPCode).FirstOrDefault();
            var procurementProject = db.APPProjectItems.Where(d => d.PAPCode == ProcurementProject.PAPCode).FirstOrDefault();
            if(institutionalProject != null)
            {
                institutionalProject.ProjectCoordinator = ProcurementProject.ProjectCoordinator;
                institutionalProject.ProjectSupport = ProcurementProject.ProjectSupport;
                institutionalProject.ProjectStatus = "Project Assigned";
                if(db.SaveChanges() == 0)
                {
                    return false;
                }
            }
            if(procurementProject != null)
            {
                procurementProject.ProjectCoordinator = ProcurementProject.ProjectCoordinator;
                procurementProject.ProjectSupport = ProcurementProject.ProjectSupport;
                procurementProject.ProjectStatus = "Project Assigned";
                if (db.SaveChanges() == 0)
                {
                    return false;
                }

            }

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                abis.Dispose();
                hris.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}