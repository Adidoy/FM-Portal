using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class DashboardDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private SystemBDL systemBDL = new SystemBDL();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();

        public List<int> GetProjectFiscalYears(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var FiscalYears = db.ProjectPlans.Where(d => d.Department == user.DepartmentCode).GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
            return FiscalYears;
        }
        //public List<int> GetInfraRequestFiscalYears(string UserEmail)
        //{
        //    var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
        //    var FiscalYears = db.ProjectInfrastructureRequest.Where(d => d.Department == user.DepartmentCode).GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
        //    return FiscalYears;
        //}
        public List<int> GetPPMPFiscalYears(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var office = hrisDataAccess.GetDepartmentDetails(user.DepartmentCode);
            var FiscalYears = db.PPMPHeader.Where(d => d.Department == office.DepartmentCode).GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
            return FiscalYears;
        }
        public int GetTotalNumberOfProjects(string UserEmail)
        {
            var fiscalYear = db.ProjectPlans.OrderByDescending(d => d.FiscalYear).FirstOrDefault();
            if (fiscalYear != null)
            {
                var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
                return db.ProjectPlans.Where(d => (user.DepartmentCode == user.UnitCode ? d.Department == user.UnitCode : d.Unit == user.UnitCode) && d.FiscalYear <= DateTime.Now.Year).Count();
            }
            else
            {
                return 0;
            }
        }
        public int GetNumberOfProjectsForwardedToResponsibilityCenters(string UserEmail)
        {
            var fiscalYear = db.ProjectPlans.Any() ? db.ProjectPlans.GroupBy(d => d.FiscalYear).Select(d => new { FiscalYear = d.Key }).OrderByDescending(d => d.FiscalYear).Select(d => d.FiscalYear).ToList() : null;
            if (fiscalYear != null)
            {
                var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
                var count = db.ProjectPlans.Where(d => (user.DepartmentCode == user.UnitCode ? d.Department == user.UnitCode : d.Unit == user.UnitCode) && d.ProjectStatus >= ProjectStatus.ForwardedToResponsibilityCenter && fiscalYear.Contains(d.FiscalYear)).Count();
                return count;
            }
            else
            {
                return 0;
            }
        }
        public int GetNumberOfProjectsEvaluatedByResponsibilityCenters(string UserEmail)
        {
            var fiscalYear = db.ProjectPlans.Any() ? db.ProjectPlans.GroupBy(d => d.FiscalYear).Select(d => new { FiscalYear = d.Key }).OrderByDescending(d => d.FiscalYear).Select(d => d.FiscalYear).ToList() : null;
            if (fiscalYear != null)
            {
                var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
                var office = hrisDataAccess.GetDepartmentDetails(user.DepartmentCode);
                return db.ProjectPlans.Where(d => (user.DepartmentCode == user.UnitCode ? d.Department == user.UnitCode : d.Unit == user.UnitCode) && d.ProjectStatus >= ProjectStatus.EvaluatedByResponsibilityCenter && fiscalYear.Contains(d.FiscalYear)).Count();
            }
            else
            {
                return 0;
            }
        }
        public int GetNumberOfNewPPMP(string UserEmail)
        {
            var fiscalYear = db.ProjectPlans.OrderByDescending(d => d.FiscalYear).FirstOrDefault();
            if (fiscalYear != null)
            {
                var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
                var office = hrisDataAccess.GetDepartmentDetails(user.DepartmentCode);
                return db.PPMPHeader.Where(d => d.Department == office.DepartmentCode && d.PPMPStatus == PPMPStatus.NewPPMP && d.FiscalYear == fiscalYear.FiscalYear).Count();
            }
            else
            {
                return 0;
            }
        }
        public int GetNumberOfPPMPs(string UserEmail)
        {
            var FiscalYear = db.PPMPHeader.Any() ? db.PPMPHeader.GroupBy(d => d.FiscalYear).Select(d => new { FiscalYear = d.Key }).OrderByDescending(d => d.FiscalYear).Select(d => d.FiscalYear).ToList() : null;
            if (FiscalYear != null)
            {
                var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
                var office = hrisDataAccess.GetDepartmentDetails(user.DepartmentCode);
                return db.PPMPHeader.Where(d => d.Department == office.DepartmentCode && FiscalYear.Contains(d.FiscalYear)).Count();
            }
            else
            {
                return 0;
            }
        }
        public int GetNumberOfApprovedPPMPs(string UserEmail)
        {
            var FiscalYear = db.PPMPHeader.Any() ? db.PPMPHeader.GroupBy(d => d.FiscalYear).Select(d => new { FiscalYear = d.Key }).OrderByDescending(d => d.FiscalYear).Select(d => d.FiscalYear).ToList() : null;
            if (FiscalYear != null)
            {
                var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
                var office = hrisDataAccess.GetDepartmentDetails(user.DepartmentCode);
                return db.PPMPHeader.Where(d => d.Department == office.DepartmentCode && d.PPMPStatus == PPMPStatus.EvaluatedByBudgetOffice && FiscalYear.Contains(d.FiscalYear)).Count();
            }
            else
            {
                return 0;
            }
        }
        public string GetProposedBudget(string UserEmail)
        {
            var FiscalYear = db.PPMPHeader.Any() ? db.PPMPHeader.GroupBy(d => d.FiscalYear).Select(d => new { FiscalYear = d.Key }).OrderByDescending(d => d.FiscalYear).Select(d => d.FiscalYear).ToList() : null;
            if (FiscalYear != null)
            {
                //var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
                //var office = hrisDataAccess.GetDepartmentDetails(user.DepartmentCode);
                //var projectItems = db.ProjectPlanItems.Where(d => (office.DepartmentCode == null ? d.FKPPMPReference.Department == office.DepartmentCode : d.FKPPMPReference.Unit == office.DepartmentCode) && FiscalYear.Contains(d.FKPPMPReference.FiscalYear)).ToList();
                //var projectService = db.ProjectPlanServices.Where(d => (office.DepartmentCode == null ? d.FKPPMPReference.Department == office.DepartmentCode : d.FKPPMPReference.Unit == office.DepartmentCode) && FiscalYear.Contains(d.FKPPMPReference.FiscalYear)).ToList();
                //var proposedItemsBudget = projectItems.Count != 0 ? projectItems.Sum(d => d.ProjectEstimatedBudget) : 0.00m;
                //var proposedServicesBudget = projectService.Count != 0 ? projectService.Sum(d => d.ProjectEstimatedBudget) : 0.00m;
                //var estimatedBudget = (proposedItemsBudget + proposedServicesBudget).ToString("C", new System.Globalization.CultureInfo("en-ph"));
                //return estimatedBudget;
                return "0.00";
            }
            return (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph"));
        }
        //public string GetApprovedBudget(string UserEmail)
        //{
        //    var FiscalYear = db.PPMPHeader.Any() ? db.PPMPHeader.GroupBy(d => d.FiscalYear).Select(d => new { FiscalYear = d.Key }).OrderByDescending(d => d.FiscalYear).Select(d => d.FiscalYear).ToList() : null;
        //    if (FiscalYear != null)
        //    {
        //        var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
        //        var office = hrisDataAccess.GetDepartmentDetails(user.DepartmentCode);
        //        var budget = db.PPMPHeader.Where(d => (office.DepartmentCode == null ? d.Department == office.DepartmentCode : d.Unit == office.DepartmentCode) && d.Status == "PPMP Evaluated" && FiscalYear.Contains(d.FiscalYear)).Sum(d => (decimal?)d.ABC);
        //        var approvedBudget = (budget == null) ? (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph")) : ((decimal)budget).ToString("C", new System.Globalization.CultureInfo("en-ph"));
        //        return approvedBudget;
        //    }
        //    return (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph"));
        //}
        //public string GetOngoingBudget(string UserEmail)
        //{
        //    var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
        //    var office = hrisDataAccess.GetDepartmentDetails(user.DepartmentCode);
        //    var budget = db.PPMPHeader.Where(d => (office.DepartmentCode == null ? d.Department == office.DepartmentCode : d.Unit == office.DepartmentCode) && d.Status == "Procurement On-going").Sum(d => (decimal?)d.ABC);
        //    var ongoingBudget = (budget == null) ? (0.00m).ToString("C", new System.Globalization.CultureInfo("en-ph")) : ((decimal)budget).ToString("C", new System.Globalization.CultureInfo("en-ph"));
        //    return ongoingBudget;
        //}
        public List<SwitchBoardVM> GetSwitchBoard(string UserEmail)
        {
            var employee = hrisDataAccess.GetEmployee(UserEmail);
            var switchBoardList = (from switchBoard in db.SwitchBoard
                                   join switchBoardBody in db.SwitchBoardBody on switchBoard.ID equals switchBoardBody.SwitchBoardReference
                                   where switchBoard.DepartmentCode == employee.DepartmentCode
                                   select new
                                   {
                                       MessageType = switchBoard.MessageType,
                                       Reference = switchBoard.Reference,
                                       Subject = switchBoard.Subject,
                                       UpdatedAt = switchBoardBody.UpdatedAt
                                   } into results
                                   group results by new { results.MessageType, results.Reference, results.Subject } into groupedResults
                                   select new
                                   {
                                       MessageType = groupedResults.Key.MessageType,
                                       Reference = groupedResults.Key.Reference,
                                       Subject = groupedResults.Key.Subject,
                                       UpdatedAt = groupedResults.Max(d => d.UpdatedAt)
                                   }).OrderByDescending(d => d.UpdatedAt).ToList();

            List<SwitchBoardVM> switchBoardVMList = new List<SwitchBoardVM>();
            foreach (var item in switchBoardList.OrderByDescending(d => d.UpdatedAt).ToList())
            {
                switchBoardVMList.Add(new SwitchBoardVM
                {
                    MessageType = item.MessageType,
                    Reference = item.Reference,
                    Subject = item.Subject,
                    UpdatedAt = item.UpdatedAt.ToString("dd MMMM yyyy hh:mm:ss tt")
                });
            }
            return switchBoardVMList;
        }
        public List<SwitchBoardBodyVM> GetSwitchBoardBody(string ReferenceNo)
        {
            var switchBoardBodyList = db.SwitchBoardBody.Where(d => d.FKSwitchBoardReference.Reference == ReferenceNo)
                                        .Select(d => new
                                        {
                                            UpdatedAt = d.UpdatedAt,
                                            Remarks = d.Remarks,
                                            Department = d.DepartmentCode,
                                            ActionBy = d.ActionBy
                                        }).ToList();

            List<SwitchBoardBodyVM> switchBoardBodyVMList = new List<SwitchBoardBodyVM>();
            foreach (var item in switchBoardBodyList)
            {
                switchBoardBodyVMList.Add(new SwitchBoardBodyVM
                {
                    UpdatedAt = item.UpdatedAt.ToString("dd MMMM yyyy hh:mm:ss tt"),
                    Remarks = item.Remarks,
                    Department = hrisDataAccess.GetDepartmentDetails(item.Department).Department,
                    ActionBy = hrisDataAccess.GetEmployeeByCode(item.ActionBy).EmployeeName
                });
            }

            return switchBoardBodyVMList.ToList();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hrisDataAccess.Dispose();
                systemBDL.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}