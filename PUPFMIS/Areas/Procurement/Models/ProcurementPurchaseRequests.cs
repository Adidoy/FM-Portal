using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    public class ProjectContractVM
    {
        public ProcurementProjectTypes ProcurementProjectType { get; set; }

        [Display(Name = "Contract Reference")]
        public string ContractCode { get; set; }

        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Reference")]
        public string ProjectName { get; set; }

        [Display(Name = "PPMP Reference")]
        public string PPMPReferenceNo { get; set; }

        [Display(Name = "PPMP Type")]
        public string PPMPType { get; set; }

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Display(Name = "Fund Description")]
        public string FundDescription { get; set; }

        [Display(Name = "Approved Budget")]
        public decimal ApprovedBudget { get; set; }
    }

    public class PurchaseRequestCoordinatorDashboard
    {
        public List<ProcurementProjectListVM> NewProcurementProjects { get; set; }
        public List<ProcurementProjectListVM> OpenPRSubmissions { get; set; }
        public List<PurchaseRequestHeaderVM> ForReceiving { get; set; }
        public List<int> ProjectFiscalYears { get; set; }
        public List<int> PRFiscalYears { get; set; }
    }

    public class PurchaseRequestHeaderVM
    {
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "PR Number")]
        public string PRNumber { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Status")]
        public PurchaseRequestStatus PRStatus { get; set; }

        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string FundCluster { get; set; }

        [Display(Name = "Purpose")]
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

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime? SubmittedAt { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }

        [Display(Name = "Date Received")]
        public DateTime? ReceivedAt { get; set; }

        [Display(Name = "Received By")]
        public string ReceivedBy { get; set; }
    }
    public class PurchaseRequestDetailsVM
    {
        public int ClassificationID { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

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

        public int UOMReference { get; set; }

        [Display(Name = "Unit of Measure")]
        public string Unit { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Total Cost")]
        public decimal TotalCost { get; set; }
    }
    public class PurchaseRequestVM
    {
        [Display(Name = "Contract Type")]
        public ProcurementProjectTypes ProcurementProjectType { get; set; }

        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "PR No.")]
        public string PRNumber { get; set; }

        [Display(Name = "Fund Source")]

        public string FundSource { get; set; }

        [Display(Name = "Fund Cluster")]
        public string FundCluster { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Purpose")]
        [Required(ErrorMessage = "Purpose of this request is required.")]
        public string Purpose { get; set; }

        [Display(Name = "Requested By")]
        public string RequestedBy { get; set; }

        public string Department { get; set; }

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
        public DateTime CreatedAt { get; set; }

        public List<PurchaseRequestDetailsVM> PRDetails { get; set; }
    }
    public class PurchaseRequestMemoOffices
    {
        public string DepartmentHead { get; set; }
        public string DepartmentHeadLastName { get; set; }
        public string Department { get; set; }
        public string Unit { get; set; }
        public string Designation { get; set; }
        public string ContractCode { get; set; }
        public List<ProcurementProjectDetailsVM> Items { get; set; }
    }

    public enum PurchaseRequestStatus
    {
        [Display(Name = "Purchase Request Created")]
        PurchaseRequestCreated,

        [Display(Name = "Purchase Request Submitted")]
        PurchaseRequestSubmitted,

        [Display(Name = "Purchase Request Reived by Procument Office")]
        PurchaseRequestReceived
    }
}