//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.ProcurementPipelineHeader")]
//    public class ProcurementPipelineHeader
//    {
//        [Key]
//        public int ID { get; set; }

//        [Display(Name = "Line No.")]
//        public string ProcurementLineNumber { get; set; }

//        public int ProcurementCategory { get; set; }

//        [Display(Name = "Inventory Type")]
//        public InventoryType InventoryType { get; set; }

//        [Display(Name = "Mode of Procurement")]
//        public ProcurementModes ProcurementMode { get; set; }

//        [Display(Name = "Project Coordinator")]
//        public string ProjectCoordinator { get; set; }

//        [Display(Name = "Status")]
//        public string Status { get; set; }

//        [Display(Name = "Procurement Process Start")]
//        public DateTime ProcurmentStart { get; set; }

//        [ForeignKey("ProcurementCategory")]
//        [Display(Name = "Procurement Category")]
//        public virtual ItemSubCategoryMaster FKItemCategory { get; set; }
//    }
//}