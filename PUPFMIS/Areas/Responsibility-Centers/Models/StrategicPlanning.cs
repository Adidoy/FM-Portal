using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PUPFMIS.Models
{
    [Table("PROC_TRXN_Market_Survey")]
    public class MarketSurvey
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Article")]
        public int ArticleReference { get; set; }

        [Required]
        [MaxLength(2)]
        public string ItemSequence { get; set; }

        [Display(Name = "Item Full Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(200, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string ItemFullName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Is Item Obolete?")]
        public bool IsObsolete { get; set; }

        [Display(Name = "Supplier1")]
        public int? Supplier1Reference { get; set; }

        [Display(Name = "Supplier2")]
        public int? Supplier2Reference { get; set; }

        [Display(Name = "Supplier3")]
        public int? Supplier3Reference { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? UnitCost { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier1UnitCost { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier2UnitCost { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier3UnitCost { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreateAt { get; set; }

        [Required]
        [Display(Name = "Conducted by")]
        public string ConductedBy { get; set; }

        [Required]
        [Display(Name = "Date Conducted")]
        public DateTime LastUpdated { get; set; }

        [Required]
        [Display(Name = "Valid Until")]
        public DateTime ExpirationDate { get; set; }

        [ForeignKey("Supplier1Reference")]
        public virtual Supplier FKSupplier1Reference { get; set; }

        [ForeignKey("Supplier2Reference")]
        public virtual Supplier FKSupplier2Reference { get; set; }

        [ForeignKey("Supplier3Reference")]
        public virtual Supplier FKSupplier3Reference { get; set; }

        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKArticleReference { get; set; }
    }

    public enum MarketSurveyStatus
    {
        Lapsed,
        Updated
    }
    public class MarketSurveyDashboard
    {
        public int NoOfNewItems { get; set; }
        public int NoOfLapsedMarketSurvey { get; set; }
        public int NoOfUpdatedMarketSurvey { get; set; }
        public List<MarketSurveyVM> MarketSurveyList { get; set; }
    }
    public class MarketSurveyVM
    {
        [Display(Name = "Item Type")]
        public string ItemType { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        public bool IsSpecsUserDefined { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public string ProcurementSource { get; set; }

        [Display(Name = "Account Class")]
        public string AccountClass { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Individual UOM")]
        public string IndividualUOMReference { get; set; }

        [Display(Name = "Minimum Issuance Qty.")]
        public int MinimumIssuanceQty { get; set; }

        [Display(Name = "Status")]
        public MarketSurveyStatus Status { get; set; }

        [Display(Name = "Mark this as obsolete or off market")]
        public bool IsObsolete { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? UnitCost { get; set; }

        [Display(Name = "Supplier 1 ID")]
        public int? Supplier1ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier1Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier1Address { get; set; }

        [Display(Name = "Contact No.")]
        public string Supplier1ContactNo { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier1EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier1UnitCost { get; set; }

        [Display(Name = "Supplier 2 ID")]
        public int? Supplier2ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier2Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier2Address { get; set; }

        [Display(Name = "Contact No.")]
        public string Supplier2ContactNo { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier2EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier2UnitCost { get; set; }

        [Display(Name = "Supplier 3 ID")]
        public int? Supplier3ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier3Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier3Address { get; set; }

        [Display(Name = "Contact No.")]
        public string Supplier3ContactNo { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier3EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier3UnitCost { get; set; }

        [Required]
        [Display(Name = "Date Created")]
        public DateTime CreateAt { get; set; }

        [Required]
        [Display(Name = "Last Updated")]
        public DateTime? LastUpdated { get; set; }

        [ForeignKey("Supplier1Reference")]
        public virtual Supplier FKSupplier1Reference { get; set; }

        [ForeignKey("Supplier2Reference")]
        public virtual Supplier FKSupplier2Reference { get; set; }

        [ForeignKey("Supplier3Reference")]
        public virtual Supplier FKSupplier3Reference { get; set; }
    }
    public class InstitutionalItemDetails
    {
        public BudgetProposalType ProposalType { get; set; }

        [Display(Name = "Classification")]
        public string Classification { get; set; }

        [Display(Name = "Item Type")]
        public string ItemType { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Minimum Issuance Qty.")]
        public int MinimumIssuanceQty { get; set; }

        [Display(Name = "Procurement Source")]
        public string ProcurementSource { get; set; }

        [Display(Name = "Account Class")]
        public string AccountClass { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? UnitCost { get; set; }

        [Display(Name = "Estimated Budget")]
        public decimal? EstimatedBudget { get; set; }

        [Display(Name = "Packaging UOM")]
        public string PackagingUOMReference { get; set; }

        [Display(Name = "Individual UOM")]
        public string IndividualUOMReference { get; set; }

        [Display(Name = "Supplier 1 ID")]
        public int? Supplier1ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier1Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier1Address { get; set; }

        [Display(Name = "Contact No.")]
        public string Supplier1ContactNo { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier1EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier1UnitCost { get; set; }

        [Display(Name = "Supplier 2 ID")]
        public int? Supplier2ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier2Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier2Address { get; set; }

        [Display(Name = "Contact No.")]
        public string Supplier2ContactNo { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier2EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier2UnitCost { get; set; }

        [Display(Name = "Supplier 3 ID")]
        public int? Supplier3ID { get; set; }

        [Display(Name = "Supplier Name")]
        public string Supplier3Name { get; set; }

        [Display(Name = "Address")]
        public string Supplier3Address { get; set; }

        [Display(Name = "Contact No.")]
        public string Supplier3ContactNo { get; set; }

        [Display(Name = "Email Address")]
        public string Supplier3EmailAddress { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal? Supplier3UnitCost { get; set; }
    }
    public class StrategicProjectPlanDetails
    {
        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "Program")]
        public string Program { get; set; }

        [Display(Name = "Unit/Office")]
        public string UnitName { get; set; }

        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Project Description")]
        public string Description { get; set; }

        [Display(Name = "Justification")]
        public string Justification { get; set; }

        [Display(Name = "Delivery Month")]
        public int DeliveryMonth { get; set; }

        [Required]
        [Display(Name = "JAN")]
        public int JanQty { get; set; }

        [Required]
        [Display(Name = "FEB")]
        public int FebQty { get; set; }

        [Required]
        [Display(Name = "MAR")]
        public int MarQty { get; set; }

        [Required]
        [Display(Name = "APR")]
        public int AprQty { get; set; }

        [Required]
        [Display(Name = "MAY")]
        public int MayQty { get; set; }

        [Required]
        [Display(Name = "JUN")]
        public int JunQty { get; set; }

        [Required]
        [Display(Name = "JUL")]
        public int JulQty { get; set; }

        [Required]
        [Display(Name = "AUG")]
        public int AugQty { get; set; }

        [Required]
        [Display(Name = "SEP")]
        public int SepQty { get; set; }

        [Required]
        [Display(Name = "OCT")]
        public int OctQty { get; set; }

        [Required]
        [Display(Name = "NOV")]
        public int NovQty { get; set; }

        [Required]
        [Display(Name = "DEC")]
        public int DecQty { get; set; }

        [Required]
        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }

        [Display(Name = "Action")]
        public ResponsibilityCenterAction ResponsibilityCenterAction { get; set; }

        [Display(Name = "Reason for Revision")]
        public ResponsibilityCenterReasonForRevision ReasonForNonAcceptance { get; set; }
    }
    public class InstitutionalItemPlan
    {
        public InstitutionalItemDetails Item { get; set; }
        public List<StrategicProjectPlanDetails> ProjectPlans { get; set; }
    }
}