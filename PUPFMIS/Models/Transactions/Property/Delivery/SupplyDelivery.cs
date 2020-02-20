//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.SupplyDelivery")]
//    public class SupplyDelivery
//    {
//        [Key, Column(Order = 1)]
//        public int ID { get; set; }

//        [Key, Column(Order = 2)]
//        public int SupplyID { get; set; }

//        public int DeliveryID { get; set; }

//        [Required]
//        [Display(Name = "Quantity Delivered")]
//        public int QuantityDelivered { get; set; }

//        [Required]
//        [Display(Name = "Backlog")]
//        public int QuantityBacklog { get; set; }

//        [ForeignKey("SupplyID")]
//        [Display(Name = "Supply")]
//        public virtual SuppliesMaster FKSupplies { get; set; }

//        [ForeignKey("DeliveryID")]
//        public virtual DeliveryHeader FKDelivery { get; set; }
//    }
//}