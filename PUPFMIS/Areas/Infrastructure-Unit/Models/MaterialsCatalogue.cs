using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{


    [Table("PROC_MSTR_Infrastructure_Materials")]
    public class InfrastructureMaterials
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(75)]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Item Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Unit of Measure")]
        public int UOMReference { get; set; }

        [Display(Name = "Work Classification")]
        public int? WorkClassificationReference { get; set; }

        [Display(Name = "Work Requirement")]
        public int? WorkRequirementReference { get; set; }

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

        [ForeignKey("UOMReference")]
        public virtual UnitOfMeasure FKUOMReference { get; set; }

        [ForeignKey("WorkClassificationReference")]
        public virtual InfrastructureRequirementsClassification FKWorkClassificationReference { get; set; }

        [ForeignKey("WorkRequirementReference")]
        public virtual InfrastructureRequirements FKWorkRequirementReference { get; set; }
    }

    public class MaterialsCatalogueVM
    {
        public int ID { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Item Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }

        public string WorkClassification { get; set; }

        [Display(Name = "Work Requirement")]
        public string WorkRequirement { get; set; }
    }

    public class MaterialsBasket
    {
        public string InfraProjectCode { get; set; }
        public List<InfrastructureDetailedEstimateVM> ItemList { get; set; }
    }
}