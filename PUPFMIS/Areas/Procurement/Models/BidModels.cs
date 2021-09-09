using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROC_MSTR_Eligibility_Documents")]
    public class EligibilityRequirements
    {

    }

    [Table("PROC_MSTR_Securities")]
    public class Securities
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public SecurityTypes SecurityType { get; set; }

        [Required]
        [Display(Name = "Form of Bid Security")]
        public string FormOfSecurity { get; set; }

        [Required]
        public decimal Percentage { get; set; }
    }
    [Table("PROC_TRXN_Bid")]
    public class BidsHeader
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public BidTypes BidType { get; set; }

        [Required]
        [Display(Name = "Bid Reference No.")]
        public string BidReferenceNo { get; set; }

        [Required]
        public int SupplierReference { get; set; }

        [Required]
        public int ProcurementProject { get; set; }

        public int? BidSecurity { get; set; }

        [Display(Name = "Bank/Company")]
        public string BidSecurityBankReference { get; set; }

        [Display(Name = "Reference No.")]
        public string BidSecurityBankReferenceNo { get; set; }

        [Display(Name = "Validity Period")]
        public string BidSecurityValidityPeriod { get; set; }

        [Display(Name = "Bid Security Amount")]
        public decimal? BidSecurityAmount { get; set; }

        public int? PerformanceSecurity { get; set; }

        [Display(Name = "Bank/Company")]
        public string PerformanceSecurityBankReference { get; set; }

        [Display(Name = "Reference No.")]
        public string PerformanceSecurityBankReferenceNo { get; set; }

        [Display(Name = "Validity Period")]
        public string PerformanceSecurityValidityPeriod { get; set; }

        [Display(Name = "Bid Security Amount")]
        public decimal? PerformanceSecurityAmount { get; set; }

        [Display(Name = "Date Bid Submitted")]
        public DateTime? BidSubmittedAt { get; set; }

        [Display(Name = "Total Amount of Bid as Read")]
        public decimal TotalAmountOfBidAsRead { get; set; }

        [Display(Name = "Bid Security Required?")]
        public bool IsBidSecurityRequired { get; set; }

        [Display(Name = "Performance Security Required")]
        public bool IsPerformaceSecurityRequired { get; set; }

        [Display(Name = "Bid Sufficiency")]
        public BidSufficiencyOptions? BidSufficiency { get; set; }

        [Display(Name = "Bid Remarks")]
        public BidRemarksOptions? BidRemarks { get; set; }

        [Display(Name = "Eligibility Requirements")]
        public BidRemarksOptions? EligibilityRequirements { get; set; }

        [Display(Name = "Technical Requirements")]
        public BidRemarksOptions? TechnicalRequirements { get; set; }

        [Display(Name = "Financial Requirements")]
        public BidRemarksOptions? FinancialRequirements { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Recommend to Award Contract")]
        public bool? RecommendToAward { get; set; }

        [Display(Name = "Date Recorded")]
        public DateTime RecordedAt { get; set; }

        [Display(Name = "Recorded By")]
        public string RecordedBy { get; set; }

        [ForeignKey("SupplierReference")]
        public virtual Supplier FKSupplierReference { get; set; }

        [ForeignKey("ProcurementProjectReference")]
        public virtual ProcurementProject FKProcurementProject { get; set; }

        [ForeignKey("BidSecurity")]
        public virtual Securities FKBidSecurityReference { get; set; }

        [ForeignKey("PerformanceSecurity")]
        public virtual Securities FKPerformanceSecurityReference { get; set; }
    }
    [Table("PROC_TRXN_Bid_Details")]
    public class BidDetails
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Bid Action")]
        public BidActions BidAction { get; set; }

        public int BidReference { get; set; }

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

        [Required]
        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Bid Unit Price")]
        public decimal? BidUnitPrice { get; set; }

        [Display(Name = "Bid Total Price")]
        public decimal? BidTotalPrice { get; set; }

        [ForeignKey("BidReference")]
        public virtual BidsHeader FKBidsReference { get; set; }

        [ForeignKey("UOMReference")]
        public virtual UnitOfMeasure FKUOMReference { get; set; }

        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKArticleReference { get; set; }
    }

    public class BidDetailsVM
    {
        [Display(Name = "Bid Action")]
        public BidActions BidAction { get; set; }

        public int BidReference { get; set; }

        [Display(Name = "Article")]
        public int? ArticleReference { get; set; }

        public string ItemSequence { get; set; }

        [Display(Name = "Item Full Name")]
        public string ItemFullName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Unit of Measure")]
        public string Unit { get; set; }

        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Total Cost")]
        public decimal TotalCost { get; set; }

        public int Quantity { get; set; }

        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        [Display(Name = "Bid Unit Price")]
        public decimal BidUnitPrice { get; set; }

        [Display(Name = "Bid Total Price")]
        public decimal? BidTotalPrice { get; set; }
    }
    public class BidsVM
    {
        [Required]
        [Display(Name = "Date Submitted")]
        public DateTime SubmittedAt { get; set; }

        [Required]
        [Display(Name = "Annual Procurement Plan PAP Code")]
        public string PAPCode { get; set; }

        [Required]
        [Display(Name = "Classification")]
        public int ClassificationReference { get; set; }

        [Required]
        [Display(Name = "Classification")]
        public string Classification { get; set; }

        [Required]
        [Display(Name = "Mode of Procurement")]
        public int ModeOfProcurementReference { get; set; }

        [Required]
        [Display(Name = "Mode of Procurement")]
        public string ModeOfProcurement { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Required]
        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Required]
        [Display(Name = "Fund Source")]
        public string FundDescription { get; set; }

        [Display(Name = "Contract Type")]
        public ProcurementProjectTypes ProcurementProjectType { get; set; }

        [Display(Name = "Contract Strategy")]
        public ContractStrategies ContractStrategy { get; set; }

        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Contract Location")]
        public string ContractLocation { get; set; }

        [Display(Name = "Contract Status")]
        public ProcurementProjectStatus ContractStatus { get; set; }

        [Display(Name = "Contract Stage")]
        public ProcurementProjectStages ProcurementProjectStage { get; set; }

        [Required]
        [Display(Name = "Approved Budget for the Contract")]
        public decimal ApprovedBudgetForContract { get; set; }

        [Required]
        [Display(Name = "Project Completion/Delivery Period")]
        public int DeliveryPeriod { get; set; }

        [Required]
        [Display(Name = "Project Coordinator")]
        public string ProjectCoordinator { get; set; }

        [Required]
        public BidTypes BidType { get; set; }

        [Required]
        [Display(Name = "Bid Reference No.")]
        public string BidReferenceNo { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public int SupplierReference { get; set; }

        [Required]
        public int ProcurementProject { get; set; }

        public int? BidSecurity { get; set; }

        [Display(Name = "Bank/Company")]
        public string BidSecurityBankReference { get; set; }

        [Display(Name = "Reference No.")]
        public string BidSecurityBankReferenceNo { get; set; }

        [Display(Name = "Validity Period")]
        public string BidSecurityValidityPeriod { get; set; }

        [Display(Name = "Bid Security Amount")]
        public decimal? BidSecurityAmount { get; set; }

        public int? PerformanceSecurity { get; set; }

        [Display(Name = "Bank/Company")]
        public string PerformanceSecurityBankReference { get; set; }

        [Display(Name = "Reference No.")]
        public string PerformanceSecurityBankReferenceNo { get; set; }

        [Display(Name = "Validity Period")]
        public string PerformanceSecurityValidityPeriod { get; set; }

        [Display(Name = "Bid Security Amount")]
        public decimal? PerformanceSecurityAmount { get; set; }

        [Display(Name = "Date Bid Submitted")]
        public DateTime? BidSubmittedAt { get; set; }

        [Display(Name = "Total Amount of Bid as Read")]
        public decimal TotalAmountOfBidAsRead { get; set; }

        [Display(Name = "Bid Security Required?")]
        public bool IsBidSecurityRequired { get; set; }

        [Display(Name = "Is Performance Security Required?")]
        public bool IsPerformaceSecurityRequired { get; set; }

        [Display(Name = "Bid Sufficiency")]
        public BidSufficiencyOptions? BidSufficiency { get; set; }

        [Display(Name = "Bid Remarks")]
        public BidRemarksOptions? BidRemarks { get; set; }

        [Display(Name = "Eligibility Requirements")]
        public BidRemarksOptions? EligibilityRequirements { get; set; }

        [Display(Name = "Technical Requirements")]
        public BidRemarksOptions? TechnicalRequirements { get; set; }

        [Display(Name = "Financial Requirements")]
        public BidRemarksOptions? FinancialRequirements { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Recommend to Award Contract")]
        public bool? RecommendToAward { get; set; }

        [Display(Name = "Date Recorded")]
        public DateTime RecordedAt { get; set; }

        [Display(Name = "Recorded By")]
        public string RecordedBy { get; set; }

        public List<BidDetailsVM> BidDetails { get; set; }
    }
    public class BidsListVM
    {
        [Required]
        [Display(Name = "Bid Reference No.")]
        public string BidReferenceNo { get; set; }

        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Display(Name = "Contract")]
        public string ContractName { get; set; }

        public int SupplierReference { get; set; }

        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Display(Name = "Total Amount of Bid")]
        public decimal TotalAmountOfBid { get; set; }

        [Display(Name = "Date Bid Submitted")]
        public DateTime BidSubmittedAt { get; set; }

        [Display(Name = "Date Recorded")]
        public DateTime RecordedAt { get; set; }
    }
    public class BidsDashboard
    {
        public List<ProcurementProjectListVM> Contracts { get; set; }
    }

    public enum SecurityTypes
    {
        BidSecurity,
        PerformanceSecurity
    }
    public enum BidTypes
    {
        [Display(Name = "Bid", ShortName = "BID")]
        Bid,

        [Display(Name = "Quotation", ShortName = "QTN")]
        Quotation
    }
    public enum BidActions
    {
        [Display(Name = "With Bid")]
        WithBid,

        [Display(Name = "Blank")]
        Blank,

        [Display(Name = "No Bid")]
        NoBid
    }
    public enum BidSufficiencyOptions
    {
        [Display(Name = "Sufficient")]
        Sufficient,

        [Display(Name = "Insufficient")]
        Insufficient
    }
    public enum BidRemarksOptions
    {
        [Display(Name = "Passed")]
        Passed,

        [Display(Name = "Failed")]
        Failed
    }
    public enum BidRegistrationOptions
    {
        WinningBidsOnly,
        AllBids
    }
}