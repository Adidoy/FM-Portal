//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.EvaluationResponses")]
//    public class EvaluationResponses
//    {
//        [Key]
//        public int ID { get; set; }

//        public int EvaluationID { get; set; }

//        public int CriteriaID { get; set; }

//        public int Response { get; set; }

//        [ForeignKey("EvaluationID")]
//        public SupplierEvaluation FKSupplierEvaluation { get; set; }

//        [ForeignKey("CriteriaID")]
//        public EvaluationCriteria FKEvaluationCriteria { get; set; }
//    }
//}