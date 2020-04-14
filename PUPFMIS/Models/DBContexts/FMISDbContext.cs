//using MySql.Data.Entity;
using System.Data.Entity;

namespace PUPFMIS.Models
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class FMISDbContext : DbContext
    {
        public FMISDbContext() : base("FMISDbContext") { }

        public static FMISDbContext Create()
        {
            return new FMISDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PPMPHeader>()
                .HasRequired<InventoryType>(s => s.FKPPMPTypeReference)
                .WithMany()
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Services>()
                .HasRequired<InventoryType>(s => s.FKInventoryTypeReference)
                .WithMany()
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Services>()
                .HasRequired<PhilGEPSCategories>(s => s.FKCategoryReference)
                .WithMany()
                .WillCascadeOnDelete(false);
        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<UnitOfMeasure> UOM { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<DBMCategories> DBMCategories { get; set; }
        public DbSet<PhilGEPSCategories> PhilGEPSCategories { get; set; }
        public DbSet<InventoryType> InventoryTypes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<ItemPrice> ItemPrices { get; set; } 

        public DbSet<ProjectPlans> ProjectPlans { get; set; }
        public DbSet<ProjectPlanItems> ProjectPlanItems { get; set; }
        public DbSet<ProjectPlanServices> ProjectPlanServices { get; set; }
        public DbSet<PPMPHeader> PPMPHeader { get; set; }
        public DbSet<PPMPItemDetails> PPMPItemDetails { get; set; }
        public DbSet<PPMPServiceDetails> PPMPServiceDetails { get; set; }
        public DbSet<PPMPDeadlines> PPMPDeadlines { get; set; }
        public DbSet<PPMPApprovalWorkflow> PPMPApprovalWorkflow { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<StockCard> StockCard { get; set; }
        public DbSet<RequestHeader> RequestHeader { get; set; }
        public DbSet<SuppliesRequestDetails> SuppliesRequestDetails { get; set; }
        public DbSet<SuppliesIssueDetails> SuppliesIssueDetails { get; set; }
        public DbSet<SystemVariables> SystemVariables { get; set; }

        //===============================================================//
        //=================== PROCUREMENT TABLES ========================//
        //===============================================================//
        //public DbSet<PlantsMaster> Plants { get; set; }
        //public DbSet<LocationsMaster> Locations { get; set; }
        //public DbSet<ItemsCatalogue> ItemsCatalogue { get; set; }
        //public DbSet<PPMP> PPMPHeader { get; set; }
        //public DbSet<PPMPDetails> PPMPDetails { get; set; }
        //public DbSet<PPMPApproval> PPMPApproval { get; set; }
        //public DbSet<APPHeader> APPHeader { get; set; }
        //public DbSet<PurchaseRequest> PurchaseRequest { get; set; }
        //public DbSet<PurchaseRequestApproval> PRApproval { get; set; }
        //public DbSet<PurchaseOrderHeader> POHeader { get; set; }
        //public DbSet<PurchaseOrderLineItems> PODetails { get; set; }
        //public DbSet<AgencyProcurementRequest> AgencyProcurementRequest { get; set; }
        //public DbSet<AgencyProcurementRequestDetails> AgencyProcurementRequestDetails { get; set; }
        //public DbSet<ProcurementPipelineHeader> ProcurementPipelineHeader { get; set; }
        //public DbSet<ProcurementPipeline> ProcurementPipeline { get; set; }
        //public DbSet<ProcurementPipelineStatusUpdate> ProcurementPipelineStatusUpdate { get; set; }
        //public DbSet<EvaluationCriteriaCategory> EvaluationCriteriaCategory { get; set; }
        //public DbSet<EvaluationScale> EvaluationScale { get; set; }
        //public DbSet<EvaluationCriteria> EvaluationCriteria { get; set; }
        //public DbSet<SupplierEvaluation> SupplierEvaluation { get; set; }
        //public DbSet<EvaluationResponses> EvaluationResponses { get; set; }

        ////property
        //public DbSet<DeliveryHeader> DeliveryHeader { get; set; }
        //public DbSet<SupplyDelivery> SupplyDelivery { get; set; }
        //public DbSet<PropertyDelivery> PropertyDelivery { get; set; }
        //public DbSet<InspectionHeader> InspectionHeader { get; set; }
        //public DbSet<InspectionDetails> InspectionDetails { get; set; }
        //public DbSet<SuppliesMaster> SuppliesMaster { get; set; }
        //public DbSet<StockCard> StockCard { get; set; }
        //public DbSet<RequestHeader> RequestHeader { get; set; }
        //public DbSet<RequestDetails> RequestDetails { get; set; }
        //public DbSet<ItemPullOutHeader> ItemPullOutHeader { get; set; }
        //public DbSet<ItemPullOutDetails> ItemPullOutDetails { get; set; }
        //public DbSet<PPE> PPE { get; set; }
        //public DbSet<PropertyCard> PropertyCards { get; set; }
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
        public DbSet<UserInformation> UserInformation { get; set; }
        public DbSet<UserAccounts> UserAccounts { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
    }
}