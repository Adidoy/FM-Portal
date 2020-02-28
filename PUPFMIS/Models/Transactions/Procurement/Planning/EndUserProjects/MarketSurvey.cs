//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("planning_marketSurveyProperty")]
//    public class MarketSurvey
//    {
//        [Key]
//        public int ID { get; set; }
        
//        public int ProjectReference { get; set; }

//        public int? ItemReference { get; set; }

//        [Display(Name = "Scope of Work / Terms of Reference / Specifications")]
//        public string ItemSpecifications { get; set; }

//        public int? ExpenditureObject { get; set; }

//        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
//        [Display(Name = "Q1 Qty.")]
//        public int Qtr1 { get; set; }

//        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
//        [Display(Name = "Q2 Qty.")]
//        public int Qtr2 { get; set; }

//        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
//        [Display(Name = "Q3 Qty.")]
//        public int Qtr3 { get; set; }

//        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
//        [Display(Name = "Q4 Qty.")]
//        public int Qtr4 { get; set; }

//        [Display(Name = "Total Qty.")]
//        public int TotalQty { get; set; }

//        public int? Supplier1Reference { get; set; }

//        [Display(Name = "Is sole distributor?")]
//        public bool IsSoleDistributor { get; set; }

//        [Display(Name = "Unit Cost")]
//        public decimal? Supplier1UnitCost { get; set; }

//        [Display(Name = "Estimated Budget")]
//        public decimal? Supplier1EstimatedBudget { get; set; }

//        public int? Supplier2Reference { get; set; }

//        [Display(Name = "Unit Cost")]
//        public decimal? Supplier2UnitCost { get; set; }

//        [Display(Name = "Estimated Budget")]
//        public decimal? Supplier2EstimatedBudget { get; set; }

//        public int? Supplier3Reference { get; set; }

//        [Display(Name = "Unit Cost")]
//        public decimal? Supplier3UnitCost { get; set; }

//        [Display(Name = "Estimated Budget")]
//        public decimal? Supplier3EstimatedBudget { get; set; }

//        [Display(Name = "Total Estimated Budget")]
//        public decimal? TotalEstimatedBudget { get; set; }

//        public int? ExpenditureSubClassReference { get; set; }

//        [ForeignKey("Supplier1Reference")]
//        public virtual Supplier FkSupplierReference1 { get; set; }

//        [ForeignKey("Supplier2Reference")]
//        public virtual Supplier FkSupplierReference2 { get; set; }

//        [ForeignKey("Supplier3Reference")]
//        public virtual Supplier FkSupplierReference3 { get; set; }

//        [ForeignKey("ProjectReference")]
//        [Display(Name = "Project Code")]
//        public virtual EndUserProjectHeader FKProjectReference { get; set; }

//        [ForeignKey("ItemReference")]
//        [Display(Name = "Item Information")]
//        public virtual Item FKItem { get; set; }
//    }
//}