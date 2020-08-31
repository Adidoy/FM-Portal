using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PP_PROCUREMENT_TIMELINE_BIDDING")]
    public class ProcurementTimeline
    {
        [Key]
        public int ID { get; set; }

        public string PAPCode { get; set; }

        [Display(Name = "Purchase Request Submission")]
        public DateTime? PurchaseRequestSubmission { get; set; }

        [Display(Name = "Purchase Request Submission Closing")]
        public DateTime? PurchaseRequestClosing { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? PreProcurementConference { get; set; }

        [Display(Name = "Actual Date")]
        public DateTime? ActualPreProcurementConference { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string PreProcurementConferenceRemarks { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? PostingOfIB { get; set; }

        [Display(Name = "Actual Date")]
        public DateTime? ActualPostingOfIB { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string PostingOfIBRemarks { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? PreBidConference { get; set; }

        [Display(Name = "Actual Date")]
        public DateTime? ActualPreBidConference { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string PreBidConferenceRemarks { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? OpeningOfBids { get; set; }

        [Display(Name = "Preliminary Examination of Bids")]
        public DateTime? PrelimenryExamination { get; set; }

        [Display(Name = "Detailed Examination of Bids")]
        public DateTime? DetailedExamination { get; set; }

        [Display(Name = "Reporting of Bid Evaluation to BAC")]
        public DateTime? EvaluationReporting { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string BidsExaminationRemarks { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? PostQualification { get; set; }

        [Display(Name = "Actual Date")]
        public DateTime? ActualPostQualification { get; set; }

        [Display(Name = "Date of Reporting to BAC")]
        public DateTime? PostQualificationReportedToBAC { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string PostQualificationRemarks { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? NOAIssuance { get; set; }

        [Display(Name = "Date BAC Resolution Generated")]
        public DateTime? BACResolutionCreated { get; set; }

        [Display(Name = "Date Forwarded to BAC Member")]
        public DateTime? BACMemberForwarded { get; set; }

        [Display(Name = "Date Forwarded to HOPE")]
        public DateTime? HOPEForwarded { get; set; }

        [Display(Name = "Date Received by PMO")]
        public DateTime? PMOReceived { get; set; }

        [Display(Name = "Date Notice of Award Generated")]
        public DateTime? ActualNOAIssuance { get; set; }

        [Display(Name = "Date Signed by HOPE")]
        public DateTime? NOASignedByHOPE { get; set; }

        [Display(Name = "Date Received by Supplier")]
        public DateTime? NOAReceivedBySupplier { get; set; }

        [Display(Name = "Remarks")]
        public string NOAIssuanceRemarks { get; set; }
    }
    [Table("PP_PURCHASE_REQUEST_HEADER")]
    public class PurchaseRequestHeader
    {
        [Key]
        public int ID { get; set; }

        public int FiscalYear { get; set; }

        public string PAPCode { get; set; }

        [Display(Name = "PR Number")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(12, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string PRNumber { get; set; }

        [Display(Name = "Office")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string Department { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Fund Cluster")]
        [MaxLength(30, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string FundCluster { get; set; }

        [Display(Name = "Purpose")]
        [Column(TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string Purpose { get; set; }

        [Display(Name = "Requested By")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string RequestedBy { get; set; }

        [Display(Name = "Designation")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string RequestedByDesignation { get; set; }

        [Display(Name = "Office")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string RequestedByDepartment { get; set; }

        [Display(Name = "Approved By")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string ApprovedBy { get; set; }

        [Display(Name = "Designation")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string ApprovedByDesignation { get; set; }

        [Display(Name = "Office")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string ApprovedByDepartment { get; set; }

        [Display(Name = "Created By")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(30, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Received")]
        public DateTime? ReceivedAt { get; set; }

        [Display(Name = "Received By")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(30, ErrorMessage = "{0} is up to {1} characters only.")]
        public string ReceivedBy { get; set; }
    }
    [Table("PP_PURCHASE_REQUEST_DETAILS")]
    public class PurchaseRequestDetails
    {
        [Key]
        public int ID { get; set; }
        public int PRHeaderReference { get; set; }
        public int ItemReference { get; set; }
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }
        public int? UnitReference { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        [ForeignKey("ItemReference")]
        public virtual Item FKItemReference { get; set; }
        [ForeignKey("UnitReference")]
        public virtual UnitOfMeasure FKUnitReference { get; set; }
        [ForeignKey("PRHeaderReference")]
        public virtual PurchaseRequestHeader FKPRHeaderReference { get; set; }

    }

    [Table("PP_PURCHASE_ORDER_HEADER")]
    public class PurchaseOrderHeader
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Purchase Order No.")]
        public string PurchaseOrderNumber { get; set; }

        [Required]
        [Display(Name = "Supplier")]
        public int SupplierReference { get; set; }

        [Required]
        [Display(Name = "Place of Delivery")]
        public string PlaceOfDelivery { get; set; }

        [Display(Name = "Date of Delivery")]
        public int? DateOfDelivery { get; set; }

        [Required]
        [Display(Name = "Mode of Procurement")]
        public int ModeOfProcurementReference { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Display(Name = "Chief Accountant")]
        public string ChiefAccountant { get; set; }

        [Required]
        [Display(Name = "Office")]
        public string ChiefAccountantOffice { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public string ChiefAccountantDesignation { get; set; }

        [Required]
        [Display(Name = "Authorized Signature")]
        public string AuthorizedSignature { get; set; }

        [Required]
        [Display(Name = "Office")]
        public string AuthorizedSignatureOffice { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public string AuthorizedSignatureDesignation { get; set; }

        [ForeignKey("SupplierReference")]
        public virtual Supplier FKSupplierReference { get; set; }

        [ForeignKey("ModeOfProcurementReference")]
        public virtual ModeOfProcurement FKModeOfProcurementReference { get; set; }
    }

    [Table("PP_PURCHASE_ORDER_DETAILS")]
    public class PurchaseOrderDetails
    {
        [Key, Column(Order = 0)]
        public int ItemReference { get; set; }

        [Key, Column(Order = 1)]
        public int PurchaseOrderReference { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [ForeignKey("ItemReference")]
        public virtual Item FKItemReference { get; set; }

        [ForeignKey("PurchaseOrderReference")]
        public virtual PurchaseOrderHeader FKPurchaseOrderHeaderReference { get; set; }
    }

    public class PurchaseRequestDetailsVM
    {
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Item Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Unit of Measure")]
        public string UOM { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Total Cost")]
        public decimal TotalCost { get; set; }

        [Display(Name = "Reference")]
        public string References { get; set; }
    }
    public class PurchaseRequestVM
    {
        [Display(Name = "PR No.")]
        public string PRNumber { get; set; }

        [Display(Name = "Office")]
        public string Department { get; set; }

        [Display(Name = "Fund Cluster")]
        public string FundCluster { get; set; }

        [Display(Name = "Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string Purpose { get; set; }

        [Display(Name = "Requested By")]
        public string RequestedBy { get; set; }

        [Display(Name = "Designation")]
        public string RequestedByDesignation { get; set; }

        [Display(Name = "Office")]
        public string RequestedByDepartment { get; set; }

        [Display(Name = "Approved By")]
        public string ApprovedBy { get; set; }

        [Display(Name = "Designation")]
        public string ApprovedByDesignation { get; set; }

        [Display(Name = "Office")]
        public string ApprovedByDepartment { get; set; }

        [Display(Name = "Date Created")]
        public string CreatedAt { get; set; }

        public List<PurchaseRequestDetailsVM> PRDetails { get; set; }
    }
    public class PurchaseRequestCSEVM
    {
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "Period")]
        public string Period { get; set; }

        public List<PurchaseRequestDetailsVM> CSEItems { get; set; }
    }
    public class PurchaseRequestDashboard
    {
        public List<int> FiscalYears { get; set; }
        public List<ProcurementProjectsVM> PRItemsOpen { get; set; }
    }


    public class ProcurementProjectScheduleVM
    {
        public int APPReference { get; set; }

        [Display(Name = "Purchase Request Submission")]
        public string PurchaseRequestSubmission { get; set; }

        [Display(Name = "Pre-Procurement Conference")]
        public string PreProcurementConference { get; set; }

        [Display(Name = "Advertisement / Posting of Invitation to Bid")]
        public string PostingOfIB { get; set; }

        [Display(Name = "Pre-Bid Conference")]
        public string PreBidConference { get; set; }

        [Display(Name = "Deadline of Submission of Bids")]
        public string SubmissionOfBids { get; set; }

        [Display(Name = "Evaluation of Bids")]
        public string BidEvaluation { get; set; }

        [Display(Name = "Post-Qualification of Bids")]
        public string PostQualification { get; set; }

        [Display(Name = "Approval of Resolution / Issuance of Notice of Award")]
        public string NOAIssuance { get; set; }

        [Display(Name = "Contract Preparation and Signing")]
        public string ContractSigning { get; set; }

        [Display(Name = "Approval by Higher Authority")]
        public string Approval { get; set; }

        [Display(Name = "Issuance of Notice to Proceed")]
        public string NTPIssuance { get; set; }

        [Display(Name = "Receiving of Purchase Order - Supplier")]
        public string POReceived { get; set; }
    }
    public class ProcurementProjectActualAccomplishmentVM
    {
        public int APPReference { get; set; }

        [Display(Name = "Purchase Request Submission")]
        public DateTime? ActualPurchseRequestSubmission { get; set; }

        [AllowHtml]
        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public DateTime? PRRemarks { get; set; }

        [Display(Name = "P/R Closing")]
        public string ActualPRClosing { get; set; }

        [Display(Name = "Pre-Procurement Conference")]
        public DateTime? ActualPreProcurementConference { get; set; }

        [Display(Name = "Advertisement")]
        public DateTime? ActualPostingOfIB { get; set; }

        [Display(Name = "Pre-Bid Conference")]
        public DateTime? ActualPreBidConference { get; set; }

        [Display(Name = "Submission of Bids")]
        public DateTime? ActualSubmissionOfBids { get; set; }

        [Display(Name = "Evaluation of Bids")]
        public DateTime? ActualBidEvaluation { get; set; }

        [Display(Name = "Post-Qualification of Bids")]
        public DateTime? AcutalPostQualification { get; set; }

        [Display(Name = "Issuance of Notice of Award")]
        public DateTime? ActualNOAIssuance { get; set; }

        [Display(Name = "Contract Preparation and Signing")]
        public DateTime? ActualContractSigning { get; set; }

        [Display(Name = "Approval by Higher Authority")]
        public DateTime? ActualApproval { get; set; }

        [Display(Name = "Issuance of Notice to Proceed")]
        public DateTime? ActualNTPIssuance { get; set; }
    }
    public class ProcurementProjectItemsVM
    {
        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "PPMP Reference")]
        public string PPMPReference { get; set; }

        [Display(Name = "Delivery Month")]
        public string DeliveryMonth { get; set; }

        [Display(Name = "End-User")]
        public string EndUser { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public string ProcurementSource { get; set; }

        [Display(Name = "Inventory Type")]
        public string InventoryType { get; set; }

        [Display(Name = "Category")]
        public string ItemCategory { get; set; }

        [Display(Name = "Packaging UOM")]
        public string PackagingUOMReference { get; set; }

        [Display(Name = "Individual UOM")]
        public string IndividualUOMReference { get; set; }

        [Display(Name = "Purpose")]
        public string Purpose { get; set; }

        [Display(Name = "Procurement Status")]
        public string Status { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [Display(Name = "Purchase Request No.")]
        public string PRNumber { get; set; }

        [Display(Name = "Date Received")]
        public DateTime? DatePRReceived { get; set; }
    }
    public class ProcurementProjectsVM
    {
        public string APPReference { get; set; }

        public string ProcurmentProjectType { get; set; }

        public bool IsTangible { get; set; }

        public int Month { get; set; }

        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Display(Name = "Procurement Program")]
        public string ProcurementProgram { get; set; }

        [Display(Name = "Approved Budget")]
        public decimal ApprovedBudget { get; set; }

        [Display(Name = "Account Name")]
        public string ObjectClassification { get; set; }

        [Display(Name = "Fund Cluster")]
        public string FundCluster { get; set; }

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Display(Name = "End-User")]
        public string EndUser { get; set; }

        [Display(Name = "Start Month")]
        public string StartMonth { get; set; }

        [Display(Name = "End Month")]
        public string EndMonth { get; set; }

        [Display(Name = "Project Status")]
        public string ProjectStatus { get; set; }

        [Display(Name = "Project Coordinator")]
        public string ProjectCoordinator { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "APP Mode of Procurement")]
        public string APPModeOfProcurement { get; set; }

        [Display(Name = "Mode of Procurement")]
        public int? ModeOfProcurement { get; set; }

        [Display(Name = "Project Support")]
        public string ProjectSupport { get; set; }

        [Display(Name = "MOOE Total")]
        public decimal MOOETotal { get; set; }

        [Display(Name = "Capital Outlay Total")]
        public decimal CapitalOutlayTotal { get; set; }

        [Display(Name = "Total Estimated Budget")]
        public decimal TotalEstimatedBudget { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Purchase Request Submission")]
        public DateTime? PurchaseRequestSubmission { get; set; }

        [Display(Name = "Purchase Request Submission Closing")]
        public DateTime? PurchaseRequestClosing { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? PreProcurementConference { get; set; }

        [Display(Name = "Date Conducted")]
        public DateTime? ActualPreProcurementConference { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string PreProcurementConferenceRemarks { get; set; }

        [Display(Name = "Date of Posting")]
        public DateTime? PostingOfIB { get; set; }

        [Display(Name = "Date of Posting")]
        public DateTime? ActualPostingOfIB { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string PostingOfIBRemarks { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? PreBidConference { get; set; }

        [Display(Name = "Date Conducted")]
        public DateTime? ActualPreBidConference { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string PreBidConferenceRemarks { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? OpeningOfBids { get; set; }

        [Display(Name = "Preliminary Examination of Bids")]
        public DateTime? PrelimenryExamination { get; set; }

        [Display(Name = "Detailed Examination of Bids")]
        public DateTime? DetailedExamination { get; set; }

        [Display(Name = "Reporting of Evaluation to BAC")]
        public DateTime? EvaluationReporting { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string BidsExaminationRemarks { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? PostQualification { get; set; }

        [Display(Name = "Date Conducted")]
        public DateTime? ActualPostQualification { get; set; }

        [Display(Name = "Date of Reporting to BAC")]
        public DateTime? PostQualificationReportedToBAC { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string PostQualificationRemarks { get; set; }

        [Display(Name = "Target Date")]
        public DateTime? NOAIssuance { get; set; }

        [Display(Name = "Date BAC Resolution Generated")]
        public DateTime? BACResolutionCreated { get; set; }

        [Display(Name = "Date Forwarded to BAC Member")]
        public DateTime? BACMemberForwarded { get; set; }

        [Display(Name = "Date Forwarded to HOPE")]
        public DateTime? HOPEForwarded { get; set; }

        [Display(Name = "Date Received by PMO")]
        public DateTime? PMOReceived { get; set; }

        [Display(Name = "Date Notice of Award Generated")]
        public DateTime? ActualNOAIssuance { get; set; }

        [Display(Name = "Date Signed by HOPE")]
        public DateTime? NOASignedByHOPE { get; set; }

        [Display(Name = "Date Received by Supplier")]
        public DateTime? NOAReceivedBySupplier { get; set; }

        [Display(Name = "Remarks")]
        [DataType(DataType.MultilineText)]
        public string NOAIssuanceRemarks { get; set; }

        [Display(Name = "Supplier")]
        public int? Supplier { get; set; }

        [Display(Name = "Project Cost")]
        public decimal ProjectCost { get; set; }

        [Display(Name = "Purchase Order No.")]
        public string PONumber { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? POCreatedAt { get; set; }

        [Display(Name = "Place of Delivery")]
        public string PlaceOfDelivery { get; set; }

        [Display(Name = "Date of Delivery")]
        public int? DateOfDelivery { get; set; }

        public List<ProcurementProjectItemsVM> Items { get; set; }
    }
    public class ProcurementProgramsVM
    {
        public int Month { get; set; }

        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Display(Name = "Procurement Program")]
        public string ProcurementProgram { get; set; }

        [Display(Name = "Approved Budget")]
        public decimal ApprovedBudget { get; set; }

        [Display(Name = "Account Name")]
        public string ObjectClassification { get; set; }

        [Display(Name = "Fund Cluster")]
        public string FundCluster { get; set; }

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Display(Name = "Start Month")]
        public string StartMonth { get; set; }

        [Display(Name = "End Month")]
        public string EndMonth { get; set; }

        [Display(Name = "Project Coordinator")]
        public string ProjectCoordinator { get; set; }

        [Display(Name = "Mode of Procurement")]
        public string ModeOfProcurement { get; set; }

        [Display(Name = "Project Support")]
        public string ProjectSupport { get; set; }

        [Display(Name = "Project Cost")]
        public decimal ProjectCost { get; set; }

        public bool HasSchedule { get; set; }
    }

    public class ProcurementPipelineDashboardVM
    {
        public int TotalProcurementItems { get; set; }
        public int TotalItemsApproved { get; set; }
        public int TotalItemsPostedInAPP { get; set; }
        public int TotalItemsWithPurchaseRequest { get; set; }

        public List<ProcurementProjectItemsVM> ItemStatus { get; set; }
        public List<ProcurementProjectItemsVM> PPMPItems { get; set; }
        public List<ProcurementProjectItemsVM> APPItems { get; set; }
        public List<ProcurementProjectItemsVM> PurchaseRequestItems { get; set; }
        public List<ProcurementProjectsVM> ProcurementPrograms { get; set; }
    }

    public class ProcurementTimelineValidation : AbstractValidator<ProcurementProjectScheduleVM>
    {
        public ProcurementTimelineValidation()
        {
            RuleFor(x => new { x.PreProcurementConference, x.PurchaseRequestSubmission })
                .Must(x => ConvertToDateTime(x.PreProcurementConference, x.PurchaseRequestSubmission))
                .WithMessage("Date for Pre-Procurement Conference must not be less than or equal to the Purchase Request Submission");
            RuleFor(x => new { x.PostingOfIB, x.PreProcurementConference })
                .Must(x => ConvertToDateTime(x.PostingOfIB, x.PreProcurementConference))
                .WithMessage("Date for Posting of IB must not be less than or equal to the Pre-Procurement Conference");
            RuleFor(x => new { x.PreBidConference, x.PostingOfIB })
                .Must(x => ConvertToDateTime(x.PreBidConference, x.PostingOfIB))
                .WithMessage("Date for Pre-Bid Conference must not be less than or equal to Posting of IB");
            RuleFor(x => new { x.SubmissionOfBids, x.PreBidConference })
                .Must(x => ConvertToDateTime(x.SubmissionOfBids, x.PreBidConference))
                .WithMessage("Date for Deadline for Submission of Bids must not be less than or equal to Pre-Bid Conference");
            RuleFor(x => new { x.BidEvaluation, x.SubmissionOfBids })
                .Must(x => ConvertToDateTime(x.BidEvaluation, x.SubmissionOfBids))
                .WithMessage("Date for Bids Evaluation must not be less than or equal to Deadline for Submission of Bids");
            RuleFor(x => new { x.PostQualification, x.BidEvaluation })
                .Must(x => ConvertToDateTime(x.PostQualification, x.BidEvaluation))
                .WithMessage("Date for Post Quaification must not be less than or equal to Bids Evaluation");
            RuleFor(x => new { x.NOAIssuance, x.PostQualification })
                .Must(x => ConvertToDateTime(x.NOAIssuance, x.PostQualification))
                .WithMessage("Date for Issuance of Notice to Award must not be less than or equal to Post Qualification");
            RuleFor(x => new { x.ContractSigning, x.NOAIssuance })
                .Must(x => ConvertToDateTime(x.ContractSigning, x.NOAIssuance))
                .WithMessage("Date for Contract Preparation and Signing must not be less than or equal to NOA Issuance");
            RuleFor(x => new { x.Approval, x.ContractSigning })
                .Must(x => ConvertToDateTime(x.Approval, x.ContractSigning))
                .WithMessage("Date for Approval of Higher Authority must not be less than or equal to Contract Preparation and Signing");
            RuleFor(x => new { x.NTPIssuance, x.Approval })
                .Must(x => ConvertToDateTime(x.NTPIssuance, x.Approval))
                .WithMessage("Date for Issuance of Notice to Proceed must not be less than or equal to Approval of Higher Authority");
            RuleFor(x => new { x.POReceived, x.NTPIssuance })
                .Must(x => ConvertToDateTime(x.POReceived, x.NTPIssuance))
                .WithMessage("Date for Receiving of Purchase Order must not be less than or equal to NTP Issuance");
        }

        private bool ConvertToDateTime(string DateA, string DateB)
        {
            var dateA = Convert.ToDateTime(DateA);
            var dateB = Convert.ToDateTime(DateB);
            return dateA <= dateB ? false : true;
        }
    }
}