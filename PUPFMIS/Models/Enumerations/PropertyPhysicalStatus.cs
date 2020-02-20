using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public enum PropertyPhysicalStatus
    {
        [Display(Name = "Confirmed")]
        Confirmed = 0,
        [Display(Name = "Stolen")]
        Stolen = 1,
        [Display(Name = "Lost")]
        Lost = 2,
        [Display(Name = "Damaged")]
        Damaged = 3,
        [Display(Name = "Destroyed")]
        Destroyed = 4,
        [Display(Name = "Location Changed")]
        LocationChanged = 5,
        [Display(Name = "Accountability Transferred")]
        AccountabilityTransferred = 6
    }
}