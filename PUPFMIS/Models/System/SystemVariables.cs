using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("system_variables")]
    public class SystemVariables
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Variable")]
        public string VariableName { get; set; }

        [Required]
        [Display(Name = "Value")]
        public string Value { get; set; }
    }
}