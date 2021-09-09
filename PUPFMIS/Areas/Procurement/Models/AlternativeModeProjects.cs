//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Web.Mvc;

//namespace PUPFMIS.Models
//{
//    [Table("PROC_TRXN_Projects_Alternative_Mode")]
//    public class AlternativeModeProjects
//    {
//        [Key]
//        public int ID { get; set; }

//        public int APPDetailsReference { get; set; }

//        [Required]
//        [Display(Name = "Fiscal Year")]
//        public int FiscalYear { get; set; }

//        [Display(Name = "Fund Source")]
//        public string FundSource { get; set; }

//        [Required]
//        [Display(Name = "Contract Code")]
//        public string ContractCode { get; set; }

//        [Required]
//        [Display(Name = "Contract Name")]
//        public string ContractName { get; set; }

//        [Display(Name = "Is this an Early Procurement Activity?")]
//        public bool? IsEPA { get; set; }

//        [Required]
//        [Display(Name = "Approved Budget for the Contract")]
//        public decimal ABC { get; set; }

//        [Display(Name = "Mode of Procurement")]
//        public int ModeOfProcurementReference { get; set; }

//        [Display(Name = "Project Status")]
//        public AlternativeModeProjectStatus ProjectStatus { get; set; }

//        [Display(Name = "Project Stages")]
//        public AlternativeModeStages ProjectStage { get; set; }

//        [Display(Name = "Delivery Period (Calendar Days)")]
//        public int DeliveryPeriod { get; set; }

//        [Display(Name = "P/R Submission Opening")]
//        public DateTime? PRSubmissionOpen { get; set; }

//        [Display(Name = "P/R Submission Closing")]
//        public DateTime? PRSubmissionClose { get; set; }

//        [Display(Name = "Preparation of Request for Quotation")]
//        public DateTime? PreparationOfRFQ { get; set; }

//        [Display(Name = "Posting of Request for Quotation")]
//        public DateTime? PostingOfRFQ { get; set; }

//        [Display(Name = "Closing of Submission of RFQ")]
//        public DateTime? ClosingOfSubmissionOfRFQ { get; set; }

//        [Display(Name = "Opening of Quotations")]
//        public DateTime? OpeningOfQuotations { get; set; }

//        [Display(Name = "NOA Issuance")]
//        public DateTime? NOAIssuance { get; set; }

//        [Display(Name = "NTP Issuance")]
//        public DateTime? NTPIssuance { get; set; }

//        [Display(Name = "Project Coordinator")]
//        public string ProjectCoordinator { get; set; }

//        [Display(Name = "Project Support")]
//        public string ProjectSupport { get; set; }

//        [Display(Name = "Supplier")]
//        public int? SupplierReference { get; set; }

//        [Display(Name = "RFQ Reference")]
//        public int? RFQReference { get; set; }

//        //[Display(Name = "Purchase Order Reference")]
//        //public int? PurchaseOrderReference { get; set; }

//        [Display(Name = "Notice of Award Reference")]
//        public int? NOAReference { get; set; }

//        [Required]
//        [Display(Name = "Created By")]
//        public string CreatedBy { get; set; }

//        [Required]
//        [Display(Name = "Date Created")]
//        public DateTime CreatedAt { get; set; }

//        [Display(Name = "Date Cancelled")]
//        public DateTime? CancelledAt { get; set; }

//        [Display(Name = "Date Deleted")]
//        public DateTime? DeletedAt { get; set; }

//        [ForeignKey("ModeOfProcurementReference")]
//        public virtual ModesOfProcurement FKModeOfProcurementReference { get; set; }

//        [ForeignKey("SupplierReference")]
//        public virtual Supplier FKSupplierReference { get; set; }

//        [ForeignKey("RFQReference")]
//        public virtual RequestForQuotation FKRFQReference { get; set; }

//        //[ForeignKey("PurchaseOrderReference")]
//        //public virtual PurchaseOrderHeader FKPurchaseOrderReference { get; set; }

//        [ForeignKey("NOAReference")]
//        public virtual NoticeOfAward FKNOAReference { get; set; }

