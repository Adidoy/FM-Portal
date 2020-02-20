using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("property.PhysicalCount")]
    public class PhysicalCount
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Physical Count No.")]
        public string PhysicalInventoryNo { get; set; }

        [Display(Name = "Fund Cluster")]
        public string FundCluster { get; set; }

        [Display(Name = "Period")]
        public string Period { get; set; }

        [Display(Name = "Count for")]
        public string CountFor { get; set; }

        [Display(Name = "Date of Count")]
        public DateTime DateOfCount { get; set; }

        [Display(Name = "Processed by")]
        public string ProcessedBy { get; set; }

        [Display(Name = "Verified by")]
        public string VerifiedBy { get; set; }
    }
}