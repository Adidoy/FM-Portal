using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROC_MSTR_Item_Classification")]
    public class ItemClassification
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(75)]
        [Display(Name = "General Class")]
        public string GeneralClass { get; set; }

        [Required]
        [MaxLength(75)]
        [Display(Name = "Classification")]
        public string Classification { get; set; }

        [Required]
        [MaxLength(75)]
        [Display(Name = "Project Prefix")]
        public string ProjectPrefix { get; set; }
    }

    [Table("PROC_MSTR_Item_Types")]
    public class ItemTypes
    {
        private string _itemType;
        private string _itemTypeCode;

        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(4)]
        [Display(Name = "Item Type Code")]
        public string ItemTypeCode { get { return _itemTypeCode; } set { _itemTypeCode = value == null ? null : value.ToUpper(); } }

        [Required]
        [MaxLength(150)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Item Type")]
        public string ItemType { get { return _itemType; } set { _itemType = value == null ? null : value.ToUpper(); } }

        [MaxLength(30)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Responsibility Center")]
        public string ResponsibilityCenter { get; set; }

        [MaxLength(30)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Purchase Request Center")]
        public string PurchaseRequestCenter { get; set; }

        public int ItemClassificationReference { get; set; }

        [Required]
        [Display(Name = "Is Deleted?")]
        public bool PurgeFlag { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("ItemClassificationReference")]
        public virtual ItemClassification FKItemClassificationReference { get; set; }
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

    [Table("PROC_MSTR_Item_Articles")]
    public class ItemArticles
    {
        private string _articleName = string.Empty;

        [Key]
        public int ID { get; set; }

        [MaxLength(30)]
        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Article Code")]
        [Required(ErrorMessage = "{0} field must be filled out.", AllowEmptyStrings = false)]
        public string ArticleCode { get; set; }

        [Display(Name = "Article Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(150, ErrorMessage = "{0} is up to {1} characters only.")]
        [Required(ErrorMessage = "{0} field must be filled out.", AllowEmptyStrings = false)]
        public string ArticleName { get { return _articleName; } set { _articleName = value.ToUpper(); } }

        [Required]
        [Display(Name = "Item Type")]
        public int ItemTypeReference { get; set; }

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

        [ForeignKey("ItemTypeReference")]
        public virtual ItemTypes FKItemTypeReference { get; set; }
    }

    [Table("PROC_MSTR_Item")]
    public class Item
    {
        private string _itemCode = string.Empty;
        private string _itemFullName = string.Empty;
        private string _shortSpecs = string.Empty;
        private string _sequence = string.Empty;

        [Key]
        public int ID { get; set; }

        [NotMapped]
        [Display(Name = "Item Code")]
        public string ItemCode
        {
            get { return this.FKArticleReference.ArticleCode + "-" + this.Sequence; }
        }

        [Required]
        [MaxLength(2)]
        public string Sequence { get; set; }

        [Display(Name = "Article")]
        public int ArticleReference { get; set; }

        [Display(Name = "Full Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ItemFullName
        {
            get { return _itemFullName.ToUpper(); }
            set { _itemFullName = value.ToUpper(); }
        }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Short Specifications")]
        [MaxLength(100, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ItemShortSpecifications { get { return _shortSpecs; } set { _shortSpecs = value.ToUpper(); } }

        [Required]
        [Display(Name = "Item Specification is set by End-User?")]
        public bool IsSpecsUserDefined { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Category")]
        public int CategoryReference { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Required]
        [Display(Name = "Packaging Unit")]
        public int PackagingUOMReference { get; set; }

        [Required]
        [Display(Name = "Individual Unit")]
        public int IndividualUOMReference { get; set; }

        [Required]
        [Display(Name = "Quantity per Package")]
        public int QuantityPerPackage { get; set; }

        [Required]
        [Display(Name = "Minimum Issuance Quantity")]
        public int MinimumIssuanceQty { get; set; }

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

        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKArticleReference { get; set; }

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

    [Table("PROC_MSTR_Item_Price_Catalogue")]
    public class ItemPrice
    {
        [Key]
        public int ID { get; set; }

        public int Item { get; set; }

        [Display(Name = "Unit Price")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Display(Name = "Is Prevailing Price?")]
        public bool IsPrevailingPrice { get; set; }

        [Column(TypeName = "datetime2")]
        [Display(Name = "Effectivity Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public DateTime EffectivityDate { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("Item")]
        [Display(Name = "Item")]
        public virtual Item FKItemReference { get; set; }
    }

    public class ItemVM
    {
        public int ID { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Article")]
        public int Article { get; set; }

        [Display(Name = "Item Type")]
        public string ItemType { get; set; }
        
        [Display(Name = "UACS Object Class")]
        public string AccountClass { get; set; }

        [Display(Name = "Classification")]
        public string Classification { get; set; }

        [Display(Name = "Category")]
        public int Category { get; set; }

        [Display(Name = "Full Name")]
        public string ItemFullName { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Short Specifications")]
        public string ItemShortSpecifications { get; set; }

        [Required]
        [Display(Name = "Item Specification is set by End-User?")]
        public bool IsSpecsUserDefined { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Responsibility Center")]
        public string ResponsibilityCenter { get; set; }

        [Display(Name = "Purchase Request Office")]
        public string PurchaseRequestOffice { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "Packaging Unit")]
        public int PackagingUOMReference { get; set; }

        [Display(Name = "Individual Unit")]
        public int IndividualUOMReference { get; set; }

        [Display(Name = "Unit Price")]
        public decimal? UnitPrice { get; set; }

        [Display(Name = "Quantity per Package")]
        public int QuantityPerPackage { get; set; }

        [Display(Name = "Minimum Issuance Quantity")]
        public int MinimumIssuanceQty { get; set; }

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

        public virtual ItemArticles ArticleInformation { get; set; }
        public virtual ItemCategory CategoryInfomation { get; set; }
        public virtual UnitOfMeasure IndividualUnitInformation { get; set; }
        public virtual UnitOfMeasure PackagingUnitInformation { get; set; }
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
    public class ItemTypeValidator : AbstractValidator<ItemTypes>
    {
        FMISDbContext db = new FMISDbContext();
        public ItemTypeValidator()
        {
            RuleFor(x => x.ItemTypeCode).Must(BeUniqueCode).WithMessage("Item Type Code already exists in the system's database.");
            RuleFor(x => x.ItemType).Must(NotBeDeleted).WithMessage("Item Type was recently deleted. If you want to use the unit name, please restore the record.");
            RuleFor(x => x.ItemType).Must(BeUnique).WithMessage("Item Type Name already exists in the system's database.");
        }

        public bool BeUnique(string ItemTypeName)
        {
            return (db.ItemTypes.Where(x => x.ItemType == ItemTypeName).Count() == 0) ? true : false;
        }

        public bool BeUniqueCode(string ItemTypeCode)
        {
            return (db.ItemTypes.Where(x => x.ItemTypeCode == ItemTypeCode).Count() == 0) ? true : false;
        }

        public bool NotBeDeleted(string ItemTypeName)
        {
            return (db.ItemTypes.Where(x => x.ItemType == ItemTypeName && x.PurgeFlag == true).Count() == 0) ? true : false;
        }
    }
    public class ItemArticleValidator : AbstractValidator<ItemArticles>
    {
        FMISDbContext db = new FMISDbContext();
        public ItemArticleValidator()
        {
            RuleFor(x => x.ArticleCode).Must(BeUniqueCode).WithMessage("Article Code already exists in the system's database.");
            RuleFor(x => x.ArticleName).Must(NotBeDeleted).WithMessage("Article Name was recently deleted. If you want to use the unit name, please restore the record.");
            RuleFor(x => x.ArticleName).Must(BeUnique).WithMessage("Article Name already exists in the system's database.");
        }

        public bool BeUnique(string ArticleName)
        {
            return (db.ItemArticles.Where(x => x.ArticleName == ArticleName).Count() == 0) ? true : false;
        }

        public bool BeUniqueCode(string ArticleCode)
        {
            return (db.ItemArticles.Where(x => x.ArticleCode == ArticleCode).Count() == 0) ? true : false;
        }

        public bool NotBeDeleted(string ArticleName)
        {
            return (db.ItemArticles.Where(x => x.ArticleName == ArticleName && x.PurgeFlag == true).Count() == 0) ? true : false;
        }
    }
    public class ItemValidator : AbstractValidator<ItemVM>
    {
        public ItemValidator()
        {
            RuleFor(x => x.ItemShortSpecifications).NotEmpty().WithMessage("Item Short Specification field must be filled out.");
            RuleFor(x => x.QuantityPerPackage).NotEmpty().WithMessage("Quanity per Package field must be filled out.");
            RuleFor(x => x.MinimumIssuanceQty).NotEmpty().WithMessage("Minimum Issuance Quantity field must be filled out.");
        }
    }
}