//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.PurchaseRequestLineItems")]
//    public class PurchaseRequestLineItems
//    {
//        [Key]
//        public int ID { get; set; }

//        public int PurchaseRequestID { get; set; }

//        public int PPMPLineItem { get; set; }

//        [ForeignKey("PPMPLineItem")]
//        [Display(Name = "Item")]
//        public virtual PPMPDetails FKPPMPLineItem { get; set; }

//        [ForeignKey("PurchaseRequestID")]
//        [Display(Name = "Purchase Request Number")]
//        public virtual PurchaseRequest FKPurchaseRequestNumber { get; set; }
//    }
//}