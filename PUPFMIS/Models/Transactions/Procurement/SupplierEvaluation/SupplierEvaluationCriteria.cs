//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("procurement.EvaluationCriteria")]
//    public class EvaluationCriteria
//    {
//        [Key]
//        public int ID { get; set; }

//        [Display(Name = "Criteria")]
//        public string Criteria { get; set; }

//        public int Category { get; set; }

//        public int EvaluationScale { get; set; }

//        [ForeignKey("Category")]
//        [Display(Name = "Category")]
//        public virtual EvaluationCriteriaCategory FKCriteriaCategory { get; set; }

//        [ForeignKey("EvaluationScale")]
//        [Display(Name = "Evaluation Scale")]
//        public virtual EvaluationScale FKEvaluationScale { get; set; }
//    }
//}