using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    public class OpenProcurementProjectVM
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

        [Display(Name = "Reason for Failure of Bidding")]
        public BidsFailureReasons? OpeningOfBidsFailureReason { get; set; }

        [Display(Name = "Reason for Failure of Bidding")]
        public BidsFailureReasons? PostQualificationFailureReason { get; set; }

        [Required]
        [Display(Name = "NTP Issuance")]
        public DateTime? NTPIssuance { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
    }
    public class PreProcurementConferenceSetupVM
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

        [Display(Name = "Pre-Procurement Conference")]
        public DateTime? PreProcurementConference { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Required]
        [Display(Name = "Pre-Procurement Conference Location")]
        public string PreProcurementConferenceLocation { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
    }
    public class PreProcurementConferenceUpdateVM
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

        [Display(Name = "Pre-Procurement Conference")]
        public DateTime? PreProcurementConference { get; set; }

        [Required]
        [Display(Name = "Pre-Procurement Conference Location")]
        public string PreProcurementConferenceLocation { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
        public ContractUpdates ContractUpdate { get; set; }
    }
    public class PreBidConferenceSetupVM
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
        [Display(Name = "Pre-Bid Conference")]
        public DateTime PreBidConference { get; set; }

        [Required]
        [Display(Name = "Pre-Bid Conference Location")]
        public string PreBidConferenceLocation { get; set; }

        [Required]
        [Display(Name = "Deadline of Submission of Bids")]
        public DateTime DeadlineOfSubmissionOfBids { get; set; }

        [Display(Name = "Opening of Bids")]
        public DateTime OpeningOfBids { get; set; }

        [Display(Name = "Video Conferencing Options")]
        public VideoConferencingOptions PreBidVideoConferencingOptions { get; set; }

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

        [DataType(DataType.MultilineText)]
        [Display(Name = "Additional Instructions")]
        public string PreBidAdditionalInstructions { get; set; }

        [Required]
        [Display(Name = "Bid Document Price")]
        public decimal BidDocumentPrice { get; set; }

        [Required]
        [Display(Name = "Opening of Bids Location")]
        public string OpeningOfBidsLocation { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
    }
    public class PreBidConferenceUpdateVM
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

        [Display(Name = "Pre-Bid Conference")]
        public DateTime? PreBidConference { get; set; }

        [Required]
        [Display(Name = "Pre-Bid Conference Location")]
        public string PreBidConferenceLocation { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
        public ContractUpdates ContractUpdate { get; set; }
    }
    public class OpeningOfBidsUpdateVM
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
        [Display(Name = "Opening of Bids")]
        public DateTime OpeningOfBids { get; set; }

        [Required]
        [Display(Name = "Opening of Bids Location")]
        public string OpeningOfBidsLocation { get; set; }

        [Required]
        [Display(Name = "Failure of Bidding Declared")]
        public bool FailureOfBiddingDeclared { get; set; }

        [Required]
        [Display(Name = "Reason for Failure of Bidding (Opening)")]
        public BidsFailureReasons OpeningOfBidsFailureReason { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
        public ContractUpdates ContractUpdate { get; set; }
    }
    public class EvaluationOfBidsUpdateVM
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
        public ContractUpdates ContractUpdate { get; set; }
    }
    public class PostQualificationUpdateVM
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
        [Display(Name = "Failure of Bidding Declared")]
        public bool FailureOfBiddingDeclared { get; set; }

        [Required]
        [Display(Name = "Reason for Failure of Bidding (Post-Qualification)")]
        public BidsFailureReasons PostQualificationFailureReason { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
        public ContractUpdates ContractUpdate { get; set; }
    }
    public class NoticeOfAwardSetupVM
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

        [Display(Name = "Contractor/Supplier")]
        public int Supplier { get; set; }

        [Required]
        [Display(Name = "Contract Status")]
        public ContractTypes ContractType { get; set; }

        [Required]
        [Display(Name = "Failure of Bidding Declared")]
        public bool FailureOfBiddingDeclared { get; set; }

        [Required]
        [Display(Name = "Reason for Failure of Bidding (Opening)")]
        public BidsFailureReasons OpeningOfBidsFailureReason { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
        public List<BidDetailsVM> BidDetails { get; set; }
    }
    public class LineItemContractorVM
    {
        [Display(Name = "Contractor/Supplier")]
        public int Supplier { get; set; }

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

        [Range(1.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        [Display(Name = "Bid Unit Price")]
        public decimal BidUnitPrice { get; set; }

        [Display(Name = "Bid Total Price")]
        public decimal? BidTotalPrice { get; set; }
    }
    public class NoticeOfAwardSetupLineItemsVM
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

        [Required]
        [Display(Name = "Contract Status")]
        public ContractTypes ContractType { get; set; }

        [Required]
        [Display(Name = "Failure of Bidding Declared")]
        public bool FailureOfBiddingDeclared { get; set; }

        [Required]
        [Display(Name = "Reason for Failure of Bidding (Opening)")]
        public BidsFailureReasons OpeningOfBidsFailureReason { get; set; }

        public List<LineItemContractorVM> LineItems { get; set; }
        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
    }
    public class NoticeOfAwardUpdateVM
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

        [Required]
        [Display(Name = "Date Notice of Award Signed by Supplier/Contractor")]
        public DateTime? NOAAcceptedAt { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
        public ContractUpdates ContractUpdate { get; set; }
    }
    public class NoticeToProceedSetupVM
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

        [Display(Name = "Date Contract signed by Supplier/Contractor")]
        public DateTime? ContractSignedAt { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
        public ContractUpdates ContractUpdate { get; set; }
    }
    public class NoticeToProceedUpdateVM
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

        [Required]
        [Display(Name = "Date Notice to Proceed signed by Supplier/Contractor")]
        public DateTime NTPSignedAt { get; set; }

        [Required]
        [Display(Name = "Contract Effectivity Date")]
        public DateTime EffectivityAt { get; set; }

        public List<ProcurementProjectDetailsVM> ContractItems { get; set; }
        public List<ContractUpdates> Updates { get; set; }
        public ContractUpdates ContractUpdate { get; set; }
    }
    public class PreProcurementConferenceTemplateVM
    {
        public DateTime? PreProcurementConferenceDate { get; set; }
        public string PreProcurementConferenceLocation { get; set; }
        public string ProjectCoordinator { get; set; }
        public string DepartmentHead { get; set; }
        public string DepartmentHeadLastName { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public string Designation { get; set; }
        public string ContractCode { get; set; }
        public string ContractName { get; set; }
    }
    public class InvitationToBidTemplateVM
    {
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "Contract Strategy")]
        public ContractStrategies ContractStrategy { get; set; }

        [Display(Name = "Approved Budget for the Contract")]
        public decimal ApprovedBudgetForContract { get; set; }

        [Display(Name = "Approved Budget for the Contract")]
        public string ApprovedBudgetForContractWords { get; set; }

        [Display(Name = "Fund Source")]
        public string FundDescription { get; set; }

        [Display(Name = "Classification")]
        public string Classification { get; set; }

        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Display(Name = "Project Completion/Delivery Period")]
        public int DeliveryPeriod { get; set; }

        [Display(Name = "Project Completion/Delivery Period")]
        public string DeliveryPeriodWords { get; set; }

        [Display(Name = "Submission Start")]
        public DateTime SubmissionStart { get; set; }

        [Display(Name = "Deadline of Submission of Bids")]
        public DateTime DeadlineOfSubmissionOfBids { get; set; }

        [Display(Name = "Opening of Bids")]
        public DateTime OpeningOfBids { get; set; }

        [Display(Name = "Opening of Bids Location")]
        public string OpeningOfBidsLocation { get; set; }

        [Display(Name = "Bid Document Price")]
        public decimal BidDocumentPrice { get; set; }

        [Display(Name = "Bid Document Price")]
        public string BidDocumentPriceWords { get; set; }

        [Display(Name = "Pre-Bid Conference")]
        public DateTime PreBidConference { get; set; }

        [Display(Name = "Pre-Bid Conference Location")]
        public string PreBidConferenceLocation { get; set; }

        [Display(Name = "Video Conferencing Options")]
        public VideoConferencingOptions PreBidVideoConferencingOptions { get; set; }

        [Display(Name = "Video Conference Mode")]
        public string PreBidVideoConferenceMode { get; set; }

        [Display(Name = "Access Request Email")]
        public string PreBidVideoConferenceAccessRequestEmail { get; set; }

        [Display(Name = "Access Request Contact Number")]
        public string PreBidVideoConferenceAccessRequestContactNo { get; set; }

        [Display(Name = "Additional Instructions")]
        public string PreBidAdditionalInstructions { get; set; }
    }
    public class RequestForQuotationTemplateVM
    {
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "ABC")]
        public decimal ABC { get; set; }

        [Display(Name = "Mode of Procurement")]
        public string ModeOfProcurement { get; set; }

        [Display(Name = "Solicitation No.")]
        public string SolicitationNo { get; set; }

        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Display(Name = "Deadline")]
        public DateTime Deadline { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Quotation No.")]
        public string QuotationNo { get; set; }

        public List<RequestForQuotationDetailsVM> Details { get; set; }
    }
    public class NoticeOfAwardTemplateVM
    {
        [Display(Name = "Contractor/Supplier")]
        public string Supplier { get; set; }

        [Display(Name = "Business Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Display(Name = "Designation")]
        public string ContactPersonDesignation { get; set; }

        [Display(Name = "Authorized Agent/Representative")]
        public string AuthorizedAgent { get; set; }

        [Display(Name = "Designation")]
        public string AuthorizedDesignation { get; set; }

        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Required]
        [Display(Name = "Contract Price")]
        public decimal ContractPrice { get; set; }

        [Required]
        [Display(Name = "Contract Price")]
        public string ContractPriceWords { get; set; }

        [Required]
        [Display(Name = "Mode of Procurement")]
        public string ModeOfProcurement { get; set; }
    }
    public class NoticeToProceedTemplateVM
    {
        [Display(Name = "Contractor/Supplier")]
        public string Supplier { get; set; }

        [Display(Name = "Business Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Display(Name = "Designation")]
        public string ContactPersonDesignation { get; set; }

        [Display(Name = "Authorized Agent/Representative")]
        public string AuthorizedAgent { get; set; }

        [Display(Name = "Designation")]
        public string AuthorizedDesignation { get; set; }

        [Display(Name = "Purchase Order No")]
        public string PurchaseOrderNo { get; set; }

        [Display(Name = "Purchase Order Date")]
        public DateTime PurchaseOrderDate { get; set; }

        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Required]
        [MaxLength(255)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Required]
        [Display(Name = "Contract Price")]
        public decimal ContractPrice { get; set; }

        [Required]
        [Display(Name = "Contract Price")]
        public string ContractPriceWords { get; set; }

        [Required]
        [Display(Name = "Mode of Procurement")]
        public string ModeOfProcurement { get; set; }

        [Display(Name = "Delivery")]
        public int Delivery { get; set; }

        [Display(Name = "Delivery")]
        public string DeliveryWords { get; set; }
    }
    public class PurchaseOrderTemplateVM
    {
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Display(Name = "Purchase Order No.")]
        public string PurchaseOrderNumber { get; set; }

        [Display(Name = "Place of Delivery")]
        public string PlaceOfDelivery { get; set; }

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }

        [Display(Name = "Supplier Address")]
        public string SupplierAddress { get; set; }

        [Display(Name = "Supplier TIN")]
        public string SupplierTIN { get; set; }

        [Display(Name = "Supplier Representative")]
        public string SupplierRepresentative { get; set; }

        [Display(Name = "Supplier Representative Designation")]
        public string SupplierRepresentativeDesignation { get; set; }

        [Display(Name = "Mode of Procurement")]
        public string ModeOfProcurement { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Delivery Period (Calendar Days)")]
        public int DeliveryPeriod { get; set; }

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

        public List<PurchaseOrderDetailsVM> Details { get; set; }
    }
    public class PurchaseOrderDetailsVM
    {
        [Display(Name = "Article")]
        public int? ArticleReference { get; set; }

        [MaxLength(2)]
        public string ItemSequence { get; set; }

        [Display(Name = "Item Full Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ItemFullName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Unit of Reference")]
        public string UnitOfMeasure { get; set; }

        public int UOMReference { get; set; }

        public int Quantity { get; set; }

        public decimal UnitCost { get; set; }

        public decimal TotalCost { get; set; }
    }
}