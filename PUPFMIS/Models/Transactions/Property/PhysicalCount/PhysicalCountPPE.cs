//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.PhysicalCountPPE")]
//    public class PhysicalCountPPE
//    {
//        [Key]
//        public int ID { get; set; }

//        public int PhysicalCountReference { get; set; }

//        public int PPEInstanceReference { get; set; }

//        [Display(Name = "Unit Value")]
//        public decimal UnitValue { get; set; }

//        [Display(Name = "Condition")]
//        public PropertyCondition Condition { get; set; }

//        [Display(Name = "Physical Status")]
//        public PropertyPhysicalStatus PhysicalStatus { get; set; }

//        public string Remarks { get; set; }

//        [ForeignKey("PhysicalCountReference")]
//        public virtual PhysicalCount FKCountReference { get; set; }

//        [ForeignKey("PPEInstanceReference")]
//        [Display(Name = "PPE")]
//        public virtual PPEInstance FKPPEReference { get; set; }
//    }
//}