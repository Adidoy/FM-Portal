using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using System.Linq;


namespace PUPFMIS.Models
{
    [Table("PROC_TRXN_Procurement_Project_Mgmt_Plan_Deadlines")]
    public class PPMPDeadlines
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Fiscal Year")]
        [Required(ErrorMessage = "Please enter the Fiscal Year for which the PPMP shall be implemented.")]
        [MaxLength(4)]
        [MinLength(4)]
        public string FiscalYear { get; set; }

        [Display(Name = "Start Date")]
        [Required(ErrorMessage = "Please enter the Start Date of PPMP Submission.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Closing Date")]
        [Required(ErrorMessage = "Please enter the Closing Date of PPMP Submission.")]
        public DateTime ClosingDate { get; set; }

        [Required]
        [Display(Name = "Status")]
        [MaxLength(20)]
        public string Status { get; set; }

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

    public class PPMPDeadlinesValidation : AbstractValidator<PPMPDeadlines>
    {
        private FMISDbContext db = new FMISDbContext();
        public PPMPDeadlinesValidation()
        {
            RuleFor(x => x.FiscalYear).Must(NotBeExisting).WithMessage("Fiscal Year already exists in the database.");
        }

        public bool NotBeExisting(string FiscalYear)
        {
            return (db.PPMPDeadlines.Where(x => x.FiscalYear == FiscalYear && x.PurgeFlag == false).Count() == 0) ? true : false;
        }
    }
}