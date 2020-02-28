using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("planning_projectprocurementplan")]
    public class ProjectProcurementPlan
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(20, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(4, ErrorMessage = "{0} field must consist of {1} characters only.")]
        [MinLength(4, ErrorMessage = "{0} field must consist of {1} characters only.")]
        [Display(Name = "Fiscal Year")]
        public string FiscalYear { get; set; }

        [Display(Name = "Office")]
        public int? Office { get; set; }

        [Display(Name = "PreparedBy")]
        public int? PreparedBy { get; set; }

        [Display(Name = "SubmittedBy")]
        public string SubmittedBy { get; set; }

        [Display(Name = "Project Status")]
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

    [Table("planning_projectprocurementplanitems")]
    public class ProjectProcurementPlanItems
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Project")]
        public int ProjectReference { get; set; }

        [Display(Name = "Item")]
        public int ItemReference { get; set; }

        [Required]
        [Display(Name = "Qtr 1")]
        public int Qtr1 { get; set; }

        [Required]
        [Display(Name = "Qtr 2")]
        public int Qtr2 { get; set; }

        [Required]
        [Display(Name = "Qtr 3")]
        public int Qtr3 { get; set; }

        [Required]
        [Display(Name = "Qtr 4")]
        public int Qtr4 { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Item Specifications")]
        [ForeignKey("ItemReference")]
        public virtual Item FKItemReference { get; set; }

        [ForeignKey("ProjectReference")]
        public virtual ProjectProcurementPlan FKProjectReference { get; set; }
    }

    [Table("planning_marketsurvey")]
    public class MarketSurvey
    {
        [Key]
        public int ProjectItemReference { get; set; }

        [Display(Name = "Is sole distributor?")]
        public bool IsSoleDistributor { get; set; }

        [Display(Name = "Supplier 1")]
        public int? Supplier1Reference { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier1UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? Supplier1EstimatedBudget { get; set; }

        [Display(Name = "Supplier 2")]
        public int? Supplier2Reference { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier2UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? Supplier2EstimatedBudget { get; set; }

        [Display(Name = "Supplier 3")]
        public int? Supplier3Reference { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier3UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? Supplier3EstimatedBudget { get; set; }

        [Display(Name = "Total Estimated Budget")]
        public decimal? TotalEstimatedBudget { get; set; }

        [ForeignKey("Supplier1Reference")]
        public virtual Supplier FkSupplierReference1 { get; set; }

        [ForeignKey("Supplier2Reference")]
        public virtual Supplier FkSupplierReference2 { get; set; }

        [ForeignKey("Supplier3Reference")]
        public virtual Supplier FkSupplierReference3 { get; set; }

        [ForeignKey("ProjectItemReference")]
        public virtual ProjectProcurementPlanItems FKProjectItemReference { get; set; }
    }

    [Table("planning_projectprocurementplanservices")]
    public class ProjectProcurementPlanServices
    {
        [Key, Column(Order = 0)]
        public int ProjectReference { get; set; }

        [Key, Column(Order = 1)]
        public int ItemReference { get; set; }

        [Required]
        [Display(Name = "Qtr 1")]
        public int Qtr1 { get; set; }

        [Required]
        [Display(Name = "Qtr 2")]
        public int Qtr2 { get; set; }

        [Required]
        [Display(Name = "Qtr 3")]
        public int Qtr3 { get; set; }

        [Required]
        [Display(Name = "Qtr 4")]
        public int Qtr4 { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? EstimatedBudget { get; set; }

        [Display(Name = "Item Specifications")]
        [ForeignKey("ItemReference")]
        public virtual Item FKItemReference { get; set; }

        [ForeignKey("ProjectReference")]
        public virtual ProjectProcurementPlan FKProjectReference { get; set; }
    }

    public class ProjectsProcurementHeaderViewModel
    {
        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Purpose")]
        [DataType(DataType.MultilineText)]
        public string Purpose { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(4, ErrorMessage = "{0} field must consist of {1} characters only.")]
        [MinLength(4, ErrorMessage = "{0} field must consist of {1} characters only.")]
        [Display(Name = "Fiscal Year")]
        public string FiscalYear { get; set; }

        [Display(Name = "Project Status")]
        public string ProjectStatus { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Start Month")]
        public int ProjectMonthStart { get; set; }

        [Display(Name = "Total Estimated Budget")]
        public decimal? TotalEstimatedBudget { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }
    }

    public class ProjectProcurementViewModel
    {
        public ProjectProcurementPlan Header { get; set; }
        public List<ProjectProcurementPlanItems> Items { get; set; }
    }
}