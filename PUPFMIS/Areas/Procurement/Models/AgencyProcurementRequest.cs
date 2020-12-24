using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PROC_TRXN_Agency_Procurement_Request")]
    public class AgencyProcurementRequest
    {
        [Key]
        public int ID { get; set; }
        public string AgencyControlNo { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ProcurementHead { get; set; }
        public string ProcurementDepartment { get; set; }
        public string ProcurementHeadDesignation { get; set; }
        public string ChiefAccountant { get; set; }
        public string ChiefAccountantDepartment { get; set; }
        public string ChiefAccountantDesignation { get; set; }
        public string AgencyHead { get; set; }
        public string AgencyHeadDepartment { get; set; }
        public string AgencyHeadDesignation { get; set; }
    }

    [Table("PROC_TRXN_Agency_Procurement_Request_Details")]
    public class AgencyProcurementRequestDetails
    {
        [Key]
        public int ID { get; set; }

        public int APRHeaderReference { get; set; }

        public int ItemReference { get; set; }

        public int Quantity { get; set; }

        public int UnitReference { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Amount { get; set; }

        public int? PRReference { get; set; }

        [ForeignKey("APRHeaderReference")]
        public virtual AgencyProcurementRequest FKAPRReference { get; set; }

        [ForeignKey("ItemReference")]
        public virtual Item FKItemReference { get; set; }

        [ForeignKey("UnitReference")]
        public virtual UnitOfMeasure FKUnitOfMeasureReference { get; set; }

        [ForeignKey("PRReference")]
        public virtual PurchaseRequestHeader FKPRHeader { get; set; }
    }

    public class AgencyProcurementRequestVM
    {
        [Display(Name = "Agency Control No.")]
        public string AgencyControlNo { get; set; }

        [Display(Name = "Date Prepared")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Procurement Head")]
        public string ProcurementHead { get; set; }

        [Display(Name = "Department")]
        public string ProcurementDepartment { get; set; }

        [Display(Name = "Designation")]
        public string ProcurementHeadDesignation { get; set; }

        [Display(Name = "Chief Accountant")]
        public string ChiefAccountant { get; set; }

        [Display(Name = "Department")]
        public string ChiefAccountantDepartment { get; set; }

        [Display(Name = "Designation")]
        public string ChiefAccountantDesignation { get; set; }

        [Display(Name = "Agency Head")]
        public string AgencyHead { get; set; }

        [Display(Name = "Department")]
        public string AgencyHeadDepartment { get; set; }

        [Display(Name = "Designation")]
        public string AgencyHeadDesignation { get; set; }

        public List<APRDetailVM> APRDetails { get; set; }
    }

    public class APRDetailVM
    {
        [Display(Name = "Item Description")]
        public string ItemFullName { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Unit")]
        public string UnitReference { get; set; }

        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
    }
}