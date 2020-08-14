using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models.AIS
{
    [Table("ASMNGAUACS")]
    public class ChartOfAccounts
    {
        [Key]
        [Display(Name = "UACS")]
        [Column(TypeName = "VARCHAR")]
        public string UACS_Code { get; set; }

        [Display(Name = "Class Code")]
        [Column(TypeName = "VARCHAR")]
        public string ClassCode { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "General Account Name")]
        public string GenAcctName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Sub-Account Name")]
        public string SubAcctName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Account Name")]
        public string AcctName { get; set; }
    }

    [Table("AVS_R_FUND_HDR")]
    public class FundSources
    {
        [Key]
        [Display(Name = "Fund Cluster")]
        [Column(TypeName = "VARCHAR")]
        public string FUND_CLUSTER { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "Fund Description")]
        public string FUND_DESC { get; set; }
    }

    public class ChartOfAccountsDetailed
    {
        [Display(Name = "UACS")]
        public string UACS_Code { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Display(Name = "General Account Name")]
        public string GenAcctName { get; set; }

        [Display(Name = "Account Name")]
        public string AcctName { get; set; }
    }
}