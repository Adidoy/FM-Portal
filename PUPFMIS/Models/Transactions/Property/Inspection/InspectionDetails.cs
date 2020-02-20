using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("property.InspectionDetails")]
    public class InspectionDetails
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Quantity Passed")]
        [Required]
        public int QuantityPassed { get; set; }
        
        [Display(Name = "Quantity Failed")]
        public int QuantityFailed { get; set; } 
    }
}