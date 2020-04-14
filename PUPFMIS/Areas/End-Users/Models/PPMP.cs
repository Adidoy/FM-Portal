using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace PUPFMIS.Models
{
    [Table("procurement_ppmpHeader")]
    public class PPMPHeader
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "PPMP Reference No.")]
        [MaxLength(75, ErrorMessage = "{0} must not exceed {1} characters.")]
        [Column(TypeName = "VARCHAR")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        [MaxLength(4)]
        [MinLength(4)]
        [Column(TypeName = "VARCHAR")]
        public string FiscalYear { get; set; }

        [Display(Name = "PPMP Type")]
        public int PPMPType { get; set; }

        [Required]
        [Display(Name = "Status")]
        [MaxLength(150)]
        [Column(TypeName = "VARCHAR")]
        public string Status { get; set; }

        [Display(Name = "Fund Source")]
        public int? FundSource { get; set; }

        [Display(Name = "Approved Budget of Contract")]
        public decimal? ABC { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime? SubmittedAt { get; set; }

        [Display(Name = "Date Approved")]
        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "Prepared By")]
        public int? PreparedBy { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }

        public int OfficeReference { get; set; }

        [ForeignKey("PPMPType")]
        public InventoryType FKPPMPTypeReference { get; set; }
    }
    [Table("procurement_ppmpItemDetails")]
    public class PPMPItemDetails
    {
        [Key]
        public int ID { get; set; }

        public int PPMPReference { get; set; }

        public int ItemReference { get; set; }

        public int ProjectPlanReference { get; set; }

        [MaxLength(100)]
        [Display(Name = "JAN")]
        [Column(TypeName = "VARCHAR")]
        public string JanMilestone { get; set; }

        [MaxLength(100)]
        [Display(Name = "FEB")]
        [Column(TypeName = "VARCHAR")]
        public string FebMilestone { get; set; }

        [MaxLength(100)]
        [Display(Name = "MAR")]
        [Column(TypeName = "VARCHAR")]
        public string MarMilestone { get; set; }

        [MaxLength(100)]
        [Display(Name = "APR")]
        [Column(TypeName = "VARCHAR")]
        public string AprMilestone { get; set; }

        [MaxLength(100)]
        [Display(Name = "MAY")]
        [Column(TypeName = "VARCHAR")]
        public string MayMilestone { get; set; }

        [MaxLength(100)]
        [Display(Name = "JUN")]
        [Column(TypeName = "VARCHAR")]
        public string JunMilestone { get; set; }

        [MaxLength(100)]
        [Display(Name = "JUL")]
        [Column(TypeName = "VARCHAR")]
        public string JulMilestone { get; set; }

        [MaxLength(100)]
        [Display(Name = "AUG")]
        [Column(TypeName = "VARCHAR")]
        public string AugMilestone { get; set; }

        [MaxLength(100)]
        [Display(Name = "SEP")]
        [Column(TypeName = "VARCHAR")]
        public string SepMilestone { get; set; }

        [MaxLength(100)]
        [Display(Name = "OCT")]
        [Column(TypeName = "VARCHAR")]
        public string OctMilestone { get; set; }

        [MaxLength(100)]
        [Display(Name = "NOV")]
        [Column(TypeName = "VARCHAR")]
        public string NovMilestone { get; set; }

        [MaxLength(100)]
        [Display(Name = "DEC")]
        [Column(TypeName = "VARCHAR")]
        public string DecMilestone { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? EstimatedBudget { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [ForeignKey("PPMPReference")]
        public virtual PPMPHeader FKPPMPReference { get; set; }

        [ForeignKey("ItemReference")]
        public virtual Item FKItem { get; set; }

        [ForeignKey("ProjectPlanReference")]
        public virtual ProjectPlans FKProjectPlanReference { get; set; }
    }
    [Table("procurement_ppmpServiceDetails")]
    public class PPMPServiceDetails
    {
        [Key]
        public int ID { get; set; }

        public int PPMPReference { get; set; }

        public int ServiceReference { get; set; }

        public int ProjectPlanReference { get; set; }

        [Display(Name = "Specifications/Terms of Reference")]
        [Column(TypeName = "VARCHAR")]
        public string Specifications { get; set; }

        [MaxLength(250)]
        [Display(Name = "JAN")]
        [Column(TypeName = "VARCHAR")]
        public string JanMilestone { get; set; }

        [MaxLength(250)]
        [Display(Name = "FEB")]
        [Column(TypeName = "VARCHAR")]
        public string FebMilestone { get; set; }

        [MaxLength(250)]
        [Display(Name = "MAR")]
        [Column(TypeName = "VARCHAR")]
        public string MarMilestone { get; set; }

        [MaxLength(250)]
        [Display(Name = "APR")]
        [Column(TypeName = "VARCHAR")]
        public string AprMilestone { get; set; }

        [MaxLength(250)]
        [Display(Name = "MAY")]
        [Column(TypeName = "VARCHAR")]
        public string MayMilestone { get; set; }

        [MaxLength(250)]
        [Display(Name = "JUN")]
        [Column(TypeName = "VARCHAR")]
        public string JunMilestone { get; set; }

        [MaxLength(250)]
        [Display(Name = "JUL")]
        [Column(TypeName = "VARCHAR")]
        public string JulMilestone { get; set; }

        [MaxLength(250)]
        [Display(Name = "AUG")]
        [Column(TypeName = "VARCHAR")]
        public string AugMilestone { get; set; }

        [MaxLength(250)]
        [Display(Name = "SEP")]
        [Column(TypeName = "VARCHAR")]
        public string SepMilestone { get; set; }

        [MaxLength(250)]
        [Display(Name = "OCT")]
        [Column(TypeName = "VARCHAR")]
        public string OctMilestone { get; set; }

        [MaxLength(250)]
        [Display(Name = "NOV")]
        [Column(TypeName = "VARCHAR")]
        public string NovMilestone { get; set; }

        [MaxLength(250)]
        [Display(Name = "DEC")]
        [Column(TypeName = "VARCHAR")]
        public string DecMilestone { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? EstimatedBudget { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [ForeignKey("PPMPReference")]
        public virtual PPMPHeader FKPPMPReference { get; set; }

        [ForeignKey("ServiceReference")]
        public virtual Services FKServiceReference { get; set; }

        [ForeignKey("ProjectPlanReference")]
        public virtual ProjectPlans FKProjectPlanReference { get; set; }
    }
    public class PPMPHeaderViewModel
    {
        public bool IsSelected { get; set; }

        [Required]
        [Display(Name = "PPMP Reference No.")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        [MaxLength(4)]
        public string FiscalYear { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string PPMPType { get; set; }

        [Required]
        [Display(Name = "Office")]
        public string Office { get; set; }

        [Required]
        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

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
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? EstimatedBudget { get; set; }

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
    [Table("workflow_ppmpapproval")]
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
        public int ActionMadeByOffice { get; set; }

        [ForeignKey("PPMPId")]
        public virtual PPMPHeader FKPPMPHeader { get; set; }
    }
    public enum AcceptanceCodes
    {
        [Display(Name = "Accepted")]
        Accepted = 0,
        [Display(Name = "Reduce Quantity")]
        ReduceQuantity = 1
    }
}