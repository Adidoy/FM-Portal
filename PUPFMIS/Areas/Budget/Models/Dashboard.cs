using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public class PPMPApprovalDashboardVM
    {
        public List<int> PPMPFiscalYears { get; set; }
        public int BudgetProposalsSubmitted { get; set; }
        public int PPMPsToReview { get; set; }
        public int BudgetProposalsReviewed { get; set; }
        public int PPMPEvaluated { get; set; }
        public decimal ProposedCapitalOutlayBudget { get; set; }
        public decimal ProposedMOOEBudget { get; set; }
        public decimal ApprovedCapitalOutlayBudget { get; set; }
        public decimal ApprovedMOOEBudget { get; set; }
        public List<MOOEViewModel> MOOE { get; set; }
        public List<CapitalOutlayVM> CapitalOutlay { get; set; }
    }
}