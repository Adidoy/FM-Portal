using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PP_MASTER_SUPPLIES")]
    public class Supply
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Stock Number", ShortName = "Stock No.")]
        public string StockNo { get; set; }

        [Display(Name = "Re-order Point")]
        public int ReOrderPoint { get; set; }

        [Display(Name = "Days to Consume")]
        public int DaysToConsume { get; set; }

        public int? UOMDelivery { get; set; }

        public int? UOMDistribution { get; set; }

        public int? ItemID { get; set; }

        [ForeignKey("ItemID")]
        [Display(Name = "Item")]
        public virtual Item FKItem { get; set; }

        [ForeignKey("UOMDelivery")]
        [Display(Name = "Unit of Measure - Delivery")]
        public virtual UnitOfMeasure FKUOMDelivery { get; set; }

        [ForeignKey("UOMDistribution")]
        [Display(Name = "Unit of Measure - Distribution")]
        public virtual UnitOfMeasure FKUOMDistribution { get; set; }
    }
}