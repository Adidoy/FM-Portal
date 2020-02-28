using FluentValidation;
using System.Collections.Generic;
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

        [Display(Name = "Inventory Type")]
        public string ItemInventoryType { get; set; }

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

    public class BasketItem
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

        [Display(Name = "Inventory Type")]
        public string ItemInventoryType { get; set; }

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

        [Display(Name = "Maximum Request Quantity")]
        public int? TotalConsumption { get; set; }

        [MaxLength(150)]
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Is sole distributor/contractor?")]
        public bool IsSoleDistributor { get; set; }

        [Display(Name = "Supplier ID")]
        public int? Supplier1ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier1Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier1Address { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier1EmailAddress { get; set; }

        [Display(Name = "Contact Number")]
        public string Supplier1ContactNo { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier1UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? Supplier1EstimatedBudget { get; set; }

        [Display(Name = "Supplier ID")]
        public int? Supplier2ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier2Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier2Address { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier2EmailAddress { get; set; }

        [Display(Name = "Contact Number")]
        public string Supplier2ContactNo { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier2UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? Supplier2EstimatedBudget { get; set; }

        [Display(Name = "Supplier ID")]
        public int? Supplier3ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier3Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier3Address { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier3EmailAddress { get; set; }

        [Display(Name = "Contact Number")]
        public string Supplier3ContactNo { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier3UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? Supplier3EstimatedBudget { get; set; }

        [Display(Name = "Total Estimated Budget")]
        public decimal? TotalEstimatedBudget { get; set; }
    }

    public class Basket
    {
        public ProjectProcurementPlan BasketHeader { get; set; }
        public List<BasketItem> BasketItems { get; set; }
    }

    public class BasketValidator : AbstractValidator<BasketItem>
    {
        private FMISDbContext db = new FMISDbContext();

        public BasketValidator()
        {
            RuleFor(x => new { x.Qtr1Qty, x.MinimumIssuanceQty }).Must(x => NotBeLessThanMinQty(x.Qtr1Qty, x.MinimumIssuanceQty)).WithMessage("Quantity for Qtr 1 must not be less than the Minimum Request Quantity");
            RuleFor(x => new { x.Qtr2Qty, x.MinimumIssuanceQty }).Must(x => NotBeLessThanMinQty(x.Qtr2Qty, x.MinimumIssuanceQty)).WithMessage("Quantity for Qtr 2 must not be less than the Minimum Request Quantity");
            RuleFor(x => new { x.Qtr3Qty, x.MinimumIssuanceQty }).Must(x => NotBeLessThanMinQty(x.Qtr3Qty, x.MinimumIssuanceQty)).WithMessage("Quantity for Qtr 3 must not be less than the Minimum Request Quantity");
            RuleFor(x => new { x.Qtr4Qty, x.MinimumIssuanceQty }).Must(x => NotBeLessThanMinQty(x.Qtr4Qty, x.MinimumIssuanceQty)).WithMessage("Quantity for Qtr 4 must not be less than the Minimum Request Quantity");
            RuleFor(x => new { x.Qtr1Qty, x.Qtr2Qty, x.Qtr3Qty, x.Qtr4Qty }).Must(x => NotBeAllNull(x.Qtr1Qty, x.Qtr2Qty, x.Qtr3Qty, x.Qtr4Qty)).WithMessage("Please enter quantity for at least 1 quarter.");
        }

        private bool NotBeLessThanMinQty(int? Quantity, decimal MinimumQty)
        {
            return (Quantity < MinimumQty && !string.IsNullOrEmpty(Quantity.ToString())) ? false : true;
        }
        
        public bool NotBeAllNull(int? Qtr1, int? Qtr2, int? Qtr3, int? Qtr4)
        {
            return (string.IsNullOrEmpty(Qtr1.ToString()) && string.IsNullOrEmpty(Qtr2.ToString()) && string.IsNullOrEmpty(Qtr3.ToString()) && string.IsNullOrEmpty(Qtr4.ToString())) ? false : true;
        } 
    }
}