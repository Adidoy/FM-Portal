//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.PurchaseOrderLineItems")]
//    public class PurchaseOrderLineItems
//    {
//        [Key]
//        public int ID { get; set; }

//        public int PurchaseOrderReference { get; set; }

//        public int ItemsReference { get; set; }

//        public int Quantity { get; set; }

//        [Display(Name = "Unit Cost")]
//        public decimal UnitCost { get; set; }

//        [ForeignKey("PurchaseOrderReference")]
//        public virtual PurchaseOrderHeader FKPurchaseOrderReference { get; set; }

//        [ForeignKey("ItemsReference")]
//        [Display(Name = "Item")]
//        public virtual ItemsMaster FKItemsReference { get; set; }
//    }
//}