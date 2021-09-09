using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public class Months
    {
        public int MonthValue { get; set; }

        [Display(Name = "Month")]
        public string MonthName { get; set; }
    }
}