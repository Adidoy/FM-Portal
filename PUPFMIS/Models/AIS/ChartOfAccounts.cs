using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models.AIS
{
    [Table("Master.ChartOfAccounts")]
    public class ChartOfAccounts
    {
        [Key]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(15, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "UACS Code")]
        public string UACS { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(250, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Account Title")]
        public string AccountTitle { get; set; }
    }
}