using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PP_APP_HEADER")]
    public class APPHeader
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "APP Type")]
        public string APPType { get; set; }

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
    [Table("PP_APP_PROCUREMENT_PROGRAMS")]
    public class ProcurementPrograms
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(30, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string PAPCode { get; set; }

        public int APPHeaderReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(175, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ProcurementProgram { get; set; }

        [Required]
        [Display(Name = "Mode of Procurement")]
        public string APPModeOfProcurementReference { get; set; }

        [Display(Name = "Mode of Procurement")]
        public int? ModeOfProcurementReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string ObjectClassification { get; set; }

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

        public string ProjectReferences { get; set; }

        public string PPMPReferences { get; set; }

        [Display(Name = "Project Cost")]
        public decimal ProjectCost { get; set; }

        [Display(Name = "Supplier")]
        public int? SupplierReference { get; set; }

        [Display(Name = "Project Coordinator")]
        public string ProjectCoordinator { get; set; }

        [Display(Name = "Project Support")]
        public string ProjectSupport { get; set; }

        [Display(Name = "Status")]
        public string ProjectStatus { get; set; }

        public bool IsInstitutional { get; set; }

        public bool IsTangible { get; set; }

        public bool IsAccepted { get; set; }

        [Display(Name = "Purchase Order Number")]
        public int? PurchaseOrderReference { get; set; }

        [ForeignKey("PurchaseOrderReference")]
        public virtual PurchaseOrderHeader FKPurchaseOrderHeaderReference { get; set; }

        [ForeignKey("APPHeaderReference")]
        public virtual APPHeader FKAPPHeaderReference { get; set; }

        [ForeignKey("SupplierReference")]
        public virtual Supplier FKSupplierReference { get; set; }

        [ForeignKey("ModeOfProcurementReference")]
        public virtual ModeOfProcurement FKModeOfProcurementReference { get; set; }
    }
    [Table("PP_APP_CSE_ITEMS")]
    public class APPCSEDetails
    {
        [Key, Column(Order = 0)]
        public int APPHeaderReference { get; set; }

        [Key, Column(Order = 1)]
        public int ItemReference { get; set; }

        [Display(Name = "Price Catalogue")]
        public decimal PriceCatalogue { get; set; }

        [Display(Name = "JAN")]
        public int JanQty { get; set; }

        [Display(Name = "FEB")]
        public int FebQty { get; set; }

        [Display(Name = "MAR")]
        public int MarQty { get; set; }

        [Display(Name = "APR")]
        public int AprQty { get; set; }

        [Display(Name = "MAY")]
        public int MayQty { get; set; }

        [Display(Name = "JUN")]
        public int JunQty { get; set; }

        [Display(Name = "JUL")]
        public int JulQty { get; set; }

        [Display(Name = "AUG")]
        public int AugQty { get; set; }

        [Display(Name = "SEP")]
        public int SepQty { get; set; }

        [Display(Name = "OCT")]
        public int OctQty { get; set; }

        [Display(Name = "NOV")]
        public int NovQty { get; set; }

        [Display(Name = "DEC")]
        public int DecQty { get; set; }

        [ForeignKey("APPHeaderReference")]
        public virtual APPHeader FKAPPHeaderReference { get; set; }

        [ForeignKey("ItemReference")]
        public virtual Item FKItemReference { get; set; }
    }
    public class AnnualProcurementPlanCSEItemsVM
    {
        public int ItemID { get; set; }

        [Display(Name = "Item and Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }

        [Display(Name = "JAN")]
        public int JanQty { get; set; }

        [Display(Name = "FEB")]
        public int FebQty { get; set; }

        [Display(Name = "MAR")]
        public int MarQty { get; set; }

        [Display(Name = "Q1 Total")]
        public int Q1Total { get; set; }

        [Display(Name = "Q1 Amount")]
        public decimal Q1Amount { get; set; }

        [Display(Name = "APR")]
        public int AprQty { get; set; }

        [Display(Name = "MAY")]
        public int MayQty { get; set; }

        [Display(Name = "JUN")]
        public int JunQty { get; set; }

        [Display(Name = "Q2 Total")]
        public int Q2Total { get; set; }

        [Display(Name = "Q2 Amount")]
        public decimal Q2Amount { get; set; }

        [Display(Name = "JUL")]
        public int JulQty { get; set; }

        [Display(Name = "AUG")]
        public int AugQty { get; set; }

        [Display(Name = "SEP")]
        public int SepQty { get; set; }

        [Display(Name = "Q3 Total")]
        public int Q3Total { get; set; }

        [Display(Name = "Q3 Amount")]
        public decimal Q3Amount { get; set; }

        [Display(Name = "OCT")]
        public int OctQty { get; set; }

        [Display(Name = "NOV")]
        public int NovQty { get; set; }

        [Display(Name = "DEC")]
        public int DecQty { get; set; }

        [Display(Name = "Q4 Total")]
        public int Q4Total { get; set; }

        [Display(Name = "Q4 Amount")]
        public decimal Q4Amount { get; set; }

        [Display(Name = "Total Quantity for the Year")]
        public int TotalQty { get; set; }

        [Display(Name = "Price Catalogue")]
        public decimal PriceCatalogue { get; set; }

        [Display(Name = "Total Amount for the Year")]
        public decimal TotalAmount { get; set; }
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

    public class ApprovedItems
    {
        public bool IsTangible { get; set; }
        public bool IsInstitutional { get; set; }

        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Item Specification")]
        public string ItemSpecification { get; set; }

        [Display(Name = "UACS")]
        public string UACS { get; set; }

        [Display(Name = "Object Classification")]
        public string ObjectClassification { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit")]
        public string UnitOfMeasure { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Display(Name = "Fund Description")]
        public string FundDescription { get; set; }

        [Display(Name = "Mode of Procurement")]
        public string[] ModeOfProcurement { get; set; }

        [Display(Name = "End-User")]
        public string EndUser { get; set; }

        [Display(Name = "MOOE")]
        public decimal MOOE { get; set; }

        [Display(Name = "CO")]
        public decimal CapitalOutlay { get; set; }

        [Display(Name = "Inventory Code")]
        public string InventoryCode { get; set; }

        [Display(Name = "Month")]
        public int Month { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
    }
}