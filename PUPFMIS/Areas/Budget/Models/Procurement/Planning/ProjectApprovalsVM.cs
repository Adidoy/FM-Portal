using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    public class ProjectsHeaderVM
    {
        [Display(Name = "Project Code")]
        public string ProjectCode { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Start Month")]
        public string StartMonth { get; set; }

        [Display(Name = "Fiscal Year")]
        public string FiscalYear { get; set; }

        [Display(Name = "Office")]
        public string Office { get; set; }

        [Display(Name = "Total Estimated Budget")]
        public decimal TotalEstimatedBudget { get; set; }

        [Display(Name = "DateSubmitted")]
        public string SubmittedAt { get; set; }

        [Display(Name = "CTS No.")]
        public string CTSNo { get; set; }
    }

    public class ProjectItemsVM
    {
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Item Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Qtr1 Qty.")]
        public int Qtr1Qty { get; set; }

        [Display(Name = "Qtr2 Qty.")]
        public int Qtr2Qty { get; set; }

        [Display(Name = "Qtr3 Qty.")]
        public int Qtr3Qty { get; set; }

        [Display(Name = "Qtr4 Qty.")]
        public int Qtr4Qty { get; set; }

        [Display(Name = "Total Qty.")]
        public int TotalQty { get; set; }
    }

    public class OfficeProjectsVM
    {
        [Display(Name = "Fiscal Year")]
        public string FiscalYear { get; set; }

        [Display(Name = "Office")]
        public string Office { get; set; }

        [Display(Name = "No. of Projects")]
        public int NoOfProjects { get; set; }

        [Display(Name = "Total Estimated Budget")]
        public decimal TotalEstimatedBudget { get; set; }
    }

    public class ProjectAccountsVM
    {
        public string UACS { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
    }
}