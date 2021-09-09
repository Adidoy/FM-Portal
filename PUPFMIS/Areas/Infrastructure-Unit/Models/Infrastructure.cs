using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PUPFMIS.Models
{
    [Table("PROC_MSTR_Infrastructure_Work_Classification")]
    public class InfrastructureRequirementsClassification
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Classification Name")]
        public string ClassificationName { get; set; }

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

    [Table("PROC_MSTR_Infrastructure_Work_Requirements")]
    public class InfrastructureRequirements
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Requirement")]
        public string Requirement { get; set; }

        [Display(Name = "Requirements Classification")]
        public int RequirementClassificationReference { get; set; }

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

        [ForeignKey("RequirementClassificationReference")]
        public virtual InfrastructureRequirementsClassification FKRequirementClassReference { get; set; }
    }

    [Table("PROC_TRXN_Infrastructure_Project")]
    public class InfrastructureProject
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(75)]
        [Display(Name = "Infrastructure Project Code")]
        public string InfraProjectCode { get; set; }

        [MaxLength(150)]
        [Display(Name = "Project Title")]
        public string ProjectTitle { get; set; }

        [MaxLength(150)]
        [Display(Name = "Project Location")]
        public string ProjectLocation { get; set; }

        [Display(Name = "Contract Duration (Calendar Days)")]
        public int ContractDuration { get; set; }

        [Display(Name = "End-User Project Reference")]
        public int? EndUserProjectReference { get; set; }

        [Display(Name = "Article Reference")]
        public int? ArticleReference { get; set; }

        [Display(Name = "Item Sequence")]
        public string ItemSequence { get; set; }

        [ForeignKey("EndUserProjectReference")]
        public virtual ProjectPlans FKEndUserProjectReference { get; set; }

        [ForeignKey("ArticleReference")]
        public virtual ItemArticles FKArticleReference { get; set; }
    }

    [Table("PROC_TRXN_Infrastructure_Project_Detailed_Estimate")]
    public class InfrastructureDetailedEstimate
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Project Reference")]
        public int InfrastructureProjectReference { get; set; }

        [Display(Name = "Material")]
        public int InfrastructureMaterialReference { get; set; }

        [Display(Name = "Classification")]
        public int InfrastructureWorkClassification { get; set; }

        [Display(Name = "Work Requirement")]
        public int? InfrastructureWorkRequirement { get; set; }

        [Display(Name = "Unit of Measure")]
        public int? UOMReference { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal ItemUnitCost { get; set; }

        [Display(Name = "Total Cost")]
        public decimal ItemTotalCost { get; set; }

        [Display(Name = "Unit Cost")]
        public decimal LaborUnitCost { get; set; }

        [Display(Name = "Total Cost")]
        public decimal LaborTotalCost { get; set; }

        [Display(Name = "Estsimated Direct Cost")]
        public decimal EstimatedDirectCost { get; set; }

        [Display(Name = "Mobilization/Demobilization (1% of Direct Cost)")]
        public decimal MobDemobilizationCost { get; set; }

        [Display(Name = "OCM (12% of Direct Cost)")]
        public decimal OCMCost { get; set; }

        [Display(Name = "Profit (12% of Direct Cost)")]
        public decimal ProfitCost { get; set; }

        [Display(Name = "Total Mark-up")]
        public decimal TotalMarkUp { get; set; }

        [Display(Name = "VAT (12% of Direct Cost + Total Mark-up)")]
        public decimal VAT { get; set; }

        [Display(Name = "Total Indirect Cost")]
        public decimal TotalIndirectCost { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [ForeignKey("InfrastructureProjectReference")]
        public virtual InfrastructureProject FKInfraProjectReference { get; set; }

        [ForeignKey("InfrastructureMaterialReference")]
        public virtual InfrastructureMaterials FKInfraMaterialsReference { get; set; }

        [ForeignKey("InfrastructureWorkClassification")]
        public virtual InfrastructureRequirementsClassification FKWorkClass { get; set; }

        [ForeignKey("InfrastructureWorkRequirement")]
        public virtual InfrastructureRequirements FKWorkRequirement { get; set; }

        [ForeignKey("UOMReference")]
        public virtual UnitOfMeasure FKUOMReference { get; set; }
    }


    public class InfrastructureDetailedEstimateVM
    {
        public int ID { get; set; }

        [Display(Name = "Material")]
        public string Material { get; set; }

        [Display(Name = "Material Specification")]
        public string MaterialSpecification { get; set; }

        [Display(Name = "Classification")]
        public string Classification { get; set; }

        [Display(Name = "Work Requirement")]
        public string Requirement { get; set; }

        [Display(Name = "Unit of Measure")]
        public string UOM { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Material Unit Cost")]
        public decimal ItemUnitCost { get; set; }

        [Display(Name = "Total Cost")]
        public decimal ItemTotalCost { get; set; }

        [Display(Name = "Labor Unit Cost")]
        public decimal LaborUnitCost { get; set; }

        [Display(Name = "Total Cost")]
        public decimal LaborTotalCost { get; set; }

        [Display(Name = "Estsimated Direct Cost")]
        public decimal EstimatedDirectCost { get; set; }

        [Display(Name = "Mobilization/Demobilization (1% of DC)")]
        public decimal MobDemobilizationCost { get; set; }

        [Display(Name = "OCM (12% of DC)")]
        public decimal OCMCost { get; set; }

        [Display(Name = "Profit (12% of DC)")]
        public decimal ProfitCost { get; set; }

        [Display(Name = "Total Mark-up")]
        public decimal TotalMarkUp { get; set; }

        [Display(Name = "VAT (12% of DC + Total Mark-up)")]
        public decimal VAT { get; set; }

        [Display(Name = "Total Indirect Cost")]
        public decimal TotalIndirectCost { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
    }
    public class InfrastructureProjectVM
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(75)]
        [Display(Name = "Infrastructure Project Code")]
        public string InfraProjectCode { get; set; }

        [Display(Name = "PAP Code")]
        public string PAPCode { get; set; }

        [Display(Name = "Program")]
        public string Program { get; set; }

        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        public string EndUserProjectName { get; set; }

        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Unit")]
        public string Unit { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Delivery Month")]
        public string DeliveryMonth { get; set; }

        [Display(Name = "Fiscal Year")]
        public int FiscalYear { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Infrastructure Project Type")]
        public string InfraProjectType { get; set; }

        [MaxLength(150)]
        [Display(Name = "Project Title")]
        public string ProjectTitle { get; set; }

        [MaxLength(150)]
        [Display(Name = "Project Location")]
        public string ProjectLocation { get; set; }

        [Display(Name = "Contract Duration (Calendar Days)")]
        public int ContractDuration { get; set; }

        [Display(Name = "End-User Project Reference")]
        public int? EndUserProjectReference { get; set; }
        public List<InfrastructureDetailedEstimateVM> DetailedEstimates { get; set; }
    }
    public class InfrastructureRequestsVM
    {
        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectTitle { get; set; }

        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Infrastructure Type")]
        public string InfrastructureType { get; set; }
    }


    public class InfrastructureRequirementsClassificationValidator : AbstractValidator<InfrastructureRequirementsClassification>
    {
        FMISDbContext db = new FMISDbContext();
        public InfrastructureRequirementsClassificationValidator()
        {
            RuleFor(x => x.ClassificationName).Must(NotBeDeleted).WithMessage("Classification was recently deleted. If you want to use the classification, please restore the record.");
            RuleFor(x => x.ClassificationName).Must(BeUnique).WithMessage("Classification already exists in the system's database.");
        }

        public bool BeUnique(string UnitName)
        {
            return (db.InfrastructureRequirementsClass.Where(x => x.ClassificationName == UnitName).Count() == 0) ? true : false;
        }

        public bool NotBeDeleted(string UnitName)
        {
            return (db.InfrastructureRequirementsClass.Where(x => x.ClassificationName == UnitName && x.PurgeFlag == true).Count() == 0) ? true : false;
        }
    }
    public class InfrastructureRequirementsValidator : AbstractValidator<InfrastructureRequirements>
    {
        FMISDbContext db = new FMISDbContext();
        public InfrastructureRequirementsValidator()
        {
            RuleFor(x => x.Requirement).Must(NotBeDeleted).WithMessage("Requirement was recently deleted. If you want to use the requirement, please restore the record.");
            RuleFor(x => x.Requirement).Must(BeUnique).WithMessage("Requirement already exists in the system's database.");
        }

        public bool BeUnique(string UnitName)
        {
            return (db.InfrastructureRequirements.Where(x => x.Requirement == UnitName).Count() == 0) ? true : false;
        }

        public bool NotBeDeleted(string UnitName)
        {
            return (db.InfrastructureRequirements.Where(x => x.Requirement == UnitName && x.PurgeFlag == true).Count() == 0) ? true : false;
        }
    }
    public class InfrastructureMaterialsValidator : AbstractValidator<InfrastructureMaterials>
    {
        FMISDbContext db = new FMISDbContext();
        public InfrastructureMaterialsValidator()
        {
            RuleFor(x => x.ItemName).Must(NotBeDeleted).WithMessage("Material was recently deleted. If you want to use the material, please restore the record.");
            RuleFor(x => x.ItemName).Must(BeUnique).WithMessage("Material already exists in the system's database.");
        }

        public bool BeUnique(string UnitName)
        {
            return (db.InfrastructureMaterials.Where(x => x.ItemName == UnitName).Count() == 0) ? true : false;
        }

        public bool NotBeDeleted(string UnitName)
        {
            return (db.InfrastructureMaterials.Where(x => x.ItemName == UnitName && x.PurgeFlag == true).Count() == 0) ? true : false;
        }
    }

    public enum RequestAcceptance
    {
        [Display(Name = "Request Accepted for Evaluation")]
        AcceptedForEvaluation = 0,

        [Display(Name = "Request Not Accepted")]
        NotAccepted = 1
    }
}