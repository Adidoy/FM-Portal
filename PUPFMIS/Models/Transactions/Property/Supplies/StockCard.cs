﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PP_SUPPLY_STOCKCARDS")]
    public class StockCard
    {
        [Key]
        public int ID { get; set; }

        public int SupplyReference { get; set; }

        public int? SupplierReference { get; set; }

        public DateTime Date { get; set; }

        public string Reference { get; set; }

        [Display(Name = "Receipt Qty")]
        public int ReceiptQty { get; set; }

        [Display(Name = "Receipt Unit Cost")]
        public decimal ReceiptUnitCost { get; set; }

        [Display(Name = "Issue Qty")]
        public int IssueQty { get; set; }

        [Display(Name = "Issue Unit Cost")]
        public decimal IssueUnitCost { get; set; }

        [Display(Name = "Balance Qty")]
        public int BalanceQty { get; set; }

        [Display(Name = "Balance Unit Cost")]
        public int BalanceUnitCost { get; set; }

        [Display(Name = "Stock")]
        [ForeignKey("SupplyReference")]
        public virtual Supply FKSupplyReference { get; set; }

        [Display(Name = "Supplier")]
        [ForeignKey("SupplierReference")]
        public virtual Supplier FKSupplierReference { get; set; }
    }
}