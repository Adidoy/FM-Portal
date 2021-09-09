using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;

namespace PUPFMIS.Models
{
    public class CatalogueVM
    {
        [Display(Name = "Classification")]
        public string Classification { get; set; }

        [Display(Name = "Item Type")]
        public string ItemType { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        public bool IsSpecsUserDefined { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public string ProcurementSource { get; set; }

        [Display(Name = "Account Class")]
        public string AccountClass { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? UnitCost { get; set; }

        [Display(Name = "Packaging UOM")]
        public string PackagingUOMReference { get; set; }

        [Display(Name = "Individual UOM")]
        public string IndividualUOMReference { get; set; }

        [Display(Name = "Minimum Issuance Quantity")]
        public int? MinimumIssuanceQty { get; set; }

        [Display(Name = "Quantity per Package")]
        public int? QuantityPerPackage { get; set; }

        [Display(Name = "Responsibility Center")]
        public string ResponsibilityCenter { get; set; }

        [Display(Name = "Purchase Request Center")]
        public string PurchaseRequestCenter { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Justification")]
        public string Justification { get; set; }
    }
    public class Basket
    {
        [Display(Name = "Project Code")]
        [MaxLength(40, ErrorMessage = "{0} is up to {1} characters only.")]
        public string ProjectCode { get; set; }

        [Display(Name = "Delivery Month")]
        public int DeliveryMonth { get; set; }

        public List<BasketItems> BasketItems { get; set; }
    }
    public class BasketItems
    {
        public BudgetProposalType ProposalType { get; set; }

        [Display(Name = "Classification")]
        public string Classification { get; set; }

        [Display(Name = "Item Type")]
        public string ItemType { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Article Code")]
        public string ArticleCode { get; set; }

        [Display(Name = "Sequence")]
        public string ItemSequence { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        public bool IsSpecsUserDefined { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public string ProcurementSource { get; set; }

        [Display(Name = "Account Class")]
        public string AccountClass { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? EstimatedBudget { get; set; }

        [Display(Name = "Packaging UOM")]
        public string PackagingUOMReference { get; set; }

        [Display(Name = "Individual UOM")]
        public string IndividualUOMReference { get; set; }

        [Display(Name = "Minimum Issuance Quantity")]
        public int? MinimumIssuanceQty { get; set; }

        [Display(Name = "Quantity per Package")]
        public int? QuantityPerPackage { get; set; }

        [Display(Name = "Responsibility Center")]
        public string ResponsibilityCenter { get; set; }

        [Display(Name = "Purchase Request Center")]
        public string PurchaseRequestCenter { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
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

        [Display(Name = "Project Item Status")]
        public string ProjectItemStatus { get; set; }

        [Display(Name = "Supplier 1 ID")]
        public int? Supplier1ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier1Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier1Address { get; set; }

        [Display(Name = "Contact No.")]
        public string Supplier1ContactNo { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier1EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier1UnitCost { get; set; }

        [Display(Name = "Supplier 2 ID")]
        public int? Supplier2ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier2Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier2Address { get; set; }

        [Display(Name = "Contact No.")]
        public string Supplier2ContactNo { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier2EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier2UnitCost { get; set; }

        [Display(Name = "Supplier 3 ID")]
        public int? Supplier3ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier3Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier3Address { get; set; }

        [Display(Name = "Contact No.")]
        public string Supplier3ContactNo { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier3EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier3UnitCost { get; set; }

        [Display(Name = "Unit Cost")]
        public ResponsibilityCenterReasonForRevision? ReasonForNonAcceptance { get; set; }

        public bool UpdateFlag { get; set; }
    }
}