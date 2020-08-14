using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace PUPFMIS.Models
{
    [Table("PP_ACCOUNTS_USER_ACCOUNTS")]
    public class UserAccounts
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "{0} is a required field.", AllowEmptyStrings = false)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is a required field.", AllowEmptyStrings = false)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Employee Code")]
        public string EmpCode { get; set; }

        [Required(ErrorMessage = "{0} is a required field.", AllowEmptyStrings = false)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Display(Name = "User Role")]
        public int? RoleReference { get; set; }

        [Display(Name = "Is Locked Out?")]
        public bool IsLockedOut { get; set; }

        [Display(Name = "Lockout Duration")]
        public DateTime? LockoutDuration { get; set; }

        [Required]
        [Display(Name = "Is Active User?")]
        public bool PurgeFlag { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("RoleReference")]
        public virtual Roles FKRoleReference { get; set; }
    }
    public class UsersVM
    {
        public int UserID { get; set; }

        public string EmpCode { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Employee Name")]
        public string Employee { get; set; }

        [Display(Name = "Office Code")]
        public string OfficeCode { get; set; }

        [Display(Name = "Office")]
        public string Office { get; set; }

        [MaxLength(75)]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [MaxLength(75)]
        [Display(Name = "User Role")]
        public string UserRole { get; set; }
    }
    public class LoginVM
    {
        [MaxLength(75)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is a required field.", AllowEmptyStrings = false)]
        public string Email { get; set; }

        [MaxLength(75)]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is a required field.", AllowEmptyStrings = false)]
        public string Password { get; set; }

        public int NoOfAttempts { get; set; }

        public string ReturnUrl { get; set; }
    }
    public class UserAccountsValidator : AbstractValidator<UsersVM>
    {
        private FMISDbContext db = new FMISDbContext();

        public UserAccountsValidator()
        {
            RuleFor(x => x.Email).Must(NotBePurged).WithMessage("Email is already taken.");
            RuleFor(x => x.Email).Must(BeUniqueEmail).WithMessage("Email is already taken.");
            RuleFor(x => x.EmpCode).Must(BeUniqueUser).WithMessage("User already exists in the database.");
        }
        public bool BeUniqueEmail(string Email)
        {
            return (db.UserAccounts.Where(d => d.Email == Email).Count() == 0) ? true : false;
        }
        public bool BeUniqueUser(string EmpCode)
        {
            return (db.UserAccounts.Where(d => d.EmpCode == EmpCode).Count() == 0) ? true : false;
        }
        public bool NotBePurged(string Email)
        {
            return (db.UserAccounts.Where(d => d.Email == Email && d.PurgeFlag == true).Count() == 0) ? true : false;
        }
    }
    public class EmployeeListVM
    {
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
    }
    public class DepartmentListVM
    {
        public string DepartmentCode { get; set; }
        public string Department { get; set; }
    }
}