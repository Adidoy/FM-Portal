using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PP_MASTER_PLANTS")]
    public class PlantsMaster
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Building Name")]
        public string BuildingName { get; set; }

        [Display(Name = "Campus")]
        public string CampusName { get; set; }
    }
}