using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("system_ppmpApprovalRoutes")]
    public class PPMPApprovalRoutes
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int SequenceNo { get; set; }

        [Required]
        public int OfficeReference { get; set; }
    }
}