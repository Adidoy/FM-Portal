using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROC_TRXN_Agency_Procurement_Request")]
    public class AgencyProcurementRequest
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Required]
        [Display(Name = "Agency Control No.")]
        public string AgencyControlNo { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Required]
        [Display(Name = "Procurement Head")]
        public string ProcurementHead { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string ProcurementDepartment { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public string ProcurementHeadDesignation { get; set; }

        [Required]
        [Display(Name = "Chief Accountant")]
        public string ChiefAccountant { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string ChiefAccountantDepartment { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public string ChiefAccountantDesignation { get; set; }

        [Required]
        [Display(Name = "Agency Head")]
        public string AgencyHead { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string AgencyHeadDepartment { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public string AgencyHeadDesignation { get; set; }
    }

    [Table("PROC_TRXN_Agency_Procurement_Request_Details")]
    public class AgencyProcurementRequestDetails
    {
        [Key]
        public int ID { get; set; }

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

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Required]
        [Display(Name = "Total Cost")]
        public decimal TotalCost { get; set; }

        [Required]
        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        public int APRReference { get; set; }

        [ForeignKey("APRReference")]
        public virtual AgencyProcurementRequest FKAPRReference { get; set; }

        [ForeignKey("UOMReference")]
        public virtual UnitOfMeasure FKUOMReference { get; set; }

        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKArticleReference { get; set; }
    }

    public class AgencyProcurementRequestVM
    {
        [Required]
        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "Contract Code")]
        public string ContractCode { get; set; }

        [Display(Name = "Contract Name")]
        public string ContractName { get; set; }

        [Display(Name = "Agency Control No.")]
        public string AgencyControlNo { get; set; }

        [Display(Name = "Date Prepared")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Procurement Head")]
        public string ProcurementHead { get; set; }

        [Display(Name = "Department")]
        public string ProcurementDepartment { get; set; }

        [Display(Name = "Department")]
        public string ProcurementDepartmentCode { get; set; }

        [Display(Name = "Designation")]
        public string ProcurementHeadDesignation { get; set; }

        [Display(Name = "Chief Accountant")]
        public string ChiefAccountant { get; set; }

        [Display(Name = "Department")]
        public string ChiefAccountantDepartment { get; set; }

        [Display(Name = "Department")]
        public string ChiefAccountantDepartmentCode { get; set; }

        [Display(Name = "Designation")]
        public string ChiefAccountantDesignation { get; set; }

        [Display(Name = "Agency Head")]
        public string AgencyHead { get; set; }

        [Display(Name = "Department")]
        public string AgencyHeadDepartment { get; set; }

        [Display(Name = "Department")]
        public string AgencyHeadDepartmentCode { get; set; }

        [Display(Name = "Designation")]
        public string AgencyHeadDesignation { get; set; }

        public List<APRDetailVM> APRDetails { get; set; }
    }
    public class APRDetailVM
    {
        [Display(Name = "Article")]
        public int ArticleReference { get; set; }

        public string ItemSequence { get; set; }

        [Display(Name = "Item Full Name")]
        public string ItemFullName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Required]
        [Display(Name = "Total Cost")]
        public decimal TotalCost { get; set; }

        [Required]
        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        [Required]
        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }

        public int APRReference { get; set; }
    }

    public class APRDashboardVM
    {
        public List<int> FiscalYears { get; set; }
        public List<ProcurementProjectListVM> NewProjects { get; set; }
    }
}