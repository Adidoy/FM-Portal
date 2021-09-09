using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROC_TRXN_Project_Plan")]
    public class ProjectPlans
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "PAP Code")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(40, ErrorMessage = "{0} is up to {1} characters only.")]
        public string PAPCode { get; set; }

        [Display(Name = "Project Type")]
        public ProjectTypes ProjectType { get; set; }

        [Display(Name = "Parent Project")]
        public int? ParentProject { get; set; }

        [Display(Name = "Project Code")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(40, ErrorMessage = "{0} is up to {1} characters only.")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        [Column(TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        public string ProjectName { get; set; }

        [Display(Name = "Description")]
        [Column(TypeName = "VARCHAR")]
        [DataType(DataType.MultilineText)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        public string Description { get; set; }

        [Display(Name = "Fiscal Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public int FiscalYear { get; set; }

        [MaxLength(20)]
        [Display(Name = "Sector")]
        [Column(TypeName = "VARCHAR")]
        public string Sector { get; set; }

        [MaxLength(20)]
        [Display(Name = "Department")]
        [Column(TypeName = "VARCHAR")]
        public string Department { get; set; }

        [MaxLength(20)]
        [Display(Name = "Unit")]
        [Column(TypeName = "VARCHAR")]
        public string Unit { get; set; }

        [Display(Name = "Prepared By")]
        public string PreparedBy { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }

        [Display(Name = "Project Status")]
        public ProjectStatus ProjectStatus { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Delivery Month")]
        public int DeliveryMonth { get; set; }

        [Display(Name = "Total Estimated Budget")]
        public decimal? TotalEstimatedBudget { get; set; }

        [Required]
        [Display(Name = "Is Deleted?")]
        public bool PurgeFlag { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("ParentProject")]
        public virtual ProjectPlans FKParentProject { get; set; }
    }
    [Table("PROC_TRXN_Project_Plan_Details")]
    public class ProjectDetails
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Project")]
        public int ProjectReference { get; set; }

        [Required]
        public int ClassificationReference { get; set; }

        [Display(Name = "UACS")]
        public string UACS { get; set; }

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

        public BudgetProposalType ProposalType { get; set; }

        public ProcurementSources ProcurementSource { get; set; }

        [Required]
        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        [Display(Name = "Category")]
        public int? CategoryReference { get; set; }

        [AllowHtml]
        [Display(Name = "Justification")]
        public string Justification { get; set; }

        [Required]
        [Display(Name = "Project Item Status")]
        public ProjectDetailsStatus ProjectItemStatus { get; set; }

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
        [Display(Name = "Total Qty")]
        public int TotalQty { get; set; }

        [Display(Name = "Supplier1")]
        public int? Supplier1Reference { get; set; }

        [Display(Name = "Supplier2")]
        public int? Supplier2Reference { get; set; }

        [Display(Name = "Supplier3")]
        public int? Supplier3Reference { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? UnitCost { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier1UnitCost { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier2UnitCost { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier3UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? EstimatedBudget { get; set; }

        public bool UpdateFlag { get; set; }

        [Display(Name = "Responsibility Center Action")]
        public ResponsibilityCenterAction? ResponsibilityCenterAction { get; set; }

        [Display(Name = "Reason for Revision")]
        public ResponsibilityCenterReasonForRevision? ReasonForNonAcceptance { get; set; }

        [ForeignKey("ProjectReference")]
        public virtual ProjectPlans FKProjectPlanReference { get; set; }

        [Display(Name = "Classification")]
        [ForeignKey("ClassificationReference")]
        public virtual ItemClassification FKClassificationReference { get; set; }

        [Display(Name = "Article")]
        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKItemArticleReference { get; set; }

        [ForeignKey("UOMReference")]
        [Display(Name = "Unit of Measure")]
        public virtual UnitOfMeasure FKUOMReference { get; set; }

        [ForeignKey("CategoryReference")]
        [Display(Name = "Category")]
        public virtual ItemCategory FKCategoryReference { get; set; }

        [ForeignKey("Supplier1Reference")]
        public virtual Supplier FKSupplier1Reference { get; set; }

        [ForeignKey("Supplier2Reference")]
        public virtual Supplier FKSupplier2Reference { get; set; }

        [ForeignKey("Supplier3Reference")]
        public virtual Supplier FKSupplier3Reference { get; set; }
    }

    public class ProjectPlanVM
    {
        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "Program")]
        public string Program { get; set; }

        [Display(Name = "Project Type")]
        public ProjectTypes ProjectType { get; set; }

        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "Sector Code")]
        public string SectorCode { get; set; }

        [Display(Name = "Sector")]
        public string Sector { get; set; }

        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Unit Code")]
        public string UnitCode { get; set; }

        [Display(Name = "Unit")]
        public string Unit { get; set; }

        [Display(Name = "Prepared By")]
        public string PreparedBy { get; set; }

        public string PreparedByEmpCode { get; set; }

        [Display(Name = "Designation")]
        public string PreparedByDesignation { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }

        [Display(Name = "Designation")]
        public string SubmittedByDesignation { get; set; }

        [Display(Name = "Project Status")]
        public string ProjectStatus { get; set; }

        [Display(Name = "Delivery Month")]
        public string DeliveryMonth { get; set; }

        [Display(Name = "Total Estimated Budget")]
        public decimal? TotalEstimatedBudget { get; set; }

        public List<BasketItems> ProjectPlanItems { get; set; }
    }
    public class ProjectPlanListVM
    {
        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "Program")]
        public string Program { get; set; }

        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Unit")]
        public string Office { get; set; }

        [Display(Name = "Project Status")]
        public string ProjectStatus { get; set; }

        [Display(Name = "Project Type")]
        public ProjectTypes ProjectType { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }
    }

    public enum ProjectTypes
    {
        [Display(Name = "Common-office Supplies Project Plan", ShortName = "CSPR")]
        CommonSuppliesProjectPlan = 0,

        [Display(Name = "End-User Project Plan", ShortName = "EUPR")]
        EndUserProjectPlan = 1,

        [Display(Name = "Infrastructure Project Plan", ShortName = "INPR")]
        InfrastructureProjectPlan = 2,

        [Display(Name = "Repair and Maintenance Project Plan", ShortName = "RMPR")]
        RepairAndMaintenanceProjectPlan = 3
    }
    public enum ProjectStatus
    {
        [Display(Name = "New Project")]
        NewProject = 0,

        [Display(Name = "Forwarded to Responsibility Center")]
        ForwardedToResponsibilityCenter = 1,

        [Display(Name = "Returned for Revision")]
        ReturnedForRevision = 2,

        [Display(Name = "Evaluated by Responsiblity Center")]
        EvaluatedByResponsibilityCenter = 3,

        [Display(Name = "Posted to PPMP")]
        PostedToPPMP = 4,

        [Display(Name = "Forwarded to Budget Office")]
        ForwardedToBudgetOffice = 5,

        [Display(Name = "Evaluated by Budget Office")]
        EvaluatedByBudgetOffice = 6,

        [Display(Name = "Posted to APP")]
        PostedToAPP = 7
    }
    public enum ProjectDetailsStatus
    {
        [Display(Name = "Posted to Project")]
        PostedToProject = 0,

        [Display(Name = "For Evaluation")]
        ForEvaluation = 1,

        [Display(Name = "For Revision")]
        ForRevision = 2,

        [Display(Name = "Item Revised")]
        ItemRevised = 3,

        [Display(Name = "Item Accepted")]
        ItemAccepted = 4,

        [Display(Name = "Item Not Accepted")]
        ItemNotAccepted = 5,

        [Display(Name = "Posted to PPMP")]
        PostedToPPMP = 6,

        [Display(Name = "For Approval - Budget Office")]
        ForApproval = 7,

        [Display(Name = "For Revision - Budget Office")]
        ForRevisionFromBudget = 8,

        [Display(Name = "Approved")]
        Approved = 9,

        [Display(Name = "Disapproved")]
        Disapproved = 10,

        [Display(Name = "Posted to APP")]
        PostedToAPP = 11,

        [Display(Name = "Posted to Contract Project")]
        PostedToProcurementProject = 12,

        [Display(Name = "Posted to Purchase Request")]
        PostedToPurchaseRequest = 13,

        [Display(Name = "Posted to Purchase Order / Agency Procurement Request / Contract")]
        PostedToPurchaseOrder = 14,

        [Display(Name = "Item Delivered")]
        ItemDelivered = 15
    }
    public enum ResponsibilityCenterAction
    {
        [Display(Name = "Accepted")]
        Accepted = 0,

        [Display(Name = "Needs Revision")]
        ForRevision = 1,

        [Display(Name = "Remove From Project")]
        RemoveFromProject = 2
    }
    public enum ResponsibilityCenterReasonForRevision
    {
        [Display(Name = "Unnecessary for the Project")]
        UnnecessaryForProject,

        [Display(Name = "Excessive / Unreasonable Quantity")]
        ExcessiveQuantity,

        [Display(Name = "Unacceptable Justification")]
        UnacceptableJustification,

        [Display(Name = "Not Aligned with Program")]
        NotAlignedWithProgram
    }
    public enum ProcurementSources
    {
        [Display(Name = "Agency-to-Agency")]
        AgencyToAgency = 0,
        [Display(Name = "External Suppliers")]
        ExternalSuppliers = 1,
    }
    public enum InfrastructurePlanRequestStatus
    {
        [Display(Name = "Request Created")]
        RequestCreated = 0,

        [Display(Name = "Forwarded to Implementing Unit")]
        ForwardedToImplementingUnit = 1,

        [Display(Name = "Request Accepted for Evaluation")]
        AcceptedForEvaluation = 2,

        [Display(Name = "Request Not Accepted")]
        NotAccepted = 3,

        [Display(Name = "Posted to Project")]
        PostedToProject = 4,

        [Display(Name = "Posted to PPMP")]
        PostedToPPMP = 5
    }
}