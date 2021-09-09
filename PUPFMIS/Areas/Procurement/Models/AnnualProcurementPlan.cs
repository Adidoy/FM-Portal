﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROC_TRXN_Annual_Procurement_Plan")]
    public class AnnualProcurementPlan
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "APP Type")]
        public APPTypes APPType { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Reference No.")]
        [MaxLength(30, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ReferenceNo { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Prepared By")]
        [MaxLength(50, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string PreparedBy { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(50, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string PreparedByDepartmentCode { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string PreparedByDesignation { get; set; }

        [Display(Name = "Date Signed")]
        public DateTime? PreparedAt { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Recommending Approval")]
        [MaxLength(50, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string RecommendingApproval { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(50, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string RecommendingApprovalActionBy { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(50, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string RecommendingApprovalDepartmentCode { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string RecommendingApprovalDesignation { get; set; }

        [Display(Name = "Date Signed")]
        public DateTime? RecommendedAt { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Approved By")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string ApprovedBy { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(50, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string ApprovalActionBy { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string ApprovedByDepartmentCode { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string ApprovedByDesignation { get; set; }

        [Display(Name = "Date Signed")]
        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [MaxLength(30)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
    }
    [Table("PROC_TRXN_Annual_Procurement_Plan_Details")]
    public class AnnualProcurementPlanDetails
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(30, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string PAPCode { get; set; }

        public int APPHeaderReference { get; set; }

        public ProcurementSources ProcurementSource { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(175, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ProcurementProgram { get; set; }

        [Required]
        [Display(Name = "Mode of Procurement")]
        public string APPModeOfProcurementReference { get; set; }

        [Display(Name = "Classification")]
        public int? ClassificationReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string ObjectClassification { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string ObjectSubClassification { get; set; }

        [Required]
        [Display(Name = "Inventory Code")]
        public string InventoryCode { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string EndUser { get; set; }

        public int Month { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string StartMonth { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string EndMonth { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Source of Funds")]
        public string FundSourceReference { get; set; }

        [Display(Name = "MOOE")]
        public decimal MOOEAmount { get; set; }

        [Display(Name = "CO")]
        public decimal COAmount { get; set; }

        [Display(Name = "Total")]
        public decimal Total { get; set; }

        public string Remarks { get; set; }
        
        [Display(Name = "Project Cost")]
        public decimal ProjectCost { get; set; }
        
        [Display(Name = "Project Coordinator")]
        public string ProjectCoordinator { get; set; }

        [Display(Name = "Project Support")]
        public string ProjectSupport { get; set; }

        //[Display(Name = "Status")]
        //public APPStatus ProjectStatus { get; set; }

        public bool IsInstitutional { get; set; }

        public bool IsTangible { get; set; }

        [ForeignKey("APPHeaderReference")]
        public virtual AnnualProcurementPlan FKAPPHeaderReference { get; set; }

        [ForeignKey("ClassificationReference")]
        public virtual ItemClassification FKClassificationReference { get; set; }
    }
    [Table("PROC_TRXN_Annual_Procurement_Plan_CSE")]
    public class APPCSEDetails
    {
        [Key]
        public int ID { get; set; }

        public int APPHeaderReference { get; set; }

        [Required]
        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Required]
        public int ProjectDetailsID { get; set; }

        public int PPMPHeaderReference { get; set; }

        [Required]
        [Display(Name = "Article")]
        public int ArticleReference { get; set; }

        [Required]
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

        public ProcurementSources ProcurementSource { get; set; }

        [Required]
        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int? CategoryReference { get; set; }

        [Display(Name = "JAN")]
        public int JanQty { get; set; }

        [Display(Name = "FEB")]
        public int FebQty { get; set; }

        [Display(Name = "MAR")]
        public int MarQty { get; set; }

        [Required]
        [Display(Name = "Q1 Total")]
        public int Q1TotalQty { get; set; }

        [Display(Name = "APR")]
        public int AprQty { get; set; }

        [Display(Name = "MAY")]
        public int MayQty { get; set; }

        [Display(Name = "JUN")]
        public int JunQty { get; set; }

        [Required]
        [Display(Name = "Q2 Total")]
        public int Q2TotalQty { get; set; }

        [Display(Name = "JUL")]
        public int JulQty { get; set; }

        [Display(Name = "AUG")]
        public int AugQty { get; set; }

        [Display(Name = "SEP")]
        public int SepQty { get; set; }

        [Required]
        [Display(Name = "Q3 Total")]
        public int Q3TotalQty { get; set; }

        [Display(Name = "OCT")]
        public int OctQty { get; set; }

        [Display(Name = "NOV")]
        public int NovQty { get; set; }

        [Display(Name = "DEC")]
        public int DecQty { get; set; }

        [Required]
        [Display(Name = "Q4 Total")]
        public int Q4TotalQty { get; set; }

        [Display(Name = "Total Quantity")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Required]
        [Display(Name = "Approved Budget")]
        public decimal ApprovedBudget { get; set; }

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [ForeignKey("APPHeaderReference")]
        public virtual AnnualProcurementPlan FKAPPHeaderReference { get; set; }

        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKItemArticleReference { get; set; }

        [ForeignKey("CategoryReference")]
        public virtual ItemCategory FKItemCategoryReference { get; set; }

        [ForeignKey("UOMReference")]
        public virtual UnitOfMeasure FKUOMReference { get; set; }
    }

    public class AnnualProcurementPlanCSEItemsVM
    {
        [Required]
        [Display(Name = "Classification")]
        public int ClassificationReference { get; set; }

        [Required]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Required]
        [Display(Name = "Article")]
        public int ArticleReference { get; set; }

        [Required]
        [MaxLength(2)]
        public string ItemSequence { get; set; }

        [Display(Name = "Item Full Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ItemFullName { get; set; }

        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Category")]
        public int? CategoryReference { get; set; }

        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Display(Name = "Unit of Measure")]
        public int UnitOfMeasure { get; set; }

        [Display(Name = "Unit of Measure")]
        public string UOM { get; set; }

        [Display(Name = "JAN")]
        public int JanQty { get; set; }

        [Display(Name = "FEB")]
        public int FebQty { get; set; }

        [Display(Name = "MAR")]
        public int MarQty { get; set; }

        [Required]
        [Display(Name = "Q1 Total")]
        public int Q1TotalQty { get; set; }

        [Required]
        [Display(Name = "Q1 Total Amount")]
        public decimal Q1TotalAmount { get; set; }

        [Display(Name = "APR")]
        public int AprQty { get; set; }

        [Display(Name = "MAY")]
        public int MayQty { get; set; }

        [Display(Name = "JUN")]
        public int JunQty { get; set; }

        [Required]
        [Display(Name = "Q2 Total")]
        public int Q2TotalQty { get; set; }

        [Required]
        [Display(Name = "Q2 Total Amount")]
        public decimal Q2TotalAmount { get; set; }

        [Display(Name = "JUL")]
        public int JulQty { get; set; }

        [Display(Name = "AUG")]
        public int AugQty { get; set; }

        [Display(Name = "SEP")]
        public int SepQty { get; set; }

        [Required]
        [Display(Name = "Q3 Total")]
        public int Q3TotalQty { get; set; }

        [Required]
        [Display(Name = "Q3 Total Amount")]
        public decimal Q3TotalAmount { get; set; }

        [Display(Name = "OCT")]
        public int OctQty { get; set; }

        [Display(Name = "NOV")]
        public int NovQty { get; set; }

        [Display(Name = "DEC")]
        public int DecQty { get; set; }

        [Required]
        [Display(Name = "Q4 Total")]
        public int Q4TotalQty { get; set; }

        [Required]
        [Display(Name = "Q4 Total Amount")]
        public decimal Q4TotalAmount { get; set; }

        [Display(Name = "Total Quantity")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Required]
        [Display(Name = "Approved Budget")]
        public decimal ApprovedBudget { get; set; }

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }
    }
    public class AnnualProcurementPlanCSEVM
    {
        [Display(Name = "Fiscal Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public int FiscalYear { get; set; }

        [Display(Name = "APP Type")]
        public string APPType { get; set; }

        [Display(Name = "Reference No.")]
        [MaxLength(30, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string ReferenceNo { get; set; }

        [Display(Name = "Account Code")]
        [MaxLength(15, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string AccountCode { get; set; }

        [Display(Name = "Organization Type")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string OrganizationType { get; set; }

        [Display(Name = "Department/Bureau/Office")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string AgencyName { get; set; }

        [Display(Name = "Region")]
        [MaxLength(30, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string Region { get; set; }

        [Display(Name = "Address")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string Address { get; set; }

        [Display(Name = "Prepared By:")]
        public string PreparedBy { get; set; }

        [Display(Name = "Office")]
        public string PreparedByDepartment { get; set; }

        [Display(Name = "Designation")]
        public string PreparedByDesignation { get; set; }

        [Display(Name = "Certified Funds Available / Certified Appropriate Funds Available:")]
        public string CertifiedBy { get; set; }

        [Display(Name = "Office")]
        public string CertifiedByDepartment { get; set; }

        [Display(Name = "Designation")]
        public string CertifiedByDesignation { get; set; }

        [Display(Name = "Approved By:")]
        public string ApprovedBy { get; set; }

        [Display(Name = "Office")]
        public string ApprovedByDepartment { get; set; }

        [Display(Name = "Designation")]
        public string ApprovedByDesignation { get; set; }

        [Display(Name = "Procurement Officer")]
        public string ProcurementOfficer { get; set; }

        [Display(Name = "Office")]
        public string ProcurementDepartment { get; set; }

        [Display(Name = "Designation")]
        public string ProcurementOfficerDesignation { get; set; }

        [Display(Name = "Bids and Awards Committee Secretariat")]
        public string BACSecretariat { get; set; }

        [Display(Name = "Office")]
        public string BACOffice { get; set; }

        [Display(Name = "Designation")]
        public string BACSecretariatDesignation { get; set; }

        public List<AnnualProcurementPlanCSEItemsVM> APPDBMItems { get; set; }
        public List<AnnualProcurementPlanCSEItemsVM> APPNonDBMItems { get; set; }
    }
    public class APPDashboardVM
    {
        public List<int> CSEFiscalYears { get; set; }
        public List<int> APPCSEFiscalYears { get; set; }
        public List<int> APPFiscalYears { get; set; }
        public int PPMPsToBeReviewed { get; set; }
        public int PPMPsEvaluated { get; set; }
    }
    public class AnnualProcurementPlanDetailsVM
    {
        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Display(Name = "Object Classification")]
        public string ObjectClassification { get; set; }

        [Display(Name = "Code (PAP)")]
        public string PAPCode { get; set; }

        [Display(Name = "Procurement Program/Project")]
        public string ProcurementProject { get; set; }

        [MaxLength(20)]
        [Display(Name = "End-User")]
        public string EndUser { get; set; }

        [Display(Name = "Mode of Procurement")]
        public string ModeOfProcurement { get; set; }

        public int Month { get; set; }

        public string StartMonth { get; set; }

        public string EndMonth { get; set; }

        [Display(Name = "Source of Funds")]
        public string FundDescription { get; set; }

        public string FundCluster { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [Display(Name = "MOOE")]
        public decimal MOOE { get; set; }

        [Display(Name = "CO")]
        public decimal CapitalOutlay { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        public string ProjectReferences { get; set; }

        public string PPMPReferences { get; set; }
        
        public string APPItemType { get; set; }
    }
    public class AnnualProcurementPlanVM
    {
        [Display(Name = "Fiscal Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public int FiscalYear { get; set; }

        [Display(Name = "APP Type")]
        public string APPType { get; set; }

        [Display(Name = "Reference No.")]
        [MaxLength(30, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string ReferenceNo { get; set; }

        [Display(Name = "Account Code (DBM)")]
        [MaxLength(15, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string AccountCode { get; set; }

        [Display(Name = "Organization Type")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string OrganizationType { get; set; }

        [Display(Name = "Department/Bureau/Office")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string AgencyName { get; set; }

        [Display(Name = "Region")]
        [MaxLength(30, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string Region { get; set; }

        [Display(Name = "Address")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string Address { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Prepared By")]
        public string PreparedBy { get; set; }

        [Display(Name = "Date Prepared")]
        public DateTime? PreparedAt { get; set; }

        [Display(Name = "Office")]
        public string PreparedByDepartment { get; set; }

        [Display(Name = "Designation")]
        public string PreparedByDesignation { get; set; }

        [Display(Name = "Certified Funds Available / Certified Appropriate Funds Available:")]
        public string CertifiedBy { get; set; }

        [Display(Name = "Date Certified")]
        public DateTime? CertifiedAt { get; set; }

        [Display(Name = "Office")]
        public string CertifiedByDepartment { get; set; }

        [Display(Name = "Designation")]
        public string CertifiedByDesignation { get; set; }

        [Display(Name = "Approved By")]
        public string ApprovedBy { get; set; }

        [Display(Name = "Date Approved")]
        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "Office")]
        public string ApprovedByDepartment { get; set; }

        [Display(Name = "Designation")]
        public string ApprovedByDesignation { get; set; }

        [Display(Name = "Procurement Officer")]
        public string ProcurementOfficer { get; set; }

        [Display(Name = "Office")]
        public string ProcurementDepartment { get; set; }

        [Display(Name = "Designation")]
        public string ProcurementOfficerDesignation { get; set; }

        [Display(Name = "Bids and Awards Committee Secretariat")]
        public string BACSecretariat { get; set; }

        [Display(Name = "Office")]
        public string BACOffice { get; set; }

        [Display(Name = "Designation")]
        public string BACSecretariatDesignation { get; set; }

        public List<AnnualProcurementPlanDetailsVM> APPLineItems { get; set; }
    }
    public class AnnualProcurementPlanHeaderVM
    {
        [Display(Name = "Reference No.")]
        public string ReferenceNo { get; set; }

        [Display(Name = "APP Type")]
        public string APPType { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "Date Prepared")]
        public string PreparedAt { get; set; }

        [Display(Name = "Date Recommended")]
        public string RecommendedAt { get; set; }

        [Display(Name = "Date Approved")]
        public string ApprovedAt { get; set; }
    }

    public class APPLineItemVM
    {
        public string UACS { get; set; }
        public string ObjectClassification { get; set; }
        public List<ApprovedItems> ApprovedItems { get; set; }
    }
    public class ApprovedItems
    {
        public bool IsTangible { get; set; }
        public bool IsInstitutional { get; set; }
        public ProcurementSources ProcurementSource { get; set; }
        public int Month { get; set; }

        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Item Specification")]
        public string ItemSpecification { get; set; }

        [Display(Name = "Classification")]
        public int ClassificationReference { get; set; }

        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Display(Name = "Object Classification")]
        public string ObjectClassification { get; set; }

        [Display(Name = "UACS SubClass")]
        public string UACSSubClass { get; set; }

        [Display(Name = "Object Classification")]
        public string ObjectSubClassification { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit")]
        public string UnitOfMeasure { get; set; }

        [Display(Name = "Total Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Display(Name = "Fund Description")]
        public string FundDescription { get; set; }

        [Display(Name = "Mode of Procurement")]
        public string[] ModeOfProcurement { get; set; }

        [Display(Name = "End-User")]
        public string EndUser { get; set; }

        [Display(Name = "End-User")]
        public string EndUserName { get; set; }

        [Display(Name = "MOOE")]
        public decimal MOOE { get; set; }

        [Display(Name = "Capital Outlay")]
        public decimal CapitalOutlay { get; set; }

        [Display(Name = "Inventory Code")]
        public string InventoryCode { get; set; }

        [Display(Name = "Schedule for Procurement Activity")]
        public string Schedule { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
    }

    public enum APPTypes
    {
        [Display(Name = "Common-use Supplies and Equipment", ShortName = "CSE")]
        APPCSE = 0,

        [Display(Name = "Indicative Annual Procurement Plan", ShortName = "INDC")]
        Indicative = 1,

        [Display(Name = "Annual Procurement Plan", ShortName = "ORIG")]
        Original = 2,

        [Display(Name = "Supplemental Annual Procurement Plan", ShortName = "SUPP")]
        Supplemental = 3,

        [Display(Name = "Amendatory Annual Procurement Plan", ShortName = "AMND")]
        Amendatory = 4
    }
    public enum APPStatus
    {
        [Display(Name = "Posted to APP")]
        PostedToAPP = 0,

        [Display(Name = "Posted to Procurement Project")]
        PostedToProcurementProject = 1
    }
}