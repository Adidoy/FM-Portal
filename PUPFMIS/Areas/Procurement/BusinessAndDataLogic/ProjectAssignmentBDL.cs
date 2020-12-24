using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ProjectAssignmentDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hris = new HRISDataAccess();
        private ABISDataAccess abis = new ABISDataAccess();
        private SystemBDL system = new SystemBDL();

        public List<HRISEmployeeDetailsVM> GetProcurementEmployees()
        {
            var agencyDetails = db.AgencyDetails.FirstOrDefault();
            var procurementEmployeees = new List<HRISEmployeeDetailsVM>();
            var pmo = agencyDetails.ProcurementOfficeReference;
            var pmoEmployees = hris.GetEmployees(pmo);
            procurementEmployeees.AddRange(pmoEmployees);
            return procurementEmployeees.OrderBy(d => d.EmployeeName).ToList();
        }

        public List<AnnualProcurementPlan> GetAPPs()
        {
            return db.APPHeader.Where(d => d.APPType != "CSE").ToList();
        }
        private List<ProcurementProjectItemsVM> GetProjectItems(string PAPCode)
        {
            var appDetails = db.APPDetails.Where(d => d.PAPCode == PAPCode).First();
            var procurementItems = new List<ProcurementProjectItemsVM>();
            var procurementProgram = db.APPDetails.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var items = db.ProjectPlanItems.Where(d => d.APPLineReference == appDetails.ID).ToList();
            var services = db.ProjectPlanServices.Where(d => d.APPLineReference == appDetails.ID).ToList();

            foreach (var item in items)
            {
                var purchaseRequestHeader = db.PurchaseRequestHeader.Find(item.PRReference);
                procurementItems.Add(new ProcurementProjectItemsVM
                {
                    EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department + (item.FKProjectReference.Department == item.FKProjectReference.Unit ? string.Empty : " - " + hris.GetDepartmentDetails(item.FKProjectReference.Unit).Department),
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
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
                    UnitCost = item.UnitCost,
                    DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart) + ", " + item.FKProjectReference.FiscalYear.ToString(),
                    PRNumber = item.PRReference == null ? null : purchaseRequestHeader.PRNumber,
                    DatePRReceived = item.PRReference == null ? null : purchaseRequestHeader.ReceivedAt,
                    Status = item.Status
                });
            }

            foreach (var item in services)
            {
                procurementItems.Add(new ProcurementProjectItemsVM
                {
                    EndUser = hris.GetDepartmentDetails(item.FKProjectReference.Department).Department + (item.FKProjectReference.Department == item.FKProjectReference.Unit ? string.Empty : " - " + hris.GetDepartmentDetails(item.FKProjectReference.Unit).Department),
                    ProjectCode = item.FKProjectReference.ProjectCode,
                    ProjectName = item.FKProjectReference.ProjectName,
                    ItemCode = item.FKItemReference.ItemCode,
                    ItemName = item.FKItemReference.ItemFullName,
                    ItemSpecifications = item.ItemSpecifications,
                    ProcurementSource = item.FKItemReference.ProcurementSource == ProcurementSources.PS_DBM ? "Procurement System - Department of Budget and Management" : "Private Suppliers",
                    InventoryType = item.FKItemReference.FKItemTypeReference.FKInventoryTypeReference.InventoryTypeName,
                    ItemCategory = item.FKItemReference.FKCategoryReference.ItemCategoryName,
                    PackagingUOMReference = item.FKItemReference.PackagingUOMReference == null ? string.Empty : item.FKItemReference.FKPackagingUnitReference.Abbreviation,
                    IndividualUOMReference = item.FKItemReference.IndividualUOMReference == null ? string.Empty : item.FKItemReference.FKIndividualUnitReference.Abbreviation,
                    TotalQty = item.PPMPQuantity,
                    EstimatedBudget = item.PPMPEstimatedBudget,
                    UnitCost = item.UnitCost,
                    DeliveryMonth = system.GetMonthName(item.FKProjectReference.ProjectMonthStart) + ", " + item.FKProjectReference.FiscalYear.ToString(),
                    PRNumber = item.PRReference == null ? null : db.PurchaseRequestHeader.Find(item.PRReference).PRNumber,
                    Status = item.Status
                });
            }

            return procurementItems;
        }
        public ProcurementProjectsVM GetProcurementProgramDetailsByPAPCode(string PAPCode)
        {
            var procurementProgram = db.APPDetails.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var procurementTimeline = db.ProcurementTimeline.Where(d => d.PAPCode == PAPCode).FirstOrDefault();
            var procurementItems = GetProjectItems(PAPCode);

            var procurementModes = procurementProgram.APPModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            var modesOfProcurement = string.Empty;
            for (int i = 0; i < procurementModes.Count; i++)
            {
                if (i == procurementModes.Count - 1)
                {
                    modesOfProcurement += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName;
                }
                else
                {
                    modesOfProcurement += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "\n";
                }
            }

            var procurementProject = new ProcurementProjectsVM()
            {
                IsTangible = procurementProgram.IsTangible,
                APPReference = procurementProgram.FKAPPHeaderReference.ReferenceNo,
                Month = procurementProgram.Month,
                PAPCode = procurementProgram.PAPCode,
                UACS = procurementProgram.ObjectClassification,
                ProcurementProgram = procurementProgram.ProcurementProgram,
                ApprovedBudget = procurementProgram.Total,
                ObjectClassification = abis.GetChartOfAccounts(procurementProgram.ObjectClassification).AcctName,
                FundCluster = procurementProgram.FundSourceReference,
                FundSource = abis.GetFundSources(procurementProgram.FundSourceReference).FUND_DESC,
                EndUser = hris.GetDepartmentDetails(procurementProgram.EndUser).Department,
                StartMonth = procurementProgram.StartMonth,
                EndMonth = procurementProgram.EndMonth,
                APPModeOfProcurement = modesOfProcurement,
                MOOETotal = procurementProgram.MOOEAmount,
                CapitalOutlayTotal = procurementProgram.COAmount,
                TotalEstimatedBudget = procurementProgram.Total,
                Remarks = procurementProgram.Remarks,
                ProjectCoordinator = procurementProgram.ProjectCoordinator == null ? null : hris.GetEmployeeDetailByCode(procurementProgram.ProjectCoordinator).EmployeeName,
                ProjectSupport = procurementProgram.ProjectSupport == null ? null : hris.GetEmployeeDetailByCode(procurementProgram.ProjectSupport).EmployeeName,
                ProjectStatus = procurementProgram.ProjectStatus,
                PurchaseRequestSubmission = procurementTimeline == null ? null : procurementTimeline.PurchaseRequestSubmission == null ? null : procurementTimeline.PurchaseRequestSubmission,
                PurchaseRequestClosing = procurementTimeline == null ? null : procurementTimeline.PurchaseRequestClosing == null ? null : procurementTimeline.PurchaseRequestClosing,
                ActualPreProcurementConference = procurementTimeline == null ? null : procurementTimeline.ActualPreProcurementConference,
                PreProcurementConferenceRemarks = procurementTimeline == null ? null : procurementTimeline.PreProcurementConferenceRemarks,
                ActualPostingOfIB = procurementTimeline == null ? null : procurementTimeline.ActualPostingOfIB,
                PostingOfIBRemarks = procurementTimeline == null ? null : procurementTimeline.PostingOfIBRemarks,
                ActualPreBidConference = procurementTimeline == null ? null : procurementTimeline.ActualPreBidConference,
                PreBidConferenceRemarks = procurementTimeline == null ? null : procurementTimeline.PreBidConferenceRemarks,
                PrelimenryExamination = procurementTimeline == null ? null : procurementTimeline.PrelimenryExamination,
                DetailedExamination = procurementTimeline == null ? null : procurementTimeline.DetailedExamination,
                EvaluationReporting = procurementTimeline == null ? null : procurementTimeline.EvaluationReporting,
                BidsExaminationRemarks = procurementTimeline == null ? null : procurementTimeline.BidsExaminationRemarks,
                ActualPostQualification = procurementTimeline == null ? null : procurementTimeline.ActualPostQualification,
                PostQualificationReportedToBAC = procurementTimeline == null ? null : procurementTimeline.PostQualificationReportedToBAC,
                PostQualificationRemarks = procurementTimeline == null ? null : procurementTimeline.PostQualificationRemarks,
                BACResolutionCreated = procurementTimeline == null ? null : procurementTimeline.BACResolutionCreated,
                BACMemberForwarded = procurementTimeline == null ? null : procurementTimeline.BACMemberForwarded,
                HOPEForwarded = procurementTimeline == null ? null : procurementTimeline.HOPEForwarded,
                PMOReceived = procurementTimeline == null ? null : procurementTimeline.PMOReceived,
                ActualNOAIssuance = procurementTimeline == null ? null : procurementTimeline.ActualNOAIssuance,
                NOASignedByHOPE = procurementTimeline == null ? null : procurementTimeline.NOASignedByHOPE,
                NOAReceivedBySupplier = procurementTimeline == null ? null : procurementTimeline.NOAReceivedBySupplier,
                NOAIssuanceRemarks = procurementTimeline == null ? null : procurementTimeline.NOAIssuanceRemarks,
                ProjectCost = procurementProgram.ProjectCost,
                Items = procurementItems
            };

            return procurementProject;
        }
        public List<ProcurementProjectsVM> GetUnassignedProcurementProgams(string ReferenceNo)
        {
            var procurementPrograms = new List<ProcurementProjectsVM>();
            var institutionalPrograms = db.APPDetails.Where(d => (d.ProjectCoordinator == null && d.ProjectSupport == null)).ToList();
            foreach(var program in institutionalPrograms)
            {
                var procurementModes = program.APPModeOfProcurementReference.Split("_".ToCharArray(), StringSplitOptions.None);
                var modesOfProcurement = string.Empty;
                for(var i = 0; i < procurementModes.Count(); i++)
                {
                    if(i == procurementModes.Count() - 1)
                    {
                        modesOfProcurement += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName;
                    }
                    else
                    {
                        modesOfProcurement += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "<br />";
                    }
                }
                
                procurementPrograms.Add(new ProcurementProjectsVM {
                    APPReference = program.FKAPPHeaderReference.ReferenceNo,
                    Month = program.Month,
                    PAPCode = program.PAPCode,
                    UACS = program.ObjectClassification,
                    ProcurementProgram = program.ProcurementProgram.ToUpper(),
                    ApprovedBudget = program.Total,
                    ObjectClassification = abis.GetChartOfAccounts(program.ObjectClassification).AcctName,
                    FundCluster = program.FundSourceReference.Replace("\r\n",""),
                    FundSource = abis.GetFundSources(program.FundSourceReference).FUND_DESC,
                    StartMonth = program.StartMonth,
                    EndMonth = program.EndMonth,
                    APPModeOfProcurement = modesOfProcurement,
                    ProjectCoordinator = program.ProjectCoordinator,
                    ProjectSupport = program.ProjectSupport
                });
            }

            return procurementPrograms;
        }
        public bool AssignProject(ProcurementProjectsVM ProcurementProject)
        {
            var institutionalProject = db.APPDetails.Where(d => d.PAPCode == ProcurementProject.PAPCode).FirstOrDefault();
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