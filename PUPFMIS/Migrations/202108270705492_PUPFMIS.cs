namespace PUPFMIS.Migrations.FMISMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PUPFMIS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PROC_SYTM_Agency_Details",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AccountCode = c.String(nullable: false, maxLength: 15, unicode: false),
                        OrganizationType = c.String(nullable: false, maxLength: 75, unicode: false),
                        AgencyName = c.String(nullable: false, maxLength: 150, unicode: false),
                        Region = c.String(nullable: false, maxLength: 30, unicode: false),
                        Address = c.String(nullable: false, maxLength: 150, unicode: false),
                        Website = c.String(maxLength: 8000, unicode: false),
                        ProcurementOfficeReference = c.String(maxLength: 8000, unicode: false),
                        ProcurementPlanningOfficeReference = c.String(maxLength: 8000, unicode: false),
                        ProcurementOfficeAddress = c.String(maxLength: 8000, unicode: false),
                        ProcurementOfficeEmail = c.String(maxLength: 8000, unicode: false),
                        ProcurementOfficeContactNo = c.String(maxLength: 8000, unicode: false),
                        ProcurementOfficeAlternateContactNo = c.String(maxLength: 8000, unicode: false),
                        AccountingOfficeReference = c.String(maxLength: 8000, unicode: false),
                        BACOfficeReference = c.String(maxLength: 8000, unicode: false),
                        BACOfficeAddress = c.String(maxLength: 8000, unicode: false),
                        BACOfficeEmail = c.String(maxLength: 8000, unicode: false),
                        BACOfficeContactNo = c.String(maxLength: 8000, unicode: false),
                        BACOfficeAlternateContactNo = c.String(maxLength: 8000, unicode: false),
                        PropertyOfficeReference = c.String(maxLength: 8000, unicode: false),
                        SuppliesInventoryOfficeReference = c.String(maxLength: 8000, unicode: false),
                        InspectionManagementReference = c.String(maxLength: 8000, unicode: false),
                        HOPEReference = c.String(maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_TRXN_Annual_Procurement_Plan_CSE",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        APPHeaderReference = c.Int(nullable: false),
                        UACS = c.String(nullable: false),
                        ProjectDetailsID = c.Int(nullable: false),
                        PPMPHeaderReference = c.Int(nullable: false),
                        ArticleReference = c.Int(nullable: false),
                        ItemSequence = c.String(nullable: false, maxLength: 2),
                        ItemFullName = c.String(nullable: false, maxLength: 200, unicode: false),
                        ItemSpecifications = c.String(),
                        ProcurementSource = c.Int(nullable: false),
                        UOMReference = c.Int(nullable: false),
                        CategoryReference = c.Int(nullable: false),
                        JanQty = c.Int(nullable: false),
                        FebQty = c.Int(nullable: false),
                        MarQty = c.Int(nullable: false),
                        Q1TotalQty = c.Int(nullable: false),
                        AprQty = c.Int(nullable: false),
                        MayQty = c.Int(nullable: false),
                        JunQty = c.Int(nullable: false),
                        Q2TotalQty = c.Int(nullable: false),
                        JulQty = c.Int(nullable: false),
                        AugQty = c.Int(nullable: false),
                        SepQty = c.Int(nullable: false),
                        Q3TotalQty = c.Int(nullable: false),
                        OctQty = c.Int(nullable: false),
                        NovQty = c.Int(nullable: false),
                        DecQty = c.Int(nullable: false),
                        Q4TotalQty = c.Int(nullable: false),
                        TotalQty = c.Int(nullable: false),
                        UnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApprovedBudget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FundSource = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Annual_Procurement_Plan", t => t.APPHeaderReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Item_Articles", t => t.ArticleReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Item_Categories", t => t.CategoryReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.UOMReference, cascadeDelete: true)
                .Index(t => t.APPHeaderReference)
                .Index(t => t.ArticleReference)
                .Index(t => t.UOMReference)
                .Index(t => t.CategoryReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Annual_Procurement_Plan",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FiscalYear = c.Int(nullable: false),
                        APPType = c.Int(nullable: false),
                        ReferenceNo = c.String(nullable: false, maxLength: 30, unicode: false),
                        PreparedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        PreparedByDepartmentCode = c.String(nullable: false, maxLength: 50, unicode: false),
                        PreparedByDesignation = c.String(nullable: false, maxLength: 150, unicode: false),
                        PreparedAt = c.DateTime(),
                        RecommendingApproval = c.String(maxLength: 50, unicode: false),
                        RecommendingApprovalActionBy = c.String(maxLength: 50, unicode: false),
                        RecommendingApprovalDepartmentCode = c.String(maxLength: 50, unicode: false),
                        RecommendingApprovalDesignation = c.String(maxLength: 150, unicode: false),
                        RecommendedAt = c.DateTime(),
                        ApprovedBy = c.String(maxLength: 150, unicode: false),
                        ApprovalActionBy = c.String(maxLength: 50, unicode: false),
                        ApprovedByDepartmentCode = c.String(maxLength: 150, unicode: false),
                        ApprovedByDesignation = c.String(maxLength: 150, unicode: false),
                        ApprovedAt = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_MSTR_Item_Articles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArticleCode = c.String(nullable: false, maxLength: 30, unicode: false),
                        ArticleName = c.String(nullable: false, maxLength: 150, unicode: false),
                        ItemTypeReference = c.Int(nullable: false),
                        UACSObjectClass = c.String(maxLength: 20),
                        GLAccount = c.String(maxLength: 5),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Item_Types", t => t.ItemTypeReference, cascadeDelete: true)
                .Index(t => t.ItemTypeReference);
            
            CreateTable(
                "dbo.PROC_MSTR_Item_Types",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemTypeCode = c.String(nullable: false, maxLength: 4),
                        ItemType = c.String(nullable: false, maxLength: 150, unicode: false),
                        ResponsibilityCenter = c.String(maxLength: 30, unicode: false),
                        PurchaseRequestCenter = c.String(maxLength: 30, unicode: false),
                        ItemClassificationReference = c.Int(nullable: false),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Item_Classification", t => t.ItemClassificationReference, cascadeDelete: true)
                .Index(t => t.ItemClassificationReference);
            
            CreateTable(
                "dbo.PROC_MSTR_Item_Classification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GeneralClass = c.String(nullable: false, maxLength: 75),
                        Classification = c.String(nullable: false, maxLength: 75),
                        ProjectPrefix = c.String(nullable: false, maxLength: 75),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_MSTR_Item_Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemCategoryName = c.String(nullable: false, maxLength: 150, unicode: false),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_MSTR_UnitsOfMeasure",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UnitName = c.String(nullable: false, maxLength: 75),
                        Abbreviation = c.String(nullable: false, maxLength: 5),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_TRXN_Annual_Procurement_Plan_Details",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PAPCode = c.String(nullable: false, maxLength: 30, unicode: false),
                        APPHeaderReference = c.Int(nullable: false),
                        ProcurementSource = c.Int(nullable: false),
                        ProcurementProgram = c.String(nullable: false, maxLength: 175, unicode: false),
                        APPModeOfProcurementReference = c.String(nullable: false),
                        ClassificationReference = c.Int(),
                        ObjectClassification = c.String(maxLength: 8000, unicode: false),
                        ObjectSubClassification = c.String(maxLength: 8000, unicode: false),
                        InventoryCode = c.String(nullable: false),
                        EndUser = c.String(maxLength: 8000, unicode: false),
                        Month = c.Int(nullable: false),
                        StartMonth = c.String(maxLength: 8000, unicode: false),
                        EndMonth = c.String(maxLength: 8000, unicode: false),
                        FundSourceReference = c.String(maxLength: 8000, unicode: false),
                        MOOEAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        COAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remarks = c.String(),
                        ProjectCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProjectCoordinator = c.String(),
                        ProjectSupport = c.String(),
                        IsInstitutional = c.Boolean(nullable: false),
                        IsTangible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Annual_Procurement_Plan", t => t.APPHeaderReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Item_Classification", t => t.ClassificationReference)
                .Index(t => t.APPHeaderReference)
                .Index(t => t.ClassificationReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Agency_Procurement_Request_Details",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArticleReference = c.Int(),
                        ItemSequence = c.String(maxLength: 2),
                        ItemFullName = c.String(nullable: false, maxLength: 200, unicode: false),
                        ItemSpecifications = c.String(),
                        Quantity = c.Int(nullable: false),
                        UnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UOMReference = c.Int(nullable: false),
                        APRReference = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Agency_Procurement_Request", t => t.APRReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Item_Articles", t => t.ArticleReference)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.UOMReference, cascadeDelete: true)
                .Index(t => t.ArticleReference)
                .Index(t => t.UOMReference)
                .Index(t => t.APRReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Agency_Procurement_Request",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FiscalYear = c.Int(nullable: false),
                        AgencyControlNo = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        ProcurementHead = c.String(nullable: false),
                        ProcurementDepartment = c.String(nullable: false),
                        ProcurementHeadDesignation = c.String(nullable: false),
                        ChiefAccountant = c.String(nullable: false),
                        ChiefAccountantDepartment = c.String(nullable: false),
                        ChiefAccountantDesignation = c.String(nullable: false),
                        AgencyHead = c.String(nullable: false),
                        AgencyHeadDepartment = c.String(nullable: false),
                        AgencyHeadDesignation = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_SYTM_BAC_Secretariat",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Member = c.String(nullable: false, maxLength: 75),
                        Membership = c.Int(nullable: false),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreateBy = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_TRXN_Contract",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ContractType = c.Int(nullable: false),
                        ReferenceNumber = c.String(maxLength: 75),
                        FiscalYear = c.Int(nullable: false),
                        ProcurementProjectReference = c.Int(nullable: false),
                        ContractPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommencedAt = c.DateTime(),
                        DeliveryDeadline = c.DateTime(),
                        CompletedAt = c.DateTime(),
                        ContractStatus = c.Int(),
                        SupplierReference = c.Int(nullable: false),
                        PMOffice = c.String(nullable: false),
                        PMOHead = c.String(nullable: false),
                        PMOHeadDesignation = c.String(nullable: false),
                        AccountingOffice = c.String(nullable: false),
                        AccountingOfficeHead = c.String(nullable: false),
                        AccountingOfficeHeadDesignation = c.String(nullable: false),
                        HOPEOffice = c.String(nullable: false),
                        HOPE = c.String(nullable: false),
                        HOPEDesignation = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Procurement_Project", t => t.ProcurementProjectReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Suppliers", t => t.SupplierReference, cascadeDelete: true)
                .Index(t => t.ProcurementProjectReference)
                .Index(t => t.SupplierReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Procurement_Project",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ParentProjectReference = c.Int(),
                        APPReference = c.Int(nullable: false),
                        ClassificationReference = c.Int(nullable: false),
                        ModeOfProcurementReference = c.Int(nullable: false),
                        FiscalYear = c.Int(nullable: false),
                        FundSource = c.String(),
                        ProcurementProjectType = c.Int(nullable: false),
                        ContractStrategy = c.Int(nullable: false),
                        ContractCode = c.String(),
                        ContractName = c.String(nullable: false, maxLength: 255, unicode: false),
                        ContractLocation = c.String(nullable: false, maxLength: 255, unicode: false),
                        ContractStatus = c.Int(nullable: false),
                        ProcurementProjectStage = c.Int(nullable: false),
                        ApprovedBudgetForContract = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryPeriod = c.Int(),
                        ProjectCoordinator = c.String(),
                        PRSubmissionOpen = c.DateTime(),
                        PRSubmissionClose = c.DateTime(),
                        PreProcurementConference = c.DateTime(),
                        IBPreparation = c.DateTime(),
                        PostingOfIB_RFQPosting = c.DateTime(),
                        PreBidConference = c.DateTime(),
                        DeadlineOfSubmissionOfBids_DeadlineOfSubmisionOfBids = c.DateTime(),
                        OpeningOfBids_OpeningOfQuotations = c.DateTime(),
                        NOAIssuance = c.DateTime(),
                        NTPIssuance = c.DateTime(),
                        PreProcurementConferenceLocation = c.String(),
                        PreBidConferenceLocation = c.String(),
                        PreBidVideoConferencingOptions = c.Int(),
                        PreBidVideoConferenceMode = c.String(maxLength: 75, unicode: false),
                        PreBidVideoConferenceAccessRequestEmail = c.String(maxLength: 75, unicode: false),
                        PreBidVideoConferenceAccessRequestContactNo = c.String(maxLength: 20, unicode: false),
                        PreBidAdditionalInstructions = c.String(),
                        BidDocumentPrice = c.Decimal(precision: 18, scale: 2),
                        OpeningOfBidsLocation = c.String(),
                        OpeningOfBidsFailureReason = c.Int(),
                        PostQualificationFailureReason = c.Int(),
                        SolicitationNo = c.String(),
                        NOAAcceptedAt = c.DateTime(),
                        ContractSignedAt = c.DateTime(),
                        NTPSignedAt = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Annual_Procurement_Plan_Details", t => t.APPReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Item_Classification", t => t.ClassificationReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Procurement_Modes", t => t.ModeOfProcurementReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_TRXN_Procurement_Project", t => t.ParentProjectReference)
                .Index(t => t.ParentProjectReference)
                .Index(t => t.APPReference)
                .Index(t => t.ClassificationReference)
                .Index(t => t.ModeOfProcurementReference);
            
            CreateTable(
                "dbo.PROC_MSTR_Procurement_Modes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ModeOfProcurementName = c.String(nullable: false, maxLength: 175, unicode: false),
                        ShortName = c.String(nullable: false, maxLength: 10, unicode: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_MSTR_Suppliers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SupplierName = c.String(nullable: false, maxLength: 75),
                        Address = c.String(nullable: false, maxLength: 250),
                        City = c.String(nullable: false, maxLength: 250),
                        State = c.String(nullable: false, maxLength: 250),
                        PostalCode = c.String(nullable: false, maxLength: 15),
                        ContactPerson = c.String(nullable: false, maxLength: 75),
                        ContactPersonDesignation = c.String(nullable: false, maxLength: 75),
                        AuthorizedAgent = c.String(maxLength: 75),
                        AuthorizedDesignation = c.String(maxLength: 75),
                        ContactNumber = c.String(nullable: false, maxLength: 20),
                        AlternateContactNumber = c.String(maxLength: 20),
                        TaxIdNumber = c.String(maxLength: 20),
                        EmailAddress = c.String(maxLength: 75),
                        Website = c.String(maxLength: 75),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_TRXN_Contract_Details",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ContractReference = c.Int(nullable: false),
                        ArticleReference = c.Int(),
                        ItemSequence = c.String(maxLength: 2),
                        ItemFullName = c.String(nullable: false, maxLength: 200, unicode: false),
                        ItemSpecifications = c.String(),
                        Quantity = c.Int(nullable: false),
                        ContractUnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ContractTotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Savings = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UOMReference = c.Int(nullable: false),
                        DeliveredQuantity = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Item_Articles", t => t.ArticleReference)
                .ForeignKey("dbo.PROC_TRXN_Contract", t => t.ContractReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.UOMReference, cascadeDelete: true)
                .Index(t => t.ContractReference)
                .Index(t => t.ArticleReference)
                .Index(t => t.UOMReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Contract_Monitoring_Updates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ContractReference = c.Int(nullable: false),
                        ContractStatus = c.Int(nullable: false),
                        AccomplishedAt = c.DateTime(),
                        Remarks = c.String(nullable: false),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Contract", t => t.ContractReference, cascadeDelete: true)
                .Index(t => t.ContractReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Procurement_Project_Updates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProcurementProjectReference = c.Int(nullable: false),
                        ProcurementProjectStage = c.Int(nullable: false),
                        AccomplishedAt = c.DateTime(),
                        Remarks = c.String(nullable: false),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Procurement_Project", t => t.ProcurementProjectReference, cascadeDelete: true)
                .Index(t => t.ProcurementProjectReference);
            
            CreateTable(
                "dbo.PROP_TRXN_Delivery",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DeliveryAcceptanceNumber = c.String(nullable: false),
                        ContractReference = c.Int(nullable: false),
                        DateProcessed = c.DateTime(nullable: false),
                        ProcessedBy = c.String(nullable: false),
                        ReceivedBy = c.String(nullable: false),
                        ContractType = c.Int(nullable: false),
                        Reference = c.String(),
                        InvoiceNumber = c.String(nullable: false),
                        InvoiceDate = c.DateTime(nullable: false),
                        DRNumber = c.String(nullable: false),
                        DeliveryDate = c.DateTime(nullable: false),
                        DeliveryCompleteness = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Contract", t => t.ContractReference, cascadeDelete: true)
                .Index(t => t.ContractReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Infrastructure_Detailed_Estimate",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        InfrastructureProjectReference = c.Int(nullable: false),
                        InfrastructureMaterialReference = c.Int(nullable: false),
                        InfrastructureWorkClassification = c.Int(nullable: false),
                        InfrastructureWorkRequirement = c.Int(),
                        UOMReference = c.Int(),
                        Quantity = c.Int(nullable: false),
                        ItemUnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ItemTotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LaborUnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LaborTotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EstimatedDirectCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MobDemobilizationCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OCMCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProfitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalMarkUp = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalIndirectCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Infrastructure_Materials", t => t.InfrastructureMaterialReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_TRXN_Infrastructure_Project", t => t.InfrastructureProjectReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.UOMReference)
                .ForeignKey("dbo.PROC_MSTR_Infrastructure_Classification", t => t.InfrastructureWorkClassification, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Infrastructure_Requirements", t => t.InfrastructureWorkRequirement)
                .Index(t => t.InfrastructureProjectReference)
                .Index(t => t.InfrastructureMaterialReference)
                .Index(t => t.InfrastructureWorkClassification)
                .Index(t => t.InfrastructureWorkRequirement)
                .Index(t => t.UOMReference);
            
            CreateTable(
                "dbo.PROC_MSTR_Infrastructure_Materials",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemName = c.String(nullable: false, maxLength: 75),
                        ItemSpecifications = c.String(),
                        UOMReference = c.Int(nullable: false),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.UOMReference, cascadeDelete: true)
                .Index(t => t.UOMReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Infrastructure_Project",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectTitle = c.String(maxLength: 150),
                        ProjectLocation = c.String(maxLength: 150),
                        ContractDuration = c.Int(nullable: false),
                        RequestReference = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Project_Infrastructure_Request", t => t.RequestReference)
                .Index(t => t.RequestReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Project_Infrastructure_Request",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IsPartOfAnotherProject = c.Boolean(nullable: false),
                        PAPCode = c.String(maxLength: 40, unicode: false),
                        ProjectReference = c.Int(),
                        FiscalYear = c.Int(nullable: false),
                        ProjectCode = c.String(maxLength: 40, unicode: false),
                        ProposedProject = c.String(nullable: false, maxLength: 150),
                        BackgroundOfProject = c.String(nullable: false),
                        DeliveryMonth = c.Int(nullable: false),
                        Sector = c.String(maxLength: 20, unicode: false),
                        Department = c.String(maxLength: 20, unicode: false),
                        Unit = c.String(maxLength: 20, unicode: false),
                        Status = c.Int(nullable: false),
                        Remarks = c.String(maxLength: 8000, unicode: false),
                        ReasonForNonAcceptance = c.String(maxLength: 8000, unicode: false),
                        PreparedBy = c.String(),
                        SubmittedBy = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Project_Plan", t => t.ProjectReference)
                .Index(t => t.ProjectReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Project_Plan",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PAPCode = c.String(maxLength: 40, unicode: false),
                        ProjectType = c.Int(nullable: false),
                        ParentProject = c.Int(),
                        ProjectCode = c.String(maxLength: 40, unicode: false),
                        ProjectName = c.String(nullable: false, maxLength: 175, unicode: false),
                        Description = c.String(nullable: false, maxLength: 175, unicode: false),
                        FiscalYear = c.Int(nullable: false),
                        Sector = c.String(maxLength: 20, unicode: false),
                        Department = c.String(maxLength: 20, unicode: false),
                        Unit = c.String(maxLength: 20, unicode: false),
                        PreparedBy = c.String(),
                        SubmittedBy = c.String(),
                        ProjectStatus = c.Int(nullable: false),
                        DeliveryMonth = c.Int(nullable: false),
                        TotalEstimatedBudget = c.Decimal(precision: 18, scale: 2),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Project_Plan", t => t.ParentProject)
                .Index(t => t.ParentProject);
            
            CreateTable(
                "dbo.PROC_MSTR_Infrastructure_Classification",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClassificationName = c.String(nullable: false, maxLength: 100),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_MSTR_Infrastructure_Requirements",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Requirement = c.String(nullable: false, maxLength: 100),
                        RequirementClassificationReference = c.Int(nullable: false),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Infrastructure_Classification", t => t.RequirementClassificationReference, cascadeDelete: true)
                .Index(t => t.RequirementClassificationReference);
            
            CreateTable(
                "dbo.PROP_TRXN_Inspection",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IARNumber = c.String(nullable: false),
                        SupplierReference = c.Int(nullable: false),
                        ContractType = c.Int(nullable: false),
                        ReferenceNumber = c.String(nullable: false),
                        ReferenceDate = c.DateTime(nullable: false),
                        InvoiceNumber = c.String(nullable: false),
                        InvoiceDate = c.DateTime(nullable: false),
                        ResponsibilityCenter = c.String(nullable: false),
                        Remarks = c.String(nullable: false),
                        FundSource = c.String(nullable: false),
                        DeliveryCompleteness = c.Int(),
                        DeliveryReference = c.Int(),
                        InspectedBy = c.String(nullable: false),
                        InspectedAt = c.DateTime(nullable: false),
                        ProcessedBy = c.String(nullable: false),
                        ProcessedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROP_TRXN_Delivery", t => t.DeliveryReference)
                .ForeignKey("dbo.PROC_MSTR_Suppliers", t => t.SupplierReference, cascadeDelete: true)
                .Index(t => t.SupplierReference)
                .Index(t => t.DeliveryReference);
            
            CreateTable(
                "dbo.PROC_MSTR_Item_Allowed_Users",
                c => new
                    {
                        ItemReference = c.Int(nullable: false),
                        DepartmentReference = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ItemReference, t.DepartmentReference })
                .ForeignKey("dbo.PROC_MSTR_Item", t => t.ItemReference, cascadeDelete: true)
                .Index(t => t.ItemReference);
            
            CreateTable(
                "dbo.PROC_MSTR_Item",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Sequence = c.String(nullable: false, maxLength: 2),
                        ArticleReference = c.Int(nullable: false),
                        ItemFullName = c.String(nullable: false, maxLength: 200, unicode: false),
                        ItemImage = c.Binary(),
                        ItemShortSpecifications = c.String(nullable: false, maxLength: 100, unicode: false),
                        IsSpecsUserDefined = c.Boolean(nullable: false),
                        ItemSpecifications = c.String(),
                        CategoryReference = c.Int(nullable: false),
                        ProcurementSource = c.Int(nullable: false),
                        PackagingUOMReference = c.Int(nullable: false),
                        IndividualUOMReference = c.Int(nullable: false),
                        QuantityPerPackage = c.Int(nullable: false),
                        MinimumIssuanceQty = c.Int(nullable: false),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Item_Articles", t => t.ArticleReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Item_Categories", t => t.CategoryReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.IndividualUOMReference)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.PackagingUOMReference)
                .Index(t => t.ArticleReference)
                .Index(t => t.CategoryReference)
                .Index(t => t.PackagingUOMReference)
                .Index(t => t.IndividualUOMReference);
            
            CreateTable(
                "dbo.PROC_MSTR_Item_Price_Catalogue",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Item = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPrevailingPrice = c.Boolean(nullable: false),
                        EffectivityDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Item", t => t.Item, cascadeDelete: true)
                .Index(t => t.Item);
            
            CreateTable(
                "dbo.PROC_LOGS_Master_Tables",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Action = c.String(maxLength: 15),
                        TableName = c.String(maxLength: 50),
                        ColumnName = c.String(maxLength: 50),
                        AuditableKey = c.Int(nullable: false),
                        OldValue = c.String(),
                        NewValue = c.String(),
                        ProcessedBy = c.Int(),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_TRXN_Market_Survey",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ArticleReference = c.Int(nullable: false),
                        ItemSequence = c.String(nullable: false, maxLength: 2),
                        ItemFullName = c.String(nullable: false, maxLength: 200, unicode: false),
                        ItemSpecifications = c.String(),
                        IsObsolete = c.Boolean(nullable: false),
                        Supplier1Reference = c.Int(),
                        Supplier2Reference = c.Int(),
                        Supplier3Reference = c.Int(),
                        UnitCost = c.Decimal(precision: 18, scale: 2),
                        Supplier1UnitCost = c.Decimal(precision: 18, scale: 2),
                        Supplier2UnitCost = c.Decimal(precision: 18, scale: 2),
                        Supplier3UnitCost = c.Decimal(precision: 18, scale: 2),
                        CreateAt = c.DateTime(nullable: false),
                        ConductedBy = c.String(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Item_Articles", t => t.ArticleReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Suppliers", t => t.Supplier1Reference)
                .ForeignKey("dbo.PROC_MSTR_Suppliers", t => t.Supplier2Reference)
                .ForeignKey("dbo.PROC_MSTR_Suppliers", t => t.Supplier3Reference)
                .Index(t => t.ArticleReference)
                .Index(t => t.Supplier1Reference)
                .Index(t => t.Supplier2Reference)
                .Index(t => t.Supplier3Reference);
            
            CreateTable(
                "dbo.PROP_MSTR_PPE",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PropertyNumberRoot = c.String(),
                        ArticleReference = c.Int(nullable: false),
                        Sequence = c.String(nullable: false, maxLength: 2),
                        Description = c.String(nullable: false, maxLength: 200, unicode: false),
                        ItemImage = c.Binary(),
                        IndividualUOMReference = c.Int(),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Item_Articles", t => t.ArticleReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.IndividualUOMReference)
                .Index(t => t.ArticleReference)
                .Index(t => t.IndividualUOMReference);
            
            CreateTable(
                "dbo.PROP_TRXN_Delivery_PPE",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PPEReference = c.Int(nullable: false),
                        DeliveryReference = c.Int(),
                        QuantityDelivered = c.Int(nullable: false),
                        QuantityBacklog = c.Int(nullable: false),
                        ReceiptUnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceiptTotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROP_TRXN_Delivery", t => t.DeliveryReference)
                .ForeignKey("dbo.PROP_MSTR_PPE", t => t.PPEReference, cascadeDelete: true)
                .Index(t => t.PPEReference)
                .Index(t => t.DeliveryReference);
            
            CreateTable(
                "dbo.PROP_TRXN_Inspection_PPE",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        InspectionReference = c.Int(nullable: false),
                        PropertyReference = c.Int(nullable: false),
                        UnitReference = c.Int(nullable: false),
                        QuantityReceived = c.Int(nullable: false),
                        QuantityAccepted = c.Int(nullable: false),
                        QuantityRejected = c.Int(nullable: false),
                        InspectionNotes = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROP_TRXN_Inspection", t => t.InspectionReference, cascadeDelete: true)
                .ForeignKey("dbo.PROP_MSTR_PPE", t => t.PropertyReference, cascadeDelete: true)
                .Index(t => t.InspectionReference)
                .Index(t => t.PropertyReference);
            
            CreateTable(
                "dbo.PROC_SYTM_PPMP_Deadlines",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FiscalYear = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        ClosingDate = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 20),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_TRXN_PPMP_Details",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UACS = c.String(nullable: false),
                        ProjectDetailsID = c.Int(nullable: false),
                        PPMPHeaderReference = c.Int(nullable: false),
                        APPDetailReference = c.Int(),
                        ClassificationReference = c.Int(),
                        ArticleReference = c.Int(),
                        ItemSequence = c.String(maxLength: 2),
                        ItemFullName = c.String(nullable: false, maxLength: 200, unicode: false),
                        ItemSpecifications = c.String(),
                        ProposalType = c.Int(nullable: false),
                        ProcurementSource = c.Int(nullable: false),
                        UOMReference = c.Int(nullable: false),
                        CategoryReference = c.Int(),
                        Justification = c.String(),
                        JANMilestone = c.Int(),
                        FEBMilestone = c.Int(),
                        MARMilestone = c.Int(),
                        APRMilestone = c.Int(),
                        MAYMilestone = c.Int(),
                        JUNMilestone = c.Int(),
                        JULMilestone = c.Int(),
                        AUGMilestone = c.Int(),
                        SEPMilestone = c.Int(),
                        OCTMilestone = c.Int(),
                        NOVMilestone = c.Int(),
                        DECMilestone = c.Int(),
                        JanQty = c.Int(nullable: false),
                        FebQty = c.Int(nullable: false),
                        MarQty = c.Int(nullable: false),
                        Q1TotalQty = c.Int(nullable: false),
                        AprQty = c.Int(nullable: false),
                        MayQty = c.Int(nullable: false),
                        JunQty = c.Int(nullable: false),
                        Q2TotalQty = c.Int(nullable: false),
                        JulQty = c.Int(nullable: false),
                        AugQty = c.Int(nullable: false),
                        SepQty = c.Int(nullable: false),
                        Q3TotalQty = c.Int(nullable: false),
                        OctQty = c.Int(nullable: false),
                        NovQty = c.Int(nullable: false),
                        DecQty = c.Int(nullable: false),
                        Q4TotalQty = c.Int(nullable: false),
                        TotalQty = c.Int(nullable: false),
                        UnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PPMPDetailStatus = c.Int(nullable: false),
                        BudgetOfficeAction = c.Int(),
                        BudgetOfficeReasonForNonAcceptance = c.Int(),
                        EstimatedBudget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FundSource = c.String(),
                        ProcurementProjectType = c.Int(),
                        ProcurementProject = c.Int(),
                        PurchaseRequestReference = c.Int(),
                        UpdateFlag = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Annual_Procurement_Plan_Details", t => t.APPDetailReference)
                .ForeignKey("dbo.PROC_MSTR_Item_Categories", t => t.CategoryReference)
                .ForeignKey("dbo.PROC_MSTR_Item_Classification", t => t.ClassificationReference)
                .ForeignKey("dbo.PROC_MSTR_Item_Articles", t => t.ArticleReference)
                .ForeignKey("dbo.PROC_TRXN_PPMP", t => t.PPMPHeaderReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_TRXN_Procurement_Project", t => t.ProcurementProject)
                .ForeignKey("dbo.PROC_TRXN_Project_Plan_Details", t => t.ProjectDetailsID, cascadeDelete: true)
                .ForeignKey("dbo.PROC_TRXN_Purchase_Request", t => t.PurchaseRequestReference)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.UOMReference, cascadeDelete: true)
                .Index(t => t.ProjectDetailsID)
                .Index(t => t.PPMPHeaderReference)
                .Index(t => t.APPDetailReference)
                .Index(t => t.ClassificationReference)
                .Index(t => t.ArticleReference)
                .Index(t => t.UOMReference)
                .Index(t => t.CategoryReference)
                .Index(t => t.ProcurementProject)
                .Index(t => t.PurchaseRequestReference);
            
            CreateTable(
                "dbo.PROC_TRXN_PPMP",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReferenceNo = c.String(nullable: false, maxLength: 75, unicode: false),
                        FiscalYear = c.Int(nullable: false),
                        UACS = c.String(nullable: false),
                        PPMPType = c.Int(nullable: false),
                        PPMPStatus = c.Int(nullable: false),
                        EstimatedBudget = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sector = c.String(nullable: false),
                        Department = c.String(nullable: false),
                        IsInstitutional = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        SubmittedAt = c.DateTime(),
                        ApprovedAt = c.DateTime(),
                        PreparedBy = c.String(),
                        PreparedByDesignation = c.String(),
                        SubmittedBy = c.String(),
                        SubmittedByDesignation = c.String(),
                        EvaluatedBy = c.String(),
                        EvaluatedByDesignation = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_TRXN_Project_Plan_Details",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProjectReference = c.Int(nullable: false),
                        ClassificationReference = c.Int(nullable: false),
                        UACS = c.String(),
                        ArticleReference = c.Int(),
                        ItemSequence = c.String(maxLength: 2),
                        ItemFullName = c.String(nullable: false, maxLength: 200, unicode: false),
                        ItemSpecifications = c.String(),
                        ProposalType = c.Int(nullable: false),
                        ProcurementSource = c.Int(nullable: false),
                        UOMReference = c.Int(nullable: false),
                        CategoryReference = c.Int(),
                        Justification = c.String(),
                        ProjectItemStatus = c.Int(nullable: false),
                        JanQty = c.Int(nullable: false),
                        FebQty = c.Int(nullable: false),
                        MarQty = c.Int(nullable: false),
                        AprQty = c.Int(nullable: false),
                        MayQty = c.Int(nullable: false),
                        JunQty = c.Int(nullable: false),
                        JulQty = c.Int(nullable: false),
                        AugQty = c.Int(nullable: false),
                        SepQty = c.Int(nullable: false),
                        OctQty = c.Int(nullable: false),
                        NovQty = c.Int(nullable: false),
                        DecQty = c.Int(nullable: false),
                        TotalQty = c.Int(nullable: false),
                        Supplier1Reference = c.Int(),
                        Supplier2Reference = c.Int(),
                        Supplier3Reference = c.Int(),
                        UnitCost = c.Decimal(precision: 18, scale: 2),
                        Supplier1UnitCost = c.Decimal(precision: 18, scale: 2),
                        Supplier2UnitCost = c.Decimal(precision: 18, scale: 2),
                        Supplier3UnitCost = c.Decimal(precision: 18, scale: 2),
                        EstimatedBudget = c.Decimal(precision: 18, scale: 2),
                        UpdateFlag = c.Boolean(nullable: false),
                        ResponsibilityCenterAction = c.Int(),
                        ReasonForNonAcceptance = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Item_Categories", t => t.CategoryReference)
                .ForeignKey("dbo.PROC_MSTR_Item_Classification", t => t.ClassificationReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Item_Articles", t => t.ArticleReference)
                .ForeignKey("dbo.PROC_TRXN_Project_Plan", t => t.ProjectReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Suppliers", t => t.Supplier1Reference)
                .ForeignKey("dbo.PROC_MSTR_Suppliers", t => t.Supplier2Reference)
                .ForeignKey("dbo.PROC_MSTR_Suppliers", t => t.Supplier3Reference)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.UOMReference)
                .Index(t => t.ProjectReference)
                .Index(t => t.ClassificationReference)
                .Index(t => t.ArticleReference)
                .Index(t => t.UOMReference)
                .Index(t => t.CategoryReference)
                .Index(t => t.Supplier1Reference)
                .Index(t => t.Supplier2Reference)
                .Index(t => t.Supplier3Reference);
            
            CreateTable(
                "dbo.PROC_TRXN_Purchase_Request",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FiscalYear = c.Int(nullable: false),
                        PRNumber = c.String(nullable: false, maxLength: 12, unicode: false),
                        PRStatus = c.Int(nullable: false),
                        ProcurementProjectReference = c.Int(nullable: false),
                        Department = c.String(nullable: false, maxLength: 20, unicode: false),
                        FundCluster = c.String(nullable: false, maxLength: 30, unicode: false),
                        Purpose = c.String(nullable: false, maxLength: 8000, unicode: false),
                        RequestedBy = c.String(nullable: false, maxLength: 175, unicode: false),
                        RequestedByDesignation = c.String(nullable: false, maxLength: 175, unicode: false),
                        RequestedByDepartment = c.String(nullable: false, maxLength: 175, unicode: false),
                        ApprovedBy = c.String(nullable: false, maxLength: 175, unicode: false),
                        ApprovedByDesignation = c.String(nullable: false, maxLength: 175, unicode: false),
                        ApprovedByDepartment = c.String(nullable: false, maxLength: 175, unicode: false),
                        CreatedBy = c.String(nullable: false, maxLength: 30, unicode: false),
                        CreatedAt = c.DateTime(nullable: false),
                        SubmittedAt = c.DateTime(),
                        SubmittedBy = c.String(maxLength: 30, unicode: false),
                        ReceivedAt = c.DateTime(),
                        ReceivedBy = c.String(maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_TRXN_Procurement_Project", t => t.ProcurementProjectReference, cascadeDelete: true)
                .Index(t => t.ProcurementProjectReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Procurement_Project_Details",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProcurementProjectReference = c.Int(nullable: false),
                        ArticleReference = c.Int(),
                        ItemSequence = c.String(maxLength: 2),
                        ItemFullName = c.String(nullable: false, maxLength: 200, unicode: false),
                        ItemSpecifications = c.String(),
                        Quantity = c.Int(nullable: false),
                        EstimatedUnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApprovedBudgetForItem = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UOMReference = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Item_Articles", t => t.ArticleReference)
                .ForeignKey("dbo.PROC_TRXN_Procurement_Project", t => t.ProcurementProjectReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.UOMReference, cascadeDelete: true)
                .Index(t => t.ProcurementProjectReference)
                .Index(t => t.ArticleReference)
                .Index(t => t.UOMReference);
            
            CreateTable(
                "dbo.PROP_MSTR_PropertyCards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PropertyReference = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Organization = c.String(),
                        Reference = c.String(),
                        ReferenceType = c.Int(nullable: false),
                        ReceiptQty = c.Int(nullable: false),
                        ReceiptUnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IssueQty = c.Int(nullable: false),
                        IssueUnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceQty = c.Int(nullable: false),
                        BalanceUnitCost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROP_MSTR_PPE", t => t.PropertyReference, cascadeDelete: true)
                .Index(t => t.PropertyReference);
            
            CreateTable(
                "dbo.PROC_TRXN_Purchase_Request_Details",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PRHeaderReference = c.Int(nullable: false),
                        ClassificationID = c.Int(nullable: false),
                        ArticleReference = c.Int(),
                        ItemSequence = c.String(maxLength: 2),
                        ItemFullName = c.String(nullable: false, maxLength: 200, unicode: false),
                        ItemSpecifications = c.String(),
                        UOMReference = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Item_Articles", t => t.ArticleReference)
                .ForeignKey("dbo.PROC_TRXN_Purchase_Request", t => t.PRHeaderReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.UOMReference, cascadeDelete: true)
                .Index(t => t.PRHeaderReference)
                .Index(t => t.ArticleReference)
                .Index(t => t.UOMReference);
            
            CreateTable(
                "dbo.PROC_USRM_User_Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROP_MSTR_StockCards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SupplyID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Organization = c.String(),
                        Reference = c.String(),
                        FundSource = c.String(),
                        ReferenceType = c.Int(nullable: false),
                        ReceiptQty = c.Int(nullable: false),
                        ReceiptUnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceiptTotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IssuedQty = c.Int(nullable: false),
                        IssuedUnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IssuedTotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceQty = c.Int(nullable: false),
                        BalanceUnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BalanceTotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROP_MSTR_Supplies", t => t.SupplyID, cascadeDelete: true)
                .Index(t => t.SupplyID);
            
            CreateTable(
                "dbo.PROP_MSTR_Supplies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StockNumber = c.String(nullable: false, maxLength: 50, unicode: false),
                        ArticleReference = c.Int(nullable: false),
                        Sequence = c.String(nullable: false, maxLength: 2),
                        StockSequence = c.String(nullable: false, maxLength: 2),
                        Description = c.String(nullable: false, maxLength: 200, unicode: false),
                        ItemImage = c.Binary(),
                        ReOrderPoint = c.Int(nullable: false),
                        IndividualUOMReference = c.Int(),
                        MinimumIssuanceQty = c.Int(nullable: false),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_MSTR_Item_Articles", t => t.ArticleReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_UnitsOfMeasure", t => t.IndividualUOMReference)
                .Index(t => t.ArticleReference)
                .Index(t => t.IndividualUOMReference);
            
            CreateTable(
                "dbo.PROC_MSTR_Suppliers_Categories",
                c => new
                    {
                        SupplierReference = c.Int(nullable: false),
                        CategoryReference = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SupplierReference, t.CategoryReference })
                .ForeignKey("dbo.PROC_MSTR_Item_Categories", t => t.CategoryReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Suppliers", t => t.SupplierReference, cascadeDelete: true)
                .Index(t => t.SupplierReference)
                .Index(t => t.CategoryReference);
            
            CreateTable(
                "dbo.PROC_MSTR_Suppliers_ItemTypes",
                c => new
                    {
                        SupplierReference = c.Int(nullable: false),
                        ItemTypeReference = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SupplierReference, t.ItemTypeReference })
                .ForeignKey("dbo.PROC_MSTR_Item_Types", t => t.ItemTypeReference, cascadeDelete: true)
                .ForeignKey("dbo.PROC_MSTR_Suppliers", t => t.SupplierReference, cascadeDelete: true)
                .Index(t => t.SupplierReference)
                .Index(t => t.ItemTypeReference);
            
            CreateTable(
                "dbo.PROP_TRXN_Delivery_Supplies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SupplyReference = c.Int(nullable: false),
                        DeliveryReference = c.Int(),
                        QuantityDelivered = c.Int(nullable: false),
                        QuantityBacklog = c.Int(nullable: false),
                        ReceiptUnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceiptTotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROP_TRXN_Delivery", t => t.DeliveryReference)
                .ForeignKey("dbo.PROP_MSTR_Supplies", t => t.SupplyReference, cascadeDelete: true)
                .Index(t => t.SupplyReference)
                .Index(t => t.DeliveryReference);
            
            CreateTable(
                "dbo.PROP_TRXN_Inspection_Supplies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        InspectionReference = c.Int(nullable: false),
                        SupplyReference = c.Int(nullable: false),
                        UnitReference = c.Int(nullable: false),
                        QuantityReceived = c.Int(nullable: false),
                        QuantityAccepted = c.Int(nullable: false),
                        QuantityRejected = c.Int(nullable: false),
                        InspectionNotes = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROP_TRXN_Inspection", t => t.InspectionReference, cascadeDelete: true)
                .ForeignKey("dbo.PROP_MSTR_Supplies", t => t.SupplyReference, cascadeDelete: true)
                .Index(t => t.InspectionReference)
                .Index(t => t.SupplyReference);
            
            CreateTable(
                "dbo.PROC_SYTM_SwitchBoard",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DepartmentCode = c.String(nullable: false, maxLength: 8000, unicode: false),
                        MessageType = c.String(nullable: false, maxLength: 75, unicode: false),
                        Reference = c.String(nullable: false, maxLength: 75, unicode: false),
                        Subject = c.String(nullable: false, maxLength: 250, unicode: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PROC_SYTM_SwitchBoard_Body",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SwitchBoardReference = c.Int(nullable: false),
                        Remarks = c.String(nullable: false, maxLength: 250, unicode: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        ActionBy = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DepartmentCode = c.String(nullable: false, maxLength: 8000, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_SYTM_SwitchBoard", t => t.SwitchBoardReference, cascadeDelete: true)
                .Index(t => t.SwitchBoardReference);
            
            CreateTable(
                "dbo.PROC_USRM_User_Accounts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        EmpCode = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DepartmentCode = c.String(nullable: false, maxLength: 8000, unicode: false),
                        UnitCode = c.String(nullable: false, maxLength: 8000, unicode: false),
                        RoleReference = c.Int(),
                        IsLockedOut = c.Boolean(nullable: false),
                        LockoutDuration = c.DateTime(),
                        PurgeFlag = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(),
                        DeletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PROC_USRM_User_Role", t => t.RoleReference)
                .Index(t => t.RoleReference);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PROC_USRM_User_Accounts", "RoleReference", "dbo.PROC_USRM_User_Role");
            DropForeignKey("dbo.PROC_SYTM_SwitchBoard_Body", "SwitchBoardReference", "dbo.PROC_SYTM_SwitchBoard");
            DropForeignKey("dbo.PROP_TRXN_Inspection_Supplies", "SupplyReference", "dbo.PROP_MSTR_Supplies");
            DropForeignKey("dbo.PROP_TRXN_Inspection_Supplies", "InspectionReference", "dbo.PROP_TRXN_Inspection");
            DropForeignKey("dbo.PROP_TRXN_Delivery_Supplies", "SupplyReference", "dbo.PROP_MSTR_Supplies");
            DropForeignKey("dbo.PROP_TRXN_Delivery_Supplies", "DeliveryReference", "dbo.PROP_TRXN_Delivery");
            DropForeignKey("dbo.PROC_MSTR_Suppliers_ItemTypes", "SupplierReference", "dbo.PROC_MSTR_Suppliers");
            DropForeignKey("dbo.PROC_MSTR_Suppliers_ItemTypes", "ItemTypeReference", "dbo.PROC_MSTR_Item_Types");
            DropForeignKey("dbo.PROC_MSTR_Suppliers_Categories", "SupplierReference", "dbo.PROC_MSTR_Suppliers");
            DropForeignKey("dbo.PROC_MSTR_Suppliers_Categories", "CategoryReference", "dbo.PROC_MSTR_Item_Categories");
            DropForeignKey("dbo.PROP_MSTR_StockCards", "SupplyID", "dbo.PROP_MSTR_Supplies");
            DropForeignKey("dbo.PROP_MSTR_Supplies", "IndividualUOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROP_MSTR_Supplies", "ArticleReference", "dbo.PROC_MSTR_Item_Articles");
            DropForeignKey("dbo.PROC_TRXN_Purchase_Request_Details", "UOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROC_TRXN_Purchase_Request_Details", "PRHeaderReference", "dbo.PROC_TRXN_Purchase_Request");
            DropForeignKey("dbo.PROC_TRXN_Purchase_Request_Details", "ArticleReference", "dbo.PROC_MSTR_Item_Articles");
            DropForeignKey("dbo.PROP_MSTR_PropertyCards", "PropertyReference", "dbo.PROP_MSTR_PPE");
            DropForeignKey("dbo.PROC_TRXN_Procurement_Project_Details", "UOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROC_TRXN_Procurement_Project_Details", "ProcurementProjectReference", "dbo.PROC_TRXN_Procurement_Project");
            DropForeignKey("dbo.PROC_TRXN_Procurement_Project_Details", "ArticleReference", "dbo.PROC_MSTR_Item_Articles");
            DropForeignKey("dbo.PROC_TRXN_PPMP_Details", "UOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROC_TRXN_PPMP_Details", "PurchaseRequestReference", "dbo.PROC_TRXN_Purchase_Request");
            DropForeignKey("dbo.PROC_TRXN_Purchase_Request", "ProcurementProjectReference", "dbo.PROC_TRXN_Procurement_Project");
            DropForeignKey("dbo.PROC_TRXN_PPMP_Details", "ProjectDetailsID", "dbo.PROC_TRXN_Project_Plan_Details");
            DropForeignKey("dbo.PROC_TRXN_Project_Plan_Details", "UOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROC_TRXN_Project_Plan_Details", "Supplier3Reference", "dbo.PROC_MSTR_Suppliers");
            DropForeignKey("dbo.PROC_TRXN_Project_Plan_Details", "Supplier2Reference", "dbo.PROC_MSTR_Suppliers");
            DropForeignKey("dbo.PROC_TRXN_Project_Plan_Details", "Supplier1Reference", "dbo.PROC_MSTR_Suppliers");
            DropForeignKey("dbo.PROC_TRXN_Project_Plan_Details", "ProjectReference", "dbo.PROC_TRXN_Project_Plan");
            DropForeignKey("dbo.PROC_TRXN_Project_Plan_Details", "ArticleReference", "dbo.PROC_MSTR_Item_Articles");
            DropForeignKey("dbo.PROC_TRXN_Project_Plan_Details", "ClassificationReference", "dbo.PROC_MSTR_Item_Classification");
            DropForeignKey("dbo.PROC_TRXN_Project_Plan_Details", "CategoryReference", "dbo.PROC_MSTR_Item_Categories");
            DropForeignKey("dbo.PROC_TRXN_PPMP_Details", "ProcurementProject", "dbo.PROC_TRXN_Procurement_Project");
            DropForeignKey("dbo.PROC_TRXN_PPMP_Details", "PPMPHeaderReference", "dbo.PROC_TRXN_PPMP");
            DropForeignKey("dbo.PROC_TRXN_PPMP_Details", "ArticleReference", "dbo.PROC_MSTR_Item_Articles");
            DropForeignKey("dbo.PROC_TRXN_PPMP_Details", "ClassificationReference", "dbo.PROC_MSTR_Item_Classification");
            DropForeignKey("dbo.PROC_TRXN_PPMP_Details", "CategoryReference", "dbo.PROC_MSTR_Item_Categories");
            DropForeignKey("dbo.PROC_TRXN_PPMP_Details", "APPDetailReference", "dbo.PROC_TRXN_Annual_Procurement_Plan_Details");
            DropForeignKey("dbo.PROP_TRXN_Inspection_PPE", "PropertyReference", "dbo.PROP_MSTR_PPE");
            DropForeignKey("dbo.PROP_TRXN_Inspection_PPE", "InspectionReference", "dbo.PROP_TRXN_Inspection");
            DropForeignKey("dbo.PROP_TRXN_Delivery_PPE", "PPEReference", "dbo.PROP_MSTR_PPE");
            DropForeignKey("dbo.PROP_TRXN_Delivery_PPE", "DeliveryReference", "dbo.PROP_TRXN_Delivery");
            DropForeignKey("dbo.PROP_MSTR_PPE", "IndividualUOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROP_MSTR_PPE", "ArticleReference", "dbo.PROC_MSTR_Item_Articles");
            DropForeignKey("dbo.PROC_TRXN_Market_Survey", "Supplier3Reference", "dbo.PROC_MSTR_Suppliers");
            DropForeignKey("dbo.PROC_TRXN_Market_Survey", "Supplier2Reference", "dbo.PROC_MSTR_Suppliers");
            DropForeignKey("dbo.PROC_TRXN_Market_Survey", "Supplier1Reference", "dbo.PROC_MSTR_Suppliers");
            DropForeignKey("dbo.PROC_TRXN_Market_Survey", "ArticleReference", "dbo.PROC_MSTR_Item_Articles");
            DropForeignKey("dbo.PROC_MSTR_Item_Price_Catalogue", "Item", "dbo.PROC_MSTR_Item");
            DropForeignKey("dbo.PROC_MSTR_Item_Allowed_Users", "ItemReference", "dbo.PROC_MSTR_Item");
            DropForeignKey("dbo.PROC_MSTR_Item", "PackagingUOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROC_MSTR_Item", "IndividualUOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROC_MSTR_Item", "CategoryReference", "dbo.PROC_MSTR_Item_Categories");
            DropForeignKey("dbo.PROC_MSTR_Item", "ArticleReference", "dbo.PROC_MSTR_Item_Articles");
            DropForeignKey("dbo.PROP_TRXN_Inspection", "SupplierReference", "dbo.PROC_MSTR_Suppliers");
            DropForeignKey("dbo.PROP_TRXN_Inspection", "DeliveryReference", "dbo.PROP_TRXN_Delivery");
            DropForeignKey("dbo.PROC_TRXN_Infrastructure_Detailed_Estimate", "InfrastructureWorkRequirement", "dbo.PROC_MSTR_Infrastructure_Requirements");
            DropForeignKey("dbo.PROC_MSTR_Infrastructure_Requirements", "RequirementClassificationReference", "dbo.PROC_MSTR_Infrastructure_Classification");
            DropForeignKey("dbo.PROC_TRXN_Infrastructure_Detailed_Estimate", "InfrastructureWorkClassification", "dbo.PROC_MSTR_Infrastructure_Classification");
            DropForeignKey("dbo.PROC_TRXN_Infrastructure_Detailed_Estimate", "UOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROC_TRXN_Infrastructure_Detailed_Estimate", "InfrastructureProjectReference", "dbo.PROC_TRXN_Infrastructure_Project");
            DropForeignKey("dbo.PROC_TRXN_Infrastructure_Project", "RequestReference", "dbo.PROC_TRXN_Project_Infrastructure_Request");
            DropForeignKey("dbo.PROC_TRXN_Project_Infrastructure_Request", "ProjectReference", "dbo.PROC_TRXN_Project_Plan");
            DropForeignKey("dbo.PROC_TRXN_Project_Plan", "ParentProject", "dbo.PROC_TRXN_Project_Plan");
            DropForeignKey("dbo.PROC_TRXN_Infrastructure_Detailed_Estimate", "InfrastructureMaterialReference", "dbo.PROC_MSTR_Infrastructure_Materials");
            DropForeignKey("dbo.PROC_MSTR_Infrastructure_Materials", "UOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROP_TRXN_Delivery", "ContractReference", "dbo.PROC_TRXN_Contract");
            DropForeignKey("dbo.PROC_TRXN_Procurement_Project_Updates", "ProcurementProjectReference", "dbo.PROC_TRXN_Procurement_Project");
            DropForeignKey("dbo.PROC_TRXN_Contract_Monitoring_Updates", "ContractReference", "dbo.PROC_TRXN_Contract");
            DropForeignKey("dbo.PROC_TRXN_Contract_Details", "UOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROC_TRXN_Contract_Details", "ContractReference", "dbo.PROC_TRXN_Contract");
            DropForeignKey("dbo.PROC_TRXN_Contract_Details", "ArticleReference", "dbo.PROC_MSTR_Item_Articles");
            DropForeignKey("dbo.PROC_TRXN_Contract", "SupplierReference", "dbo.PROC_MSTR_Suppliers");
            DropForeignKey("dbo.PROC_TRXN_Contract", "ProcurementProjectReference", "dbo.PROC_TRXN_Procurement_Project");
            DropForeignKey("dbo.PROC_TRXN_Procurement_Project", "ParentProjectReference", "dbo.PROC_TRXN_Procurement_Project");
            DropForeignKey("dbo.PROC_TRXN_Procurement_Project", "ModeOfProcurementReference", "dbo.PROC_MSTR_Procurement_Modes");
            DropForeignKey("dbo.PROC_TRXN_Procurement_Project", "ClassificationReference", "dbo.PROC_MSTR_Item_Classification");
            DropForeignKey("dbo.PROC_TRXN_Procurement_Project", "APPReference", "dbo.PROC_TRXN_Annual_Procurement_Plan_Details");
            DropForeignKey("dbo.PROC_TRXN_Agency_Procurement_Request_Details", "UOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROC_TRXN_Agency_Procurement_Request_Details", "ArticleReference", "dbo.PROC_MSTR_Item_Articles");
            DropForeignKey("dbo.PROC_TRXN_Agency_Procurement_Request_Details", "APRReference", "dbo.PROC_TRXN_Agency_Procurement_Request");
            DropForeignKey("dbo.PROC_TRXN_Annual_Procurement_Plan_Details", "ClassificationReference", "dbo.PROC_MSTR_Item_Classification");
            DropForeignKey("dbo.PROC_TRXN_Annual_Procurement_Plan_Details", "APPHeaderReference", "dbo.PROC_TRXN_Annual_Procurement_Plan");
            DropForeignKey("dbo.PROC_TRXN_Annual_Procurement_Plan_CSE", "UOMReference", "dbo.PROC_MSTR_UnitsOfMeasure");
            DropForeignKey("dbo.PROC_TRXN_Annual_Procurement_Plan_CSE", "CategoryReference", "dbo.PROC_MSTR_Item_Categories");
            DropForeignKey("dbo.PROC_TRXN_Annual_Procurement_Plan_CSE", "ArticleReference", "dbo.PROC_MSTR_Item_Articles");
            DropForeignKey("dbo.PROC_MSTR_Item_Articles", "ItemTypeReference", "dbo.PROC_MSTR_Item_Types");
            DropForeignKey("dbo.PROC_MSTR_Item_Types", "ItemClassificationReference", "dbo.PROC_MSTR_Item_Classification");
            DropForeignKey("dbo.PROC_TRXN_Annual_Procurement_Plan_CSE", "APPHeaderReference", "dbo.PROC_TRXN_Annual_Procurement_Plan");
            DropIndex("dbo.PROC_USRM_User_Accounts", new[] { "RoleReference" });
            DropIndex("dbo.PROC_SYTM_SwitchBoard_Body", new[] { "SwitchBoardReference" });
            DropIndex("dbo.PROP_TRXN_Inspection_Supplies", new[] { "SupplyReference" });
            DropIndex("dbo.PROP_TRXN_Inspection_Supplies", new[] { "InspectionReference" });
            DropIndex("dbo.PROP_TRXN_Delivery_Supplies", new[] { "DeliveryReference" });
            DropIndex("dbo.PROP_TRXN_Delivery_Supplies", new[] { "SupplyReference" });
            DropIndex("dbo.PROC_MSTR_Suppliers_ItemTypes", new[] { "ItemTypeReference" });
            DropIndex("dbo.PROC_MSTR_Suppliers_ItemTypes", new[] { "SupplierReference" });
            DropIndex("dbo.PROC_MSTR_Suppliers_Categories", new[] { "CategoryReference" });
            DropIndex("dbo.PROC_MSTR_Suppliers_Categories", new[] { "SupplierReference" });
            DropIndex("dbo.PROP_MSTR_Supplies", new[] { "IndividualUOMReference" });
            DropIndex("dbo.PROP_MSTR_Supplies", new[] { "ArticleReference" });
            DropIndex("dbo.PROP_MSTR_StockCards", new[] { "SupplyID" });
            DropIndex("dbo.PROC_TRXN_Purchase_Request_Details", new[] { "UOMReference" });
            DropIndex("dbo.PROC_TRXN_Purchase_Request_Details", new[] { "ArticleReference" });
            DropIndex("dbo.PROC_TRXN_Purchase_Request_Details", new[] { "PRHeaderReference" });
            DropIndex("dbo.PROP_MSTR_PropertyCards", new[] { "PropertyReference" });
            DropIndex("dbo.PROC_TRXN_Procurement_Project_Details", new[] { "UOMReference" });
            DropIndex("dbo.PROC_TRXN_Procurement_Project_Details", new[] { "ArticleReference" });
            DropIndex("dbo.PROC_TRXN_Procurement_Project_Details", new[] { "ProcurementProjectReference" });
            DropIndex("dbo.PROC_TRXN_Purchase_Request", new[] { "ProcurementProjectReference" });
            DropIndex("dbo.PROC_TRXN_Project_Plan_Details", new[] { "Supplier3Reference" });
            DropIndex("dbo.PROC_TRXN_Project_Plan_Details", new[] { "Supplier2Reference" });
            DropIndex("dbo.PROC_TRXN_Project_Plan_Details", new[] { "Supplier1Reference" });
            DropIndex("dbo.PROC_TRXN_Project_Plan_Details", new[] { "CategoryReference" });
            DropIndex("dbo.PROC_TRXN_Project_Plan_Details", new[] { "UOMReference" });
            DropIndex("dbo.PROC_TRXN_Project_Plan_Details", new[] { "ArticleReference" });
            DropIndex("dbo.PROC_TRXN_Project_Plan_Details", new[] { "ClassificationReference" });
            DropIndex("dbo.PROC_TRXN_Project_Plan_Details", new[] { "ProjectReference" });
            DropIndex("dbo.PROC_TRXN_PPMP_Details", new[] { "PurchaseRequestReference" });
            DropIndex("dbo.PROC_TRXN_PPMP_Details", new[] { "ProcurementProject" });
            DropIndex("dbo.PROC_TRXN_PPMP_Details", new[] { "CategoryReference" });
            DropIndex("dbo.PROC_TRXN_PPMP_Details", new[] { "UOMReference" });
            DropIndex("dbo.PROC_TRXN_PPMP_Details", new[] { "ArticleReference" });
            DropIndex("dbo.PROC_TRXN_PPMP_Details", new[] { "ClassificationReference" });
            DropIndex("dbo.PROC_TRXN_PPMP_Details", new[] { "APPDetailReference" });
            DropIndex("dbo.PROC_TRXN_PPMP_Details", new[] { "PPMPHeaderReference" });
            DropIndex("dbo.PROC_TRXN_PPMP_Details", new[] { "ProjectDetailsID" });
            DropIndex("dbo.PROP_TRXN_Inspection_PPE", new[] { "PropertyReference" });
            DropIndex("dbo.PROP_TRXN_Inspection_PPE", new[] { "InspectionReference" });
            DropIndex("dbo.PROP_TRXN_Delivery_PPE", new[] { "DeliveryReference" });
            DropIndex("dbo.PROP_TRXN_Delivery_PPE", new[] { "PPEReference" });
            DropIndex("dbo.PROP_MSTR_PPE", new[] { "IndividualUOMReference" });
            DropIndex("dbo.PROP_MSTR_PPE", new[] { "ArticleReference" });
            DropIndex("dbo.PROC_TRXN_Market_Survey", new[] { "Supplier3Reference" });
            DropIndex("dbo.PROC_TRXN_Market_Survey", new[] { "Supplier2Reference" });
            DropIndex("dbo.PROC_TRXN_Market_Survey", new[] { "Supplier1Reference" });
            DropIndex("dbo.PROC_TRXN_Market_Survey", new[] { "ArticleReference" });
            DropIndex("dbo.PROC_MSTR_Item_Price_Catalogue", new[] { "Item" });
            DropIndex("dbo.PROC_MSTR_Item", new[] { "IndividualUOMReference" });
            DropIndex("dbo.PROC_MSTR_Item", new[] { "PackagingUOMReference" });
            DropIndex("dbo.PROC_MSTR_Item", new[] { "CategoryReference" });
            DropIndex("dbo.PROC_MSTR_Item", new[] { "ArticleReference" });
            DropIndex("dbo.PROC_MSTR_Item_Allowed_Users", new[] { "ItemReference" });
            DropIndex("dbo.PROP_TRXN_Inspection", new[] { "DeliveryReference" });
            DropIndex("dbo.PROP_TRXN_Inspection", new[] { "SupplierReference" });
            DropIndex("dbo.PROC_MSTR_Infrastructure_Requirements", new[] { "RequirementClassificationReference" });
            DropIndex("dbo.PROC_TRXN_Project_Plan", new[] { "ParentProject" });
            DropIndex("dbo.PROC_TRXN_Project_Infrastructure_Request", new[] { "ProjectReference" });
            DropIndex("dbo.PROC_TRXN_Infrastructure_Project", new[] { "RequestReference" });
            DropIndex("dbo.PROC_MSTR_Infrastructure_Materials", new[] { "UOMReference" });
            DropIndex("dbo.PROC_TRXN_Infrastructure_Detailed_Estimate", new[] { "UOMReference" });
            DropIndex("dbo.PROC_TRXN_Infrastructure_Detailed_Estimate", new[] { "InfrastructureWorkRequirement" });
            DropIndex("dbo.PROC_TRXN_Infrastructure_Detailed_Estimate", new[] { "InfrastructureWorkClassification" });
            DropIndex("dbo.PROC_TRXN_Infrastructure_Detailed_Estimate", new[] { "InfrastructureMaterialReference" });
            DropIndex("dbo.PROC_TRXN_Infrastructure_Detailed_Estimate", new[] { "InfrastructureProjectReference" });
            DropIndex("dbo.PROP_TRXN_Delivery", new[] { "ContractReference" });
            DropIndex("dbo.PROC_TRXN_Procurement_Project_Updates", new[] { "ProcurementProjectReference" });
            DropIndex("dbo.PROC_TRXN_Contract_Monitoring_Updates", new[] { "ContractReference" });
            DropIndex("dbo.PROC_TRXN_Contract_Details", new[] { "UOMReference" });
            DropIndex("dbo.PROC_TRXN_Contract_Details", new[] { "ArticleReference" });
            DropIndex("dbo.PROC_TRXN_Contract_Details", new[] { "ContractReference" });
            DropIndex("dbo.PROC_TRXN_Procurement_Project", new[] { "ModeOfProcurementReference" });
            DropIndex("dbo.PROC_TRXN_Procurement_Project", new[] { "ClassificationReference" });
            DropIndex("dbo.PROC_TRXN_Procurement_Project", new[] { "APPReference" });
            DropIndex("dbo.PROC_TRXN_Procurement_Project", new[] { "ParentProjectReference" });
            DropIndex("dbo.PROC_TRXN_Contract", new[] { "SupplierReference" });
            DropIndex("dbo.PROC_TRXN_Contract", new[] { "ProcurementProjectReference" });
            DropIndex("dbo.PROC_TRXN_Agency_Procurement_Request_Details", new[] { "APRReference" });
            DropIndex("dbo.PROC_TRXN_Agency_Procurement_Request_Details", new[] { "UOMReference" });
            DropIndex("dbo.PROC_TRXN_Agency_Procurement_Request_Details", new[] { "ArticleReference" });
            DropIndex("dbo.PROC_TRXN_Annual_Procurement_Plan_Details", new[] { "ClassificationReference" });
            DropIndex("dbo.PROC_TRXN_Annual_Procurement_Plan_Details", new[] { "APPHeaderReference" });
            DropIndex("dbo.PROC_MSTR_Item_Types", new[] { "ItemClassificationReference" });
            DropIndex("dbo.PROC_MSTR_Item_Articles", new[] { "ItemTypeReference" });
            DropIndex("dbo.PROC_TRXN_Annual_Procurement_Plan_CSE", new[] { "CategoryReference" });
            DropIndex("dbo.PROC_TRXN_Annual_Procurement_Plan_CSE", new[] { "UOMReference" });
            DropIndex("dbo.PROC_TRXN_Annual_Procurement_Plan_CSE", new[] { "ArticleReference" });
            DropIndex("dbo.PROC_TRXN_Annual_Procurement_Plan_CSE", new[] { "APPHeaderReference" });
            DropTable("dbo.PROC_USRM_User_Accounts");
            DropTable("dbo.PROC_SYTM_SwitchBoard_Body");
            DropTable("dbo.PROC_SYTM_SwitchBoard");
            DropTable("dbo.PROP_TRXN_Inspection_Supplies");
            DropTable("dbo.PROP_TRXN_Delivery_Supplies");
            DropTable("dbo.PROC_MSTR_Suppliers_ItemTypes");
            DropTable("dbo.PROC_MSTR_Suppliers_Categories");
            DropTable("dbo.PROP_MSTR_Supplies");
            DropTable("dbo.PROP_MSTR_StockCards");
            DropTable("dbo.PROC_USRM_User_Role");
            DropTable("dbo.PROC_TRXN_Purchase_Request_Details");
            DropTable("dbo.PROP_MSTR_PropertyCards");
            DropTable("dbo.PROC_TRXN_Procurement_Project_Details");
            DropTable("dbo.PROC_TRXN_Purchase_Request");
            DropTable("dbo.PROC_TRXN_Project_Plan_Details");
            DropTable("dbo.PROC_TRXN_PPMP");
            DropTable("dbo.PROC_TRXN_PPMP_Details");
            DropTable("dbo.PROC_SYTM_PPMP_Deadlines");
            DropTable("dbo.PROP_TRXN_Inspection_PPE");
            DropTable("dbo.PROP_TRXN_Delivery_PPE");
            DropTable("dbo.PROP_MSTR_PPE");
            DropTable("dbo.PROC_TRXN_Market_Survey");
            DropTable("dbo.PROC_LOGS_Master_Tables");
            DropTable("dbo.PROC_MSTR_Item_Price_Catalogue");
            DropTable("dbo.PROC_MSTR_Item");
            DropTable("dbo.PROC_MSTR_Item_Allowed_Users");
            DropTable("dbo.PROP_TRXN_Inspection");
            DropTable("dbo.PROC_MSTR_Infrastructure_Requirements");
            DropTable("dbo.PROC_MSTR_Infrastructure_Classification");
            DropTable("dbo.PROC_TRXN_Project_Plan");
            DropTable("dbo.PROC_TRXN_Project_Infrastructure_Request");
            DropTable("dbo.PROC_TRXN_Infrastructure_Project");
            DropTable("dbo.PROC_MSTR_Infrastructure_Materials");
            DropTable("dbo.PROC_TRXN_Infrastructure_Detailed_Estimate");
            DropTable("dbo.PROP_TRXN_Delivery");
            DropTable("dbo.PROC_TRXN_Procurement_Project_Updates");
            DropTable("dbo.PROC_TRXN_Contract_Monitoring_Updates");
            DropTable("dbo.PROC_TRXN_Contract_Details");
            DropTable("dbo.PROC_MSTR_Suppliers");
            DropTable("dbo.PROC_MSTR_Procurement_Modes");
            DropTable("dbo.PROC_TRXN_Procurement_Project");
            DropTable("dbo.PROC_TRXN_Contract");
            DropTable("dbo.PROC_SYTM_BAC_Secretariat");
            DropTable("dbo.PROC_TRXN_Agency_Procurement_Request");
            DropTable("dbo.PROC_TRXN_Agency_Procurement_Request_Details");
            DropTable("dbo.PROC_TRXN_Annual_Procurement_Plan_Details");
            DropTable("dbo.PROC_MSTR_UnitsOfMeasure");
            DropTable("dbo.PROC_MSTR_Item_Categories");
            DropTable("dbo.PROC_MSTR_Item_Classification");
            DropTable("dbo.PROC_MSTR_Item_Types");
            DropTable("dbo.PROC_MSTR_Item_Articles");
            DropTable("dbo.PROC_TRXN_Annual_Procurement_Plan");
            DropTable("dbo.PROC_TRXN_Annual_Procurement_Plan_CSE");
            DropTable("dbo.PROC_SYTM_Agency_Details");
        }
    }
}
