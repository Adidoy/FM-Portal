//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.ProcurementPipelineStatusUpdate")]
//    public class ProcurementPipelineStatusUpdate
//    {
//        [Key]
//        public int ID { get; set; }

//        public int PipelineID { get; set; }

//        [Display(Name = "Status")]
//        public string Status { get; set; }

//        [Display(Name = "Date Updated")]
//        public DateTime DateUpdated { get; set; }

//        [Display(Name = "Remarks")]
//        public string Remarks { get; set; }

//        [Display(Name = "Staff")]
//        public string Staff { get; set; }

//        [ForeignKey("PipelineID")]
//        [Display(Name = "Pipeline ID")]
//        public ProcurementPipelineHeader FKPipelineHeader { get; set; }
//    }
//}