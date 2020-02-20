//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.InspectionHeader")]
//    public class InspectionHeader
//    {
//        [Key]
//        public int ID { get; set; }

//        [Display(Name = "Inspection and Acceptance Report No.", ShortName = "IAR No.")]
//        [Required]
//        public string IARNo { get; set; }

//        [Required]
//        public int POReference { get; set; }

//        [ForeignKey("POReference")]
//        [Display(Name = "Purchase Order Reference")]
//        public virtual PurchaseOrderHeader FKPOReference { get; set; }

//        [Display(Name = "Inspection Personnel")]
//        public string InspectionPersonnel { get; set; }

//        [Display(Name = "Inspection Date")]
//        [Required]
//        public DateTime InspectionDate { get; set; }

//        [Display(Name = "Remarks")]
//        [DataType(DataType.MultilineText)]
//        public string OverallRemarks { get; set; }
//    }
//}