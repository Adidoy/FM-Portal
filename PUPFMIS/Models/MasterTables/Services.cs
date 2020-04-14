using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("master_services")]
    public class Services
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Service Code")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(20, ErrorMessage = "{0} field must be up to {1} characters only.")]
        public string ServiceCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Display(Name = "Service Name")]
        [Column(TypeName = "VARCHAR")]
        public string ServiceName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        [Display(Name = "Short Specifications")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Column(TypeName = "VARCHAR")]
        public string ItemShortSpecifications { get; set; }

        [Display(Name = "Service Type")]
        public int ServiceTypeReference { get; set; }

        [Display(Name = "Service Category")]
        public int ServiceCategoryReference { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "UACS Class")]
        public string AccountClass { get; set; }

        [ForeignKey("ServiceTypeReference")]
        [Display(Name = "Inventory Type")]
        public virtual InventoryType FKInventoryTypeReference { get; set; }

        [ForeignKey("ServiceCategoryReference")]
        [Display(Name = "Sevice Category")]
        public virtual PhilGEPSCategories FKCategoryReference { get; set; }

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
}