using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public class CatalogueVM
    {
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Inventory Type")]
        public string ItemInventoryType { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

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

        [Display(Name = "Quantity per Package")]
        public int? DistributionQtyPerPack { get; set; }
    }
    public class CatalogueBasketItemVM
    {
        [Display(Name = "ProjectCode")]
        public string ProjectCode { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Full Specifications")]
        [DataType(DataType.MultilineText)]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Inventory Type")]
        public string InventoryType { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "Category")]
        public string ItemCategory { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Packaging UOM")]
        public string PackagingUOMReference { get; set; }

        [Display(Name = "Individual UOM")]
        public string IndividualUOMReference { get; set; }

        [Display(Name = "Minimum Issuance Quantity")]
        public int MinimumIssuanceQty { get; set; }

        [Display(Name = "Actual Obligation (Previous Year)")]
        public int? ActualObligation { get; set; }

        [Display(Name = "Quantity per Package")]
        public int DistributionQtyPerPack { get; set; }

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

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

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
}