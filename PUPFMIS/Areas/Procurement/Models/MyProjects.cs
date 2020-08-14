using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PUPFMIS.Models
{
    public class MyProjectsDashboard
    {
        public List<ProcurementProjectsVM> ProjectsWithSetTimeLine { get; set; }
        public List<string> ProjectsWithoutSetTimeLine { get; set; } 
        public int NoOfProjectsWithSetTimeLine { get; set; }
        public int NoOfProjectsWithoutSetTimeLine { get; set; }
        public int TotalProjectsAssignedToMe { get; set; }
        public int TotalProjectsCoordinated { get; set; }
        public int TotalProjectsSupported { get; set; }
    }
}