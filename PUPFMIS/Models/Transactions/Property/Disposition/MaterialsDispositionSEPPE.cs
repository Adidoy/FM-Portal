﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace PUPFMIS.Models
//{
//    [Table("property.MaterialsDispositionSEPPE")]
//    public class MaterialsDispositionSEPPE
//    {
//        [Key]
//        public int ID { get; set; }

//        public int PPEReference { get; set; }

//        public int Quantity { get; set; }

//        [Display(Name = "Manner of Disposal")]
//        public DispositionAction Action { get; set; }

//        [Display(Name = "Official Receipt No.")]
//        public string SalesORNumber { get; set; }

//        [Display(Name = "O.R. Date")]
//        public DateTime SalesDateTime { get; set; }

//        [Display(Name = "Amount")]
//        public decimal SalesAmount { get; set; }

//        [Display(Name = "Name of Agency")]
//        public string TransferAgency { get; set; }

//        [Display(Name = "Item Name")]
//        [ForeignKey("PPEReference")]
//        public virtual PPE FKPPEReference { get; set; }
//    }
//}