//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("PROC_TRXN_Request_for_")]
//    public class RequestForQuotation
//    {
//        [Key]
//        public int ID { get; set; }

//        [Required]
//        [Display(Name = "Fiscal Year")]
//        public int FiscalYear { get; set; }

//        [Required]
//        public string SolicitationNo { get; set; }

//        public DateTime Deadline { get; set; }

//        public DateTime CreatedAt { get; set; }

//        public string CreatedBy { get; set; }

//        public int? ProcurementProject { get; set; }

//        public int ModeOfProcurementReference { get; set; }

//        public bool IsSubmissionOpen { get; set; }

//        public DateTime? ClosedAt { get; set; }

//        public string ClosedBy { get; set; }

//        [Display(Name = "Date Opened")]
//        public DateTime? OpenedAt { get; set; }

//        [Display(Name = "Place Opened")]
//        public string PlaceOpened { get; set; }

//        [Display(Name = "Abstract Prepared By")]
//        public string AbstractCreatedBy { get; set; }

//        [Display(Name = "Date Abstract Prepared")]
//        public DateTime? AbstractCreatedAt { get; set; }

//        [ForeignKey("ModeOfProcurementReference")]
//        public virtual ModesOfProcurement FKModeOfProcurementReference { get; set; }

//        [ForeignKey("ProcurementProject")]
//        public virtual ProcurementProject FKProcurementProject { get; set; }
//    }
//    [Table("PROC_TRXN_Request_for_Quotation_Header")]
//    public class RequestForQuotationHeader
//    {
//        [Key]
//        public int ID { get; set; }

//        [Required]
//        [Display(Name = "Quotation No.")]
//        public string QuotationNo { get; set; }

//        [Required]
//        [Display(Name = "Supplier")]
//        public int? SupplierReference { get; set; }

//        [Required]
//        [Display(Name = "RFQ Reference")]
//        public int RFQReference { get; set; }

//        [Required]
//        [Display(Name = "Date Recorded")]
//        public DateTime RecordedAt { get; set; }

//        [Required]
//        [Display(Name = "Date Submitted")]
//        public DateTime? SubmittedAt { get; set; }

//        [Display(Name = "Eligibility Requirements")]
//        public bool? EligibilityRequirements { get; set; }

//        [Display(Name = "Technical Requirements")]
//        public bool? TechnicalRequirements { get; set; }

//        [Display(Name = "Financial Requirements")]
//        public bool? FinancialRequirements { get; set; }

//        [Display(Name = "Recommend to Award")]
//        public bool? RecommendToAward { get; set; }

//        [Display(Name = "Lowest Calculated and Responsive Quotation")]
//        public bool? LCRQ { get; set; }

//        [Display(Name = "Remarks")]
//        public string Remarks { get; set; }

//        [ForeignKey("RFQReference")]
//        public virtual RequestForQuotation FKRFQReference { get; set; }

//        [ForeignKey("SupplierReference")]
//        public virtual Supplier FKSupplierReference { get; set; }
//    }
//    [Table("PROC_TRXN_Request_for_Quotation_Details")]
//    public class RequestForQuotationDetails
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

//        [System.Web.Mvc.AllowHtml]
//        [DataType(DataType.MultilineText)]
//        [Display(Name = "Full Specifications")]
//        public string ItemSpecifications { get; set; }

//        public int Quantity { get; set; }

//        [Display(Name = "Unit Price")]
//        public decimal? UnitPrice { get; set; }

//        [Display(Name = "No Office")]
//        public bool NoOffer { get; set; }

//        [Display(Name = "Total Unit Price")]
//        public decimal? TotalUnitPrice { get; set; }

//        [Required]
//        [Display(Name = "Unit of Measure")]
//        public int UOMReference { get; set; }

//        public int RFQHeaderReference { get; set; }

//        [ForeignKey("RFQHeaderReference")]
//        public virtual RequestForQuotationHeader FKRFQHeaderReference { get; set; }

//        [ForeignKey("UOMReference")]
//        public virtual UnitOfMeasure FKUOMReference { get; set; }

//        [ForeignKey("ArticleReference")]
//        public virtual ItemArticles FKArticleReference { get; set; }
//    }

//    public class RFQDashboardVM
//    {
//        public List<int> FiscalYears { get; set; }
//        public List<int> AbstractFiscalYears { get; set; }
//        public List<int> PreparedAbstractFiscalYears { get; set; }
//    }

