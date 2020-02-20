//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.PhysicalCountSupplies")]
//    public class PhysicalCountSupplies
//    {
//        [Key]
//        public int ID { get; set; }

//        public int PhysicalCountReference { get; set; }

//        public int SuppliesReference { get; set; }

//        [Display(Name = "Unit Value")]
//        public decimal UnitValue { get; set; }

//        [Display(Name = "On Hand per Count")]
//        public int OnHandPerCount { get; set; }

//        [Display(Name = "Shortage/Overage")]
//        public int OverageShortage { get; set; }

//        public string Remarks { get; set; }

//        [ForeignKey("PhysicalCountReference")]
//        public virtual PhysicalCount FKCountReference { get; set; }

//        [ForeignKey("SuppliesReference")]
//        [Display(Name = "Supply")]
//        public virtual SuppliesMaster FKSuppliesReference { get; set; }
//    }
//}