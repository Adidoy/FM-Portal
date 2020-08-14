using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models
{
    [Table("PP_MASTER_LOCATIONS")]
    public class LocationsMaster
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Location Name")]
        public string LocationName { get; set; }

        [Display(Name = "Room No.")]
        public string RoomNo { get; set; }

        public int PlantReference { get; set; }

        [ForeignKey("PlantReference")]
        [Display(Name = "Plant/Building")]
        public PlantsMaster FKPlantsMaster { get; set; }
    }
}