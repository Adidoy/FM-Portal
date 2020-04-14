using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("planning_projectPlans")]
    public class ProjectPlans
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Project Code")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20, ErrorMessage = "{0} is up to {1} characters only.")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        [Column(TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        public string ProjectName { get; set; }

        [Display(Name = "Description")]
        [Column(TypeName = "VARCHAR")]
        [DataType(DataType.MultilineText)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        public string Description { get; set; }

        [Display(Name = "Fiscal Year")]
        [Column(TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MinLength(4, ErrorMessage = "{0} field must consist of {1} characters only.")]
        [MaxLength(4, ErrorMessage = "{0} field must consist of {1} characters only.")]
        public string FiscalYear { get; set; }

        [Display(Name = "Office")]
        public int? Office { get; set; }

        [Display(Name = "Prepared By")]
        public int? PreparedBy { get; set; }

        [Display(Name = "Submitted By")]
        public int? SubmittedBy { get; set; }

        [Display(Name = "Project Status")]
        [Column(TypeName = "VARCHAR")]
        public string ProjectStatus { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Start Month")]
        public int ProjectMonthStart { get; set; }

        [Display(Name = "Total Estimated Budget")]
        public decimal? TotalEstimatedBudget { get; set; }

        [Required]
        [Display(Name = "Is Deleted?")]
        public bool PurgeFlag { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }
    }

    [Table("planning_projectPlanItems")]
    public class ProjectPlanItems
    {
        [Key, Column(Order = 0)]
        [Display(Name = "Project")]
        public int ProjectReference { get; set; }

        [Key, Column(Order = 1)]
        [Display(Name = "Item")]
        public int ItemReference { get; set; }

        [Key, Column(Order = 2)]
        public BudgetProposalType ProposalType { get; set; }

        [Display(Name = "JAN")]
        public int? JanQty { get; set; }

        [Display(Name = "FEB")]
        public int? FebQty { get; set; }

        [Display(Name = "MAR")]
        public int? MarQty { get; set; }

        [Display(Name = "APR")]
        public int? AprQty { get; set; }

        [Display(Name = "MAY")]
        public int? MayQty { get; set; }

        [Display(Name = "JUN")]
        public int? JunQty { get; set; }

        [Display(Name = "JUL")]
        public int? JulQty { get; set; }

        [Display(Name = "AUG")]
        public int? AugQty { get; set; }

        [Display(Name = "SEP")]
        public int? SepQty { get; set; }

        [Display(Name = "OCT")]
        public int? OctQty { get; set; }

        [Display(Name = "NOV")]
        public int? NovQty { get; set; }

        [Display(Name = "DEC")]
        public int? DecQty { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Supplier 1")]
        public int Supplier1 { get; set; }

        [Display(Name = "Supplier 1 Unit Cost")]
        public decimal Supplier1UnitCost { get; set; }

        [Display(Name = "Supplier 2")]
        public int? Supplier2 { get; set; }

        [Display(Name = "Supplier 2 Unit Cost")]
        public decimal? Supplier2UnitCost { get; set; }

        [Display(Name = "Supplier 3")]
        public int? Supplier3 { get; set; }

        [Display(Name = "Supplier 3 Unit Cost")]
        public decimal? Supplier3UnitCost { get; set; }

        [Display(Name = "Item Specifications")]
        [ForeignKey("ItemReference")]
        public virtual Item FKItemReference { get; set; }

        [ForeignKey("ProjectReference")]
        public virtual ProjectPlans FKProjectReference { get; set; }
    }

    [Table("planning_projectPlanServices")]
    public class ProjectPlanServices
    {
        [Key, Column(Order = 0)]
        [Display(Name = "Project")]
        public int ProjectReference { get; set; }

        [Key, Column(Order = 1)]
        [Display(Name = "Service")]
        public int ServiceReference { get; set; }

        [Key, Column(Order = 2)]
        public BudgetProposalType ProposalType { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Quantity/Size")]
        public int Quantity { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Supplier 1")]
        public int Supplier1 { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier1UnitCost { get; set; }

        [Display(Name = "Supplier 2")]
        public int? Supplier2 { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier2UnitCost { get; set; }

        [Display(Name = "Supplier 3")]
        public int? Supplier3 { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier3UnitCost { get; set; }

        [Display(Name = "Service Specifications")]
        [ForeignKey("ServiceReference")]
        public virtual Services FKServiceReference { get; set; }

        [ForeignKey("ProjectReference")]
        public virtual ProjectPlans FKProjectReference { get; set; }
    }

    public class ProjectPlanItemsVM
    {
        [Display(Name = "ProjectCode")]
        public string ProjectCode { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "Inventory Type")]
        public string InventoryType { get; set; }

        [Display(Name = "Category")]
        public string ItemCategory { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Packaging UOM")]
        public string PackagingUOMReference { get; set; }

        [Display(Name = "Individual UOM")]
        public string IndividualUOMReference { get; set; }

        [Display(Name = "Minimum Issuance Quantity")]
        public int? MinimumIssuanceQty { get; set; }
        
        [Display(Name = "Actual Obligation (Previous Year)")]
        public int? ActualObligation { get; set; }

        [Display(Name = "Quantity per Package")]
        public int? DistributionQtyPerPack { get; set; }

        [Display(Name = "Proposal Type")]
        public BudgetProposalType ProposalType { get; set; }

        [Display(Name = "JAN")]
        public int? JanQty { get; set; }

        [Display(Name = "FEB")]
        public int? FebQty { get; set; }

        [Display(Name = "MAR")]
        public int? MarQty { get; set; }

        [Display(Name = "APR")]
        public int? AprQty { get; set; }

        [Display(Name = "MAY")]
        public int? MayQty { get; set; }

        [Display(Name = "JUN")]
        public int? JunQty { get; set; }

        [Display(Name = "JUL")]
        public int? JulQty { get; set; }

        [Display(Name = "AUG")]
        public int? AugQty { get; set; }

        [Display(Name = "SEP")]
        public int? SepQty { get; set; }

        [Display(Name = "OCT")]
        public int? OctQty { get; set; }

        [Display(Name = "NOV")]
        public int? NovQty { get; set; }

        [Display(Name = "DEC")]
        public int? DecQty { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [AllowHtml]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? UnitCost { get; set; }

        [Display(Name = "Supplier 1 ID")]
        public int Supplier1ID { get; set; }

        [Display(Name = "Supplier 1")]
        public string Supplier1Name { get; set; }

        [Display(Name = "Supplier 1 Address")]
        public string Supplier1Address { get; set; }

        [Display(Name = "Supplier 1 Contact No.")]
        public string Supplier1ContactNo { get; set; }

        [Display(Name = "Supplier 1 Email Address")]
        public string Supplier1EmailAddress { get; set; }

        [Display(Name = "Supplier 1 Unit Cost")]
        public decimal Supplier1UnitCost { get; set; }

        [Display(Name = "Supplier 2 ID")]
        public int? Supplier2ID { get; set; }

        [Display(Name = "Supplier 2")]
        public string Supplier2Name { get; set; }

        [Display(Name = "Supplier 2 Address")]
        public string Supplier2Address { get; set; }

        [Display(Name = "Supplier 2 Contact No.")]
        public string Supplier2ContactNo { get; set; }

        [Display(Name = "Supplier 2 Email Address")]
        public string Supplier2EmailAddress { get; set; }

        [Display(Name = "Supplier 2 Unit Cost")]
        public decimal? Supplier2UnitCost { get; set; }

        [Display(Name = "Supplier 3 ID")]
        public int? Supplier3ID { get; set; }

        [Display(Name = "Supplier 3")]
        public string Supplier3Name { get; set; }

        [Display(Name = "Supplier 3 Address")]
        public string Supplier3Address { get; set; }

        [Display(Name = "Supplier 3 Contact No.")]
        public string Supplier3ContactNo { get; set; }

        [Display(Name = "Supplier 3 Email Address")]
        public string Supplier3EmailAddress { get; set; }

        [Display(Name = "Supplier 3 Unit Cost")]
        public decimal? Supplier3UnitCost { get; set; }
    }

    public class ProjectPlanListVM
    {
        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Office")]
        public string Office { get; set; }

        [Display(Name = "Project Status")]
        public string ProjectStatus { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }
    }

    public class ProjectPlanVM
    {
        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Fiscal Year")]
        public string FiscalYear { get; set; }

        [Display(Name = "Office")]
        public string Office { get; set; }

        [Display(Name = "Prepared By")]
        public string PreparedBy { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }

        [Display(Name = "Project Status")]
        public string ProjectStatus { get; set; }

        [Display(Name = "Start Month")]
        public string ProjectMonthStart { get; set; }

        [Display(Name = "Total Estimated Budget")]
        public decimal? TotalEstimatedBudget { get; set; }

        public List<ProjectPlanItemsVM> ProjectPlanItems { get; set; }
        public List<ProjectPlanItemsVM> NewItemProposals { get; set; }
    }

    public enum BudgetProposalType
    {
        [Display(Name = "Actual Obligation/Existing Project")]
        Actual = 0,
        [Display(Name = "New Spending Proposal")]
        NewProposal = 1
    }
}