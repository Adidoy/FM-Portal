using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("procurement_appCSE")]
    public class APPCSEHeader
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "APP Reference No.")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        [MaxLength(4)]
        public string FiscalYear { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "% Implemented")]
        public decimal PercentImplemented { get; set; }

        [Required]
        [Display(Name = "Date Approved")]
        public DateTime ApprovedAt { get; set; }
    }

    //[Table("procurement.APPHeader")]
    //public class APPHeader
    //{
    //    [Key]
    //    public int ID { get; set; }

    //    [Required]
    //    [Display(Name = "APP Reference No.")]
    //    public string ReferenceNo { get; set; }

    //    [Required]
    //    [Display(Name = "Fiscal Year")]
    //    [MaxLength(4)]
    //    public string FiscalYear { get; set; }

    //    [Required]
    //    [Display(Name = "Date Created")]
    //    public DateTime CreatedAt { get; set; }

    //    [Required]
    //    [Display(Name = "Procurement Plan Action")]
    //    public ProcurementPlanAction Action { get; set; }

    //    [Required]
    //    [Display(Name = "% Implemented")]
    //    public decimal PercentImplemented { get; set; }

    //    [Required]
    //    [Display(Name = "Date Approved")]
    //    public DateTime ApprovedAt { get; set; }
    //}
}