//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.ItemDispatchDetails")]
//    public class ItemPullOutDetails
//    {
//        [Key]
//        public int ID { get; set; }

//        public int DispatchReference { get; set; }

//        public int SuppliesReference { get; set; }

//        public int PPEsReference { get; set; }

//        [Display(Name = "Qty Pulled-out")]
//        public int PullOutQty { get; set; }

//        [ForeignKey("DispatchReference")]
//        public virtual ItemPullOutHeader FKDispatchReference { get; set; }

//        [ForeignKey("SuppliesReference")]
//        public virtual SuppliesMaster FKSuppliesReference { get; set; }

//        [ForeignKey("PPEsReference")]
//        public virtual PPE FKPPEsReference { get; set; }
//    }
//}