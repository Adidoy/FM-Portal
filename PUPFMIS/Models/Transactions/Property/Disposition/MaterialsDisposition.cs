using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("property.MaterialsDisposition")]
    public class MaterialsDisposition
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Fund Cluster")]
        public string FundCluster { get; set; }

        public int PlaceOfStorage { get; set; }

        public DateTime Date { get; set; }

        [Display(Name = "Certified Correct")]
        public string CertifiedCorrect { get; set; }

        [Display(Name = "Disposal Approved")]
        public string DisposalApproved { get; set; }

        [Display(Name = "Place of Storage")]
        [ForeignKey("PlaceOfStorage")]
        public virtual LocationsMaster FKLocation { get; set; }
    }
}