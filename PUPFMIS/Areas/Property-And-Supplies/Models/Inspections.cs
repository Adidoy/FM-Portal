using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROP_TRXN_Inspection")]
    public class Inspection
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Inspection and Acceptance Report No.")]
        public string IARNumber { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public int SupplierReference { get; set; }

        [Required]
        [Display(Name = "Contract Type")]
        public ContractTypes ContractType { get; set; }

        [Required]
        [Display(Name = "Reference No.")]
        public string ReferenceNumber { get; set; }

        [Required]
        [Display(Name = "Reference Date")]
        public DateTime ReferenceDate { get; set; }

        [Required]
        [Display(Name = "Invoice No.")]
        public string InvoiceNumber { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Display(Name = "Responsibility Center")]
        public string ResponsibilityCenter { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Required]
        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        public DeliveryCompleteness? DeliveryCompleteness { get; set; }

        public int? DeliveryReference { get; set; }

        [Required]
        [Display(Name = "Inspection Officer")]
        public string InspectedBy { get; set; }

        [Required]
        [Display(Name = "Date Inspected")]
        public DateTime InspectedAt { get; set; }

        [Required]
        [Display(Name = "Processed By")]
        public string ProcessedBy { get; set; }

        [Required]
        [Display(Name = "Date Processed")]
        public DateTime ProcessedAt { get; set; }

        [ForeignKey("SupplierReference")]
        public virtual Supplier FKSupplierReference { get; set; }

        [ForeignKey("DeliveryReference")]
        public virtual Delivery FKDeliveryReference { get; set; }
    }
    [Table("PROP_TRXN_Inspection_Supplies")]
    public class InspectionSupply
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int InspectionReference { get; set; }

        [Required]
        public int SupplyReference { get; set; }

        [Required]
        public int UnitReference { get; set; }

        [Required]
        public int QuantityReceived { get; set; }

        [Required]
        public int QuantityAccepted { get; set; }

        [Required]
        public int QuantityRejected { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Inspection Notes")]
        public string InspectionNotes { get; set; }

        [ForeignKey("SupplyReference")]
        public Supplies FKSupplyReference { get; set; }

        [ForeignKey("InspectionReference")]
        public Inspection FKInspectionReference { get; set; }
    }
    [Table("PROP_TRXN_Inspection_PPE")]
    public class InspectionProperty
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int InspectionReference { get; set; }

        [Required]
        public int PropertyReference { get; set; }

        [Required]
        public int UnitReference { get; set; }

        [Required]
        public int QuantityReceived { get; set; }

        [Required]
        public int QuantityAccepted { get; set; }

        [Required]
        public int QuantityRejected { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Inspection Notes")]
        public string InspectionNotes { get; set; }

        [ForeignKey("PropertyReference")]
        public PPE FKPropertyReference { get; set; }

        [ForeignKey("InspectionReference")]
        public Inspection FKInspectionReference { get; set; }
    }

    public class InspectionSupplyVM
    {
        [Required]
        public int SupplyReference { get; set; }

        [Required]
        [Display(Name = "Stock Number")]
        public string StockNumber { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        public int UnitReference { get; set; }

        [Required]
        [Display(Name = "Unit")]
        public string UnitOfMeasure { get; set; }

        [Required]
        [Display(Name = "Unit Cost")]
        public decimal ReceivedUnitCost { get; set; }

        [Required]
        [Display(Name = "Received")]
        public int QuantityReceived { get; set; }

        [Required]
        [Display(Name = "Accepted")]
        public int QuantityAccepted { get; set; }

        [Required]
        [Display(Name = "Rejected")]
        public int QuantityRejected { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Inspection Notes")]
        public string InspectionNotes { get; set; }

        [ForeignKey("SupplyReference")]
        public Supplies FKSupplyReference { get; set; }
    }
    public class InspectionSuppliesDeliveredVM
    {
        public string IARNumber { get; set; }

        [Required]
        public int SupplierReference { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public string Supplier { get; set; }

        [Required]
        [Display(Name = "Contract Type")]
        public ContractTypes ContractType { get; set; }

        [Required]
        [Display(Name = "Reference No.")]
        public string ReferenceNumber { get; set; }

        [Required]
        [Display(Name = "Reference Date")]
        public DateTime ReferenceDate { get; set; }

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

        [Required]
        [Display(Name = "Requisitioning Office/Department")]
        public string RequisitioningOffice { get; set; }

        [Required]
        [Display(Name = "Responsibility Center")]
        public string ResponsibilityCenter { get; set; }

        [Required]
        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Required]
        [Display(Name = "Fund Source")]
        public string FundDescription { get; set; }

        [Required]
        [Display(Name = "Inspection Officer")]
        public string InspectedBy { get; set; }

        [Required]
        [Display(Name = "Date Inspected")]
        public DateTime InspectedAt { get; set; }

        [Display(Name = "Date IAR Processed")]
        public DateTime ProcessedAt { get; set; }

        [Required]
        public int DeliveryReference { get; set; }

        [Required]
        [Display(Name = "Delivery Receipt Report No.")]
        public string DeliveryAcceptanceNumber { get; set; }

        public DeliveryCompleteness? DeliveryCompleteness { get; set; }

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
        [AllowHtml]
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        public List<InspectionSupplyVM> Supplies { get; set; }
    }

    public class InspectionDashboard
    {
        public List<DeliveryListVM> SupplyDeliveriesForInspection { get; set; }
        public List<DeliveryListVM> PPEDeliveriesForInspection { get; set; }
        public List<int> ReportYears { get; set; }
    }
    public class InspectionListVM
    {
        public string IARNumber { get; set; }
        public string Supplier { get; set; }
        public string InspectedBy { get; set; }
        public string InspectedAt { get; set; }
    }
}