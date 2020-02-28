using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using System.Linq;
using System.Collections.Generic;

namespace PUPFMIS.Models
{
    [Table("planning_endUserProjectHeader")]
    public class EndUserProjectHeader
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

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Purpose")]
        [DataType(DataType.MultilineText)]
        public string Purpose { get; set; }

        [Display(Name = "Project Status")]
        public string ProjectStatus { get; set; }

        [Required]
        [Display(Name = "Office")]
        public int Office { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        [Display(Name = "Project Implementation Start")]
        public DateTime ProjectStart { get; set; }

        [Display(Name = "Project Estimated Budget")]
        public decimal? ProjectEstimatedBudget { get; set; }

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

    [Table("planning_endUserProjectItems")]
    public class EndUserProjectItems
    {
        [Key]
        public int ID { get; set; }

        public int ProjectReference { get; set; }

        public int ItemReference { get; set; }

        [Required]
        public int Qtr1 { get; set; }

        [Required]
        public int Qtr2 { get; set; }

        [Required]
        public int Qtr3 { get; set; }

        [Required]
        public int Qtr4 { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? EstimatedBudget { get; set; }

        [Display(Name = "Item Specifications")]
        [ForeignKey("ItemReference")]
        public virtual Item FKItemReference { get; set; }

        [ForeignKey("ProjectReference")]
        public virtual EndUserProjectHeader FKEndUserProjectHeaderReference { get; set; }
    }

    public class EndUserProjectHeaderViewModel
    {
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

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(175, ErrorMessage = "{0} is up to {1} characters only.")]
        [Display(Name = "Purpose")]
        [DataType(DataType.MultilineText)]
        public string Purpose { get; set; }

        [Display(Name = "Project Status")]
        public string ProjectStatus { get; set; }

        [Required]
        [Display(Name = "Office")]
        public string Office { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        [Display(Name = "Project Implementation Start")]
        public DateTime ProjectStart { get; set; }

        [Display(Name = "Project Estimated Budget")]
        public decimal? ProjectEstimatedBudget { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }
    }

    public class EndUserProjectViewModel
    {
        public EndUserProjectViewModel()
        {
            Header = new EndUserProjectHeaderViewModel();
            Items = new List<EndUserProjectItems>();
        }

        public EndUserProjectHeaderViewModel Header { get; set; }
        public List<EndUserProjectItems> Items { get; set; }
    }

    //public class EndUserProjectValidator : AbstractValidator<EndUserProjectHeader>
    //{

    //    FMISDbContext db = new FMISDbContext();

    //    public EndUserProjectValidator()
    //    {
    //        RuleFor(x => x.ProjectName).Must(NotBeDeleted).WithMessage("Project name was recently deleted. If you want to use the unit name, please restore the record.");
    //        RuleFor(x => x.ProjectName).Must(BeUnique).WithMessage("Project name already exists in the system's database.");
    //    }
    //    public bool BeUnique(string ProjectName)
    //    {
    //        return (db.EndUserProjects.Where(x => x.ProjectName == ProjectName).Count() == 0) ? true : false;
    //    }

    //    public bool NotBeDeleted(string ProjectName)
    //    {
    //        return (db.EndUserProjects.Where(x => x.ProjectName == ProjectName && x.PurgeFlag == true).Count() == 0) ? true : false;
    //    }
    //}
}