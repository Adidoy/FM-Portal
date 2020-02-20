//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.PPE")]
//    public class PPE
//    {
//        [Key]
//        public int ID { get; set; }

//        public int ItemID { get; set; }

//        [Display(Name = "Object Account Code")]
//        public int UACSObjectAccountCode { get; set; }

//        [Display(Name = "Property No.")]
//        public string PropertyNo { get; set; }

//        public int Unit { get; set; }

//        [Display(Name = "Estimated Useful Life (in years)")]
//        public decimal UsefulLife { get; set; }

//        [Display(Name = "Rate of Depreciation")]
//        public decimal DepreciationRate { get; set; }

//        [ForeignKey("UACSObjectAccountCode")]
//        public virtual ChartOfAccountsMaster FKUACS { get; set; }

//        [ForeignKey("ItemID")]
//        [Display(Name = "Item")]
//        public virtual ItemsMaster FKItem { get; set; }

//        [ForeignKey("Unit")]
//        public virtual UnitsOfMeasureMaster FKUnit { get; set; }
//    }
//}