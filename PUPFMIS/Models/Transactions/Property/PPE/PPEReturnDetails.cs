//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.PPEReturnDetails")]
//    public class PPEReturnDetails
//    {
//        [Key]
//        public int ID { get; set; }

//        public int PPEReturnReference { get; set; }

//        public int PARReference { get; set; }

//        public PropertyCondition PropertyCondition { get; set; }

//        [ForeignKey("PARReference")]
//        [Display(Name = "PAR Reference")]
//        public virtual PARDetails FKPARReference { get; set; }

//        [ForeignKey("PPEReturnReference")]
//        public virtual PPEReturn FKPPEReturn { get; set; }
//    }
//}