//        [ForeignKey("APPDetailsReference")]
//        public virtual AnnualProcurementPlanDetails FKAPPDetailsReference { get; set; }
//    }
//    [Table("PROC_TRXN_Projects_Alternative_Mode_Details")]
//    public class AlternativeModeProjectsDetails
//    {
//        [Key]
//        public int ID { get; set; }

//        [Display(Name = "Article")]
//        public int? ArticleReference { get; set; }

//        [MaxLength(2)]
//        public string ItemSequence { get; set; }

//        [Display(Name = "Item Full Name")]
//        [Column(TypeName = "VARCHAR")]
//        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
//        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
//        public string ItemFullName { get; set; }

//        [AllowHtml]
//        [DataType(DataType.MultilineText)]
//        [Display(Name = "Full Specifications")]
//        public string ItemSpecifications { get; set; }

//        public int Quantity { get; set; }

//        [Display(Name = "Unit Cost")]
//        public decimal UnitCost { get; set; }

//        [Required]
//        [Display(Name = "Contract Unit Price")]
//        public decimal ContractUnitPrice { get; set; }

//        [Required]
//        [Display(Name = "Approved Budget for the Contract")]
//        public decimal ApprovedBudget { get; set; }

//        [Required]
//        [Display(Name = "Contract Price")]
//        public decimal ContractTotalPrice { get; set; }

//        [Required]
//        [Display(Name = "Savings")]
//        public decimal Savings { get; set; }

//        [Required]
//        [Display(Name = "Unit of Measure")]
//        public int UOMReference { get; set; }

//        public int AlternativeModeProjectsReference { get; set; }

//        [ForeignKey("AlternativeModeProjectsReference")]
//        public virtual AlternativeModeProjects FKAlternativeModeProjectsReference { get; set; }

//        [ForeignKey("UOMReference")]
//        public virtual UnitOfMeasure FKUOMReference { get; set; }

//        [ForeignKey("ArticleReference")]
//        public virtual ItemArticles FKArticleReference { get; set; }
//    }
//    [Table("PROC_TRXN_Projects_Alternative_Mode_Updates")]
//    public class AlternativeModeProjectsUpdates
//    {
//        [Key]
//        public int ID { get; set; }

//        public int AlternativeProjectReference { get; set; }

//        public AlternativeModeStages ProjectStages { get; set; }

//        [AllowHtml]
//        [DataType(DataType.MultilineText)]
//        [Display(Name = "Remarks")]
//        public string Remarks { get; set; }

//        [Display(Name = "Date Updated")]
//        public DateTime? DateUpdated { get; set; }

//        [Display(Name = "Updated By")]
//        public string UpdatedBy { get; set; }

//        [ForeignKey("AlternativeProjectReference")]
//        public virtual AlternativeModeProjects FKAlternativeProjectReference { get; set; }
//    }



//    [Table("PROC_TRXN_Notice_of_Award")]
//    public class NoticeOfAward
//    {
//        [Key]
//        public int ID { get; set; }

//        [Required]
//        [Display(Name = "FiscalYear")]
//        public int FiscalYear { get; set; }

//        [Required]
//        [Display(Name = "Reference No.")]
//        public string ReferenceNo { get; set; }

//        [Required]
//        public int SupplierReference { get; set; }

//        [Required]
//        [Display(Name = "Date Created")]
//        public DateTime CreatedAt { get; set; }

//        [Required]
//        [Display(Name = "Created By")]
//        public string CreatedBy { get; set; }

//        [Display(Name = "Date Posted")]
//        public DateTime? PostedAt { get; set; }

//        [Display(Name = "Posted By")]
//        public string PostedBy { get; set; }

//        [ForeignKey("SupplierReference")]
//        public virtual Supplier FKSupplierReference { get; set; }
//    }

//    public class ContractsDashboard
//    {
//        public List<AlternativeModeProjectsVM> ProjectsForNOA { get; set; }
//        public List<int> ContractYears { get; set; }
//    }

//    public class AlternativeModeContractVM
//    {
//        [Display(Name = "Purchase Order No.")]
//        public string PurchaseOrderNo { get; set; }

//        [Display(Name = "Notice of Award Reference")]
//        public string NOAReference { get; set; }

//        [Display(Name = "Place of Delivery")]
//        public string PlaceOfDelivery { get; set; }

