using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public class Catalogue
    {
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

        [Display(Name = "Category")]
        public string ItemCategory { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Unit of Measure")]
        public string IndividualUOMReference { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Minimum Issuance Quantity")]
        public decimal MinimumIssuanceQty { get; set; }
    }

    public class Basket
    {
        public int ItemID { get; set; }

        [Required]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Short Specifications")]
        public string ItemShortSpecifications { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Full Specifications")]
        [DataType(DataType.MultilineText)]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "Category")]
        public string ItemCategory { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Unit of Measure")]
        public string IndividualUOMReference { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Minimum Issuance Quantity")]
        public decimal MinimumIssuanceQty { get; set; }

        [Display(Name = "Qtr 1")]
        public int? Qtr1Qty { get; set; }

        [Display(Name = "Qtr 2")]
        public int? Qtr2Qty { get; set; }

        [Display(Name = "Qtr 3")]
        public int? Qtr3Qty { get; set; }

        [Display(Name = "Qtr 4")]
        public int? Qtr4Qty { get; set; }

        [Display(Name = "Total Qty")]
        public int? TotalQty { get; set; }

        [Display(Name = "Total Previous Consumption")]
        public int? TotalConsumption { get; set; }

        [MaxLength(150)]
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
    }

    public class BasketValidator : AbstractValidator<Basket>
    {
        private FMISDbContext db = new FMISDbContext();

        public BasketValidator()
        {
            RuleFor(x => new { x.Qtr1Qty, x.MinimumIssuanceQty }).Must(x => NotBeLessThanMinQty(x.Qtr1Qty, x.MinimumIssuanceQty)).WithMessage("Quantity for Qtr 1 must not be less than the Minimum Request Quantity");
            RuleFor(x => new { x.Qtr2Qty, x.MinimumIssuanceQty }).Must(x => NotBeLessThanMinQty(x.Qtr2Qty, x.MinimumIssuanceQty)).WithMessage("Quantity for Qtr 2 must not be less than the Minimum Request Quantity");
            RuleFor(x => new { x.Qtr3Qty, x.MinimumIssuanceQty }).Must(x => NotBeLessThanMinQty(x.Qtr3Qty, x.MinimumIssuanceQty)).WithMessage("Quantity for Qtr 3 must not be less than the Minimum Request Quantity");
            RuleFor(x => new { x.Qtr4Qty, x.MinimumIssuanceQty }).Must(x => NotBeLessThanMinQty(x.Qtr4Qty, x.MinimumIssuanceQty)).WithMessage("Quantity for Qtr 4 must not be less than the Minimum Request Quantity");
        }

        private bool NotBeLessThanMinQty(int? Quantity, decimal MinimumQty)
        {
            return (Quantity < MinimumQty && !string.IsNullOrEmpty(Quantity.ToString())) ? false : true;
        } 
    }
}