using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("master_dbmCategories")]
    public class DBMCategories
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Item Category Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(150)]
        public string ItemCategoryName { get; set; }

        [Required]
        [Display(Name = "Is Deleted?")]
        public Boolean PurgeFlag { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }
    }

    [Table("master_philgepsCategories")]
    public class PhilGEPSCategories
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Item Category Name")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(150)]
        public string ItemCategoryName { get; set; }

        [Display(Name = "Category For")]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(10, ErrorMessage = "{0} must be up to {1} characters only.")]
        public string CategoryFor { get; set; }

        [Required]
        [Display(Name = "Is Deleted?")]
        public Boolean PurgeFlag { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Date Deleted")]
        public DateTime? DeletedAt { get; set; }
    }
}