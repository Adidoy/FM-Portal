using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PROP_TRXN_Delivery")]
    public class Delivery
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Delivery Receipt No.")]
        public string DeliveryAcceptanceNumber { get; set; }

        public int ContractReference { get; set; }

        [Required]
        [Display(Name = "Date Processed")]
        public DateTime DateProcessed { get; set; }

        [Required]
        [Display(Name = "Processed by")]
        public string ProcessedBy { get; set; }

        [Required]
        [Display(Name = "Received by")]
        public string ReceivedBy { get; set; }

        public ContractTypes ContractType { get; set; }

        public string Reference { get; set; }

        [Required]
        [Display(Name = "Invoice No.")]
        public string InvoiceNumber { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Display(Name = "Delivery Receipt No.")]
        public string DRNumber { get; set; }

        [Required]
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }

        public DeliveryCompleteness? DeliveryCompleteness { get; set; }

        [ForeignKey("ContractReference")]
        public virtual ContractHeader FKContractReference { get; set; }
    }
    [Table("PROP_TRXN_Delivery_Supplies")]
    public class DeliverySupply
    {
        [Key]
        public int ID { get; set; }

        public int SupplyReference { get; set; }

        public int? DeliveryReference { get; set; }

        [Required]
        [Display(Name = "Quantity Delivered")]
        public int QuantityDelivered { get; set; }

        [Required]
        [Display(Name = "Backlog")]
        public int QuantityBacklog { get; set; }

        [Display(Name = "Receipt Unit Cost")]
        public decimal ReceiptUnitCost { get; set; }

        [Display(Name = "Receipt Total Cost")]
        public decimal ReceiptTotalCost { get; set; }

        [ForeignKey("SupplyReference")]
        public virtual Supplies FKSupplyReference { get; set; }

        [ForeignKey("DeliveryReference")]
        public virtual Delivery FKDeliveryReference { get; set; }
    }
    [Table("PROP_TRXN_Delivery_PPE")]
    public class DeliveryProperty
    {
        [Key]
        public int ID { get; set; }

        public int PPEReference { get; set; }

        public int? DeliveryReference { get; set; }

        [Required]
        [Display(Name = "Quantity Delivered")]
        public int QuantityDelivered { get; set; }

        [Required]
        [Display(Name = "Backlog")]
        public int QuantityBacklog { get; set; }

        [Display(Name = "Receipt Unit Cost")]
        public decimal ReceiptUnitCost { get; set; }

        [Display(Name = "Receipt Total Cost")]
        public decimal ReceiptTotalCost { get; set; }

        [ForeignKey("PPEReference")]
        public virtual PPE FKPPEReference { get; set; }

        [ForeignKey("DeliveryReference")]
        public virtual Delivery FKDeliveryReference { get; set; }
    }

    public class DeliveriesDashboard
    {
        public List<ContractListVM> ContractsForDelivery { get; set; }
    }

    public class UnregisteredItemsVM
    {
        public List<UnregisteredSuppliesVM> Supplies { get; set; }
        public List<UnregisteredPPEVM> PPE { get; set; }
    }
    public class UnregisteredSuppliesVM
    {
        private string _description = string.Empty;

        [Display(Name = "Article")]
        public int ArticleReference { get; set; }

        [Required]
        [MaxLength(2)]
        public string Sequence { get; set; }

        [Required]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Required]
        [Display(Name = "Article")]
        public string Article { get; set; }

        [Display(Name = "Item Description")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string Description
        {
            get { return _description.ToUpper(); }
            set { _description = value.ToUpper(); }
        }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Re-order Point")]
        public int ReOrderPoint { get; set; }

        [Required]
        [Display(Name = "Issuance Unit")]
        public int IndividualUOMReference { get; set; }

        [Required]
        [Display(Name = "Issuance Unit")]
        public string IndividualUOM { get; set; }

        [Required]
        [Display(Name = "Minimum Issuance Quantity")]
        public int MinimumIssuanceQty { get; set; }
    }
    public class UnregisteredPPEVM
    {
        private string _description = string.Empty;

        [Display(Name = "Article")]
        public int ArticleReference { get; set; }

        [Required]
        [MaxLength(2)]
        public string Sequence { get; set; }

        [Required]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Required]
        [Display(Name = "Article")]
        public string Article { get; set; }

        [Display(Name = "Item Description")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string Description
        {
            get { return _description.ToUpper(); }
            set { _description = value.ToUpper(); }
        }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Required]
        [Display(Name = "Issuance Unit")]
        public int IndividualUOMReference { get; set; }

        [Required]
        [Display(Name = "Issuance Unit")]
        public string IndividualUOM { get; set; }

        [Required]
        [Display(Name = "Minimum Issuance Quantity")]
        public int MinimumIssuanceQty { get; set; }
    }

    public class DeliveryListVM
    {
        [Required]
        [Display(Name = "Delivery Receipt No.")]
        public string DeliveryAcceptanceNumber { get; set; }

        [Required]
        [Display(Name = "Reference No.")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "Contract Type")]
        public ContractTypes ContractType { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Required]
        [Display(Name = "Invoice No.")]
        public string InvoiceNumber { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Display(Name = "Delivery Receipt No.")]
        public string DRNumber { get; set; }

        [Required]
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }
    }
    public class DeliveryVM
    {
        [Required]
        [Display(Name = "Delivery Receipt No.")]
        public string DeliveryAcceptanceNumber { get; set; }

        [Required]
        public int ContractReference { get; set; }

        [Required]
        [Display(Name = "Reference No.")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Required]
        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Required]
        [Display(Name = "Contract Type")]
        public ContractTypes ContractType { get; set; }

        [Required]
        [Display(Name = "Contract Price")]
        public decimal ContractPrice { get; set; }

        [Required]
        [Display(Name = "Status")]
        public ContractStatus ContractStatus { get; set; }

        [Required]
        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Required]
        [Display(Name = "Fund Source")]
        public string FundDescription { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Required]
        [Display(Name = "Supplier Address")]
        public string SupplierAddress { get; set; }

        [Required]
        [Display(Name = "Date Processed")]
        public DateTime DateProcessed { get; set; }

        [Required]
        [Display(Name = "Received by")]
        public string ReceivedBy { get; set; }
        
        [Required]
        [Display(Name = "Invoice No.")]
        public string InvoiceNumber { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Display(Name = "Delivery Receipt No.")]
        public string DRNumber { get; set; }

        [Required]
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }

        public List<SupplyDeliveryVM> Supplies { get; set; }

        public List<PPEDeliveryVM> PPE { get; set; }
    }
    public class SupplyDeliveryVM
    {
        [Required]
        [Display(Name = "Stock No.")]
        public string StockNumber { get; set; }

        [Required]
        public int SupplyReference { get; set; }

        [Required]
        public int ArticleReference { get; set; }

        [Required]
        public string Sequence { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Total Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Contract Unit Price")]
        public decimal ContractUnitPrice { get; set; }

        [Required]
        [Display(Name = "Contract Price")]
        public decimal ContractTotalPrice { get; set; }

        [Required]
        [Display(Name = "Quantity Delivered")]
        public int QuantityDelivered { get; set; }

        [Required]
        [Display(Name = "Delivery Packaging Unit")]
        public int PackagingUOMReference { get; set; }

        [Required]
        [Display(Name = "Quantity per Delivery Packaging Unit")]
        public int QuantityPerPackage { get; set; }

        [Required]
        [Display(Name = "Issuance Unit")]
        public int IndividualUOMReference { get; set; }

        [Required]
        [Display(Name = "Issuance Unit")]
        public string IndividualUOM { get; set; }

        [Required]
        [Display(Name = "Minimum Issuance Quantity")]
        public string MinimumIssuanceQty { get; set; }
    }
    public class PPEDeliveryVM
    {
        [Required]
        [Display(Name = "Property No.")]
        public string PropertyNo { get; set; }

        [Required]
        public int PPEReference { get; set; }

        [Required]
        public int ArticleReference { get; set; }

        [Required]
        public string Sequence { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Total Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Contract Unit Price")]
        public decimal ContractUnitPrice { get; set; }

        [Required]
        [Display(Name = "Contract Price")]
        public decimal ContractTotalPrice { get; set; }

        [Required]
        [Display(Name = "Quantity Delivered")]
        public int QuantityDelivered { get; set; }

        [Required]
        [Display(Name = "Delivery Packaging Unit")]
        public int PackagingUOMReference { get; set; }

        [Required]
        [Display(Name = "Quantity per Delivery Packaging Unit")]
        public int QuantityPerPackage { get; set; }

        [Required]
        [Display(Name = "Issuance Unit")]
        public int IndividualUOMReference { get; set; }

        [Required]
        [Display(Name = "Issuance Unit")]
        public string IndividualUOM { get; set; }
    }
    public class DeliveryAcceptanceTemplateVM
    {
        [Required]
        [Display(Name = "Delivery Receipt No.")]
        public string DeliveryAcceptanceNumber { get; set; }

        [Required]
        [Display(Name = "Reference No.")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "Reference No.")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Required]
        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Required]
        [Display(Name = "Contract Type")]
        public ContractTypes ContractType { get; set; }

        [Required]
        [Display(Name = "Contract Price")]
        public decimal ContractPrice { get; set; }

        [Required]
        [Display(Name = "Status")]
        public ContractStatus ContractStatus { get; set; }

        [Required]
        [Display(Name = "Delivery Completeness")]
        public DeliveryCompleteness DeliveryCompleteness { get; set; }

        [Required]
        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Required]
        [Display(Name = "Fund Source")]
        public string FundDescription { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Required]
        [Display(Name = "Supplier Address")]
        public string SupplierAddress { get; set; }

        [Required]
        [Display(Name = "Date Processed")]
        public DateTime DateProcessed { get; set; }

        [Required]
        [Display(Name = "Processed by")]
        public string ProcessedBy { get; set; }

        [Required]
        [Display(Name = "Received by")]
        public string ReceivedBy { get; set; }

        [Required]
        [Display(Name = "Invoice No.")]
        public string InvoiceNumber { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Display(Name = "Delivery Receipt No.")]
        public string DRNumber { get; set; }

        [Required]
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }

        public List<SupplyDeliveryVM> Supplies { get; set; }

        public List<PPEDeliveryVM> PPE { get; set; }
    }

    public enum DeliveryTypes
    {
        [Display(Name = "Supplies")]
        Supplies,

        [Display(Name = "Semi-expendable PPE")]
        SemiExpendablePPE,

        [Display(Name = "Property, Plant and Equipment")]
        PPE
    }
    public enum DeliveryCompleteness
    {
        [Display(Name = "Complete Delivery")]
        CompleteDelivery,

        [Display(Name = "Partial Delivery")]
        PartialDelivery
    }
}