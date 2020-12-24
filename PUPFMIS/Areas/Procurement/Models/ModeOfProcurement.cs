using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PROC_MSTR_Procurement_Modes")]
    public class ModeOfProcurement
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Mode of Procurement")]
        [MaxLength(175, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ModeOfProcurementName { get; set; }        

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }
    }
}