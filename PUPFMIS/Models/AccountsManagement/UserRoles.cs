using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PUPFMIS.Models
{
    [Table("PROC_USRM_User_Role")]
    public class Roles
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }

    public static class SystemRoles
    {
        public const string SuperUser = "Super User";
        public const string SystemAdmin = "System Administrator";
        public const string BudgetAdmin = "Budget Administrator";
        public const string BudgetOfficer = "Budget Officer";
        public const string EndUser = "End-User";
        public const string ProcurementAdministrator = "Procurement Administrator";
        public const string ProcurementPlanningChief = "Procurement Planning Chief";
        public const string ProcurementStaff = "Procurement Staff";
        public const string ProjectCoordinator = "Project Coordinator";
        public const string PropertyDirector = "Property Director";
        public const string SuppliesChief = "Supplies Chief";
        public const string BACSECHead = "Bids and Awards Committee Secretariat Head";
    }
}