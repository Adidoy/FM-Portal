using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROC_TRXN_Contract")]
    public class ContractHeader
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Contract Type")]
        public ContractTypes ContractType { get; set; }
        
        [MaxLength(75)]
        [Display(Name = "Reference No.")]
        public string ReferenceNumber { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Required]
        public int ProcurementProjectReference { get; set; }

        [Required]
        [Display(Name = "Contract Price")]
        public decimal ContractPrice { get; set; }

        [Display(Name = "Contract Commencement Date")]
        public DateTime? CommencedAt { get; set; }

        [Display(Name = "Delivery Deadline")]
        public DateTime? DeliveryDeadline { get; set; }

        [Display(Name = "Date Completed")]
        public DateTime? CompletedAt { get; set; }

        [Display(Name = "Status")]
        public ContractStatus? ContractStatus { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public int SupplierReference { get; set; }

        [Required]
        [Display(Name = "PMO")]
        public string PMOffice { get; set; }

        [Required]
        [Display(Name = "PMO Head")]
        public string PMOHead { get; set; }

        [Required]
        [Display(Name = "PMO Head Designation")]
        public string PMOHeadDesignation { get; set; }

        [Required]
        [Display(Name = "Accounting")]
        public string AccountingOffice { get; set; }

        [Required]
        [Display(Name = "Accounting Head")]
        public string AccountingOfficeHead { get; set; }

        [Required]
        [Display(Name = "Accounting Head Designation")]
        public string AccountingOfficeHeadDesignation { get; set; }

        [Required]
        [Display(Name = "HOPE Office")]
        public string HOPEOffice { get; set; }

        [Required]
        [Display(Name = "HOPE")]
        public string HOPE { get; set; }

        [Required]
        [Display(Name = "HOPE Designation")]
        public string HOPEDesignation { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [ForeignKey("SupplierReference")]
        public virtual Supplier FKSupplierReference { get; set; }

        [ForeignKey("ProcurementProjectReference")]
        public virtual ProcurementProject FKProcurementProjectReference { get; set; }
    }
    [Table("PROC_TRXN_Contract_Details")]
    public class ContractDetails
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Contract Reference")]
        public int ContractReference { get; set; }

        [Display(Name = "Article")]
        public int? ArticleReference { get; set; }

        [MaxLength(2)]
        public string ItemSequence { get; set; }

        [Display(Name = "Item Full Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ItemFullName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Contract Unit Price")]
        public decimal ContractUnitPrice { get; set; }

        [Required]
        [Display(Name = "Contract Price")]
        public decimal ContractTotalPrice { get; set; }

        [Required]
        [Display(Name = "Savings")]
        public decimal Savings { get; set; }

        [Required]
        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        [Display(Name = "Delivered")]
        public int? DeliveredQuantity { get; set; }

        [ForeignKey("ContractReference")]
        public virtual ContractHeader FKContractReference { get; set; }

        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKArticleReference { get; set; }

        [ForeignKey("UOMReference")]
        public virtual UnitOfMeasure FKUOMReference { get; set; }
    }
    [Table("PROC_TRXN_Contract_Monitoring_Updates")]
    public class ContractMonitoringUpdates
    {
        [Key]
        public int ID { get; set; }

        public int ContractReference { get; set; }

        public ContractStatus ContractStatus { get; set; }

        [Display(Name = "Date Accomplished")]
        public DateTime? AccomplishedAt { get; set; }

        [Required]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        [ForeignKey("ContractReference")]
        public virtual ContractHeader FKContractReference { get; set; }
    }

    [Table("PROC_TRXN_Contract_Payment")]
    public class ContractPayment
    {
        [Key]
        public int ID { get; set; }

        public int ProcurementProject { get; set; }

        [MaxLength(50)]
        [Display(Name = "Disbursement Voucher No.")]
        public string DVNumber { get; set; }

        [Display(Name = "Payment Mode")]
        public PaymentModes PaymentMode { get; set; }

        [MaxLength(75)]
        [Display(Name = "Payment Reference")]
        public string PaymentReference { get; set; }
        
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
    }

    public class ContractListVM
    {
        [MaxLength(75)]
        [Display(Name = "Reference No.")]
        public string ReferenceNumber { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Required]
        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Required]
        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Required]
        [Display(Name = "Contract Price")]
        public decimal ContractPrice { get; set; }

        [Required]
        [Display(Name = "Contract Type")]
        public ContractTypes ContractType { get; set; }
    }

    public enum ContractTypes
    {
        [Display(Name = "Agency Procurement Request", ShortName = "APR")]
        AgencyProcurementRequest = 0,

        [Display(Name = "Purchase Order", ShortName = "PO")]
        PurchaseOrder = 1,

        [Display(Name = "Contract of Service", ShortName = "COS")]
        ContractOfService = 2,

        [Display(Name = "Memorandum of Agreement", ShortName = "MOA")]
        MOA = 3,

        [Display(Name = "Memorandum of Understanding", ShortName = "MOU")]
        MOU = 4
    }
    public enum ContractStatus
    {
        [Display(Name = "NTP Signed and Received")]
        NTPSignedAndReceived = 0,

        [Display(Name = "Partial Delivery")]
        PartialDelivery = 1,

        [Display(Name = "Delivery Completed")]
        DeliveryCompleted = 2,

        [Display(Name = "Inspected - Partial Acceptance")]
        InspectedPartialAcceptance = 3,

        [Display(Name = "Inspected and Accepted")]
        InspectedAndAccepted = 4,

        [Display(Name = "Payment Processing")]
        PaymentOngoing = 5,

        [Display(Name = "Payment Completed")]
        PaymentCompleted = 6,

        [Display(Name = "Supplier/Contractor Evaluated")]
        Evaluated = 7
    }
    public enum PaymentModes
    {
        [Display(Name = "Check")]
        Check = 0,

        [Display(Name = "LDDAP")]
        LDDAP = 1
    }
}