//    public class RequestForQuotationVM
//    {
//        [Display(Name = "Fiscal Year")]
//        public int FiscalYear { get; set; }

//        [Display(Name = "Solicitation No.")]
//        public string SolicitationNo { get; set; }

//        [Display(Name = "Contract Code")]
//        public string ContractCode { get; set; }

//        [Display(Name = "Contract Name")]
//        public string ContractName { get; set; }

//        [Display(Name = "Deadline")]
//        public DateTime Deadline { get; set; }

//        [Display(Name = "Date Created")]
//        public DateTime CreatedAt { get; set; }

//        [Display(Name = "Created By")]
//        public string CreatedBy { get; set; }

//        [Display(Name = "Date Abstract Prepared")]
//        public DateTime? AbstractPreparedAt { get; set; }

//        [Display(Name = "Abstract Prepared By")]
//        public string AbstractPreparedBy { get; set; }

//        public bool IsSubmissionOpen { get; set; }
//    }
//    public class QuotationVM
//    {
//        [Display(Name = "Quotation No")]
//        public string QuotationNo { get; set; }

//        public int SupplierReference { get; set; }

//        [Display(Name = "Supplier Name")]
//        public string SupplierName { get; set; }

//        [Display(Name = "Address")]
//        public string Address { get; set; }

//        [Display(Name = "Contact Person")]
//        public string ContactPerson { get; set; }

//        [Display(Name = "Contact Number")]
//        public string ContactNumber { get; set; }

//        public string AlternateContactNumber { get; set; }

//        [Display(Name = "Tax Identification Number", ShortName = "TIN")]
//        public string TaxIdNumber { get; set; }

//        [Display(Name = "Email Address")]
//        public string EmailAddress { get; set; }

//        public List<RequestForQuotationDetailsVM> QuotationDetails { get; set; }

//        [Required]
//        [Display(Name = "Date Submitted")]
//        public DateTime SubmittedAt { get; set; }

//        public DateTime RecordedAt { get; set; }
//    }
//    public class RequestForQuotationFullVM
//    {
//        [Display(Name = "Fiscal Year")]
//        public int FiscalYear { get; set; }

//        [Display(Name = "ABC")]
//        public decimal ABC { get; set; }

//        [Display(Name = "Mode of Procurement")]
//        public string ModeOfProcurement { get; set; }

//        [Display(Name = "Solicitation No.")]
//        public string SolicitationNo { get; set; }

//        [Display(Name = "Contract Code")]
//        public string ContractCode { get; set; }

//        [Display(Name = "Contract Name")]
//        public string ContractName { get; set; }

//        [Display(Name = "Deadline")]
//        public DateTime Deadline { get; set; }

//        public List<QuotationVM> Quotations { get; set; }

//        public bool IsSubmissionOpen { get; set; }
//    }
//    public class RequestForQuotationForPostingVM
//    {
//        [Display(Name = "Fiscal Year")]
//        public int FiscalYear { get; set; }

//        [Display(Name = "ABC")]
//        public decimal ABC { get; set; }

//        [Display(Name = "Mode of Procurement")]
//        public string ModeOfProcurement { get; set; }

//        [Display(Name = "Solicitation No.")]
//        public string SolicitationNo { get; set; }

//        [Display(Name = "Contract Code")]
//        public string ContractCode { get; set; }

//        [Display(Name = "Contract Name")]
//        public string ContractName { get; set; }

//        [Display(Name = "Deadline")]
//        public DateTime Deadline { get; set; }

//        [Display(Name = "Date")]
//        public DateTime Date { get; set; }

//        [Display(Name = "Quotation No.")]
//        public string QuotationNo { get; set; }

//        public List<RequestForQuotationDetailsVM> Details { get; set; }
//    }

//    public class QuoteVM
//    {
//        [Display(Name = "Quotation No.")]
//        public string QuotationNo { get; set; }

//        [Display(Name = "Supplier")]
//        public string Supplier { get; set; }

//        [Required]
//        [Display(Name = "Remarks")]
//        [DataType(DataType.MultilineText)]
//        public string Remarks { get; set; }

//        [Display(Name = "Eligibility Requirements")]
//        public bool? EligibilityRequirements { get; set; }

