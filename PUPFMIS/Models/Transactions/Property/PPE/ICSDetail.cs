//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.ICSDetails")]
//    public class ICSDetails
//    {
//        [Key]
//        public int ICSID { get; set; }
        
//        [Display(Name = "Property No.")]
//        public string PropertyNo { get; set; }

//        [Display(Name = "Qty Issued")]
//        public int QtyIssued { get; set; }

//        [Display(Name = "Unit Cost")]
//        public decimal UnitCost { get; set; }

//        [ForeignKey("ICSID")]
//        public virtual ICSHeader FKICSHeader { get; set; }
//    }
//}