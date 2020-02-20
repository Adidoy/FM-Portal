using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("property.PPEReturn")]
    public class PPEReturn
    {
        [Key]
        public int ID { get; set; }

        public string EndUser { get; set; }

        [Display(Name = "Date Returned")]
        public DateTime DateReturned { get; set; }
    }
}