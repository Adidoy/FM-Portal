using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    
    public class MyProjectsBL : Controller
    {

    }

    public class MyProjectsDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL system = new SystemBDL();

        public int GetTotalProjectsAssigned(string Email)
        {
            var empCode = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault().EmpCode;
            var institutionalProjects = db.APPInstitutionalItems.Where(d => d.ProjectCoordinator == empCode || d.ProjectSupport == empCode).Count();
            var endUserProjects = db.APPProjectItems.Where(d => d.ProjectCoordinator == empCode || d.ProjectSupport == empCode).Count();
            return institutionalProjects + endUserProjects;
        }
        public int GetTotalProjectsCoordinated(string Email)
        {
            var empCode = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault().EmpCode;
            var institutionalProjects = db.APPInstitutionalItems.Where(d => d.ProjectCoordinator == empCode).Count();
            var endUserProjects = db.APPProjectItems.Where(d => d.ProjectCoordinator == empCode).Count();
            return institutionalProjects + endUserProjects;
        }
        public int GetTotalProjectsupported(string Email)
        {
            var empCode = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault().EmpCode;
            var institutionalProjects = db.APPInstitutionalItems.Where(d => d.ProjectSupport == empCode).Count();
            var endUserProjects = db.APPProjectItems.Where(d => d.ProjectSupport == empCode).Count();
            return institutionalProjects + endUserProjects;
        }

        public List<string> GetProjectsWithSetTimeLine(string Email)
        {
            var empCode = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault().EmpCode;
            var institutionalProjects = db.APPInstitutionalItems.Where(d => (d.ProjectCoordinator == empCode || d.ProjectSupport == empCode) && d.ProjectStatus == "Project Assigned").Select(d => d.PAPCode).ToList();
            var endUserProjects = db.APPProjectItems.Where(d => (d.ProjectCoordinator == empCode || d.ProjectSupport == empCode) && d.ProjectStatus == "Project Assigned").Select(d => d.PAPCode).ToList();
            var papCodeList = new List<string>();

            papCodeList.AddRange(institutionalProjects);
            papCodeList.AddRange(endUserProjects);

            var projectsWithTimeline = db.ProcurementTimeline.Select(d => d.PAPCode).ToList();

            return projectsWithTimeline;
        }
        public List<string> GetProjectsWithoutSetTimeLine(string Email)
        {
            var empCode = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault().EmpCode;
            var institutionalProjects = db.APPInstitutionalItems.Where(d => (d.ProjectCoordinator == empCode || d.ProjectSupport == empCode) && d.ProjectStatus == "Project Assigned").Select(d => d.PAPCode).ToList();
            var endUserProjects = db.APPProjectItems.Where(d => (d.ProjectCoordinator == empCode || d.ProjectSupport == empCode) && d.ProjectStatus == "Project Assigned").Select(d => d.PAPCode).ToList();
            var papCodeList = new List<string>();

            papCodeList.AddRange(institutionalProjects);
            papCodeList.AddRange(endUserProjects);

            var projectsWithTimeline = db.ProcurementTimeline.Select(d => d.PAPCode).ToList();
            papCodeList = papCodeList.Where(d => !projectsWithTimeline.Contains(d)).ToList();

            return papCodeList;
        }
        public ProcurementProjectsVM GetProcurementProgramDetailsByPAPCode(string PAPCode)
        {
            var procurementProject = new ProcurementProjectsVM();
            var institutionalProject = db.APPInstitutionalItems.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var project = db.APPProjectItems.Where(d => d.PAPCode == PAPCode).FirstOrDefault();

            if (institutionalProject != null)
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
                    ProjectStatus = institutionalProject.ProjectStatus,
                    Actual = new ProcurementProjectActualAccomplishmentVM(),
                    Items = projectItems,
                    Schedule = schedule,
                    ProcurmentProjectType = "Institutional"
                };
            }

            if (project != null)
            {
                var projectItems = new List<ProcurementProjectItemsVM>();
                var items = db.ProjectPlanItems
                             .Where(d => d.FKAPPReference.ReferenceNo == project.FKAPPHeaderReference.ReferenceNo
                                    && d.FKProjectReference.ProjectCode == project.PAPCode
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == project.ObjectClassification
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

                var procurementTimeline = db.ProcurementTimeline
                    .Where(d => d.PAPCode == project.PAPCode)
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
                    ProjectStatus = project.ProjectStatus,
                    Actual = new ProcurementProjectActualAccomplishmentVM(),
                    Items = projectItems,
                    Schedule = schedule,
                    ProcurmentProjectType = "Project"
                };
            }

            return procurementProject;
        }

        public List<ProcurementProjectsVM> GetProcurementProgramDetails(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var procurementProjectList = new List<ProcurementProjectsVM>();
            var institutionalProjectList = db.APPInstitutionalItems.Where(d => d.PPMPReferences != null && !d.PPMPReferences.Contains("CUOS") && (d.ProjectCoordinator == user.EmpCode || d.ProjectSupport == user.EmpCode)).ToList();
            var projectList = db.APPProjectItems.Where(d => (d.ProjectCoordinator == user.EmpCode || d.ProjectSupport == user.EmpCode)).ToList();

            foreach (var institutionalItem in institutionalProjectList)
            {
                var projectItems = new List<ProcurementProjectItemsVM>();
                var PPMPList = institutionalItem.PPMPReferences.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                foreach (var ppmpReference in PPMPList)
                {
                    var items = db.ProjectPlanItems
                             .Where(d => d.FKAPPReference.ID == institutionalItem.FKAPPHeaderReference.ID
                                    && d.FKPPMPReference.ReferenceNo == ppmpReference
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == institutionalItem.ObjectClassification)
                             .ToList();
                    foreach (var item in items)
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
                    EndMonth = institutionalItem.EndMonth,
                    ModeOfProcurement = procurementModeList,
                    ProjectStatus = institutionalItem.ProjectStatus,
                    ProjectSupport = hris.GetEmployeeByCode(institutionalItem.ProjectSupport).EmployeeName,
                    Items = projectItems
                });
            }



            foreach (var endUserProjectItem in projectList)
            {
                var projectItems = new List<ProcurementProjectItemsVM>();
                var PPMPList = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == endUserProjectItem.PAPCode).ToList();
                foreach (var ppmpReference in PPMPList)
                {
                    var items = db.ProjectPlanItems
                             .Where(d => d.FKAPPReference.ID == endUserProjectItem.FKAPPHeaderReference.ID
                                    && d.FKPPMPReference.ReferenceNo == ppmpReference.FKPPMPReference.ReferenceNo
                                    && d.FKItemReference.FKItemTypeReference.AccountClass == endUserProjectItem.ObjectClassification)
                             .ToList();
                    foreach (var item in items)
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
                var procurementModes = endUserProjectItem.ModeOfProcurementReference.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
                string procurementModeList = string.Empty;
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
                procurementProjectList.Add(new ProcurementProjectsVM
                {
                    Month = endUserProjectItem.Month,
                    PAPCode = endUserProjectItem.PAPCode,
                    UACS = endUserProjectItem.ObjectClassification,
                    ProcurementProgram = endUserProjectItem.ProcurementProgram,
                    ApprovedBudget = endUserProjectItem.Total,
                    ObjectClassification = abis.GetChartOfAccounts(endUserProjectItem.ObjectClassification).AcctName,
                    FundCluster = endUserProjectItem.FundSourceReference,
                    FundSource = abis.GetFundSources(endUserProjectItem.FundSourceReference).FUND_DESC,
                    StartMonth = endUserProjectItem.StartMonth,
                    EndMonth = endUserProjectItem.EndMonth,
                    ModeOfProcurement = procurementModeList,
                    ProjectStatus = endUserProjectItem.ProjectStatus,
                    ProjectSupport = hris.GetEmployeeByCode(endUserProjectItem.ProjectSupport).EmployeeName,
                    Items = projectItems
                });
            }

            return procurementProjectList;
        }
        public bool OpenPRSubmission(string PAPCode)
        {
            var procurementProgram = GetProcurementProgramDetailsByPAPCode(PAPCode);
            var items = procurementProgram.Items;
            foreach(var item in items)
            {
                var procurementItem = db.ProjectPlanItems.Where(d => d.FKProjectReference.ProjectCode == item.ProjectCode && d.FKItemReference.ItemCode == item.ItemCode).FirstOrDefault();
                var procurementService = db.ProjectPlanServices.Where(d => d.FKProjectReference.ProjectCode == item.ProjectCode && d.FKItemReference.ItemCode == item.ItemCode).FirstOrDefault();
                if(procurementItem != null)
                {
                    procurementItem.Status = "PR Submission Open";
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
                if(procurementService != null)
                {
                    procurementService.Status = "PR Submission Open";
                    if (db.SaveChanges() == 0)
                    {
                        return false;
                    }
                }
            }

            if(procurementProgram.ProcurmentProjectType == "Institutional")
            {
                var program = db.APPInstitutionalItems.Where(d => d.PAPCode == procurementProgram.PAPCode).FirstOrDefault();
                program.ProjectStatus = "PR Submission Open";
                if (db.SaveChanges() == 0)
                {
                    return false;
                }
            }

            if (procurementProgram.ProcurmentProjectType == "Project")
            {
                var program = db.APPProjectItems.Where(d => d.PAPCode == procurementProgram.PAPCode).FirstOrDefault();
                program.ProjectStatus = "PR Submission Open";
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