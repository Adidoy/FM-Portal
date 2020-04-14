using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{


    public class PPMPSubmittedItems
    {
        [Required]
        [Display(Name = "Item ID")]
        public int ItemID { get; set; }

        [Required]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Short Specifications")]
        public string ItemShortSpecifications { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Full Specifications")]
        [DataType(DataType.MultilineText)]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Required]
        public decimal QtyPerPackage { get; set; }

        [Required]
        public int Qtr1 { get; set; }

        [Required]
        public int Qtr2 { get; set; }

        [Required]
        public int Qtr3 { get; set; }

        [Required]
        public int Qtr4 { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? EstimatedBudget { get; set; }

        public int BulkQty { get; set; }

        public string BulkUnit { get; set; }

        [Display(Name = "Unit")]
        public string IndividualUnit { get; set; }
    }

    public class PPMPDistributionList
    {
        [Display(Name = "PPMP Reference No.")]
        public string ReferenceNo { get; set; }

        [Display(Name = "Office")]
        public string Office { get; set; }

        [Required]
        public int Qtr1 { get; set; }

        [Required]
        public int Qtr2 { get; set; }

        [Required]
        public int Qtr3 { get; set; }

        [Required]
        public int Qtr4 { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }
    }
}