using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using System.Linq;
using System.Collections.Generic;

namespace PUPFMIS.Models
{
    [Table("PROC_MSTR_Suppliers")]
    public class Supplier
    {
        private string _supplierName;

        [Key]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get { return _supplierName; } set { _supplierName = value.ToUpper(); } }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(250, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Business Address")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(250, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "City/Municipality")]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(250, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "State/Country")]
        public string State { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(15, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Designation")]
        public string ContactPersonDesignation { get; set; }

        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Authorized Agent/Representative")]
        public string AuthorizedAgent { get; set; }

        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Designation")]
        public string AuthorizedDesignation { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(20, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [MaxLength(20, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Alternate Contact Number")]
        public string AlternateContactNumber { get; set; }

        [MaxLength(20, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Tax Identification Number", ShortName = "TIN")]
        public string TaxIdNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
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
    [Table("PROC_MSTR_Suppliers_Categories")]
    public class SupplierCategories
    {
        [Key, Column(Order = 0)]
        public int SupplierReference { get; set; }

        [Key, Column(Order = 1)]
        public int CategoryReference { get; set; }

        [ForeignKey("SupplierReference")]
        public virtual Supplier FKSupplierReference { get; set; }

        [ForeignKey("CategoryReference")]
        public virtual ItemCategory FKCategoryReference { get; set; }
    }
    [Table("PROC_MSTR_Suppliers_ItemTypes")]
    public class SupplierItemTypes
    {
        [Key, Column(Order = 0)]
        public int SupplierReference { get; set; }

        [Key, Column(Order = 1)]
        public int ItemTypeReference { get; set; }

        [ForeignKey("SupplierReference")]
        public virtual Supplier FKSupplierReference { get; set; }

        [ForeignKey("ItemTypeReference")]
        public virtual ItemTypes FKItemTypeReference { get; set; }
    }

    public class SupplierItemTypesVM
    {
        public bool IsSelected { get; set; }
        public int ID { get; set; }
        public string ItemType { get; set; }
    }
    public class SupplierCategoriesVM
    {
        public bool IsSelected { get; set; }
        public int ID { get; set; }
        public string Category { get; set; }
    }
    public class SupplierVM
    {
        public int ID { get; set;}

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(250, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Business Address")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(250, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "City/Municipality")]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(250, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "State/Country")]
        public string State { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(15, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Designation")]
        public string ContactPersonDesignation { get; set; }

        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Authorized Agent/Representative")]
        public string AuthorizedAgent { get; set; }

        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Designation")]
        public string AuthorizedDesignation { get; set; }

        [MaxLength(20, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [MaxLength(20, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Alternate Contact Number")]
        public string AlternateContactNumber { get; set; }

        [MaxLength(20, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Tax Identification Number", ShortName = "TIN")]
        public string TaxIdNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
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

        [Display(Name = "Item Categories")]
        public List<SupplierCategoriesVM> CategoryList { get; set; }

        [Display(Name = "Item Types")]
        public List<SupplierItemTypesVM> ItemTypesList { get; set; }
    }

    public class SupplierValidator : AbstractValidator<SupplierVM>
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