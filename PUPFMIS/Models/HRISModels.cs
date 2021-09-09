using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PUPFMIS.Models.HRIS
{
    [Table("vHRISDepartment")]
    public class HRISDepartment
    {
        [Key]
        [Display(Name = "Department ID")]
        public int DepartmentID { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Display(Name = "Root ID")]
        public int? RootID { get; set; }

        [Display(Name = "Order Sequence")]
        public string OrderSequence { get; set; }

        [Display(Name = "Level")]
        public int? Lvl { get; set; }

        [Display(Name = "Parent Department")]
        public string ParentDepartment { get; set; }
    }

    [Table("vHRISEmployeeDesignation")]
    public class HRISEmployeeDesignation
    {
        [Key]
        public int EmployeeDesignationID { get; set; }

        [Display(Name = "Employee Code")]
        public string EmpCode { get; set; }

        [Display(Name = "DesignationID")]
        public int DesignationID { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Display(Name = "Department ID")]
        public int DepartmentID { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        public DateTime? IncStart { get; set; }

        public DateTime? IncEnd { get; set; }
    }

    [Table("vHRISEmployeeDetails")]
    public class HRISEmployeeDetails
    {
        [Key]
        [Display(Name = "Employee Code")]
        public string EmpCode { get; set; }

        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Display(Name = "Middle Initial")]
        public string MInitial { get; set; }

        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Display(Name = "Section Code")]
        public string SectionCode { get; set; }

        [Display(Name = "Position")]
        public string PlantillaPosition { get; set; }

        [Display(Name = "Is Active?")]
        public string IsActive { get; set; }
    }

    [Table("vHRISUserAccount")]
    public class HRISUserAccount
    {
        [Key]
        public string EmpCode { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class HRISDepartmentDetailsVM
    {
        [Display(Name = "Sector Code")]
        public string SectorCode { get; set; }

        [Display(Name = "Sector")]
        public string Sector { get; set; }

        [Display(Name = "Sector Head")]
        public string SectorHead { get; set; }

        [Display(Name = "Sector Head")]
        public string SectorHeadLastName { get; set; }

        [Display(Name = "Designation")]
        public string SectorHeadDesignation { get; set; }

        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Department Head")]
        public string DepartmentHead { get; set; }

        [Display(Name = "Department Head")]
        public string DepartmentHeadLastName { get; set; }

        [Display(Name = "Designation")]
        public string DepartmentHeadDesignation { get; set; }

        [Display(Name = "Section Code")]
        public string SectionCode { get; set; }

        [Display(Name = "Section")]
        public string Section { get; set; }

        [Display(Name = "Section Head")]
        public string SectionHead { get; set; }

        [Display(Name = "Section Head")]
        public string SectionHeadLastName { get; set; }

        [Display(Name = "Designation")]
        public string SectionHeadDesignation { get; set; }
    }

    public class HRISDepartmentVM
    {
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Department Head")]
        public string DepartmentHead { get; set; }

        [Display(Name = "Designation")]
        public string DepartmentHeadDesignation { get; set; }
    }

    public class HRISEmployeeDetailsVM
    {
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Display(Name = "Section Code")]
        public string SectionCode { get; set; }

        [Display(Name = "Section")]
        public string Section { get; set; }
    }

    public class HRISDesignations
    {
        public int DepartmentID { get; set; }
        public string Designation { get; set; }
        public string EmpCode { get; set; }
    }
}