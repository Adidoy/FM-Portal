using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{

    public enum ProcurementPlanAction
    {
        [Display(Name = "Original - Indicative")]
        NEW,
        [Display(Name = "Original")]
        ORG,
        [Display(Name = "Supplementary")]
        SUP,
        [Display(Name = "Amendatory")]
        AMD
    }
}