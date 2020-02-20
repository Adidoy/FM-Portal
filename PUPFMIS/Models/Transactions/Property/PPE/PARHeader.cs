//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.PARHeader")]
//    public class PARHeader
//    {
//        [Key]
//        public int ID { get; set; }

//        [Display(Name = "Property Acknowledgement Receipt No.", ShortName = "PAR No.")]
//        public string PARNo { get; set; }

//        public int POID { get; set; }

//        public int RISReference { get; set; }

//        public int Office { get; set; }

//        [Display(Name = "Accountble Officer")]
//        public string AccountableOfficer { get; set; }

//        public string Designation { get; set; }

//        [Display(Name = "Received From")]
//        public string ReceivedFrom { get; set; }

//        [Display(Name = "Received By")]
//        public int ReceivedBy { get; set; }

//        [Display(Name = "Date Issued")]
//        public DateTime DateIssued { get; set; }

//        [ForeignKey("Office")]
//        public OfficesMaster FKOffice { get; set; }

//        [ForeignKey("POID")]
//        public PurchaseOrderHeader FKPOID { get; set; }

//        [ForeignKey("RISReference")]
//        [Display(Name = "RIS Reference")]
//        public RequestHeader FKRequestHeader { get; set; }
//    }
//}