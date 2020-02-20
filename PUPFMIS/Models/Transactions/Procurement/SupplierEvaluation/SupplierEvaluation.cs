//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.EvaluationSupplier")]
//    public class SupplierEvaluation
//    {
//        [Key]
//        public int ID { get; set; }

//        public int PurchaseOrderID { get; set; }

//        public int? SupplierID { get; set; }

//        public int EndUser { get; set; }

//        public int ItemID { get; set; }

//        [ForeignKey("PurchaseOrderID")]
//        [Display(Name = "Purchase Order")]
//        public virtual PurchaseOrderHeader FKPurchaseOrder { get; set; }

//        [ForeignKey("EndUser")]
//        [Display(Name = "End-User")]
//        public virtual OfficesMaster FKOffices { get; set; }

//        [ForeignKey("ItemID")]
//        [Display(Name = "Item")]
//        public virtual ItemsMaster FKItems { get; set; }

//        [ForeignKey("SupplierID")]
//        [Display(Name = "Supplier")]
//        public virtual SuppliersMaster FKSuppliers { get; set; }
//    }
//}