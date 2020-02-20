using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public enum RequestTypes
    {
        [Display(Name = "Supplies Request")]
        SUPREQ,
        [Display(Name = "Semi-expendable PPE Request")]
        SEPREQ,
        [Display(Name = "PPE Request")]
        PPEREQ
    }
}