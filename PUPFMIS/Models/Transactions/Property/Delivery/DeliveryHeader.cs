//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.DeliveryHeader")]
//    public class DeliveryHeader
//    {
//        [Key]
//        public int ID { get; set; }

//        [Required]
//        [Display(Name = "Delivery Acceptance No.")]
//        public string DeliveryAcceptanceNumber { get; set; }

//        [Required]
//        [Display(Name = "Date Processed")]
//        public DateTime DateProcessed { get; set; }

//        [Required]
//        [Display(Name = "Processed by")]
//        public string ProcessedBy { get; set; }

//        [Required]
//        [Display(Name = "Received by")]
//        public string ReceivedBy { get; set; }

//        public int POReference { get; set; }

//        public int APRReference { get; set; }

//        [Required]
//        [Display(Name = "Invoice No.")]
//        public string InvoiceNumber { get; set; }

//        [Required]
//        [Display(Name = "Invoice Date")]
//        public DateTime InvoiceDate { get; set; }

//        [Required]
//        [Display(Name = "Delivery Receipt No.")]
//        public string DRNumber { get; set; }

//        [Required]
//        [Display(Name = "Delivery Date")]
//        public DateTime DeliveryDate { get; set; }

//        [ForeignKey("POReference")]
//        [Display(Name = "Reference")]
//        public virtual PurchaseOrderHeader FKPurchaseOrder { get; set; }

//        [ForeignKey("APRReference")]
//        [Display(Name = "Reference")]
//        public virtual AgencyProcurementRequest FKAgencyProcurementRequest { get; set; }
//    }
//}