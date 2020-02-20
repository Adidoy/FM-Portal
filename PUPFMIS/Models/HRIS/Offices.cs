using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models.HRIS
{
    [Table("master.Offices")]
    public class Offices
    {
        public int ID { get; set; }
        public Nullable<int> Sector { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public string OfficeHead { get; set; }
        public string Designation { get; set; }
        public bool PurgeFlag { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<System.DateTime> DeletedAt { get; set; }
    }
}