//        [Display(Name = "Date of Delivery")]
//        public DateTime? DateOfDelivery { get; set; }

//        [Display(Name = "PAP Code")]
//        public string PAPCode { get; set; }

//        [Required]
//        [Display(Name = "Fiscal Year")]
//        public int FiscalYear { get; set; }

//        [Display(Name = "Fund Source")]
//        public string FundSource { get; set; }

//        [Display(Name = "Fund Source")]
//        public string FundDescription { get; set; }

//        [Required]
//        [Display(Name = "Contract Code")]
//        public string ContractCode { get; set; }

//        [Required]
//        [Display(Name = "Contract Name")]
//        public string ContractName { get; set; }

//        [Required]
//        [Display(Name = "Approved Budget for the Contract")]
//        public decimal ABC { get; set; }

//        [Required]
//        [Display(Name = "Contract Price")]
//        public decimal ContractPrice { get; set; }

//        [Display(Name = "Mode of Procurement")]
//        public int ModeOfProcurementReference { get; set; }

//        [Display(Name = "Mode of Procurement")]
//        public string ModeOfProcurement { get; set; }

//        [Display(Name = "Supplier")]
//        public int SupplierReference { get; set; }

//        [Display(Name = "Supplier")]
//        public string SupplierName { get; set; }

//        [Display(Name = "Delivery Period (Calendar Days)")]
//        public int DeliveryPeriod { get; set; }

//        public List<AlternativeModeProjectsDetailsVM> ProjectDetails { get; set; }
//    }







//    public class AlternativeModeSetupVM
//    {
//        [Display(Name = "PAP Code")]
//        public string PAPCode { get; set; }

//        [Display(Name = "Program Name")]
//        public string ProgramName { get; set; }

//        [Display(Name = "Mode of Procurement")]
//        public int ModeOfProcurement { get; set; }

//        [Display(Name = "Bidding Strategy")]
//        public AlternativeModeStrategies AlternativeModeStrategy { get; set; }
//    }
//    public class AlternativeModeProjectsDetailsVM
//    {
//        public int APPDetailReference { get; set; }

//        [Required]
//        [Display(Name = "Approved Budget for the Item")]
//        public decimal ApprovedBudget { get; set; }

//        [Required]
//        [Display(Name = "Fund Source")]
//        public string FundSource { get; set; }

//        [Required]
//        [Display(Name = "Fund Description")]
//        public string FundDescription { get; set; }

//        [Display(Name = "Article")]
//        public int? ArticleReference { get; set; }

//        [MaxLength(2)]
//        public string ItemSequence { get; set; }

//        [Display(Name = "Item Full Name")]
//        [Column(TypeName = "VARCHAR")]
//        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
//        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
//        public string ItemFullName { get; set; }

//        [Display(Name = "Unit of Measure")]
//        public string UnitOfMeasure { get; set; }

//        [AllowHtml]
//        [DataType(DataType.MultilineText)]
//        [Display(Name = "Full Specifications")]
//        public string ItemSpecifications { get; set; }

//        [Required]
//        [Display(Name = "Unit of Measure")]
//        public int UOMReference { get; set; }

//        [Display(Name = "Unit Cost")]
//        public decimal UnitCost { get; set; }

//        public int Quantity { get; set; }
//    }
//    public class AlternativeModeProjectsVM
//    {
//        public int APPDetailsReference { get; set; }

//        [Display(Name = "PAP Code")]
//        public string PAPCode { get; set; }

//        [Required]
//        [Display(Name = "Fiscal Year")]
//        public int FiscalYear { get; set; }

//        [Display(Name = "Fund Source")]
//        public string FundSource { get; set; }

//        [Display(Name = "Fund Source")]
//        public string FundDescription { get; set; }

//        [Required]
//        [Display(Name = "Contract Code")]
//        public string ContractCode { get; set; }

//        [Required]
//        [Display(Name = "Contract Name")]
//        public string ContractName { get; set; }

//        [Display(Name = "Is this an Early Procurement Activity?")]
//        public bool IsEPA { get; set; }

//        [Display(Name = "Is this an Early Procurement Activity?")]
//        public string IsEPAValue { get; set; }

