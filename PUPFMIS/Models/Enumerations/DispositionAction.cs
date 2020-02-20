using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public enum DispositionAction
    {
        [Display(Name = "Destruction")]
        DSTRYD = 0,
        [Display(Name = "Private Sale")]
        SLDPRV = 1,
        [Display(Name = "Public Auction")]
        SLDPUB = 2,
        [Display(Name = "Transfer to Agency")]
        TRNSFR = 3
    }
}