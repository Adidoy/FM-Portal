using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PROP_MSTR_Supplies")]
    public class Supplies
    {
        private string _description = string.Empty;
        private string _shortSpecs = string.Empty;

        [Key]
        public int ID { get; set; }

        [Display(Name = "Stock No.")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(50, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(ErrorMessage = "{0} field must be filled out.")]
        public string StockNumber { get; set; }

        [Display(Name = "Article")]
        public int ArticleReference { get; set; }

        [Required]
        [MaxLength(2)]
        public string Sequence { get; set; }

        [Required]
        [MaxLength(2)]
        public string StockSequence { get; set; }

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

        [Display(Name = "Re-order Point")]
        public int ReOrderPoint { get; set; }

        [Display(Name = "Issuance Unit")]
        public int? IndividualUOMReference { get; set; }

        [Required]
        [Display(Name = "Minimum Issuance Quantity")]
        public int MinimumIssuanceQty { get; set; }

        [Required]
        [Display(Name = "Is Deleted?")]
        public bool PurgeFlag { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

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
    [Table("PROP_MSTR_StockCards")]
    public class StockCard
    {
        [Key]
        public int ID { get; set; }

        public int SupplyID { get; set; }

        public DateTime Date { get; set; }

        public string Organization { get; set; }

        public string Reference { get; set; }

        [Display(Name = "Fund Source")]
        public string FundSource { get; set; }

        [Display(Name = "Reference Type")]
        public ReferenceTypes ReferenceType { get; set; }

        [Display(Name = "Receipt Qty")]
        public int ReceiptQty { get; set; }

        [Display(Name = "Receipt Unit Cost")]
        public decimal ReceiptUnitCost { get; set; }

        [Display(Name = "Receipt Total Cost")]
        public decimal ReceiptTotalCost { get; set; }

        [Display(Name = "Issue Qty")]
        public int IssuedQty { get; set; }

        [Display(Name = "Issued Unit Cost")]
        public decimal IssuedUnitCost { get; set; }

        [Display(Name = "Receipt Total Cost")]
        public decimal IssuedTotalCost { get; set; }

        [Display(Name = "Balance Qty")]
        public int BalanceQty { get; set; }

        [Display(Name = "Balance Unit Cost")]
        public decimal BalanceUnitCost { get; set; }

        [Display(Name = "Balance Total Cost")]
        public decimal BalanceTotalCost { get; set; }

        [Display(Name = "Stock")]
        [ForeignKey("SupplyID")]
        public virtual Supplies FKSupplyReference { get; set; }
    }

    public class StockCardVM
    {
        [Display(Name = "Entity Name")]
        public string EntityName { get; set; }

        [Display(Name = "Fund Cluster")]
        public string FundCluster { get; set; }

        [Display(Name = "Stock No.")]
        public string StockNo { get; set; }

        [Display(Name = "Item")]
        public string Item { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Unit of Measurement")]
        public string UnitOfMeasurement { get; set; }

        [Display(Name = "Re-Order Point")]
        public int ReorderPoint { get; set; }

        public List<StockCardEntriesVM> Entries { get; set; }
    }
    public class StockCardEntriesVM
    {
        [Display(Name = "Date")]
        public string Date { get; set; }

        [Display(Name = "Reference")]
        public string Reference { get; set; }

        [Display(Name = "Receipt Qty.")]
        public int ReceiptQty { get; set; }

        [Display(Name = "Issue Qty.")]
        public int IssueQty { get; set; }

        [Display(Name = "Issue Office")]
        public string IssueOffice { get; set; }

        [Display(Name = "Balace Qty.")]
        public int BalanceQty { get; set; }

        [Display(Name = "No. of Days to Consume")]
        public int DaysToConsume { get; set; }
    }
    public class SuppliesLedgerCardVM
    {
        [Display(Name = "Entity Name")]
        public string EntityName { get; set; }

        [Display(Name = "Fund Cluster")]
        public string FundCluster { get; set; }

        [Display(Name = "Stock No.")]
        public string StockNo { get; set; }

        [Display(Name = "Item")]
        public string Item { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Unit of Measurement")]
        public string UnitOfMeasurement { get; set; }

        [Display(Name = "Re-Order Point")]
        public int ReorderPoint { get; set; }

        public List<SuppliesLedgerCardEntriesVM> Entries { get; set; }
    }
    public class SuppliesLedgerCardEntriesVM
    {
        [Display(Name = "Date")]
        public string Date { get; set; }

        [Display(Name = "Reference")]
        public string Reference { get; set; }

        [Display(Name = "Receipt Qty.")]
        public int ReceiptQty { get; set; }

        [Display(Name = "Receipt Unit Cost")]
        public decimal ReceiptUnitCost { get; set; }

        [Display(Name = "Receipt Total Cost")]
        public decimal ReceiptTotalCost { get; set; }

        [Display(Name = "Issue Qty.")]
        public int IssueQty { get; set; }

        [Display(Name = "Issue Unit Cost")]
        public decimal IssueUnitCost { get; set; }

        [Display(Name = "Issue Total Cost")]
        public decimal IssueTotalCost { get; set; }

        [Display(Name = "Balace Qty.")]
        public int BalanceQty { get; set; }

        [Display(Name = "Balance Unit Cost")]
        public decimal BalanceUnitCost { get; set; }

        [Display(Name = "Balance Total Cost")]
        public decimal BalanceTotalCost { get; set; }

        [Display(Name = "No. of Days to Consume")]
        public int DaysToConsume { get; set; }
    }


    public class StockCardDashboardVM
    {
        public List<Supplies> SuppliesList { get; set; }
    }

    public enum ReferenceTypes
    {
        [Display(Name = "Inspection and Acceptance")]
        InspectionAndAcceptance,

        [Display(Name = "Requisition and Issuance")]
        RequisitionAndIssuance,

        [Display(Name = "Adjustment - Receipt")]
        ReceiptAdjustment,

        [Display(Name = "Adjustment - Issuance")]
        IssuanceAdjustment,

        [Display(Name = "Correction of Entry")]
        CorrectionOfEntry,

        [Display(Name = "Beginning Balance")]
        BeginningBalance
    }
}