using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using System.Linq;

namespace PUPFMIS.Models
{
    [Table("planning_enduser_project")]
    public class EndUserProject
    {
        [Key]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(4, ErrorMessage = "{0} field must consist of {1} characters only.")]
        [MinLength(4, ErrorMessage = "{0} field must consist of {1} characters only.")]
        [Display(Name = "Fiscal Year")]
        public string FiscalYear { get; set; }

        [Display(Name = "Project Code")]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Status")]
        public string ProjectStatus { get; set; }

        [Required]
        [Display(Name = "End-User")]
        public int EndUser { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        [DisplayFormat(DataFormatString ="{0:MMMM yyyy}", ApplyFormatInEditMode =true)]
        [Display(Name = "Project Implementation Start")]
        public DateTime ProjectStart { get; set; }

        [Display(Name = "Project Estimated Budget")]
        public decimal? ProjectEstimatedBudget { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Purpose")]
        public string Purpose { get; set; }

        [Display(Name = "Date MS Conducted")]
        public DateTime? DateConducted { get; set; }

        public int? MSPreparedBy { get; set; }

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

    public class EndUserProjectValidator : AbstractValidator<EndUserProject>
    {

        FMISDbContext db = new FMISDbContext();

        public EndUserProjectValidator()
        {
            RuleFor(x => x.ProjectName).Must(NotBeDeleted).WithMessage("Project name was recently deleted. If you want to use the unit name, please restore the record.");
            RuleFor(x => x.ProjectName).Must(BeUnique).WithMessage("Project name already exists in the system's database.");
        }
        public bool BeUnique(string ProjectName)
        {
            return (db.EndUserProjects.Where(x => x.ProjectName == ProjectName).Count() == 0) ? true : false;
        }

        public bool NotBeDeleted(string ProjectName)
        {
            return (db.EndUserProjects.Where(x => x.ProjectName == ProjectName && x.PurgeFlag == true).Count() == 0) ? true : false;
        }
    }
}