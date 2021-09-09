using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PROP_MSTR_PPE")]
    public class PPE
    {
        private string _description = string.Empty;

        [Key]
        public int ID { get; set; }

        [Display(Name = "Property No.")]
        public string PropertyNumberRoot { get; set; }

        [Display(Name = "Article")]
        public int ArticleReference { get; set; }

        [Required]
        [MaxLength(2)]
        public string Sequence { get; set; }

        [Display(Name = "Full Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string Description
        {
            get { return _description.ToUpper(); }
            set { _description = value.ToUpper(); }
        }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Issuance Unit")]
        public int? IndividualUOMReference { get; set; }

        [Required]
        [Display(Name = "Is Deleted?")]
        public bool PurgeFlag { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("IndividualUOMReference")]
        [Display(Name = "Individual Unit of Measure")]
        public virtual UnitOfMeasure FKIndividualUnitReference { get; set; }

        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKArticleReference { get; set; }
    }
    [Table("PROP_MSTR_PropertyCards")]
    public class PropertyCard
    {
        [Key]
        public int ID { get; set; }

        public int PropertyReference { get; set; }

        public DateTime Date { get; set; }

        public string Organization { get; set; }

        public string Reference { get; set; }

        [Display(Name = "Reference Type")]
        public ReferenceTypes ReferenceType { get; set; }

        [Display(Name = "Receipt Qty")]
        public int ReceiptQty { get; set; }

        [Display(Name = "Receipt Unit Cost")]
        public decimal ReceiptUnitCost { get; set; }

        [Display(Name = "Issue Qty")]
        public int IssueQty { get; set; }

        [Display(Name = "Issue Unit Cost")]
        public decimal IssueUnitCost { get; set; }

        [Display(Name = "Balance Qty")]
        public int BalanceQty { get; set; }

        [Display(Name = "Balance Unit Cost")]
        public int BalanceUnitCost { get; set; }

        [ForeignKey("PropertyReference")]
        public virtual PPE FKPropertyReference { get; set; }
    }
}