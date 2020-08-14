using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PUPFMIS.Models
{
    public class APPCSEDashboard
    {
        public List<int> PPMPFiscalYears { get; set; }
        public List<string> APPCSEReferences{ get; set; }
    }
}