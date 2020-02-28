using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using System.Collections.Generic;

namespace PUPFMIS.Models
{
    [Table("procurement_ppmpheader")]
    public class PPMPHeader
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "PPMP Reference No.")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        [MaxLength(4)]
        public string FiscalYear { get; set; }

        public int? PPMPType { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime? CreatedAt { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime? SubmittedAt { get; set; }

        [Display(Name = "Date Approved")]
        public DateTime? ApprovedAt { get; set; }

        [Display(Name = "Prepared By")]
        public int? PreparedBy { get; set; }

        [Display(Name = "Submitted By")]
        public string SubmittedBy { get; set; }

        public int OfficeReference { get; set; }

        [ForeignKey("PPMPType")]
        public virtual InventoryType FKPPMPTypeReference { get; set; }
    }

    [Table("procurement_ppmpcsedetails")]
    public class PPMPCSEDetails
    {
        [Key]
        public int ID { get; set; }

        public int PPMPID { get; set; }

        public int Item { get; set; }

        [Required]
        public int Qtr1 { get; set; }

        [Required]
        public int Qtr2 { get; set; }

        [Required]
        public int Qtr3 { get; set; }

        [Required]
        public int Qtr4 { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? EstimatedBudget { get; set; }

        [Display(Name = "Is Accepted?")]
        public AcceptanceCodes? AcceptanceCode { get; set; }

        [Display(Name = "Item Specifications")]
        [ForeignKey("Item")]
        public virtual Item FKItem { get; set; }

        [ForeignKey("PPMPID")]
        public virtual PPMPHeader FKPPMPReference { get; set; }
    }

    public class PPMPHeaderViewModel
    {
        public int PPMPId { get; set; }

        [Required]
        [Display(Name = "PPMP Reference No.")]
        public string ReferenceNo { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        [MaxLength(4)]
        public string FiscalYear { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string PPMPType { get; set; }

        [Required]
        [Display(Name = "Office")]
        public string OfficeName { get; set; }

        [Display(Name = "Submitted by")]
        public string SubmittedBy { get; set; }

        [Display(Name = "Prepared by")]
        public string PreparedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Date Submitted")]
        public DateTime? SubmittedAt { get; set; }

        public string Status { get; set; }
    }

    public class PPMPCSEViewModel
    {
        public PPMPCSEViewModel()
        {
            PPMPHeader = new PPMPHeaderViewModel();
            DBMItems = new List<PPMPCSEDetails>();
            NonDBMItems = new List<PPMPCSEDetails>();
        }

        public PPMPHeaderViewModel PPMPHeader { get; set; }
        public List<PPMPCSEDetails> DBMItems { get; set; }
        public List<PPMPCSEDetails> NonDBMItems { get; set; }
    }

    public class PPMPViewModel
    {
        public PPMPViewModel()
        {
            PPMPHeader = new PPMPHeaderViewModel();
            DBMItems = new List<BasketItem>();
            NonDBMItems = new List<BasketItem>();
        }
        public PPMPHeaderViewModel PPMPHeader { get; set; }
        public List<BasketItem> DBMItems { get; set; }
        public List<BasketItem> NonDBMItems { get; set; }
    }

    public class PPMPCSE
    {
        public PPMPHeader BasketHeader { get; set; }
        public List<BasketItem> BasketItems { get; set; }
    }

    public class PPMPClientDashboard
    {
        [Display(Name = "Submitted PPMPs")]
        public int SubmittedPPMPs { get; set; }

        [Display(Name = "Items Requested")]
        public int ItemsRequested { get; set; }

        [Display(Name = "Implemented Projects")]
        public int ImplementedProjects { get; set; }

        [Display(Name = "% Implemented")]
        public decimal PercentImplemented { get; set; }
    }

    public class PPMPItemVM
    {
        public int ItemReference { get; set; }

        [Display(Name = "Q1 Qty.")]
        public int? Qtr1 { get; set; }

        [Display(Name = "Q2 Qty.")]
        public int? Qtr2 { get; set; }

        [Display(Name = "Q3 Qty.")]
        public int? Qtr3 { get; set; }

        [Display(Name = "Q4 Qty.")]
        public int? Qtr4 { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal EstimatedBudget { get; set; }

        [ForeignKey("ItemReference")]
        [Display(Name = "Item Information")]
        public virtual Item FKItemReference { get; set; }

        [Display(Name = "Remarrks/Justification")]
        [MaxLength(150)]
        public string Remarks { get; set; }
    }

    public class PPMPItemVMValidators : AbstractValidator<PPMPItemVM>
    {
        private FMISDbContext db = new FMISDbContext();

        public PPMPItemVMValidators()
        {
            RuleFor(x => new { x.Qtr1, x.ItemReference }).Must(x => BeGreaterThan(x.Qtr1, x.ItemReference)).WithMessage("Quantity specified for Qtr1 is less than the Minimum Issue Quantity.");
            RuleFor(x => new { x.Qtr2, x.ItemReference }).Must(x => BeGreaterThan(x.Qtr2, x.ItemReference)).WithMessage("Quantity specified for Qtr2 is less than the Minimum Issue Quantity.");
            RuleFor(x => new { x.Qtr3, x.ItemReference }).Must(x => BeGreaterThan(x.Qtr3, x.ItemReference)).WithMessage("Quantity specified for Qtr3 is less than the Minimum Issue Quantity.");
            RuleFor(x => new { x.Qtr4, x.ItemReference }).Must(x => BeGreaterThan(x.Qtr4, x.ItemReference)).WithMessage("Quantity specified for Qtr4 is less than the Minimum Issue Quantity.");
        }

        public bool BeGreaterThan(int? Qty, int ItemReference)
        {
            var minimumQty = db.Items.Find(ItemReference).MinimumIssuanceQty;
            if (Qty < minimumQty && !string.IsNullOrEmpty(Qty.ToString()))
            {
                return false;
            }
            return true;
        }
    }

    public enum AcceptanceCodes
    {
        [Display(Name = "Accepted")]
        Accepted = 0,
        [Display(Name = "Reduce Quantity")]
        ReduceQuantity = 1,
        [Display(Name = "Needs Justification")]
        NeedsJustification = 2
    }
}