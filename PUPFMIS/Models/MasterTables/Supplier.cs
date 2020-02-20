using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using System.Linq;

namespace PUPFMIS.Models
{
    [Table("master_suppliers")]
    public class Supplier
    {
        [Key]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(100, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(20, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [MaxLength(20, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Alternate Contact Number")]
        public string AlternateContactNumber { get; set; }

        [MaxLength(20, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Tax Identification Number", ShortName = "TIN")]
        public string TaxIdNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Website")]
        public string Website { get; set; }

        [Required]
        [Display(Name = "Is Deleted?")]
        public Boolean PurgeFlag { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }
    }

    public class SupplierValidator : AbstractValidator<Supplier>
    {
        FMISDbContext db = new FMISDbContext();
        public SupplierValidator()
        {
            RuleFor(x => x.SupplierName).Must(NotBeDeleted).WithMessage("Supplier name was recently deleted. If you want to use the unit name, please restore the record.");
            RuleFor(x => x.SupplierName).Must(BeUnique).WithMessage("Supplier name already exists in the system's database.");
            RuleFor(x => x.EmailAddress).EmailAddress().WithMessage("Please enter a valid email address.");
        }

        public bool BeUnique(string SupplierName)
        {
            return (db.Suppliers.Where(x => x.SupplierName == SupplierName).Count() == 0) ? true : false;
        }

        public bool NotBeDeleted(string SupplierName)
        {
            return (db.Suppliers.Where(x => x.SupplierName == SupplierName && x.PurgeFlag == true).Count() == 0) ? true : false;
        }
    }
}