using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace PUPFMIS.Models
{
    [Table("PP_PPMP_HEADER")]
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

        [Display(Name = "PPMP Type")]
        public int PPMPType { get; set; }

        [Required]
        [MaxLength(150)]
        [Display(Name = "Status")]
        [Column(TypeName = "VARCHAR")]
        public string Status { get; set; }

        [Display(Name = "ABC")]
        public decimal ABC { get; set; }

        public string Sector { get; set; }

        public string Department { get; set; }

        public string Unit { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime? SubmittedAt { get; set; }

        [Display(Name = "Date Approved")]
        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "Prepared By")]
        public string PreparedBy { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }

        [ForeignKey("PPMPType")]
        public virtual InventoryType FKPPMPTypeReference { get; set; }
    }
    [Table("PP_WORKFLOW_PPMP_APPROVAL")]
    public class PPMPApprovalWorkflow
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int PPMPId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Date Updated")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Action by")]
        public int ActionMadeBy { get; set; }

        [Required]
        [Display(Name = "Office")]
        public string ActionMadeByOffice { get; set; }

        [ForeignKey("PPMPId")]
        public virtual PPMPHeader FKPPMPHeader { get; set; }
    }

    public class PPMPHeaderViewModel
    {
        public bool IsSelected { get; set; }

        [Required]
        [Display(Name = "PPMP Reference No.")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string PPMPType { get; set; }

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
        public string Status { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime? SubmittedAt { get; set; }

        [Display(Name = "Prepared By")]
        public string PreparedBy { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }
    }
    public class PPMPItemDetailsVM
    {
        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project")]
        public string Project { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Individual UOM")]
        public string IndividualUOMReference { get; set; }

        [Display(Name = "JAN")]
        public string JanMilestone { get; set; }

        [Display(Name = "FEB")]
        public string FebMilestone { get; set; }

        [Display(Name = "MAR")]
        public string MarMilestone { get; set; }

        [Display(Name = "APR")]
        public string AprMilestone { get; set; }

        [Display(Name = "MAY")]
        public string MayMilestone { get; set; }

        [Display(Name = "JUN")]
        public string JunMilestone { get; set; }

        [Display(Name = "JUL")]
        public string JulMilestone { get; set; }

        [Display(Name = "AUG")]
        public string AugMilestone { get; set; }

        [Display(Name = "SEP")]
        public string SepMilestone { get; set; }

        [Display(Name = "OCT")]
        public string OctMilestone { get; set; }

        [Display(Name = "NOV")]
        public string NovMilestone { get; set; }

        [Display(Name = "DEC")]
        public string DecMilestone { get; set; }

        [Display(Name = "Total Qty.")]
        public string TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public string UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

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

        [Display(Name = "Responsibility Center")]
        public string ResponsibilityCenter { get; set; }
    }
    public class PPMPViewModel
    {
        public PPMPHeaderViewModel Header { get; set; }
        public List<PPMPItemDetailsVM> DBMItems { get; set; }
        public List<PPMPItemDetailsVM> NonDBMItems { get; set; }
        public List<PPMPApprovalWorkflowViewModel> Workflow { get; set; }
    }
    public class PPMPApprovalWorkflowViewModel
    {
        public int PPMPId { get; set; }

        [Display(Name = "Reference No.")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Date Updated")]
        public DateTime UpdatedAt { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Office")]
        public string Office { get; set; }

        [Required]
        [Display(Name = "Personnel")]
        public string Personnel { get; set; }
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
        public int FiscalYear { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string SubmittedBy { get; set; }
        public string Designation { get; set; }
        public string SubmittedAt { get; set; }
        public List<MOOEViewModel> MOOE { get; set; }
        public List<CapitalOutlayVM> CaptialOutlay { get; set; }
        public List<PPMPHeaderViewModel> PPMPList { get; set; }
        public decimal TotalProposedBudget { get; set; }
        public bool CanPost { get; set; }
    }
}