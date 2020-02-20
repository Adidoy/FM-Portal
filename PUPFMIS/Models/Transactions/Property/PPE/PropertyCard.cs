//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("proprerty.PropertyCard")]
//    public class PropertyCard
//    {
//        [Key]
//        public int ID { get; set; }

//        [Required]
//        public DateTime Date { get; set; }

//        public int Property { get; set; }

//        [Display(Name = "Fund Cluster")]
//        public string FundCluster { get; set; }

//        [Required]
//        [Display(Name = "Reference/PAR No.", ShortName = "Reference")]
//        public string Reference { get; set; }

//        [Display(Name = "Receipt Qty")]
//        public int ReceiptQty { get; set; }

//        [Display(Name = "Receipt Unit Cost")]
//        public decimal ReceiptUnitCost { get; set; }

//        [Display(Name = "Issue Qty")]
//        public int IssueQty { get; set; }

//        public int IssueOffice { get; set; }

//        public string IssueOfficer { get; set; }

//        [Display(Name = "Balance Qty")]
//        public int BalanceQty { get; set; }

//        public decimal Amount { get; set; }

//        public string Remarks { get; set; }

//        [ForeignKey("IssueOffice")]
//        public virtual OfficesMaster FKOffice { get; set; }

//        [ForeignKey("Property")]
//        [Display(Name = "Property")]
//        public virtual PPE FKPPE { get; set; }
//    }
//}