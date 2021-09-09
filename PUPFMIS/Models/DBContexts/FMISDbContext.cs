using System.Data.Entity;

namespace PUPFMIS.Models
{
    public class FMISDbContext : DbContext
    {
        public FMISDbContext() : base("FMISDbContext") { }

        public static FMISDbContext Create()
        {
            return new FMISDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasRequired<UnitOfMeasure>(s => s.FKIndividualUnitReference)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasRequired<UnitOfMeasure>(s => s.FKPackagingUnitReference)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectDetails>()
                .HasRequired<UnitOfMeasure>(s => s.FKUOMReference)
                .WithMany()
                .WillCascadeOnDelete(false);
        }

        //===============================================================//
        //====================== SYSTEM TABLES ==========================//
        //===============================================================//
        public DbSet<AgencyDetails> AgencyDetails { get; set; }
        public DbSet<BACSecretariat> BACSecretariat { get; set; }
        public DbSet<PPMPDeadlines> PPMPDeadlines { get; set; }
        public DbSet<SwitchBoard> SwitchBoard { get; set; }
        public DbSet<SwitchBoardBody> SwitchBoardBody { get; set; }

        //===============================================================//
        //================ PROCUREMENT MASTER TABLES ====================//
        //===============================================================//
        public DbSet<InfrastructureRequirementsClassification> InfrastructureRequirementsClass { get; set; }
        public DbSet<InfrastructureRequirements> InfrastructureRequirements { get; set; }
        public DbSet<InfrastructureMaterials> InfrastructureMaterials { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemAllowedUsers> ItemAllowedUsers { get; set; }
        public DbSet<ItemArticles> ItemArticles { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemClassification> ItemClassification { get; set; }
        public DbSet<ItemPrice> ItemPrices { get; set; }
        public DbSet<ItemTypes> ItemTypes { get; set; }
        public DbSet<ModesOfProcurement> ProcurementModes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierCategories> SupplierCategories { get; set; }
        public DbSet<SupplierItemTypes> SupplierItemTypes { get; set; }
        public DbSet<UnitOfMeasure> UOM { get; set; }

        //===============================================================//
        //============== PROCUREMENT TRANSACTION TABLES =================//
        //===============================================================//
        public DbSet<AgencyProcurementRequest> APRHeader { get; set; }
        public DbSet<AgencyProcurementRequestDetails> APRDetail { get; set; }
        public DbSet<APPCSEDetails> APPCSEDetails { get; set; }
        public DbSet<AnnualProcurementPlan> APPHeader { get; set; }
        public DbSet<AnnualProcurementPlanDetails> APPDetails { get; set; }
        public DbSet<ContractHeader> Contract { get; set; }
        public DbSet<ContractDetails> ContractDetails { get; set; }
        public DbSet<ContractMonitoringUpdates> ContractMonitoringUpdates { get; set; }
        public DbSet<ProcurementProject> ProcurementProjects { get; set; }
        public DbSet<ProcurementProjectDetails> ProcurementProjectDetails { get; set; }
        public DbSet<ContractUpdates> ContractUpdates { get; set; }
        public DbSet<InfrastructureDetailedEstimate> InfrastructureDetailedEstimate { get; set; }
        public DbSet<InfrastructureProject> InfrastructureProject { get; set; }
        public DbSet<MarketSurvey> MarketSurveys { get; set; }
        public DbSet<PPMPHeader> PPMPHeader { get; set; }
        public DbSet<PPMPDetails> PPMPDetails { get; set; }
        public DbSet<ProjectPlans> ProjectPlans { get; set; }
        public DbSet<ProjectDetails> ProjectDetails { get; set; }
        public DbSet<PurchaseRequestHeader> PurchaseRequestHeader { get; set; }
        public DbSet<PurchaseRequestDetails> PurchaseRequestDetails { get; set; }

        //public DbSet<RequestForQuotation> RequestForQuotation { get; set; }
        //public DbSet<RequestForQuotationHeader> RequestForQuotationHeader { get; set; }
        //public DbSet<RequestForQuotationDetails> RequestForQuotationDetails { get; set; }
        //public DbSet<Securities> Securities { get; set; }
        //public DbSet<BidsHeader> BidsHeader { get; set; }
        //public DbSet<BidDetails> BidDetails { get; set; }
        //public DbSet<PurchaseOrderHeader> PurchaseOrderHeader { get; set; }
        //public DbSet<PurchaseOrderDetails> PurchaseOrderDetails { get; set; }
        //public DbSet<NoticeOfAward> NoticeOfAward { get; set; }



        //===============================================================//
        //========== PROPERTY AND SUPPLIES TRANSACTION TABLES ===========//
        //===============================================================//
        public DbSet<Supplies> Supplies { get; set; }
        public DbSet<StockCard> StockCard { get; set; }
        public DbSet<PPE> PPE { get; set; }
        public DbSet<PropertyCard> PropertyCards { get; set; }
        public DbSet<Delivery> DeliveryHeader { get; set; }
        public DbSet<DeliverySupply> SupplyDelivery { get; set; }
        public DbSet<DeliveryProperty> PPEDelivery { get; set; }
        public DbSet<Inspection> Inspection { get; set; }
        public DbSet<InspectionSupply> SupplyInspection { get; set; }
        public DbSet<InspectionProperty> PPEInspection { get; set; }
        //public DbSet<RequestHeader> RequestHeader { get; set; }
        //public DbSet<SuppliesRequestDetails> SuppliesRequestDetails { get; set; }
        //public DbSet<SuppliesIssueDetails> SuppliesIssueDetails { get; set; }
        //public DbSet<InspectionHeader> InspectionHeader { get; set; }
        //public DbSet<InspectionDetails> InspectionDetails { get; set; }
        //public DbSet<SuppliesMaster> SuppliesMaster { get; set; }
        //public DbSet<StockCard> StockCard { get; set; }
        //public DbSet<RequestHeader> RequestHeader { get; set; }
        //public DbSet<RequestDetails> RequestDetails { get; set; }
        //public DbSet<ItemPullOutHeader> ItemPullOutHeader { get; set; }
        //public DbSet<ItemPullOutDetails> ItemPullOutDetails { get; set; }
        //public DbSet<PPEInstance> PPEInstance { get; set; }
        //public DbSet<ICSHeader> ICSHeader { get; set; }
        //public DbSet<ICSDetails> ICSDetails { get; set; }
        //public DbSet<PARHeader> PARHeader { get; set; }
        //public DbSet<PARDetails> PARDetails { get; set; }
        //public DbSet<InventoryAdjustment> InventoryAdjustment { get; set; }
        //public DbSet<InventoryAdjustmentSupplies> SuppliesAdjustment { get; set; }
        //public DbSet<PhysicalCount> PhysicalCount { get; set; }
        //public DbSet<PhysicalCountSupplies> SuppliesCount { get; set; }
        //public DbSet<PhysicalCountPPE> PPEsCount { get; set; }
        //public DbSet<PPETransferLocation> PPETransferLocation { get; set; }
        //public DbSet<PPEReturn> PPEReturn { get; set; }
        //public DbSet<PPEReturnDetails> PPEReturnDetails { get; set; }
        //public DbSet<MaterialsDisposition> MaterialsDisposition { get; set; }
        //public DbSet<MaterialsDispositionSupplies> SuppliesDisposition { get; set; }
        //public DbSet<MaterialsDispositionSEPPE> SEPPEDisposition { get; set; }

        ////workflow
        //public DbSet<DeliveryWorkflow> DeliveryWorkflow { get; set; }

        //===============================================================//
        //======================= LOG TABLES ============================//
        //===============================================================//

        public DbSet<LogsMasterTables> LogsMasterTables { get; set; }

        //===============================================================//
        //=============== ACCOUNTS MANAGEMENT TABLES ====================//
        //===============================================================//
        public DbSet<UserAccounts> UserAccounts { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}