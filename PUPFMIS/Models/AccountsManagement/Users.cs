using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace PUPFMIS.Models
{
    [Table("accounts_userAccounts")]
    public class UserAccounts
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "{0} is a required field.", AllowEmptyStrings = false)]
        [MaxLength(75)]
        [MinLength(10, ErrorMessage = "Minimum number of characters for {0} is  {1}.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is a required field.", AllowEmptyStrings = false)]
        [MaxLength(75)]
        [MinLength(10, ErrorMessage = "Minimum number of characters for {0} is  {1}.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "User")]
        public int UserInformationReference { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

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

        [Display(Name = "User Information")]
        [ForeignKey("UserInformationReference")]
        public virtual UserInformation FKUserInformationReference { get; set; }
    }

    [Table("accounts_userInformation")]
    public class UserInformation
    {
        [Key]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is a required field.")]
        [MaxLength(75)]
        [MinLength(2, ErrorMessage = "Minimum character length for {0} is {1}.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is a required field.")]
        [MaxLength(75)]
        [MinLength(2, ErrorMessage = "Minimum character length for {0} is {1}.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(75)]
        [MinLength(2, ErrorMessage = "Minimum character length for {0} is {1}.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Office")]
        public int Office { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is a required field")]
        [MaxLength(75)]
        [Display(Name = "Designation")]
        public string Designation { get; set; }
    }

    public class UsersVM
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "{0} is a required field.", AllowEmptyStrings = false)]
        [MaxLength(75)]
        [MinLength(10, ErrorMessage = "Minimum number of characters for {0} is  {1}.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is a required field.", AllowEmptyStrings = false)]
        [MaxLength(75)]
        [MinLength(10, ErrorMessage = "Minimum number of characters for {0} is  {1}.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is a required field.")]
        [MaxLength(75)]
        [MinLength(2, ErrorMessage = "Minimum character length for {0} is {1}.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is a required field.")]
        [MaxLength(75)]
        [MinLength(2, ErrorMessage = "Minimum character length for {0} is {1}.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(75)]
        [MinLength(2, ErrorMessage = "Minimum character length for {0} is {1}.")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Office")]
        public int Office { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is a required field")]
        [MaxLength(75)]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        public string Role { get; set; }
    }

    public class LoginVM
    {
        [Required(ErrorMessage = "{0} is a required field.", AllowEmptyStrings = false)]
        [MaxLength(75)]
        [MinLength(10, ErrorMessage = "Minimum number of characters for {0} is  {1}.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is a required field.", AllowEmptyStrings = false)]
        [MaxLength(75)]
        [MinLength(10, ErrorMessage = "Minimum number of characters for {0} is  {1}.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string PasswordSalt { get; set; }
    }

    public class UserAccountsValidator : AbstractValidator<UsersVM>
    {
        private FMISDbContext db = new FMISDbContext();

        public UserAccountsValidator()
        {
            RuleFor(x => x.Email).Must(NotBePurged).WithMessage("Email is already taken.");
            RuleFor(x => x.Email).Must(BeUniqueEmail).WithMessage("Email is already taken.");
            RuleFor(x => new { x.FirstName, x.MiddleName, x.LastName }).Must(x => BeUniqueUser(x.FirstName, x.MiddleName, x.LastName)).WithMessage("User already exists in the database.");
        }

        public bool BeUniqueEmail(string Email)
        {
            return (db.UserAccounts.Where(d => d.Email == Email).Count() == 0) ? true : false;
        }

        public bool BeUniqueUser(string LastName, string FirstName, string MiddleName)
        {
            return (db.UserInformation.Where(d => d.FirstName == FirstName && d.MiddleName == MiddleName && d.LastName == LastName).Count() == 0) ? true : false;
        }

        public bool NotBePurged(string Email)
        {
            return (db.UserAccounts.Where(d => d.Email == Email && d.PurgeFlag == true).Count() == 0) ? true : false;
        }
    }
}