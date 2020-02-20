//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.PARDetails")]
//    public class PARDetails
//    {
//        [Key]
//        public int PARNo { get; set; }

//        [Display(Name = "Property No.")]
//        public string PropertyNo { get; set; }

//        [Display(Name = "Qty Issued")]
//        public int QtyIssued { get; set; }

//        [Display(Name = "Unit Cost")]
//        public decimal UnitCost { get; set; }

//        public int PPEInstanceReference { get; set; }

//        [ForeignKey("PARNo")]
//        [Display(Name = "PAR Information")]
//        public virtual PARHeader FKPARHeader { get; set; }

//        [ForeignKey("PPEInstanceReference")]
//        [Display(Name = "Property Information")]
//        public virtual PPEInstance FKPPEInstanceReference { get; set; }
//    }
//}