//        [Required]
//        [Display(Name = "Approved Budget for the Contract")]
//        public decimal ABC { get; set; }

//        [Display(Name = "Mode of Procurement")]
//        public int ModeOfProcurementReference { get; set; }

//        [Display(Name = "Mode of Procurement")]
//        public string ModeOfProcurement { get; set; }

//        [Display(Name = "Project Status")]
//        public AlternativeModeProjectStatus ProjectStatus { get; set; }

//        [Display(Name = "Project Stages")]
//        public AlternativeModeStages ProjectStage { get; set; }

//        [Display(Name = "Delivery Period (Calendar Days)")]
//        public int DeliveryPeriod { get; set; }

//        [Display(Name = "P/R Submission Opening")]
//        public DateTime? PRSubmissionOpen { get; set; }

//        [Display(Name = "P/R Submission Closing")]
//        public DateTime? PRSubmissionClose { get; set; }

//        [Display(Name = "Preparation of Request for Quotation")]
//        public DateTime? PreparationOfRFQ { get; set; }

//        [Display(Name = "Posting of Request for Quotation")]
//        public DateTime? PostingOfRFQ { get; set; }

//        [Display(Name = "Closing of Submission of RFQ")]
//        public DateTime? ClosingOfSubmissionOfRFQ { get; set; }

//        [Display(Name = "Opening of Quotations")]
//        public DateTime? OpeningOfQuotations { get; set; }

//        [Display(Name = "NOA Issuance")]
//        public DateTime? NOAIssuance { get; set; }

//        [Display(Name = "NTP Issuance")]
//        public DateTime? NTPIssuance { get; set; }

//        [Display(Name = "Project Coordinator")]
//        public string ProjectCoordinator { get; set; }

//        [Display(Name = "Project Support")]
//        public string ProjectSupport { get; set; }

//        public List<AlternativeModeProjectsDetailsVM> ProjectDetails { get; set; }
//    }

//    public enum AlternativeModeProjectStatus
//    {
//        [Display(Name = "Project Created")]
//        ProjectCreated = 0,

//        [Display(Name = "Project On-going")]
//        ProjectOngoing = 1,

//        [Display(Name = "Project Failed")]
//        ProjectFailed = 2,

//        [Display(Name = "Project Cancelled")]
//        ProjectCancelled = 3,

//        [Display(Name = "Procurement Closed")]
//        ProcurementClosed = 4,

//        [Display(Name = "Contract Delivery")]
//        ContractDelivery = 5
//    }
//    public enum AlternativeModeStages
//    {
//        [Display(Name = "Alternative Mode Project Created")]
//        ProjectCreated = 0,

//        [Display(Name = "P/R Submission Open")]
//        PurchaseRequestSubmissionOpening = 1,

//        [Display(Name = "P/R Submission Closed")]
//        PurchaseRequestSubmissionClosing = 2,

//        [Display(Name = "Request for Quotation Prepared")]
//        RFQPreparation = 3,

//        [Display(Name = "Request for Quotation Posted")]
//        PostingOfRFQ = 4,

//        [Display(Name = "Request for Quotation Submission Closed")]
//        RFQSubmissionClosing = 5,

//        [Display(Name = "Quotations Opened")]
//        OpeningOfQuoatations = 6,

//        [Display(Name = "Purchase Order / Agency Procurement Request Created")]
//        PurchaseOrderCreated = 7,

//        //[Display(Name = "Detailed Examination of Bids Conducted")]
//        //DetailExaminationOfBids = 8,

//        //[Display(Name = "Post Qualification of Bids")]
//        //PostQualificationOfBids = 9,

//        //[Display(Name = "Notice of Award Issued")]
//        //IssuanceOfNoticeOfAward = 10,

//        [Display(Name = "Notice To Proceed Issued")]
//        IssuanceToProceedOfAward = 11,

//        [Display(Name = "Payment Released")]
//        PaymentReleased = 12,

//        [Display(Name = "Project Closed")]
//        ProjectClosed = 13
//    }
//    public enum AlternativeModeStrategies
//    {
//        [Display(Name = "Program-based Procurement")]
//        ProgramBased = 0,

//        [Display(Name = "Item-Based Procurement")]
//        ItemBased = 1
//    }
//}