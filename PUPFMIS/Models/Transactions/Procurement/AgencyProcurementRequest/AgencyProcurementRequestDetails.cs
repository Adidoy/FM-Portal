//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.AgencyProcurementRequestDetails")]
//    public class AgencyProcurementRequestDetails
//    {
//        [Key]
//        public int ID { get; set; }

//        public int AgencyProcurementRequestID { get; set; }

//        public int ItemID { get; set; }

//        public int Quantity { get; set; }

//        [Display(Name = "Unit Price")]
//        public decimal UnitPrice { get; set; }

//        [ForeignKey("AgencyProcurementRequestID")]
//        [Display(Name = "APR No.")]
//        public AgencyProcurementRequest FKAgencyProcurementRequest { get; set; }

//        [ForeignKey("ItemID")]
//        [Display(Name = "Item and Description")]
//        public ItemsMaster FKItemsMaster { get; set; }
//    }
//}