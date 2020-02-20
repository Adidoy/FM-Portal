//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.PurchaseRequest")]
//    public class PurchaseRequest
//    {
//        [Key]
//        public int ID { get; set; }

//        [Required]
//        [Display(Name = "Purchase Request No.", ShortName = "P.R. Number")]
//        public string PurchaseRequestNo { get; set; }

//        [Required]
//        public DateTime CreatedAt { get; set; }

//        [Required]
//        public int Department { get; set; }

//        public int? Section { get; set; }

//        [Required]
//        public string Purpose { get; set; }

//        [ForeignKey("Department")]
//        public virtual OfficesMaster FK_Department { get; set; }

//        [ForeignKey("Section")]
//        public virtual OfficesMaster FK_Section { get; set; }
//    }
//}