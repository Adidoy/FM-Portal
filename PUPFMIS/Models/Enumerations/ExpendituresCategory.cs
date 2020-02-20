using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{

    public enum ExpendituresCategory
    {
        [Display(Name = "Maintenance and Other Operating Expenses", ShortName = "MOOE")]
        MOOE,
        [Display(Name = "Captial Outlay", ShortName = "CO")]
        CO
    }
}