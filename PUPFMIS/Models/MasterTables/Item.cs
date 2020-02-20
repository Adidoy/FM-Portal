using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("master_itemCategory")]
    public class ItemCategory
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Item Category Name")]
        [MaxLength(150)]
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

    [Table("master_items")]
    public class Item
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only")]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Short Specifications")]
        public string ItemShortSpecifications { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Full Specifications")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "Inventory Type")]
        public int InventoryTypeReference { get; set; }

        [Display(Name = "Item Category")]
        public int ItemCategoryReference { get; set; }

        [Display(Name = "UACS Class")]
        public string AccountClass { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Packaging UOM")]
        public int? PackagingUOMReference { get; set; }

        [Display(Name = "Individual UOM")]
        public int? IndividualUOMReference { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Quantity per Package")]
        public decimal DistributionQtyPerPack { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Minimum Issuance Quantity")]
        public decimal MinimumIssuanceQty { get; set; }

        [ForeignKey("InventoryTypeReference")]
        [Display(Name = "Inventory Type")]
        public virtual InventoryType FKInventoryTypeReference { get; set; }

        [Display(Name = "Packaging Unit")]
        [ForeignKey("PackagingUOMReference")]
        public virtual UnitOfMeasure FKPackagingUnitReference { get; set; }

        [Display(Name = "Individual Unit of Measure")]
        [ForeignKey("IndividualUOMReference")]
        public virtual UnitOfMeasure FKIndividualUnitReference { get; set; }
        
        [ForeignKey("ItemCategoryReference")]
        [Display(Name = "Item Category")]
        public virtual ItemCategory FKItemCategoryReference { get; set; }

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

    [Table("master_itemPrices")]
    public class ItemPrice
    {
        [Key]
        public int ID { get; set; }

        public int Item { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Unit Price")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
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

    [Table("master_inventoryTypes")]
    public class InventoryType
    {
        [Key]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} must be up to {1} characters only.")]
        [Display(Name = "Inventory Type")]
        public string InventoryTypeName { get; set; }

        [Required]
        [Display(Name = "Is Tangible?")]
        public bool IsTangible { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }
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
}