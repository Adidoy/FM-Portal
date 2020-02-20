//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.ProcurementPipeline")]
//    public class ProcurementPipeline
//    {
//        [Key]
//        public Guid ID { get; set; }

//        public int ProjectReference { get; set; }

//        [Display(Name = "Fiscal Year")]
//        [MinLength(4)]
//        [MaxLength(4)]
//        public string FiscalYear { get; set; }

//        [Display(Name = "Inventory Type")]
//        public InventoryType InventoryType { get; set; }

//        public int ExpenditureObject { get; set; }

//        public int? ItemID { get; set; }

//        [Display(Name = "Scope of Work / Terms of Reference / Specifications")]
//        public string ItemSpecifications { get; set; }

//        [Display(Name = "Project Implementation Start")]
//        public DateTime ProjectStart { get; set; }

//        public int? MarketSurveyReference { get; set; }

//        [Display(Name = "Q1 Qty")]
//        public int QtyQ1 { get; set; }

//        [Display(Name = "Q2 Qty")]
//        public int QtyQ2 { get; set; }

//        [Display(Name = "Q3 Qty")]
//        public int QtyQ3 { get; set; }

//        [Display(Name = "Q4 Qty")]
//        public int QtyQ4 { get; set; }

//        [Display(Name = "Estimated Budget")]
//        public decimal EstimatedBudget { get; set; }
        
//        [Display(Name = "Expenditure Type")]
//        public ExpendituresCategory ExpenditureType { get; set; }

//        public int? PPMPReference { get; set; }
        
//        [Display(Name = "Fund Cluster")]
//        public string FundCluster { get; set; }
        
//        [Display(Name = "Procurement Mode")]
//        public ProcurementModes ProcurementMode { get; set; }

//        public int? APPReference { get; set; }
        
//        [Display(Name = "Ads/Post of IB/REI")]
//        public DateTime APPPostingAds { get; set; }
        
//        [Display(Name = "Sub/Open of Bids")]
//        public DateTime APPOpenBids { get; set; }
        
//        [Display(Name = "Notice of Award")]
//        public DateTime APPNoticeOfAward { get; set; }
        
//        [Display(Name = "Contract Signing")]
//        public DateTime APPContractSigning { get; set; }
        
//        [Display(Name = "Purchase Request Reference")]
//        public int? PRReference { get; set; }

//        [Display(Name = "Pipeline No.")]
//        public int PipelineReference { get; set; }
        
//        [Display(Name = "Purchase Order Reference")]
//        public int? POReference { get; set; }
        
//        [Display(Name = "Contract Price")]
//        public decimal ContractPrice { get; set; }
        
//        [Display(Name = "Ads/Post of IB/REI")]
//        public DateTime ActualPostingAds { get; set; }
        
//        [Display(Name = "Sub/Open of Bids")]
//        public DateTime ActualOpenBids { get; set; }
        
//        [Display(Name = "Notice of Award")]
//        public DateTime ActualNoticeOfAward { get; set; }
        
//        [Display(Name = "Contract Signing")]
//        public DateTime ActualContractSigning { get; set; }
        
//        [Display(Name = "Invoice No.")]
//        public string InvoiceNo { get; set; }
        
//        [Display(Name = "Delivery Receipt No.")]
//        public string DeliveryReceiptNo { get; set; }
        
//        //================ FK Definition ===================//

//        [ForeignKey("ProjectReference")]
//        [Display(Name = "End-User Project Reference")]
//        public virtual EndUserProject FKEndUserProjects { get; set; }

//        [ForeignKey("ItemID")]
//        [Display(Name = "Item")]
//        public virtual ItemsMaster FKItemsMaster { get; set; }

//        [ForeignKey("ExpenditureObject")]
//        [Display(Name = "Expenditure Object")]
//        public virtual ExpenditureObjects FKExpenditureObjects { get; set; }

//        [ForeignKey("MarketSurveyReference")]
//        [Display(Name = "Market Survey Reference")]
//        public virtual MarketSurvey FKMarketSurvey { get; set; }

//        [ForeignKey("PPMPReference")]
//        [Display(Name = "PPMP Reference")]
//        public virtual PPMPDetails FKPPMPReference { get; set; }

//        [ForeignKey("APPReference")]
//        [Display(Name = "APP Reference")]
//        public virtual PPMPDetails FKAPPReference { get; set; }

//        [ForeignKey("PRReference")]
//        [Display(Name = "Purchase Request Reference")]
//        public virtual PurchaseRequest FKPurchaseRequest { get; set; }

//        [ForeignKey("PipelineReference")]
//        public virtual ProcurementPipelineHeader FKPipelineHeader { get; set; }

//        [ForeignKey("POReference")]
//        [Display(Name = "Purchase Order Reference")]
//        public virtual PurchaseOrderHeader FKPurchaseOrderReference { get; set; }
//    }
//}