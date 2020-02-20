//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.PPEInstance")]
//    public class PPEInstance
//    {
//        [Key]
//        public int ID { get; set; }

//        public int POReference { get; set; }

//        public int PPEReference { get; set; }

//        [Display(Name = "Property No.")]
//        public string PropertyNo { get; set; }

//        [Display(Name = "Identifier", ShortName = "Identifier (Serial No./Service Tag No./Chasis No.)")]
//        public string Identifier { get; set; }

//        [Display(Name = "Condition")]
//        public PropertyCondition Condition { get; set; }

//        public int Location { get; set; }

//        [ForeignKey("POReference")]
//        [Display(Name = "Purchase Order Reference")]
//        public virtual PurchaseOrderHeader FKPOReference { get; set; }

//        [ForeignKey("PPEReference")]
//        public virtual PPE FKPPEReference { get; set; }

//        [ForeignKey("Location")]
//        public virtual LocationsMaster FKLocation { get; set; }
//    }
//}