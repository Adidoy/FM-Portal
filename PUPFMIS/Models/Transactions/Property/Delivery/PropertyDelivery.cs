//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;


//namespace PUPFMIS.Models
//{
//    [Table("property.PropertyDelivery")]
//    public class PropertyDelivery
//    {
//        [Key, Column(Order = 1)]
//        public int ID { get; set; }

//        [Key, Column(Order = 2)]
//        public int PPEReference { get; set; }

//        public int DeliveryID { get; set; }

//        [Required]
//        [Display(Name = "Quantity Delivered")]
//        public int QuantityDelivered { get; set; }

//        [Required]
//        [Display(Name = "Backlog")]
//        public int QuantityBacklog { get; set; }

//        [ForeignKey("PPEReference")]
//        [Display(Name = "PPE")]
//        public virtual PPE FKPPEReference { get; set; }

//        [ForeignKey("DeliveryID")]
//        public virtual DeliveryHeader FKDelivery { get; set; }
//    }
//}