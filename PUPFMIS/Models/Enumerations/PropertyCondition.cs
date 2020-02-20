using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public enum PropertyCondition
    {
        [Display(Name = "Serviceable")]
        SRVCBL,
        [Display(Name = "Unserviceable")]
        NSRVCBL
    }
}