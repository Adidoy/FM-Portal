using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PUPFMIS.Models.HRIS;

namespace PUPFMIS.Models
{
    [Table("PROC_SYTM_Agency_Details")]
    public class AgencyDetails
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Account Code")]
        [MaxLength(15, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string AccountCode { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Organization Type")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string OrganizationType { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Department/Bureau/Office")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string AgencyName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Region")]
        [MaxLength(30, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string Region { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Address")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string Address { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string ProcurementOfficeReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string AccountingOfficeReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string BACOfficeReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string PropertyOfficeReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string HOPEReference { get; set; }
    }
}