using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public class PPMPByOfficeVM
    {
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }
    }
    public class PPMPReferences
    {
        [Display(Name = "Reference No.")]
        public string ReferenceNo { get; set; }
        [Display(Name = "Date Submitted")]
        public DateTime SubmittedAt { get; set; }
        [Display(Name = "Estimated Budget")]
        public decimal Amount { get; set; }
    }
    public class AccountLineItem
    {
        public string ProjectCode { get; set; }
        [Display(Name = "PPMP Reference No")]
        public string ReferenceNo { get; set; }
        [Display(Name = "Project Title")]
        public string ProjectTitle { get; set; }
        [Display(Name = "Proposal Type")]
        public BudgetProposalType ProposalType { get; set; }
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        [Display(Name = "Item Specification")]
        public string ItemSpecifications { get; set; }
        [Display(Name = "Is Tangible")]
        public bool IsTangible { get; set; }
        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }
        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Quantity")]
        public int ReducedQuantity { get; set; }
        [Display(Name = "Reduce Quantity?")]
        public bool ReduceQuantity { get; set; }
        [Display(Name = "Estimated Cost")]
        public decimal EstimatedCost { get; set; }
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }
        [Display(Name = "Approval Action")]
        public string ApprovalAction { get; set; }
    }
    public class PPMPEvaluationVM
    {
        public string UACS { get; set; }
        public string AccountTitle { get; set; }
        public List<PPMPReferences> PPMPReferences { get; set; }
        public List<AccountLineItem> NewSpendingItems { get; set; }
        [Display(Name = "Action")]
        public string ApprovalAction { get; set; }
        [Display(Name = "Office Code")]
        public string OfficeCode { get; set; }
        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }
        [Display(Name = "Total New Spending Proposal")]
        public decimal TotalProposedBudget { get; set; }
        [Display(Name = "Approved Budget")]
        public decimal ApprovedBudget { get; set; }
    }
    public class PPMPLineItemsPerAccountVM
    {
        [Display(Name = "UACS")]
        public string UACS { get; set; }
        [Display(Name = "Object Classification")]
        public string ObjectClassification { get; set; }
        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }
    }
}