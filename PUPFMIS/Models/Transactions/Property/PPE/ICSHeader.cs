//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.ICSHeader")]
//    public class ICSHeader
//    {
//        [Key]
//        public int ID { get; set; }

//        [Display(Name = "Inventory Custodian Slip No.", ShortName = "ICS No.")]
//        public string ICSNo { get; set; }

//        public int Office { get; set; }

//        [Display(Name = "Accountble Officer")]
//        public string AccountableOfficer { get; set; }

//        public string Designation { get; set; }

//        [Display(Name = "Received From")]
//        public string ReceivedFrom { get; set; }
        
//        [Display(Name = "Received By")]
//        public int ReceivedBy { get; set; }

//        [ForeignKey("Office")]
//        public OfficesMaster FKOffice { get; set; }
//    }
//}