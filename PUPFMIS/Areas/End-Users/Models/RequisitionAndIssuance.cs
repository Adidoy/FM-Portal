using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public class ActualSuppliesObligation
    {
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Full Specifications")]
        public string ItemSpecifications { get; set; }

        [Display(Name = "Procurement Source")]
        public ProcurementSources ProcurementSource { get; set; }

        [Display(Name = "Item Image")]
        public byte[] ItemImage { get; set; }

        [Display(Name = "Packaging UOM")]
        public string PackagingUOMReference { get; set; }

        [Display(Name = "Individual UOM")]
        public string IndividualUOMReference { get; set; }

        [Display(Name = "Actual Obligation: JAN")]
        public int? JANActualObligation { get; set; }

        [Display(Name = "Actual Obligation: FEB")]
        public int? FEBActualObligation { get; set; }

        [Display(Name = "Actual Obligation: MAR")]
        public int? MARActualObligation { get; set; }

        [Display(Name = "Actual Obligation: APR")]
        public int? APRActualObligation { get; set; }

        [Display(Name = "Actual Obligation: MAY")]
        public int? MAYActualObligation { get; set; }

        [Display(Name = "Actual Obligation: JUN")]
        public int? JUNActualObligation { get; set; }

        [Display(Name = "Actual Obligation: JUL")]
        public int? JULActualObligation { get; set; }

        [Display(Name = "Actual Obligation: AUG")]
        public int? AUGActualObligation { get; set; }

        [Display(Name = "Actual Obligation: SEP")]
        public int? SEPActualObligation { get; set; }

        [Display(Name = "Actual Obligation: OCT")]
        public int? OCTActualObligation { get; set; }

        [Display(Name = "Actual Obligation: NOV")]
        public int? NOVActualObligation { get; set; }

        [Display(Name = "Actual Obligation: DEC")]
        public int? DECActualObligation { get; set; }

        [Display(Name = "Actual Obligation")]
        public int? ActualObligation { get; set; }
    }
}