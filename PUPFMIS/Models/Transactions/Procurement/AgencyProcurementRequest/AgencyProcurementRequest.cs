//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.AgencyProcurementRequest")]
//    public class AgencyProcurementRequest
//    {
//        [Key]
//        public int ID { get; set; }

//        [Required]
//        [Display(Name = "Agency Control No.")]
//        public string AgencyControlNo { get; set; }

//        [Required]
//        [Display(Name = "Date Prepared")]
//        public DateTime DatePrepared { get; set; }

//        [Required]
//        [Display(Name = "Prepared by")]
//        public string PreparedBy { get; set; }

//        [Display(Name = "APR Control No.")]
//        public string APRControlNo { get; set; }
                
//        public int PRNumber { get; set; }

//        [Display(Name = "Status")]
//        public string Status { get; set; }

//        [Display(Name = "Purchase Request No.")]
//        [ForeignKey("PRNumber")]
//        public PurchaseRequest FKPurhcaseRequest { get; set; }

//        [Display(Name = "Stocks Requested Certified by")]
//        public string StocksRequestCertifiedBy { get; set; }

//        [Display(Name = "Funds Certified Available")]
//        public string FundsCertifiedAvailable { get; set; }

//        [Display(Name = "Approved by")]
//        public string ApprovedBy { get; set; }
//    }
//}