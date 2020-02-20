using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("property.PPETransferLocation")]
    public class PPETransferLocation
    {
        [Key]
        public int ID { get; set; }
        
        public int CurrentLocation { get; set; }

        public int? TransferLocation { get; set; }

        [Display(Name = "Date Transferred")]
        public DateTime DateTransferred { get; set; }

        [ForeignKey("CurrentLocation")]
        [Display(Name = "Current Location")]
        public LocationsMaster FKCurrentLocation { get; set; }

        [ForeignKey("TransferLocation")]
        [Display(Name = "Transfer Location")]
        public LocationsMaster FKTransferLocation { get; set; }
    }
}