using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROC_MSTR_Inventory_Types")]
    public class InventoryType
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Inventory Code")]
        public string InventoryCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} must be up to {1} characters only.")]
        [Display(Name = "Inventory Type")]
        public string InventoryTypeName { get; set; }

        [Required]
        [Display(Name = "Is Tangible?")]
        public bool IsTangible { get; set; }

        [Display(Name = "Default Responsibility Center")]
        public string ResponsibilityCenter { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }
    }

    [Table("PROC_MSTR_Item_Categories")]
    public class ItemCategory
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(150)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Item Category Name")]
        public string ItemCategoryName { get; set; }

        [Required]
        [Display(Name = "Is Deleted?")]
        public Boolean PurgeFlag { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }
    }

    [Table("PROC_MSTR_Item_Type")]
    public class ItemType
    {
        private string _itemTypeName = String.Empty;

        [Key]
        public int ID { get; set; }

        [Display(Name = "Item Type Code")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(4, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(ErrorMessage = "{0} field must be filled out.", AllowEmptyStrings = false)]
        public string ItemTypeCode { get; set; }

        [Display(Name = "Item Type")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(75, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(ErrorMessage = "{0} field must be filled out.", AllowEmptyStrings = false)]
        public string ItemTypeName { get { return _itemTypeName; } set { _itemTypeName = value.ToUpper(); } }

        [Required]
        [Display(Name = "Item Specification for this type is user-defined")]
        public bool IsSpecificationUserDefined { get; set; }

        [Required]
        [Display(Name = "Inventory Type")]
        public int InventoryTypeReference { get; set; }

        [MaxLength(20)]
        [Display(Name = "UACS Object Class")]
        public string UACSObjectClass { get; set; }

        [MaxLength(5)]
        [Display(Name = "General Ledger Account")]
        public string GLAccount { get; set; }

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

        [ForeignKey("InventoryTypeReference")]
        public virtual InventoryType FKInventoryTypeReference { get; set; }
    }

    [Table("PROC_MSTR_Item")]
    public class Item
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Item Code")]
        [MaxLength(30, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Type")]
        public int ItemType { get; set; }

        [Display(Name = "Full Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ItemFullName { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Short Specifications")]
        [MaxLength(100, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ItemShortSpecifications { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Category")]
        public int CategoryReference { get; set; }

        [Display(Name = "Responsibility Center")]
        public string ResponsibilityCenter { get; set; }

        [Display(Name = "Purchase Request Office")]
        public string PurchaseRequestOffice { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "Packaging Unit")]
        public int? PackagingUOMReference { get; set; }

        [Display(Name = "Individual Unit")]
        public int? IndividualUOMReference { get; set; }

        [Display(Name = "Quantity per Package")]
        public int? QuantityPerPackage { get; set; }

        [Display(Name = "Minimum Issuance Quantity")]
        public int? MinimumIssuanceQty { get; set; }

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

        [Display(Name = "Packaging Unit")]
        [ForeignKey("PackagingUOMReference")]
        public virtual UnitOfMeasure FKPackagingUnitReference { get; set; }

        [ForeignKey("IndividualUOMReference")]
        [Display(Name = "Individual Unit of Measure")]
        public virtual UnitOfMeasure FKIndividualUnitReference { get; set; }

        [ForeignKey("ItemType")]
        public virtual ItemType FKItemTypeReference { get; set; }

        [ForeignKey("CategoryReference")]
        public virtual ItemCategory FKCategoryReference { get; set; }
    }

    [Table("PROC_MSTR_Item_Allowed_Users")]
    public class ItemAllowedUsers
    {
        [Key, Column(Order = 0)]
        public int ItemReference { get; set; }

        [Key, Column(Order = 1)]
        public string DepartmentReference { get; set; }

        [ForeignKey("ItemReference")]
        public virtual Item FKItemReference { get; set; }
    }

    [Table("PROC_MSTR_Item_Prices")]
    public class ItemPrice
    {
        [Key]
        public int ID { get; set; }

        public int Item { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Effectivity Date")]
        [Column(TypeName = "datetime2")]
        public DateTime EffectivityDate { get; set; }

        [Display(Name = "Item")]
        [ForeignKey("Item")]
        public virtual Item FKItemReference { get; set; }

        [Required]
        [Display(Name = "Is Prevailing Price?")]
        public bool IsPrevailingPrice { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }
    }
    
    public class ItemTypeVM
    {
        public int ID { get; set; }

        [Display(Name = "Item Specification for this type is user-defined")]
        public bool IsSpecificationUserDefined { get; set; }

        [Display(Name = "Item Code")]
        public string ItemTypeCode { get; set; }

        [Display(Name = "Item Type")]
        public string ItemTypeName { get; set; }

        public int InventoryTypeReference { get; set; }

        [Display(Name = "Inventory Type")]
        public string InventoryTypeName { get; set; }

        [Display(Name = "UACS Object Class")]
        public string UACSObjectClass { get; set; }

        [Display(Name = "Responsibility Center")]
        public string ResponsibilityCenter { get; set; }

        [Display(Name = "Is Tangible?")]
        public string IsTangible { get; set; }

        [Display(Name = "Is Deleted?")]
        public bool PurgeFlag { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }
    }

    public class ItemVM
    {
        public int ID { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Type")]
        public string ItemType { get; set; }

        [Display(Name = "Inventory Type")]
        public string InventoryType { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Full Name")]
        public string ItemFullName { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Short Specifications")]
        public string ItemShortSpecifications { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }
        
        [Display(Name = "UACS Object Class")]
        public string AccountClass { get; set; }

        [Display(Name = "Is Tangible?")]
        public string IsTangible { get; set; }

        [Display(Name = "Responsibility Center")]
        public string ResponsibilityCenter { get; set; }

        [Display(Name = "Purchase Request Office")]
        public string PurchaseRequestOffice { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "Packaging Unit")]
        public string PackagingUOMReference { get; set; }

        [Display(Name = "Individual Unit")]
        public string IndividualUOMReference { get; set; }

        [Display(Name = "Unit Price")]
        public decimal? UnitPrice { get; set; }

        [Display(Name = "Quantity per Package")]
        public int? QuantityPerPackage { get; set; }

        [Display(Name = "Minimum Issuance Quantity")]
        public int? MinimumIssuanceQty { get; set; }

        [Display(Name = "Is Deleted?")]
        public string PurgeFlag { get; set; }

        [Display(Name = "Date Created")]
        public string CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public string UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public string DeletedAt { get; set; }

        [Display(Name = "Allowed Users")]
        public List<string> AllowedUsers { get; set; }
    }

    public class ItemCategoryValidator : AbstractValidator<ItemCategory>
    {
        FMISDbContext db = new FMISDbContext();
        public ItemCategoryValidator()
        {
            RuleFor(x => x.ItemCategoryName).Must(NotBeDeleted).WithMessage("Item category was recently deleted. If you want to use the unit name, please restore the record.");
            RuleFor(x => x.ItemCategoryName).Must(BeUnique).WithMessage("ITem category already exists in the system's database.");
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

    public class ItemTypeValidator : AbstractValidator<ItemTypeVM>
    {
        FMISDbContext db = new FMISDbContext();
        public ItemTypeValidator()
        {
            RuleFor(x => x.ItemTypeName).Must(NotBeDeleted).WithMessage("Item Type was recently deleted. If you want to use the unit name, please restore the record.");
            RuleFor(x => x.ItemTypeName).Must(BeUnique).WithMessage("Item Type Name already exists in the system's database.");
        }

        public bool BeUnique(string ItemTypeName)
        {
            return (db.ItemTypes.Where(x => x.ItemTypeName == ItemTypeName).Count() == 0) ? true : false;
        }

        public bool NotBeDeleted(string ItemTypeName)
        {
            return (db.ItemTypes.Where(x => x.ItemTypeName == ItemTypeName && x.PurgeFlag == true).Count() == 0) ? true : false;
        }
    }

    public class ItemValidator : AbstractValidator<ItemVM>
    {
        public ItemValidator()
        {
            RuleFor(x => x.ItemShortSpecifications).NotEmpty().WithMessage("Item Short Specification field must be filled out.");
            RuleFor(x => x.QuantityPerPackage).NotEmpty().WithMessage("Quanity per Package field must be filled out.");
            RuleFor(x => x.MinimumIssuanceQty).NotEmpty().WithMessage("Minimum Issuance Quantity field must be filled out.");
            RuleFor(x => x.ItemSpecifications).NotEmpty().WithMessage("Full Specification field must be filled out.");
        }
    }
}