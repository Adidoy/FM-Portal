using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public enum EndUserProjectItemTemplate
    {
        [Display(Name = "Supply and Delivery of")]
        SupplyAndDelivery,
        [Display(Name = "Contract of Service for")]
        ContractOfService,
        [Display(Name = "Procurement of")]
        Procurement,
        [Display(Name = "Construction of")]
        Construction,
        [Display(Name = "Repair and Maintenance of")]
        RepairAndMaintenance
    }
}