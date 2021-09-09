using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROC_TRXN_Procurement_Project")]
    public class ProcurementProject
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Parent Contract")]
        public int? ParentProjectReference { get; set; }

        [Required]
        [Display(Name = "Annual Procurement Plan Reference")]
        public int APPReference { get; set; }

        [Required]
        [Display(Name = "Classification")]
        public int ClassificationReference { get; set; }

        [Required]
        [Display(Name = "Mode of Procurement")]
        public int ModeOfProcurementReference { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Display(Name = "Contract Type")]
        public ProcurementProjectTypes ProcurementProjectType { get; set; }

        [Display(Name = "Contract Strategy")]
        public ContractStrategies ContractStrategy { get; set; }

        [Display(Name = "Contract Project Code")]
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

        [Display(Name = "Project Completion/Delivery Period")]
        public int? DeliveryPeriod { get; set; }

        [Display(Name = "Project Coordinator")]
        public string ProjectCoordinator { get; set; }

        [Display(Name = "P/R Submission Opening")]
        public DateTime? PRSubmissionOpen { get; set; }

        [Display(Name = "P/R Submission Closing")]
        public DateTime? PRSubmissionClose { get; set; }

        [Display(Name = "Pre-Procurement Conference")]
        public DateTime? PreProcurementConference { get; set; }

        [Display(Name = "Preparation of IB/RFQ/RFP")]
        public DateTime? IBPreparation { get; set; }

        [Display(Name = "Posting of Invitation to Bid", ShortName = "Posting of Request for Quotation")]
        public DateTime? PostingOfIB_RFQPosting { get; set; }

        [Display(Name = "Pre-Bid Conference")]
        public DateTime? PreBidConference { get; set; }

        [Display(Name = "Deadline of Submission of Bids", ShortName = "Deadline of Submission of Request for Quotation")]
        public DateTime? DeadlineOfSubmissionOfBids_DeadlineOfSubmisionOfBids { get; set; }

        [Display(Name = "Opening of Bids", ShortName = "Opening of Quotations")]
        public DateTime? OpeningOfBids_OpeningOfQuotations { get; set; }

        [Display(Name = "NOA Issuance")]
        public DateTime? NOAIssuance { get; set; }

        [Display(Name = "NTP Issuance")]
        public DateTime? NTPIssuance { get; set; }

        [Display(Name = "Pre-Procurement Conference Location")]
        public string PreProcurementConferenceLocation { get; set; }

        [Display(Name = "Pre-Bid Conference Location")]
        public string PreBidConferenceLocation { get; set; }

        [Display(Name = "Video Conferencing Options")]
        public VideoConferencingOptions? PreBidVideoConferencingOptions { get; set; }

        [MaxLength(75)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Video Conference Mode")]
        public string PreBidVideoConferenceMode { get; set; }

        [MaxLength(75)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Access Request Email")]
        public string PreBidVideoConferenceAccessRequestEmail { get; set; }

        [MaxLength(20)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Access Request Contact Number")]
        public string PreBidVideoConferenceAccessRequestContactNo { get; set; }

        [Display(Name = "Additional Instructions")]
        public string PreBidAdditionalInstructions { get; set; }

        [Display(Name = "Bid Document Price")]
        public decimal? BidDocumentPrice { get; set; }

        [Display(Name = "Opening of Bids Location")]
        public string OpeningOfBidsLocation { get; set; }

        [Display(Name = "Reason for Failure of Bidding (Opening)")]
        public BidsFailureReasons? OpeningOfBidsFailureReason { get; set; }

        [Display(Name = "Reason for Failure of Bidding (Post-Qualification)")]
        public BidsFailureReasons? PostQualificationFailureReason { get; set; }

        [Display(Name = "Solicitation No.")]
        public string SolicitationNo { get; set; }

        [Display(Name = "Date Notice of Award Accepted by Supplier/Contractor")]
        public DateTime? NOAAcceptedAt { get; set; }

        [Display(Name = "Date Contract signed by Supplier/Contractor")]
        public DateTime? ContractSignedAt { get; set; }

        [Display(Name = "Date Notice to Proceed signed by Supplier/Contractor")]
        public DateTime? NTPSignedAt { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [ForeignKey("ParentProjectReference")]
        public virtual ProcurementProject FKParentProjectReference { get; set; }

        [ForeignKey("APPReference")]
        public virtual AnnualProcurementPlanDetails FKAPPReference { get; set; }

        [ForeignKey("ClassificationReference")]
        public virtual ItemClassification FKClassificationReference { get; set; }

        [ForeignKey("ModeOfProcurementReference")]
        public virtual ModesOfProcurement FKModeOfProcurementReference { get; set; }
    }
    [Table("PROC_TRXN_Procurement_Project_Details")]
    public class ProcurementProjectDetails
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Contract Reference")]
        public int ProcurementProjectReference { get; set; }

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

        [Display(Name = "Estimated Unit Cost")]
        public decimal EstimatedUnitCost { get; set; }

        [Required]
        [Display(Name = "Approved Budget for the Item")]
        public decimal ApprovedBudgetForItem { get; set; }

        [Required]
        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        [ForeignKey("ProcurementProjectReference")]
        public virtual ProcurementProject FKProcurementProjectReference { get; set; }

        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKArticleReference { get; set; }

        [ForeignKey("UOMReference")]
        public virtual UnitOfMeasure FKUOMReference { get; set; }
    }
    [Table("PROC_TRXN_Procurement_Project_Updates")]
    public class ContractUpdates
    {
        [Key]
        public int ID { get; set; }

        public int ProcurementProjectReference { get; set; }

        public ProcurementProjectStages ProcurementProjectStage { get; set; }

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

        [ForeignKey("ProcurementProjectReference")]
        public virtual ProcurementProject FKProcurementProjectReference { get; set; }
    }

    public class ProcurementProgramsVM
    {
        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "Program Name")]
        public string ProgramName { get; set; }
    }
    public class ProcurementProjectsVM
    {
        public List<ProcurementProjectListVM> NewProjects { get; set; }
        public List<ProcurementProjectListVM> OpenPRSubmissions { get; set; }
        public List<ProcurementProjectListVM> ForPreProcurementConference { get; set; }
        public List<ProcurementProjectListVM> ForPostingOfIB { get; set; }
        public List<ProcurementProjectListVM> ForPreBidConference { get; set; }
        public List<ProcurementProjectListVM> ForOpeningOfBids { get; set; }
    }

    public class ContractProgramsVM
    {
        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "ProcurementProgram")]
        public string ProcurementProgram { get; set; }
    }
    public class ProcurementProjectDetailsVM
    {
        [Display(Name = "Contract Reference")]
        public int ProcurementProjectReference { get; set; }

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

        [Display(Name = "Estimated Unit Cost")]
        public decimal EstimatedUnitCost { get; set; }

        [Required]
        [Display(Name = "Approved Budget for the Item")]
        public decimal ApprovedBudgetForItem { get; set; }

        [Required]
        [Display(Name = "Savings")]
        public decimal Savings { get; set; }

        [Required]
        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        [Required]
        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }
    }
    public class ProcurementProjectSetupVM
    {
        [Display(Name = "Procurement Program")]
        public string PAPCode { get; set; }

        [Display(Name = "Contract Strategy")]
        public ContractStrategies ContractStrategy { get; set; }

        [Display(Name = "Contract Type")]
        public ProcurementProjectTypes ProcurementProjectType { get; set; }

        [Display(Name = "Mode of Procurement")]
        public int ModeOfProcurement { get; set; }
    }
    public class IndividualProcurementProjectVM
    {
        [Display(Name = "Lot/Line No.")]
        public int? LotNo { get; set; }

        [Display(Name = "Parent Contract")]
        public string ParentProjectReference { get; set; }

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
        [Display(Name = "P/R Submission Opening")]
        public DateTime PRSubmissionOpen { get; set; }

        [Required]
        [Display(Name = "P/R Submission Closing")]
        public DateTime PRSubmissionClose { get; set; }

        [Display(Name = "Pre-Procurement Conference")]
        public DateTime? PreProcurementConference { get; set; }

        [Display(Name = "Preparation of IB/RFQ/RFP")]
        public DateTime? IBPreparation { get; set; }

        [Required]
        [Display(Name = "Posting of Invitation to Bid", ShortName = "Posting of Request for Quotation")]
        public DateTime PostingOfIB { get; set; }

        [Display(Name = "Pre-Bid Conference")]
        public DateTime? PreBidConference { get; set; }

        [Required]
        [Display(Name = "Deadline of Submission of Bids", ShortName = "Deadline of Submission of Request for Quotation")]
        public DateTime DeadlineOfSubmissionOfBids { get; set; }

        [Required]
        [Display(Name = "Opening of Bids", ShortName = "Evaluation of Quotations")]
        public DateTime OpeningOfBids { get; set; }

        [Required]
        [Display(Name = "NOA Issuance")]
        public DateTime? NOAIssuance { get; set; }

        [Required]
        [Display(Name = "NTP Issuance")]
        public DateTime? NTPIssuance { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
    }
    public class LotProcurementProjectVM
    {
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

        [Required]
        [Display(Name = "Project Coordinator")]
        public string ProjectCoordinator { get; set; }

        [Display(Name = "Contract Status")]
        public ProcurementProjectStatus ContractStatus { get; set; }

        [Display(Name = "Contract Stage")]
        public ProcurementProjectStages ProcurementProjectStage { get; set; }

        [Required]
        [Display(Name = "P/R Submission Opening")]
        public DateTime PRSubmissionOpen { get; set; }

        [Required]
        [Display(Name = "P/R Submission Closing")]
        public DateTime PRSubmissionClose { get; set; }

        [Display(Name = "Preparation of IB/RFQ/RFP")]
        public DateTime? IBPreparation { get; set; }

        [Required]
        [Display(Name = "Posting of Invitation to Bid", ShortName = "Posting of Request for Quotation")]
        public DateTime PostingOfIB { get; set; }

        [Required]
        [Display(Name = "NOA Issuance")]
        public DateTime? NOAIssuance { get; set; }

        [Required]
        [Display(Name = "NTP Issuance")]
        public DateTime? NTPIssuance { get; set; }

        [Required]
        [Display(Name = "Approved Budget for the Contract")]
        public decimal ApprovedBudgetForContract { get; set; }

        public List<IndividualProcurementProjectVM> SubContracts { get; set; }
    }

    public class BidsRegistrationUpdateVM
    {
        [Display(Name = "Lot/Line No.")]
        public int? LotNo { get; set; }

        [Display(Name = "Parent Contract")]
        public string ParentProjectReference { get; set; }

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

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
        public List<BidsListVM> BidList { get; set; }
    }
    public class RequestForQuotationDetailsVM
    {
        [Display(Name = "Article")]
        public int? ArticleReference { get; set; }

        [MaxLength(2)]
        public string ItemSequence { get; set; }

        [Display(Name = "Item Full Name")]
        public string ItemFullName { get; set; }

        [System.Web.Mvc.AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Has offer?")]
        public bool NoOffer { get; set; }

        [Display(Name = "Unit Price")]
        public decimal? UnitPrice { get; set; }

        [Display(Name = "Total Unit Price")]
        public decimal? TotalUnitPrice { get; set; }

        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }
    }
    public class ProcurementProjectListVM
    {
        [Display(Name = "Contract Type")]
        public ProcurementProjectTypes ProcurementProjectType { get; set; }

        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Display(Name = "Mode of Procurement")]
        public int ModeOfProcurementReference { get; set; }

        [Display(Name = "Mode of Procurement")]
        public string ModeOfProcurement { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "Contract Location")]
        public string ContractLocation { get; set; }

        [Display(Name = "Contract Strategy")]
        public ContractStrategies ContractStrategy { get; set; }

        [Display(Name = "Contract Status")]
        public ProcurementProjectStatus ContractStatus { get; set; }

        [Display(Name = "Contract Stage")]
        public ProcurementProjectStages ProcurementProjectStage { get; set; }

        [Display(Name = "Approved Budget for the Contract")]
        public decimal ApprovedBudgetForContract { get; set; }
    }

    public enum ProcurementProjectTypes
    {
        [Display(Name = "Competitive Public Bidding", ShortName = "CPB")]
        CPB,

        [Display(Name = "Alternative Mode of Procurement", ShortName = "AMP")]
        AMP
    }
    public enum ProcurementProjectStatus
    {
        [Display(Name = "Contract Created")]
        ContractCreated = 0,

        [Display(Name = "Contract Created - Reposted")]
        ContractCreatedReposted = 1,

        [Display(Name = "Procurement Ongoing")]
        ContractProcurementOngoing = 2,

        [Display(Name = "Procurement Failed")]
        ContractFailed = 3,

        [Display(Name = "Procurement Cancelled")]
        ContractProcurementCancelled = 4,

        [Display(Name = "Procurement Closed")]
        ContractProcurementClosed = 5,

        [Display(Name = "For Delivery")]
        ContractForDelivery = 6,

        [Display(Name = "Delivered")]
        ContractDelivered = 7
    }
    public enum ProcurementProjectStages
    {
        [Display(Name = "Contract Opened")]
        ContractOpened = 0,

        [Display(Name = "Purchase Request Submission Open")]
        PurchaseRequestSubmissionOpening = 1,

        [Display(Name = "Purchase Request Submission Closed")]
        PRSubmissionClosed = 2,

        [Display(Name = "Pre-Procurement Conference Setup")]
        PreProcurementConferenceSetup = 3,

        [Display(Name = "Pre-Procurement Conference Updated")]
        PreProcurementConferenceUpdate = 4,

        [Display(Name = "Pre-Bid Conference Setup")]
        PreBidConferenceSetup = 5,

        [Display(Name = "Generation of Request for Quotation")]
        GenerationOfRFQ = 6,

        [Display(Name = "Pre-Bid Conference Updated")]
        PreBidConferenceUpdate = 7,

        [Display(Name = "Bids Opened")]
        BidsOpened = 8,

        [Display(Name = "Bids Evaluated")]
        BidsEvaluated = 9,

        [Display(Name = "Post-Qualified")]
        PostQualification = 10,

        [Display(Name = "Notice of Award Setup")]
        NoticeOfAwardSetup = 11,

        [Display(Name = "Notice of Award Update")]
        NoticeOfAwardUpdate = 12,

        [Display(Name = "Notice to Proceed Setup")]
        NoticeToProceedSetup = 13,

        [Display(Name = "Notice to Proceed Update")]
        NoticeToProceedUpdate = 14,

        [Display(Name = "Failure of Bidding Declared")]
        BiddingFailed = 15,

        [Display(Name = "Procurement Closed")]
        ProcurementClosed = 16
    }
    public enum ContractStrategies
    {
        [Display(Name = "Single Contract Procurement", ShortName = "SCPR")]
        BulkBidding = 0,

        [Display(Name = "Lot Procurement", ShortName = "LTPR")]
        LotBidding = 1,

        [Display(Name = "Line Item Procurement", ShortName = "LIPR")]
        LineItemBidding = 2,

        [Display(Name = "Per Item Procurement", ShortName = "PIPR")]
        ItemProcurement = 3
    }
    public enum VideoConferencingOptions
    {
        [Display(Name = "No Video Conferencing")]
        NoVideoConferencing,

        [Display(Name = "With Video Conferencing")]
        WithVideoConferencing,

        [Display(Name = "Full Video Conferencing")]
        FullVideoConferencing
    }
    public enum BidsFailureReasons
    {
        [Display(Name = "No Bids Received")]
        NoBidders,

        [Display(Name = "All Bidders are declared ineligible")]
        AllBiddersIneligible,

        [Display(Name = "All bids failed to comply")]
        BidsFailedToComply,

        [Display(Name = "All bids failed to post-qualification")]
        BidsFailedToPostQualification,

        [Display(Name = "No successful negotiation (Consulting)")]
        NoSuccessfulNegotiation,

        [Display(Name = "Bidder refused award of contract")]
        BidderRefusedAward,

        [Display(Name = "No award made")]
        NoAwardMade
    }
}