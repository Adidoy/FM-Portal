using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PROC_Bidding_Projects")]
    public class BiddingProjects
    {
        public int ID { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Approved Budget")]
        [Range(0, 999999999999.99)]
        public decimal ApprovedBudget { get; set; }

        [Display(Name = "Project Cost")]
        [Range(0, 999999999999.99)]
        public decimal ProjectCost { get; set; }

        [Display(Name = "Savings")]
        [Range(0, 999999999999.99)]
        public decimal Savings { get; set; }

        [Display(Name = "Bidding Strategy")]
        public int BiddingStrategyReference { get; set; }

        [Display(Name = "Delivery of Goods (Calendar Days)")]
        public int DeliveryOfGoods { get; set; }

        [Display(Name = "Project Coordinator")]
        public string ProjectCoordinator { get; set; }

        [Display(Name = "Project Support")]
        public string ProjectSupport { get; set; }

        [Display(Name = "Project Status")]
        public string ProjectStatus { get; set; }

        [Display(Name = "Is this an Early Procurement Activity?")]
        public bool IsEPA { get; set; }

        [ForeignKey("BiddingStrategyReference")]
        public BiddingStrategies FKBiddingStrategyReference { get; set; }
    }

    [Table("PROC_Bidding_Items")]
    public class BiddingItems
    {
        public int BiddingProjectReference { get; set; }

        public int APPReference { get; set; }

        public string PAPCode { get; set; }

        public int ItemReference { get; set; }

        public int Quantity { get; set; }

        public int PurchaseOrderReference { get; set; }
    }

    public class SelectBiddingTypeVM
    {
        [Display(Name = "Bidding Type")]
        public BiddingTypes BiddingType { get; set; }

        [Display(Name = "Program/Activity/Project")]
        public string ProcurementProgram { get; set; }

        [Display(Name = "Bidding Strategy")]
        public BiddingStrategies BiddingStrategy { get; set; }
    }

    public class BiddingProjectVM
    {
        [Display(Name = "Bidding Type")]
        public string BiddingType { get; set; }

        [Display(Name = "Program Code")]
        public string ProgramCode { get; set; }

        [Display(Name = "Program/Activity/Project")]
        public string ProcurementProgram { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        public List<BiddingDetailsVM> BiddingDetails { get; set; }
    }

    public class BiddingDetailsVM
    {
        [Display(Name = "Include as Bidding Item?")]
        public bool IncludeToProject { get; set; }

        [Display(Name = "Alternative Modes of Procurement")]
        public string AlternativeModes { get; set; }

        [Display(Name = "Ref. APP")]
        public string ReferenceAPP { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "End-User")]
        public string EndUser { get; set; }

        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Approved Budget")]
        [Range(0, 999999999999.99)]
        public decimal ApprovedBudget { get; set; }
    }

    public class ProcurementProgramVM
    {
        public string Code { get; set; }
        public string ProgramName { get; set; }
    }

    public enum BiddingTypes
    {
        [Display(Name = "Program-based Bidding")]
        ProgramBased = 0,

        [Display(Name = "Project-Based Bidding")]
        ProjectBased = 1
    }

    public enum BiddingStrategies
    {
        [Display(Name = "Bulk Bidding")]
        BulkBidding = 0,

        [Display(Name = "Lot Bidding")]
        LotBidding = 1, 

        [Display(Name = "Line Item Bidding")]
        LineItemBidding = 2
    }
}