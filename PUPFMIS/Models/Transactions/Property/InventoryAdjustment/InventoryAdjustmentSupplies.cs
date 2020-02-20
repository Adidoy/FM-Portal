//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.InventoryAdjustmentSupplies")]
//    public class InventoryAdjustmentSupplies
//    {
//        [Key]
//        public int ID { get; set; }

//        public int InventoryAdjustmentReference { get; set; }

//        public int SupplyReference { get; set; }

//        public string Action { get; set; }

//        public int Quantity { get; set; }

//        [ForeignKey("InventoryAdjustmentReference")]
//        public virtual InventoryAdjustment FKAdjustmentReference { get; set; }

//        [ForeignKey("SupplyReference")]
//        [Display(Name = "Supply")]
//        public virtual SuppliesMaster FKSupplyReference { get; set; }
//    }
//}