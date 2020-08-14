using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PP_SYSTEM_SWITCHBOARD")]
    public class SwitchBoard
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Department")]
        [Column(TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        public string DepartmentCode { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Message Type")]
        [MaxLength(75, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        public string MessageType { get; set; }

        [Display(Name = "Reference")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(75, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        public string Reference { get; set; }

        [Display(Name = "Subject")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(250, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        public string Subject { get; set; }

        [Display(Name = "Read?")]
        public bool IsRead { get; set; }
    }
    [Table("PP_SYSTEM_SWITCHBOARD_BODY")]
    public class SwitchBoardBody
    {
        [Key]
        public int ID { get; set; }

        public int SwitchBoardReference { get; set; }

        [Display(Name = "Remarks")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(250, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        public string Remarks { get; set; }

        [Column(TypeName = "DATETIME")]
        [Display(Name = "Date Updated")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Action By")]
        [Column(TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        public string ActionBy { get; set; }

        [Display(Name = "Department")]
        [Column(TypeName = "VARCHAR")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required.")]
        public string DepartmentCode { get; set; }

        [ForeignKey("SwitchBoardReference")]
        public virtual SwitchBoard FKSwitchBoardReference { get; set; }
    }
    public class SwitchBoardVM
    {
        [Display(Name = "Message Type")]
        public string MessageType { get; set; }

        [Display(Name = "Reference")]
        public string Reference { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Last Update")]
        public string UpdatedAt { get; set; }
    }
    public class SwitchBoardBodyVM
    {
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Date Updated")]
        public string UpdatedAt { get; set; }

        [Display(Name = "Action By")]
        public string ActionBy { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }
    }
}