using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public class ProjectPlanningDashboardVM
    {
        public List<int> ProjectFiscalYears { get; set; }
        public List<int> ProjectInfraFiscalYears { get; set; }
        public List<int> PPMPFiscalYears { get; set; }
        public int TotalNumberOfProjects { get; set; }
        public int TotalProjectsForwardedResponsibilityCenter { get; set; }
        public int TotalProjectsEvaluatedResponsibilityCenter { get; set; }
        public int NumberOfNewPPMP { get; set; }
        public int NumberOfPPMPs { get; set; }
        public int NumberOfApprovedPPMPs { get; set; }
        public string PercentageOfPosting { get; set; }
        public string PercentageOfSubmission { get; set; }
        public string PercentageOfApproval { get; set; }
        public string ProposedBudget { get; set; }
        public string ApprovedBudget { get; set; }
        public string OngoingBudget { get; set; }
        public List<SwitchBoardVM> Switchboard { get; set; }
    }
}