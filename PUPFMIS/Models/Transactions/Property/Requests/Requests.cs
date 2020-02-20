using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("property_requestHeader")]
    public class RequestHeader
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "RIS No.")]
        public string RISNo { get; set; }

        [Display(Name = "Responsibility Center")]
        public int Office { get; set; }

        [Display(Name = "Request Type")]
        public int RequestType { get; set; }

        [Display(Name = "Requested By")]
        public int RequestedBy { get; set; }

        [Display(Name = "Approved By")]
        public int ApprovedBy { get; set; }

        [Required]
        [Display(Name = "Date Requested")]
        public DateTime RequestedAt { get; set; }

        [Required]
        [Display(Name = "Status")]
        [MaxLength(75)]
        public string Status { get; set; }

        [Display(Name = "Date Issued")]
        public DateTime? IssuedAt { get; set; }

        [Display(Name = "Issued By")]
        public int? IssuedBy { get; set; }

        [Display(Name = "Date Released")]
        public DateTime? ReleasedAt { get; set; }

        [Display(Name = "Released By")]
        public int? ReleasedBy { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Updated By")]
        public int? UpdatedBy { get; set; }

        [ForeignKey("RequestType")]
        public virtual InventoryType FKRequestTypeReference { get; set; }
    }

    [Table("property_suppliesRequestDetails")]
    public class SuppliesRequestDetails
    {
        [Key, Column(Order = 0)]
        public int RequestID { get; set; }

        [Key, Column(Order = 1)]
        public int SupplyID { get; set; }

        [Display(Name = "Stock Available?")]
        public bool StockAvailable { get; set; }

        [Display(Name = "Requested Qty")]
        public int QtyRequested { get; set; }

        [ForeignKey("RequestID")]
        public virtual RequestHeader FKRequestHeader { get; set; }

        [ForeignKey("SupplyID")]
        public virtual Supply FKSuppliesMaster { get; set; }
    }

    [Table("property_suppliesIssueDetails")]
    public class SuppliesIssueDetails
    {
        [Key, Column(Order = 0)]
        public int RequestID { get; set; }

        [Key, Column(Order = 1)]
        public int SupplyID { get; set; }

        [Display(Name = "Issued Qty")]
        public int QtyIssued { get; set; }

        [ForeignKey("RequestID")]
        public virtual RequestHeader FKRequestHeader { get; set; }

        [ForeignKey("SupplyID")]
        public virtual Supply FKSuppliesMaster { get; set; }
    }
}