//        [Display(Name = "Technical Requirements")]
//        public bool? TechnicalRequirements { get; set; }

//        [Display(Name = "Financial Requirements")]
//        public bool? FinancialRequirements { get; set; }

//        [Display(Name = "Recommend to Award")]
//        public bool? RecommendToAward { get; set; }

//        [Display(Name = "Lowest Calculated and Responsive Quotation")]
//        public bool LCRQ { get; set; }

//        public List<QuoteEvaluationDetailsVM> Items { get; set; }
//    }
//    public class QuoteEvaluationDetailsVM
//    {
//        [Display(Name = "Article")]
//        public int? ArticleReference { get; set; }

//        [MaxLength(2)]
//        public string ItemSequence { get; set; }

//        [Display(Name = "Item Full Name")]
//        public string ItemFullName { get; set; }

//        [System.Web.Mvc.AllowHtml]
//        [DataType(DataType.MultilineText)]
//        [Display(Name = "Full Specifications")]
//        public string ItemSpecifications { get; set; }

//        [Display(Name = "Unit of Measure")]
//        public int UOMReference { get; set; }

//        [Display(Name = "Unit of Measure")]
//        public string UnitOfMeasure { get; set; }

//        public int Quantity { get; set; }

//        [Display(Name = "Unit Price")]
//        public decimal? UnitBidPrice { get; set; }

//        [Display(Name = "Total Bid")]
//        public decimal? TotalBid { get; set; }

//        [Display(Name = "Unit Cost")]
//        public decimal? UnitCost { get; set; }

//        [Display(Name = "Approved Budget")]
//        public decimal? TotalCost { get; set; }

//        [Display(Name = "Variance")]
//        public decimal? Variance { get; set; }

//        [Display(Name = "Savings")]
//        public decimal? Savings { get; set; }
//    }
//    public class QuoteEvaluationVM
//    {
//        [Display(Name = "Solicitation No.")]
//        public string SolicitationNo { get; set; }

//        [Display(Name = "Fiscal Year")]
//        public int FiscalYear { get; set; }

//        [Display(Name = "Contract Code")]
//        public string ContractCode { get; set; }

//        [Display(Name = "Contract Name")]
//        public string ContractName { get; set; }

//        [Display(Name = "Approved Contract for the Contract")]
//        public decimal ABC { get; set; }

//        [Required]
//        [Display(Name = "Place of Opening")]
//        public string PlaceOpened { get; set; }

//        [Required]
//        [Display(Name = "Date Opened")]
//        public DateTime OpenedAt { get; set; }

//        public List<QuoteVM> Details { get; set; }
//    }

//    public class SupplierQuotesVM
//    {
//        [Display(Name = "Supplier Name")]
//        public string SupplierName { get; set; }

//        [Display(Name = "Total Bid")]
//        public decimal? TotalBid { get; set; }

//        [Display(Name = "Eligibility Requirements")]
//        public bool? EligibilityRequirements { get; set; }

//        [Display(Name = "Technical Requirements")]
//        public bool? TechnicalRequirements { get; set; }

//        [Display(Name = "Financial Requirements")]
//        public bool? FinancialRequirements { get; set; }

//        [Display(Name = "Recommend to Award")]
//        public bool? RecommendToAward { get; set; }

//        [Display(Name = "Lowest Calculated and Responsive Quotation")]
//        public bool? LCRQ { get; set; }

//        [Display(Name = "Variance")]
//        public decimal? Variance { get; set; }

//        [Display(Name = "Savings")]
//        public decimal? Savings { get; set; }

//        [Display(Name = "Remarks")]
//        public string Remarks { get; set; }
//    }
//    public class AbstactOfQuotationVM
//    {
//        [Display(Name = "Fiscal Year")]
//        public int FiscalYear { get; set; }

//        [Display(Name = "Contract Code")]
//        public string ContractCode { get; set; }

//        [Display(Name = "Contract Name")]
//        public string ContractName { get; set; }

//        [Display(Name = "Approved Contract for the Contract")]
//        public decimal ABC { get; set; }

//        [Required]
//        [Display(Name = "Place of Opening")]
//        public string PlaceOpened { get; set; }

//        [Required]
//        [Display(Name = "Date Opened")]
//        public DateTime OpenedAt { get; set; }

//        public List<SupplierQuotesVM> Suppliers { get; set; }
//    }
//}