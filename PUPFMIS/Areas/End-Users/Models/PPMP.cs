using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROC_TRXN_PPMP")]
    public class PPMPHeader
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "PPMP Reference No.")]
        [MaxLength(75, ErrorMessage = "{0} must not exceed {1} characters.")]
        public string ReferenceNo { get; set; }

        [Display(Name = "Fiscal Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public int FiscalYear { get; set; }

        [Required]
        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Project Name")]
        [MaxLength(175, ErrorMessage = "{0} must not exceed {1} characters.")]
        public string ProjectName { get; set; }

        [Required]
        [Display(Name = "PPMP Type")]
        public PPMPTypes PPMPType { get; set; }

        [Required]
        [Display(Name = "PPMP Status")]
        public PPMPStatus PPMPStatus { get; set; }

        [Required]
        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [Required]
        [Display(Name = "Sector")]
        public string Sector { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "Is Institutional PPMP?")]
        public bool IsInstitutional { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime? SubmittedAt { get; set; }

        [Display(Name = "Date Approved")]
        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "Prepared By")]
        public string PreparedBy { get; set; }

        [Display(Name = "Designation")]
        public string PreparedByDesignation { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }

        [Display(Name = "Designation")]
        public string SubmittedByDesignation { get; set; }

        [Display(Name = "Evaluated By")]
        public string EvaluatedBy { get; set; }

        [Display(Name = "Designation")]
        public string EvaluatedByDesignation { get; set; }

        [Display(Name = "Is Infrastructure?")]
        public bool IsInfrastructure { get; set; }
    }
    [Table("PROC_TRXN_PPMP_Details")]
    public class PPMPDetails
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Required]
        public int ProjectDetailsID { get; set; }

        public int PPMPHeaderReference { get; set; }

        public int? APPDetailReference { get; set; }

        public int? ClassificationReference { get; set; }

        [Display(Name = "Article")]
        public int? ArticleReference { get; set; }

        [MaxLength(2)]
        public string ItemSequence { get; set; }

        [Display(Name = "Item Full Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ItemFullName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        public BudgetProposalType ProposalType { get; set; }

        public ProcurementSources ProcurementSource { get; set; }

        [Required]
        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        [Display(Name = "Category")]
        public int? CategoryReference { get; set; }

        [AllowHtml]
        [Display(Name = "Justification")]
        public string Justification { get; set; }

        [Display(Name = "JAN")]
        public PPMPMilestones? JANMilestone { get; set; }

        [Display(Name = "FEB")]
        public PPMPMilestones? FEBMilestone { get; set; }

        [Display(Name = "MAR")]
        public PPMPMilestones? MARMilestone { get; set; }

        [Display(Name = "APR")]
        public PPMPMilestones? APRMilestone { get; set; }

        [Display(Name = "MAY")]
        public PPMPMilestones? MAYMilestone { get; set; }

        [Display(Name = "JUN")]
        public PPMPMilestones? JUNMilestone { get; set; }

        [Display(Name = "JUL")]
        public PPMPMilestones? JULMilestone { get; set; }

        [Display(Name = "AUG")]
        public PPMPMilestones? AUGMilestone { get; set; }

        [Display(Name = "SEP")]
        public PPMPMilestones? SEPMilestone { get; set; }

        [Display(Name = "OCT")]
        public PPMPMilestones? OCTMilestone { get; set; }

        [Display(Name = "NOV")]
        public PPMPMilestones? NOVMilestone { get; set; }

        [Display(Name = "DEC")]
        public PPMPMilestones? DECMilestone { get; set; }

        [Display(Name = "JAN")]
        public int JanQty { get; set; }

        [Display(Name = "FEB")]
        public int FebQty { get; set; }

        [Display(Name = "MAR")]
        public int MarQty { get; set; }

        [Required]
        [Display(Name = "Q1 Total")]
        public int Q1TotalQty { get; set; }

        [Display(Name = "APR")]
        public int AprQty { get; set; }

        [Display(Name = "MAY")]
        public int MayQty { get; set; }

        [Display(Name = "JUN")]
        public int JunQty { get; set; }

        [Required]
        [Display(Name = "Q2 Total")]
        public int Q2TotalQty { get; set; }

        [Display(Name = "JUL")]
        public int JulQty { get; set; }

        [Display(Name = "AUG")]
        public int AugQty { get; set; }

        [Display(Name = "SEP")]
        public int SepQty { get; set; }

        [Required]
        [Display(Name = "Q3 Total")]
        public int Q3TotalQty { get; set; }

        [Display(Name = "OCT")]
        public int OctQty { get; set; }

        [Display(Name = "NOV")]
        public int NovQty { get; set; }

        [Display(Name = "DEC")]
        public int DecQty { get; set; }

        [Required]
        [Display(Name = "Q4 Total")]
        public int Q4TotalQty { get; set; }

        [Display(Name = "Total Quantity")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Required]
        [Display(Name = "PPMP Detail Status")]
        public PPMPDetailStatus PPMPDetailStatus { get; set; }

        [Display(Name = "Budget Office Action")]
        public BudgetOfficeAction? BudgetOfficeAction { get; set; }

        [Display(Name = "Reason for Revision")]
        public BudgetOfficeReasonForRevision? BudgetOfficeReasonForNonAcceptance { get; set; }

        [Required]
        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Display(Name = "Contract Type")]
        public ProcurementProjectTypes? ProcurementProjectType { get; set; }

        [Display(Name = "Contract Reference")]
        public int? ProcurementProject { get; set; }

        public int? PurchaseRequestReference { get; set; }

        [ForeignKey("PPMPHeaderReference")]
        public virtual PPMPHeader FKPPMPHeaderReference { get; set; }

        [ForeignKey("APPDetailReference")]
        public virtual AnnualProcurementPlanDetails FKAPPDetailReference { get; set; }

        [ForeignKey("ProjectDetailsID")]
        public virtual ProjectDetails FKProjectDetailsReference { get; set; }

        [Display(Name = "Article")]
        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKItemArticleReference { get; set; }

        [ForeignKey("UOMReference")]
        [Display(Name = "Unit of Measure")]
        public virtual UnitOfMeasure FKUOMReference { get; set; }

        [ForeignKey("CategoryReference")]
        [Display(Name = "Category")]
        public virtual ItemCategory FKCategoryReference { get; set; }

        [ForeignKey("ClassificationReference")]
        [Display(Name = "Classification")]
        public virtual ItemClassification FKClassification { get; set; }

        [ForeignKey("ProcurementProject")]
        [Display(Name = "Contract Reference")]
        public virtual ProcurementProject FKProcurementProject { get; set; }

        [ForeignKey("PurchaseRequestReference")]
        [Display(Name = "Purchase Request Reference")]
        public virtual PurchaseRequestHeader FKPurchaseRequestReference { get; set; }

        [Required]
        public bool UpdateFlag { get; set; }
    }

    public class PPMPDashboard
    {
        public int TotalNoOfPPMPs { get; set; }
        public int TotalOrigianalPPMPs { get; set; }
        public int TotalSupplementalPPMPs { get; set; }
        public List<int> FiscalYears { get; set; }
    }
    public class PPMPHeaderVM
    {
        public bool IsSelected { get; set; }

        public bool IsInfrastructure { get; set; }

        [Required]
        [Display(Name = "PPMP Reference No.")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "PPMP Name")]
        public string ProjectName { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "PPMP Type")]
        public PPMPTypes PPMPType { get; set; }

        [Required]
        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Required]
        [Display(Name = "Classification")]
        public string Classification { get; set; }

        [Required]
        [Display(Name = "Sector")]
        public string Sector { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "Unit")]
        public string Unit { get; set; }

        [Required]
        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [Display(Name = "Approved Budget")]
        public decimal ApprovedBudget { get; set; }

        [Display(Name = "Status")]
        public PPMPStatus Status { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Evaluated")]
        public DateTime? EvaluatedAt { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime? SubmittedAt { get; set; }

        [Display(Name = "Prepared By")]
        public string PreparedBy { get; set; }

        [Display(Name = "Prepared By")]
        public string PreparedByDesignation { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedByDesignation { get; set; }
    }
    public class PPMPDetailsVM
    {
        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Item Type")]
        public string ItemType { get; set; }

        [Display(Name = "Article Code")]
        public string ArticleCode { get; set; }

        [Display(Name = "Sequence")]
        public string ItemSequence { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemFullName { get; set; }

        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "Individual UOM")]
        public string UOMReference { get; set; }

        [Display(Name = "No. of Items Posted to APP")]
        public int NoOfPostedToAPP { get; set; }

        [Display(Name = "No. of Accepted")]
        public int NoOfAccepted { get; set; }

        [Display(Name = "No. of For Revision")]
        public int NoOfForRevision { get; set; }

        [Display(Name = "No. of Not Accepted")]
        public int NoOfNotAccepted { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Justification")]
        public string Justification { get; set; }

        [Display(Name = "PPMP Item Status")]
        public BudgetOfficeAction PPMPItemStatus { get; set; }

        [Display(Name = "JAN")]
        public string JANMilestone { get; set; }

        [Display(Name = "FEB")]
        public string FEBMilestone { get; set; }

        [Display(Name = "MAR")]
        public string MARMilestone { get; set; }

        [Display(Name = "APR")]
        public string APRMilestone { get; set; }

        [Display(Name = "MAY")]
        public string MAYMilestone { get; set; }

        [Display(Name = "JUN")]
        public string JUNMilestone { get; set; }

        [Display(Name = "JUL")]
        public string JULMilestone { get; set; }

        [Display(Name = "AUG")]
        public string AUGMilestone { get; set; }

        [Display(Name = "SEP")]
        public string SEPMilestone { get; set; }

        [Display(Name = "OCT")]
        public string OCTMilestone { get; set; }

        [Display(Name = "NOV")]
        public string NOVMilestone { get; set; }

        [Display(Name = "DEC")]
        public string DECMilestone { get; set; }

        [Display(Name = "JAN")]
        public int JanQty { get; set; }

        [Display(Name = "FEB")]
        public int FebQty { get; set; }

        [Display(Name = "MAR")]
        public int MarQty { get; set; }

        [Required]
        [Display(Name = "Q1 Total")]
        public int Q1TotalQty { get; set; }

        [Display(Name = "APR")]
        public int AprQty { get; set; }

        [Display(Name = "MAY")]
        public int MayQty { get; set; }

        [Display(Name = "JUN")]
        public int JunQty { get; set; }

        [Required]
        [Display(Name = "Q2 Total")]
        public int Q2TotalQty { get; set; }

        [Display(Name = "JUL")]
        public int JulQty { get; set; }

        [Display(Name = "AUG")]
        public int AugQty { get; set; }

        [Display(Name = "SEP")]
        public int SepQty { get; set; }

        [Required]
        [Display(Name = "Q3 Total")]
        public int Q3TotalQty { get; set; }

        [Display(Name = "OCT")]
        public int OctQty { get; set; }

        [Display(Name = "NOV")]
        public int NovQty { get; set; }

        [Display(Name = "DEC")]
        public int DecQty { get; set; }

        [Required]
        [Display(Name = "Q4 Total")]
        public int Q4TotalQty { get; set; }

        [Display(Name = "Total Quantity")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Required]
        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [Display(Name = "Supplier 1")]
        public int Supplier1 { get; set; }

        [Display(Name = "Supplier 1 Name")]
        public string Supplier1Name { get; set; }

        [Display(Name = "Supplier 1 Address")]
        public string Supplier1Address { get; set; }

        [Display(Name = "Supplier 1 Contact No.")]
        public string Supplier1ContactNo { get; set; }

        [Display(Name = "Supplier 1 Email Address")]
        public string Supplier1EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal Supplier1UnitCost { get; set; }

        [Display(Name = "Supplier 2")]
        public int? Supplier2 { get; set; }

        [Display(Name = "Supplier 2 Name")]
        public string Supplier2Name { get; set; }

        [Display(Name = "Supplier 2 Address")]
        public string Supplier2Address { get; set; }

        [Display(Name = "Supplier 2 Contact No.")]
        public string Supplier2ContactNo { get; set; }

        [Display(Name = "Supplier 2 Email Address")]
        public string Supplier2EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier2UnitCost { get; set; }

        [Display(Name = "Supplier 3")]
        public int? Supplier3 { get; set; }

        [Display(Name = "Supplier 3 Name")]
        public string Supplier3Name { get; set; }

        [Display(Name = "Supplier 3 Address")]
        public string Supplier3Address { get; set; }

        [Display(Name = "Supplier 3 Contact No.")]
        public string Supplier3ContactNo { get; set; }

        [Display(Name = "Supplier 3 Email Address")]
        public string Supplier3EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier3UnitCost { get; set; }
    }
    public class PPMPViewModel
    {
        public PPMPHeaderVM Header { get; set; }
        public List<PPMPDetailsVM> Details { get; set; }
    }
    public class PPMPItemVM
    {
        [Display(Name = "Reference No.")]
        public string ReferenceNo { get; set; }

        public string ProposalType { get; set; }

        [Display(Name = "Item Type")]
        public string ItemType { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public string ProcurementSource { get; set; }

        [Display(Name = "Account Class")]
        public string AccountClass { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? EstimatedBudget { get; set; }

        [Display(Name = "Individual UOM")]
        public string IndividualUOMReference { get; set; }
    }
    public class PPMPProjectItemVM
    {
        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "Program")]
        public string Program { get; set; }

        [Display(Name = "Unit/Office")]
        public string UnitName { get; set; }

        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Description")]
        public string Description { get; set; }

        [Display(Name = "Justification")]
        public string Justification { get; set; }

        [Display(Name = "Delivery Month")]
        public int DeliveryMonth { get; set; }

        [Display(Name = "Item Status")]
        public PPMPDetailStatus ItemStatus { get; set; }

        [Display(Name = "Action")]
        public BudgetOfficeAction? BudgetOfficeAction { get; set; }

        [Display(Name = "Reason for Non-Approval")]
        public BudgetOfficeReasonForRevision? ReasonForNonAcceptance { get; set; }

        [Required]
        [Display(Name = "JAN")]
        public int JanQty { get; set; }

        [Required]
        [Display(Name = "FEB")]
        public int FebQty { get; set; }

        [Required]
        [Display(Name = "MAR")]
        public int MarQty { get; set; }

        [Required]
        [Display(Name = "APR")]
        public int AprQty { get; set; }

        [Required]
        [Display(Name = "MAY")]
        public int MayQty { get; set; }

        [Required]
        [Display(Name = "JUN")]
        public int JunQty { get; set; }

        [Required]
        [Display(Name = "JUL")]
        public int JulQty { get; set; }

        [Required]
        [Display(Name = "AUG")]
        public int AugQty { get; set; }

        [Required]
        [Display(Name = "SEP")]
        public int SepQty { get; set; }

        [Required]
        [Display(Name = "OCT")]
        public int OctQty { get; set; }

        [Required]
        [Display(Name = "NOV")]
        public int NovQty { get; set; }

        [Required]
        [Display(Name = "DEC")]
        public int DecQty { get; set; }

        [Required]
        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        public bool UpdateFlag { get; set; }
    }
    public class PPMPItemDetailsVM
    {
        public PPMPItemVM Item { get; set; }
        public List<PPMPProjectItemVM> ProjectPlans { get; set; }
    }

    public class MOOEViewModel
    {
        [Display(Name = "UACS")]
        public string UACS { get; set; }
        [Display(Name = "Sub Classification")]
        public string SubClassification { get; set; }
        [Display(Name = "Object Classification")]
        public string ObjectClassification { get; set; }
        [Display(Name = "Tier 1")]
        public decimal Tier1 { get; set; }
        [Display(Name = "Tier 2")]
        public decimal Tier2 { get; set; }
        [Display(Name = "Total Proposed Program")]
        public decimal TotalProposedProgram { get; set; }
    }
    public class CapitalOutlayVM
    {
        [Display(Name = "UACS")]
        public string UACS { get; set; }
        [Display(Name = "Sub Classification")]
        public string SubClassification { get; set; }
        [Display(Name = "Object Classification")]
        public string ObjectClassification { get; set; }
        [Display(Name = "Tier 1")]
        public decimal Tier1 { get; set; }
        [Display(Name = "Tier 2")]
        public decimal Tier2 { get; set; }
        [Display(Name = "Total Proposed Program")]
        public decimal TotalProposedProgram { get; set; }
    }
    public class BudgetPropsalVM
    {
        //public int FiscalYear { get; set; }
        //public string OfficeCode { get; set; }
        //public string OfficeName { get; set; }
        //public string SubmittedBy { get; set; }
        //public string Designation { get; set; }
        //public string SubmittedAt { get; set; }
        public List<MOOEViewModel> MOOE { get; set; }
        public List<CapitalOutlayVM> CaptialOutlay { get; set; }
        public List<PPMPHeaderVM> PPMPList { get; set; }
        public decimal TotalProposedBudget { get; set; }
    }

    public enum PPMPTypes
    {
        [Display(Name = "Common-Use Office Supplies", ShortName = "CUOS")]
        CommonUse = 0,

        [Display(Name = "Original", ShortName = "ORIG")]
        Original = 1,

        [Display(Name = "Supplemental", ShortName = "SUPP")]
        Supplemental = 2,

        [Display(Name = "Amendatory", ShortName = "AMND")]
        Amendatory = 3,

    }
    public enum PPMPStatus
    {
        [Display(Name = "New PPMP")]
        NewPPMP = 0,

        [Display(Name = "Forwarded to Budget Office")]
        ForwardedToBudgetOffice = 1,

        [Display(Name = "Evaluated by Budget Office")]
        EvaluatedByBudgetOffice = 2,

        [Display(Name = "Returned for Revision")]
        ReturnedForRevision = 3,

        [Display(Name = "Posted to APP")]
        PostedToAPP = 4
    }
    public enum PPMPDetailStatus
    {
        [Display(Name = "Posted to PPMP")]
        PostedToPPMP = 0,

        [Display(Name = "For Evaluation")]
        ForEvaluation = 1,

        [Display(Name = "Item Accepted")]
        ItemAccepted = 2,

        [Display(Name = "For Revision")]
        ForRevision = 3,

        [Display(Name = "Item Revised")]
        ItemRevised = 4,

        [Display(Name = "Item Not Accepted")]
        ItemNotAccepted = 5,

        [Display(Name = "Posted to APP")]
        PostedToAPP = 6,

        [Display(Name = "Posted to Contract Project")]
        PostedToProcurementProject = 7,

        [Display(Name = "Posted to Purchase Request")]
        PostedToPurchaseRequest = 8,

        [Display(Name = "Posted to Purchase Order / Agency Procurement Request / Contract")]
        PostedToPurchaseOrder = 9,

        [Display(Name = "Item Delivered")]
        ItemDelivered = 10
    }
    public enum PPMPMilestones
    {
        [Display(Name = "P/R Preparation / Pre-Procurement Conference (OCT (Previous Year))\nProcurement Activities (NOV-DEC (Previous Year))\nDelivery/Preparation of RIS", ShortName = "EPA-OCT")]
        PRPreparationOCT,

        [Display(Name = "P/R Preparation / Pre-Procurement Conference (NOV (Previous Year))\nProcurement Activities (DEC (Previous Year))\nDelivery/Preparation of RIS", ShortName = "EPA-NOV")]
        PRPreparationNOV,

        [Display(Name = "P/R Preparation / Pre-Procurement Conference (DEC (Previous Year))\nDelivery/Preparation of RIS", ShortName = "EPA-DEC")]
        PRPreparationDEC,

        [Display(Name = "P/R Preparation / Pre-Procurement Conference", ShortName = "PRPC")]
        PRPreparation,

        [Display(Name = "Procurement Activities", ShortName = "PRAC")]
        ProcurementActivities,

        [Display(Name = "Delivery/Preparation of RIS", ShortName = "DRIS")]
        DeliveryRISPreparation
    }
}