using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{

    public enum ProcurementSources
    {
        [Display(Name = "DBM Procurement System")]
        PS_DBM = 0,
        [Display(Name = "Non DBM-PS")]
        Non_DBM
    }
}