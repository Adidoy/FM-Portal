using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PUPFMIS.Models
{
    [Table("PP_MASTER_UOM")]
    public class UnitOfMeasure
    {
        [Key]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Unit Name")]
        public string UnitName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(5, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Abbreviation")]
        public string Abbreviation { get; set; }
        
        [Required]
        [Display(Name = "Is Deleted?")]
        public bool PurgeFlag { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }
    }

    public class UnitValidator : AbstractValidator<UnitOfMeasure>
    {
        FMISDbContext db = new FMISDbContext();
        public UnitValidator()
        {
            RuleFor(x => x.UnitName).Must(NotBeDeleted).WithMessage("Unit name was recently deleted. If you want to use the unit name, please restore the record.");
            RuleFor(x => x.UnitName).Must(BeUnique).WithMessage("Unit name already exists in the system's database.");
        }

        public bool BeUnique(string UnitName)
        {
            return (db.UOM.Where(x => x.UnitName == UnitName).Count() == 0) ? true : false;
        }

        public bool NotBeDeleted(string UnitName)
        {
            return (db.UOM.Where(x => x.UnitName == UnitName && x.PurgeFlag == true).Count() == 0) ? true : false;
        }
    }
}