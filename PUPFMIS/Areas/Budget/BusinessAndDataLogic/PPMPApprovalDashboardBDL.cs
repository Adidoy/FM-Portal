using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class PPMPApprovalDashboardDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();
        private ABISDataAccess abisDataAccess = new ABISDataAccess();

        public List<int> GetPPMPFiscalYears()
        {
            var FiscalYears = db.PPMPHeader.Where(d => d.Status == "PPMP Submitted").GroupBy(d => d.FiscalYear).Select(d => d.Key).ToList();
            return FiscalYears;
        }
        public int GetBudgetProposalsSubmitted()
        {
            return db.PPMPHeader.Where(d => d.Status == "PPMP Submitted").GroupBy(d => d.Department).Count();
        }
        public int GetPPMPsToEvaluate()
        {
            return db.PPMPHeader.Where(d => d.Status == "PPMP Submitted").Count();
        }
        public int GetBudgetProposalsReviewed()
        {
            return db.PPMPHeader.Where(d => d.Status == "PPMP Evaluated").GroupBy(d => d.Department).Count();
        }
        public int GetPPMPsEvaluated()
        {
            return db.PPMPHeader.Where(d => d.Status == "PPMP Evaluated").Count();
        }
        public decimal GetProposedCapitalOutlayBudget()
        {
            var CapitalOutlayItems = db.ProjectPlanItems.Where(d => d.UnitCost >= 15000.00m && (d.Status == "Posted" || d.Status == "Approved" || d.Status == "Disapproved")).Count() != 0 ? db.ProjectPlanItems.Where(d => d.UnitCost >= 15000.00m && (d.Status == "Posted" || d.Status == "Approved" || d.Status == "Disapproved")).Sum(d => d.ProjectEstimatedBudget) : 0.00m;
            var CapitalOutlayServices = db.ProjectPlanServices.Where(d => d.UnitCost >= 15000.00m && (d.Status == "Posted" || d.Status == "Approved" || d.Status == "Disapproved")).Count() != 0 ? db.ProjectPlanServices.Where(d => d.UnitCost >= 15000.00m && (d.Status == "Posted" || d.Status == "Approved" || d.Status == "Disapproved")).Sum(d => d.ProjectEstimatedBudget) : 0.00m;
            return (CapitalOutlayItems + CapitalOutlayServices);
        }
        public decimal GetProposeMOOEBudget()
        {
            var MOOEItems = db.ProjectPlanItems.Where(d => d.UnitCost < 15000.00m && (d.Status == "Posted" || d.Status == "Approved" || d.Status == "Disapproved")).Count() != 0 ? db.ProjectPlanItems.Where(d => d.UnitCost < 15000.00m && (d.Status == "Posted" || d.Status == "Approved" || d.Status == "Disapproved")).Sum(d => d.ProjectEstimatedBudget) : 0.00m;
            var MMOOEServices = db.ProjectPlanServices.Where(d => d.UnitCost < 15000.00m && (d.Status == "Posted" || d.Status == "Approved" || d.Status == "Disapproved")).Count() != 0 ? db.ProjectPlanServices.Where(d => d.UnitCost < 15000.00m && (d.Status == "Posted" || d.Status == "Approved" || d.Status == "Disapproved")).Sum(d => d.ProjectEstimatedBudget) : 0.00m;
            return (MOOEItems + MMOOEServices);
        }
        public decimal GetApprovedCapitalOutlayBudget()
        {
            var CapitalOutlayItems = db.ProjectPlanItems.Where(d => (d.APPReference != null || d.Status == "Approved") && d.UnitCost >= 15000.00m).Count() != 0 ? db.ProjectPlanItems.Where(d => (d.Status == "Approved" || d.APPReference != null) && d.UnitCost >= 15000.00m).Sum(d => d.PPMPEstimatedBudget) : 0.00m;
            var CapitalOutlayServices = db.ProjectPlanServices.Where(d => (d.APPReference != null || d.Status == "Approved") && d.UnitCost >= 15000.00m).Count() != 0 ? db.ProjectPlanServices.Where(d => (d.Status == "Approved" || d.APPReference != null) && d.UnitCost >= 15000.00m).Sum(d => d.PPMPEstimatedBudget) : 0.00m;
            return (CapitalOutlayItems + CapitalOutlayServices);
        }
        public decimal GetApprovedMOOEBudget()
        {
            var MOOEItems = db.ProjectPlanItems.Where(d => (d.APPReference != null || d.Status == "Approved") && d.UnitCost < 15000.00m).Count() != 0 ? db.ProjectPlanItems.Where(d => (d.APPReference != null || d.Status == "Approved") && d.UnitCost < 15000.00m).Sum(d => d.PPMPEstimatedBudget) : 0.00m;
            var MOOEServices = db.ProjectPlanServices.Where(d => (d.APPReference != null || d.Status == "Approved") && d.UnitCost < 15000.00m).Count() != 0 ? db.ProjectPlanServices.Where(d => (d.APPReference != null || d.Status == "Approved") && d.UnitCost < 15000.00m).Sum(d => d.PPMPEstimatedBudget) : 0.00m;
            return (MOOEItems + MOOEServices);
        }
        public List<MOOEViewModel> GetMOOESummary(int FiscalYear)
        {
            var MOOEItemsActual = db.ProjectPlanItems
                            .Where(d => d.FKPPMPReference.FiscalYear == FiscalYear && d.ProposalType == BudgetProposalType.Actual && ((d.PPMPReference != null && d.Status == "Approved") || (d.APPReference != null && d.Status == "Posted to APP")) && d.UnitCost < 15000.00m)
                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass })
                            .Select(d => new { UACS = d.Key.AccountClass, Tier1 = d.Sum(x => x.PPMPEstimatedBudget), Tier2 = 0.00m })
                            .ToList();
            var MOOEServicesActual = db.ProjectPlanServices
                            .Where(d => d.FKPPMPReference.FiscalYear == FiscalYear && d.ProposalType == BudgetProposalType.Actual && ((d.PPMPReference != null && d.Status == "Approved") || (d.APPReference != null && d.Status == "Posted to APP")) && d.UnitCost < 15000.00m)
                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass })
                            .Select(d => new { UACS = d.Key.AccountClass, Tier1 = d.Sum(x => x.PPMPEstimatedBudget), Tier2 = 0.00m })
                            .ToList();
            var MOOEItems = db.ProjectPlanItems
                            .Where(d => d.FKPPMPReference.FiscalYear == FiscalYear && ((d.PPMPReference != null && d.Status == "Approved") || (d.APPReference != null && d.Status == "Posted to APP")) && d.UnitCost < 15000.00m)
                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass })
                            .Select(d => new { UACS = d.Key.AccountClass, Tier1 = 0.00m, Tier2 = d.Sum(x => x.PPMPEstimatedBudget) })
                            .ToList();
            var MOOEServices = db.ProjectPlanServices
                            .Where(d => d.FKPPMPReference.FiscalYear == FiscalYear && ((d.PPMPReference != null && d.Status == "Approved") || (d.APPReference != null && d.Status == "Posted to APP")) && d.UnitCost < 15000.00m)
                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass })
                            .Select(d => new { UACS = d.Key.AccountClass, Tier1 = 0.00m, Tier2 = d.Sum(x => x.PPMPEstimatedBudget) })
                            .ToList();

            var MOOEGroup = MOOEItemsActual.Union(MOOEItems).Union(MOOEServices).Union(MOOEServicesActual).ToList();
            var MOOE = new List<MOOEViewModel>();

            foreach(var item in MOOEGroup)
            {
                MOOE.Add(new MOOEViewModel {
                    UACS = item.UACS,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == item.UACS).FirstOrDefault().AcctName,
                    Tier1 = item.Tier1,
                    Tier2 = item.Tier2,
                    TotalProposedProgram = item.Tier1 + item.Tier2
                });
            }
            return MOOE;
        }
        public List<CapitalOutlayVM> GetCaptialOutlaySummary(int FiscalYear)
        {
            var COItemsActual = db.ProjectPlanItems
                            .Where(d => d.FKPPMPReference.FiscalYear == FiscalYear && d.ProposalType == BudgetProposalType.Actual && ((d.PPMPReference != null && d.Status == "Approved") || (d.APPReference != null && d.Status == "Posted to APP")) && d.UnitCost >= 15000.00m)
                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass })
                            .Select(d => new { UACS = d.Key.AccountClass, Tier1 = d.Sum(x => x.PPMPEstimatedBudget), Tier2 = 0.00m })
                            .ToList();
            var COServicesActual = db.ProjectPlanServices
                            .Where(d => d.FKPPMPReference.FiscalYear == FiscalYear && d.ProposalType == BudgetProposalType.Actual && ((d.PPMPReference != null && d.Status == "Approved") || (d.APPReference != null && d.Status == "Posted to APP")) && d.UnitCost >= 15000.00m)
                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass })
                            .Select(d => new { UACS = d.Key.AccountClass, Tier1 = d.Sum(x => x.PPMPEstimatedBudget), Tier2 = 0.00m })
                            .ToList();
            var COItems = db.ProjectPlanItems
                            .Where(d => d.FKPPMPReference.FiscalYear == FiscalYear && ((d.PPMPReference != null && d.Status == "Approved") || (d.APPReference != null && d.Status == "Posted to APP")) && d.UnitCost >= 15000.00m)
                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass })
                            .Select(d => new { UACS = d.Key.AccountClass, Tier1 = 0.00m, Tier2 = d.Sum(x => x.PPMPEstimatedBudget) })
                            .ToList();
            var COServices = db.ProjectPlanServices
                            .Where(d => d.FKPPMPReference.FiscalYear == FiscalYear && ((d.PPMPReference != null && d.Status == "Approved") || (d.APPReference != null && d.Status == "Posted to APP")) && d.UnitCost >= 15000.00m)
                            .GroupBy(d => new { d.FKItemReference.FKItemTypeReference.AccountClass })
                            .Select(d => new { UACS = d.Key.AccountClass, Tier1 = 0.00m, Tier2 = d.Sum(x => x.PPMPEstimatedBudget) })
                            .ToList();

            var COGroup = COItems.Union(COServices).Union(COItemsActual).Union(COServicesActual).ToList();
            var CO = new List<CapitalOutlayVM>();

            foreach (var item in COGroup)
            {
                CO.Add(new CapitalOutlayVM
                {
                    UACS = item.UACS,
                    ObjectClassification = abisDataAccess.GetDetailedChartOfAccounts().Where(d => d.UACS_Code == item.UACS).FirstOrDefault().AcctName,
                    Tier1 = item.Tier1,
                    Tier2 = item.Tier2,
                    TotalProposedProgram = item.Tier1 + item.Tier2
                });
            }
            return CO;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            hrisDataAccess.Dispose();
            abisDataAccess.Dispose();
        }
    }
}