using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("BS_R_PapCode")]
    public class Programs
    {
        [Key]
        public decimal ID { get; set; }

        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "General Description")]
        public string GeneralDescription { get; set; }

        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [Display(Name = "Parent ID")]
        public decimal? ParentID { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }
    }

    public class PPMPOfficeListVM
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
    public class PPMPListVM
    {
        [Display(Name = "Reference No.")]
        public string ReferenceNo { get; set; }

        [Display(Name = "PPMP Type")]
        public PPMPTypes PPMPType { get; set; }

        [Display(Name = "PPMP Status")]
        public PPMPStatus PPMPStatus { get; set; }

        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Display(Name = "Object Classification")]
        public string ObjectClassification { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }
    }
    public class PPMPEvaluationVM
    {
        [Display(Name = "Reference No.")]
        public string ReferenceNo { get; set; }

        [Display(Name = "PPMP Type")]
        public string PPMPType { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Display(Name = "Account Title")]
        public string AccountTitle { get; set; }

        [Display(Name = "Sector")]
        public string Sector { get; set; }

        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        public List<PPMPProjectsVM> Projects { get; set; }
    }
    public class PPMPProjectsVM
    {
        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "Program")]
        public string Program { get; set; }

        [Display(Name = "DepartmentCode")]
        public string DepatmentCode { get; set; }

        [Display(Name = "Unit / Office")]
        public string UnitName { get; set; }

        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Description")]
        public string Description { get; set; }

        [Display(Name = "Delivery Month")]
        public string DeliveryMonth { get; set; }

        public List<PPMPProjectDetailsVM> Details { get; set; }
    }
    public class PPMPProjectDetailsVM
    {
        [Required]
        public int PPMPDetailID { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Full Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ItemFullName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        public BudgetProposalType ProposalType { get; set; }

        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "Justification")]
        public string Justification { get; set; }

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

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Display(Name = "Action")]
        public BudgetOfficeAction BudgetOfficeAction { get; set; }

        [Display(Name = "Reason for Revision")]
        public BudgetOfficeReasonForRevision ReasonForNonAcceptance { get; set; }
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


    public enum BudgetOfficeAction
    {
        [Display(Name = "Accepted")]
        Accepted = 0,

        [Display(Name = "Needs Revision")]
        ForRevision = 1,

        [Display(Name = "Not Accepted")]
        NotAccepted = 2
    }
    public enum BudgetOfficeReasonForRevision
    {
        [Display(Name = "Unnecessary for the Project")]
        UnnecessaryForProject,

        [Display(Name = "Excessive / Unreasonable Quantity")]
        ExcessiveQuantity,

        [Display(Name = "Unacceptable Justification")]
        UnacceptableJustification,

        [Display(Name = "Not Aligned with Program")]
        NotAlignedWithProgram
    }
    public enum BudgetProposalType
    {
        [Display(Name = "Actual Obligation/Existing Project")]
        Actual = 0,
        [Display(Name = "New Spending Proposal")]
        NewProposal = 1
    }
}