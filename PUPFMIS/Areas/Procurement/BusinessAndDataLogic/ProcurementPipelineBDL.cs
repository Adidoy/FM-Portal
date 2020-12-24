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
        public List<AnnualProcurementPlan> GetAPPs()
        {
            return db.APPHeader.Where(d => d.APPType != "CSE").ToList();
        }
        public ProcurementProjectsVM GetProcurementProgramDetailsByPAPCode(string PAPCode)
        {
            var procurementProject = new ProcurementProjectsVM();
            var institutionalProject = db.APPDetails.Where(d => d.PAPCode == PAPCode).FirstOrDefault();

            if(institutionalProject == null)
            {
                HttpNotFound();
            }

            var projectItems = new List<ProcurementProjectItemsVM>();
            var procurementModes = institutionalProject.APPModeOfProcurementReference.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
            var modesOfProcurement = string.Empty;
            for(int i = 0; i < procurementModes.Count(); i++)
            {
                if(i == procurementModes.Count() - 1)
                {
                    modesOfProcurement += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName; 
                }
                else
                {
                    modesOfProcurement += db.ProcurementModes.Find(int.Parse(procurementModes[i])).ModeOfProcurementName + "\n";
                }
            }

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
                APPModeOfProcurement = modesOfProcurement,
                MOOETotal = institutionalProject.MOOEAmount,
                CapitalOutlayTotal = institutionalProject.COAmount,
                TotalEstimatedBudget = institutionalProject.Total,
                Remarks = institutionalProject.Remarks,
                ProjectCoordinator = institutionalProject.ProjectCoordinator == null ? null : hris.GetEmployeeDetailByCode(institutionalProject.ProjectCoordinator).EmployeeName,
                ProjectSupport = institutionalProject.ProjectSupport == null ? null : hris.GetEmployeeDetailByCode(institutionalProject.ProjectSupport).EmployeeName,
                Items = projectItems
            };

            return procurementProject;
        }
        public bool SetSchedule(ProcurementProjectsVM ProcurementProject)
        {
            var procurementTimeline = new ProcurementTimeline
            {
                PAPCode = ProcurementProject.PAPCode,
                //PurchaseRequestSubmission = Convert.ToDateTime(ProcurementProject.Schedule.PurchaseRequestSubmission),
                //PreProcurementConference = Convert.ToDateTime(ProcurementProject.Schedule.PreProcurementConference),
                //PostingOfIB = Convert.ToDateTime(ProcurementProject.Schedule.PostingOfIB),
                //PreBidConference = Convert.ToDateTime(ProcurementProject.Schedule.PreBidConference),
                //SubmissionOfBids = Convert.ToDateTime(ProcurementProject.Schedule.SubmissionOfBids),
                //BidEvaluation = Convert.ToDateTime(ProcurementProject.Schedule.BidEvaluation),
                //PostQualification = Convert.ToDateTime(ProcurementProject.Schedule.PostQualification),
                //NOAIssuance = Convert.ToDateTime(ProcurementProject.Schedule.NOAIssuance),
                //ContractSigning = Convert.ToDateTime(ProcurementProject.Schedule.ContractSigning),
                //Approval = Convert.ToDateTime(ProcurementProject.Schedule.Approval),
                //NTPIssuance = Convert.ToDateTime(ProcurementProject.Schedule.NTPIssuance),
                //POReceived = Convert.ToDateTime(ProcurementProject.Schedule.POReceived)
            };
            //db.ProcurementTimeline.Add(procurementTimeline);
            //if(db.SaveChanges() == 0)
            //{
            //    return false;
            //}
            return true;
        }
        public List<ProcurementProgramsVM> GetUnassignedProcurementProgams(string ReferenceNo)
        {
            var procurementPrograms = new List<ProcurementProgramsVM>();
            var institutionalPrograms = db.APPDetails.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo && (d.ProjectCoordinator == null && d.ProjectSupport == null)).ToList();
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
                
                procurementPrograms.Add(new ProcurementProgramsVM {
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
                    ModeOfProcurement = modesOfProcurement,
                    ProjectCoordinator = program.ProjectCoordinator,
                    ProjectSupport = program.ProjectSupport
                });
            }

            return procurementPrograms;
        }
        public List<ProcurementProgramsVM> GetProcurementProgams(string ReferenceNo)
        {
            var procurementPrograms = new List<ProcurementProgramsVM>();
            var institutionalPrograms = db.APPDetails.Where(d => d.FKAPPHeaderReference.ReferenceNo == ReferenceNo
                                    && (d.ProjectCoordinator != null && d.ProjectSupport != null))
                                    .ToList();
            foreach (var program in institutionalPrograms)
            {
                var projectCoordinator = hris.GetEmployeeDetailByCode(program.ProjectCoordinator).EmployeeName;
                var projectSupport = hris.GetEmployeeDetailByCode(program.ProjectSupport).EmployeeName;
                //var procurementTimeline = db.ProcurementTimeline.Where(d => d.PAPCode == program.PAPCode).FirstOrDefault();

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
                    ProjectSupport = projectSupport,
                    //HasSchedule = procurementTimeline == null ? false : true
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