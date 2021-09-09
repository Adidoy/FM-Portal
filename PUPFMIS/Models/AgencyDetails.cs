using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PUPFMIS.Models.HRIS;

namespace PUPFMIS.Models
{
    [Table("PROC_SYTM_Agency_Details")]
    public class AgencyDetails
    {
        [Key]
        public int ID { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Account Code")]
        [MaxLength(15, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string AccountCode { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Organization Type")]
        [MaxLength(75, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string OrganizationType { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Department/Bureau/Office")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string AgencyName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Region")]
        [MaxLength(30, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string Region { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Address")]
        [MaxLength(150, ErrorMessage = "{0} field must be up to {1} characters only.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} field must be filled out.")]
        public string Address { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string Website { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string ProcurementOfficeReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string ProcurementPlanningOfficeReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string ProcurementOfficeAddress { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string ProcurementOfficeEmail { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string ProcurementOfficeContactNo { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string ProcurementOfficeAlternateContactNo { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string AccountingOfficeReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string BACOfficeReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string BACOfficeAddress { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string BACOfficeEmail { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string BACOfficeContactNo { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string BACOfficeAlternateContactNo { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string PropertyOfficeReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string SuppliesInventoryOfficeReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string InspectionManagementReference { get; set; }

        [Column(TypeName = "VARCHAR")]
        public string HOPEReference { get; set; }
    }

    [Table("PROC_SYTM_BAC_Secretariat")]
    public class BACSecretariat
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(75)]
        [Display(Name = "Member")]
        public string Member { get; set; }

        [Required]
        [Display(Name = "Membership")]
        public BACMembership Membership { get; set; }

        [Required]
        [Display(Name = "Is Deleted?")]
        public bool PurgeFlag { get; set; }

        public string CreateBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public enum BACMembership
    {
        [Display(Name = "BAC Chairman")]
        BACChairman,

        [Display(Name = "BAC Vice-Chairman")]
        BACViceChairman,

        [Display(Name = "BAC Member")]
        BACMember,

        [Display(Name = "Head, BAC Secretariat")]
        BACSECHead,

        [Display(Name = "Provisional BAC Member")]
        ProvisionalBACMember,

        [Display(Name = "TWG Chair - Goods and Services")]
        TWGGoodsAndServices,

        [Display(Name = "TWG Chair - Infrastructure")]
        TWGInfrastructure,

        [Display(Name = "TWG Chair - Consultancy")]
        TWGConsultancy,

        [Display(Name = "TWG Member")]
        TWGMember,

        [Display(Name = "Ad Hoc Member")]
        AdHocMember,

        [Display(Name = "UNAKA Representative")]
        UNAKARepresentative,

        [Display(Name = "COA Representative")]
        COARepresentative,

        [Display(Name = "PICPA Representative")]
        PICPARepresentative,

        [Display(Name = "UAP Representative")]
        UAPRepresentative,
    }
}