//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.PurchaseOrder")]
//    public class PurchaseOrderHeader
//    {
//        [Key]
//        public int ID { get; set; }

//        [Display(Name = "Fiscal Year")]
//        [Required]
//        public string FiscalYear { get; set; }

//        [Display(Name = "Fund Cluster")]
//        [Required]
//        public string FundCluster { get; set; }

//        [Display(Name = "Purchase Order Number", ShortName = "P.O. Number")]
//        [Required]
//        public string PONumber { get; set; }

//        [Display(Name = "Purchase Order Date", ShortName = "P.O. Date")]
//        public DateTime PODate { get; set; }

//        [Display(Name = "Mode of Procurement")]
//        [Required]
//        public ProcurementModes ProcurementMode { get; set; }

//        [Display(Name = "Delivery Term")]
//        [Required]
//        public string DeliveryTerm { get; set; }

//        [Display(Name = "Payment Term")]
//        [Required]
//        public string PaymentTerm { get; set; }

//        [Display(Name = "Place of Delivery")]
//        [Required]
//        public string PlaceOfDelivery { get; set; }

//        [Display(Name = "Status")]
//        [Required]
//        public string Status { get; set; }

//        [Required]
//        public int Supplier { get; set; }

//        [ForeignKey("Supplier")]
//        public virtual SuppliersMaster FKSupplier { get; set; }
//    }
//}