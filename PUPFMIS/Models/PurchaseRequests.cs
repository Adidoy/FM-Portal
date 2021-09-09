using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROC_TRXN_Purchase_Request")]
    public class PurchaseRequestHeader
    {
        [Key]
        public int ID { get; set; }

        public int FiscalYear { get; set; }

        [Display(Name = "P/R Number")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(12, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field is required.")]
        public string PRNumber { get; set; }

        [Display(Name = "Status")]
        public PurchaseRequestStatus PRStatus { get; set; }

        [Required]
        [Display(Name = "Contract Reference")]
        public int ProcurementProjectReference { get; set; }

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

        [Display(Name = "Date Submitted")]
        public DateTime? SubmittedAt { get; set; }

        [Display(Name = "Submitted By")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(30, ErrorMessage = "{0} is up to {1} characters only.")]
        public string SubmittedBy { get; set; }

        [Display(Name = "Date Received")]
        public DateTime? ReceivedAt { get; set; }

        [Display(Name = "Received By")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(30, ErrorMessage = "{0} is up to {1} characters only.")]
        public string ReceivedBy { get; set; }

        [ForeignKey("ProcurementProjectReference")]
        public virtual ProcurementProject FKProcurementProjectReference { get; set; }
    }
    [Table("PROC_TRXN_Purchase_Request_Details")]
    public class PurchaseRequestDetails
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int PRHeaderReference { get; set; }

        [Required]
        public int ClassificationID { get; set; }

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
        public int UOMReference { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitCost { get; set; }

        [Required]
        public decimal TotalCost { get; set; }

        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKArticleReference { get; set; }

        [ForeignKey("UOMReference")]
        public virtual UnitOfMeasure FKUOMReference { get; set; }

        [ForeignKey("PRHeaderReference")]
        public virtual PurchaseRequestHeader FKPRHeaderReference { get; set; }

    }

    public class PurchaseRequestDashboard
    {
        public List<ProcurementProjectListVM> OpenSubmissions { get; set; }
        public List<PurchaseRequestHeaderVM> ForSubmission { get; set; }
        public List<int> ProjectFiscalYears { get; set; }
        public List<int> PRFiscalYears { get; set; }
    }
}