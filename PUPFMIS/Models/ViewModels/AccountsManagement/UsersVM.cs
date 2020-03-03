using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
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
}