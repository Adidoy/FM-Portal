using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models.HRIS;
using PUPFMIS.Models;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class HRISDataAccess : Controller
    {
        private HRISDbContext db = new HRISDbContext();
        private FMISDbContext fmis = new FMISDbContext();

        private List<HRISDesignations> GetExecutiveDesignations(int DepartmentID)
        {
            var sectorHeadDesignations = db.HRISEmployeeDesignation
                   .Where(d => ((d.Designation.Contains("University President")) ||
                                (d.Designation.Contains("Vice President"))) &&
                                (d.DepartmentID == DepartmentID))
                   .OrderByDescending(d => d.IncStart).ThenBy(d => d.IncEnd)
                   .Select(d => new HRISDesignations { DepartmentID = d.DepartmentID, Designation = d.Designation, EmpCode = d.EmpCode })
                   .FirstOrDefault();

            var assistantSectorHeadDesignations = db.HRISEmployeeDesignation
                   .Where(d => ((d.Designation.Contains("Assistant to the"))) &&
                                (d.DepartmentID == DepartmentID))
                   .OrderByDescending(d => d.IncStart).ThenBy(d => d.IncEnd)
                   .Select(d => new HRISDesignations { DepartmentID = d.DepartmentID, Designation = d.Designation, EmpCode = d.EmpCode })
                   .FirstOrDefault();

            var executiveDesignations = new List<HRISDesignations>();
            executiveDesignations.Add(sectorHeadDesignations);
            executiveDesignations.Add(assistantSectorHeadDesignations);

            return executiveDesignations;
        }
        private HRISDesignations GetDepartmentDesignations(int DepartmentID)
        {
            return db.HRISEmployeeDesignation
                   .Where(d => ((d.Designation.Contains("Director")) ||
                                (d.Designation.Contains("Dean"))) &&
                               (d.DepartmentID == DepartmentID))
                   .OrderByDescending(d => d.IncStart).ThenBy(d => d.IncEnd)
                   .Distinct()
                   .Select(d => new HRISDesignations { DepartmentID = d.DepartmentID, Designation = d.Designation, EmpCode = d.EmpCode })
                   .FirstOrDefault();
        }
        private HRISDesignations GetUnitDesignations(int DepartmentID)
        {
            return db.HRISEmployeeDesignation
                   .Where(d => ((d.Designation.Contains("Chief")) ||
                                (d.Designation.Contains("Chairperson")) ||
                                (d.Designation.Contains("Head"))) &&
                               (d.DepartmentID == DepartmentID))
                   .OrderByDescending(d => d.IncStart).ThenBy(d => d.IncEnd)
                   .Distinct()
                   .Select(d => new HRISDesignations { DepartmentID = d.DepartmentID, Designation = d.Designation, EmpCode = d.EmpCode })
                   .FirstOrDefault();
        }
        public List<HRISDepartment> GetAllDepartments()
        {
            return db.HRISDepartments.ToList();
        }
        public HRISDepartmentDetailsVM GetDepartmentDetails(string DepartmentCode)
        {
            var office = db.HRISDepartments.Where(d => d.DepartmentCode == DepartmentCode).FirstOrDefault();
            if(office == null)
            {
                return null;
            }

            if(office.Lvl == 0)
            {
                var sectorHead = GetExecutiveDesignations(office.DepartmentID);
                return new HRISDepartmentDetailsVM
                {
                    SectorCode = office.DepartmentCode,
                    Sector = office.Department,
                    SectorHead = GetEmployeeDetailByCode(sectorHead[0].EmpCode).EmployeeName,
                    SectorHeadLastName = GetEmployeeDetailByCode(sectorHead[0].EmpCode).LastName,
                    SectorHeadDesignation = sectorHead[0].Designation,
                    DepartmentCode = office.DepartmentCode,
                    Department = office.Department,
                    DepartmentHead = GetEmployeeDetailByCode(sectorHead[1].EmpCode).EmployeeName,
                    DepartmentHeadLastName = GetEmployeeDetailByCode(sectorHead[1].EmpCode).LastName,
                    DepartmentHeadDesignation = sectorHead[1].Designation,
                    SectionCode = office.DepartmentCode,
                    Section = office.Department,
                    SectionHead = GetEmployeeDetailByCode(sectorHead[1].EmpCode).EmployeeName,
                    SectionHeadLastName = GetEmployeeDetailByCode(sectorHead[1].EmpCode).LastName,
                    SectionHeadDesignation = sectorHead[1].Designation
                };
            }
            else if(office.Lvl == 1)
            {
                var sector = db.HRISDepartments.Where(d => d.DepartmentID == office.RootID).FirstOrDefault();
                var sectorHead = GetExecutiveDesignations(sector.DepartmentID);
                var officeHead = GetDepartmentDesignations(office.DepartmentID);
                if(officeHead == null)
                {
                    officeHead = GetUnitDesignations(office.DepartmentID);
                }
                return new HRISDepartmentDetailsVM
                {
                    SectorCode = sector.DepartmentCode,
                    Sector = sector.Department,
                    SectorHead = GetEmployeeDetailByCode(sectorHead[0].EmpCode).EmployeeName,
                    SectorHeadLastName = GetEmployeeDetailByCode(sectorHead[0].EmpCode).LastName,
                    SectorHeadDesignation = sectorHead[0].Designation,
                    DepartmentCode = office.DepartmentCode,
                    Department = office.Department,
                    DepartmentHead = GetEmployeeDetailByCode(officeHead.EmpCode).EmployeeName,
                    DepartmentHeadLastName = GetEmployeeDetailByCode(officeHead.EmpCode).LastName,
                    DepartmentHeadDesignation = officeHead.Designation,
                    SectionCode = office.DepartmentCode,
                    Section = office.Department,
                    SectionHead = GetEmployeeDetailByCode(officeHead.EmpCode).EmployeeName,
                    SectionHeadLastName = GetEmployeeDetailByCode(officeHead.EmpCode).LastName,
                    SectionHeadDesignation = officeHead.Designation
                };
            }
            else
            {
                var department = db.HRISDepartments.Where(d => d.DepartmentID == office.RootID).FirstOrDefault();
                var sector = db.HRISDepartments.Where(d => d.DepartmentID == department.RootID).FirstOrDefault();
                var sectorHead = GetExecutiveDesignations(sector.DepartmentID);
                var officeHead = GetDepartmentDesignations(department.DepartmentID);
                var sectionHead = GetUnitDesignations(office.DepartmentID);
                return new HRISDepartmentDetailsVM
                {
                    SectorCode = sector.DepartmentCode,
                    Sector = sector.Department,
                    SectorHead = GetEmployeeDetailByCode(sectorHead[0].EmpCode).EmployeeName,
                    SectorHeadLastName = GetEmployeeDetailByCode(sectorHead[0].EmpCode).LastName,
                    SectorHeadDesignation = sectorHead[0].Designation,
                    DepartmentCode = department.DepartmentCode,
                    Department = department.Department,
                    DepartmentHead = GetEmployeeDetailByCode(officeHead.EmpCode).EmployeeName,
                    DepartmentHeadLastName = GetEmployeeDetailByCode(officeHead.EmpCode).LastName,
                    DepartmentHeadDesignation = officeHead.Designation,
                    SectionCode = office.DepartmentCode,
                    Section = office.Department,
                    SectionHead = sectionHead == null ? string.Empty : GetEmployeeDetailByCode(sectionHead.EmpCode).EmployeeName,
                    SectionHeadLastName = sectionHead == null ? string.Empty : GetEmployeeDetailByCode(sectionHead.EmpCode).LastName,
                    SectionHeadDesignation = sectionHead == null ? string.Empty : sectionHead.Designation
                };
            }
        }
        public List<HRISDepartment> GetUserDepartments(string EmailAddress)
        {
            var departmentList = new List<HRISDepartment>();
            var departmentCode = fmis.UserAccounts.Where(d => d.Email == EmailAddress).FirstOrDefault().UnitCode;
            var department = db.HRISDepartments.Where(d => d.DepartmentCode == departmentCode).FirstOrDefault();
            if(department.Lvl > 1)
            {
                departmentList.Add(department);
            }
            else
            {
                var units = db.HRISDepartments.Where(d => (d.RootID == department.DepartmentID || d.DepartmentID == department.DepartmentID) && !(d.Department.Contains("Inactive"))).ToList();
                departmentList.AddRange(units.Distinct());
            }
            return departmentList.OrderBy(d => d.DepartmentID).Distinct().ToList();
        }
        public HRISEmployeeDetailsVM GetEmployee(string EmailAddress)
        {
            return (from hrisEmployees in db.HRISEmployeeDetails
                    join hrisUserAccounts in db.HRISUserAccounts
                    on hrisEmployees.EmpCode equals hrisUserAccounts.EmpCode
                    where hrisUserAccounts.UserName == EmailAddress
                    select new HRISEmployeeDetailsVM
                    {
                        Email = hrisUserAccounts.UserName,
                        EmployeeCode = hrisEmployees.EmpCode,
                        EmployeeName = hrisEmployees.FName + " " + hrisEmployees.MInitial + " " + hrisEmployees.LName,
                        DepartmentCode = hrisEmployees.DepartmentCode,
                        Department = db.HRISDepartments.Where(d => d.DepartmentCode == hrisEmployees.DepartmentCode).FirstOrDefault().Department,
                        SectionCode = hrisEmployees.SectionCode,
                        Section = db.HRISDepartments.Where(d => d.DepartmentCode == hrisEmployees.SectionCode).FirstOrDefault().Department,
                        Designation = hrisEmployees.PlantillaPosition,
                    }).FirstOrDefault();
        }
        public HRISEmployeeDetailsVM GetEmployeeByCode(string EmployeeCode)
        {
            return (from hrisEmployees in db.HRISEmployeeDetails
                    join hrisUserAccounts in db.HRISUserAccounts
                    on hrisEmployees.EmpCode equals hrisUserAccounts.EmpCode
                    where hrisUserAccounts.EmpCode == EmployeeCode
                    select new HRISEmployeeDetailsVM
                    {
                        Email = hrisUserAccounts.UserName,
                        EmployeeCode = hrisEmployees.EmpCode,
                        EmployeeName = hrisEmployees.LName + ", " + hrisEmployees.FName  + " " + hrisEmployees.MInitial,
                        DepartmentCode = hrisEmployees.DepartmentCode,
                        Department = db.HRISDepartments.Where(d => d.DepartmentCode == hrisEmployees.DepartmentCode).FirstOrDefault().Department,
                        SectionCode = hrisEmployees.SectionCode,
                        Section = db.HRISDepartments.Where(d => d.DepartmentCode == hrisEmployees.SectionCode).FirstOrDefault().Department,
                        Designation = hrisEmployees.PlantillaPosition,
                    }).FirstOrDefault();
        }
        public HRISEmployeeDetailsVM GetEmployeeDetailByCode(string EmployeeCode)
        {
            return (from hrisEmployees in db.HRISEmployeeDetails
                    where hrisEmployees.EmpCode == EmployeeCode
                    select new HRISEmployeeDetailsVM
                    {
                        EmployeeCode = hrisEmployees.EmpCode,
                        EmployeeName = hrisEmployees.FName + " " + hrisEmployees.MInitial + " " + hrisEmployees.LName,
                        LastName = hrisEmployees.LName,
                        DepartmentCode = hrisEmployees.DepartmentCode,
                        Department = db.HRISDepartments.Where(d => d.DepartmentCode == hrisEmployees.DepartmentCode).FirstOrDefault().Department,
                        SectionCode = hrisEmployees.SectionCode,
                        Section = db.HRISDepartments.Where(d => d.DepartmentCode == hrisEmployees.SectionCode).FirstOrDefault().Department,
                        Designation = hrisEmployees.PlantillaPosition,
                    }).FirstOrDefault();
        }
        public List<HRISEmployeeDetailsVM> GetEmployees(string DepartmentCode)
        {
            return db.HRISEmployeeDetails.Where(d => d.DepartmentCode == DepartmentCode && d.IsActive == "Y")
                .Select(d => new HRISEmployeeDetailsVM
                {
                    EmployeeCode = d.EmpCode,
                    EmployeeName = d.FName + " " + d.MInitial + " " + d.LName
                }).ToList();
        }
        public List<HRISEmployeeDetails> GetEmployees()
        {
            return db.HRISEmployeeDetails.Where(d => d.IsActive == "Y").ToList();
        }
        private List<HRISEmployeeDetailsVM> GetDepartmentOfficials(string DepartmentCode)
        {
            var designationTbl = db.HRISEmployeeDesignation.Where(d => (d.IncStart <= DateTime.Now && d.IncEnd >= DateTime.Now)).ToList();
            var officials = (from designations in designationTbl
                             join employees in db.HRISEmployeeDetails.ToList() on designations.EmpCode equals employees.EmpCode
                             join departments in db.HRISDepartments.ToList() on designations.DepartmentID equals departments.DepartmentID
                             where departments.DepartmentCode == DepartmentCode
                             select new HRISEmployeeDetailsVM
                             {
                                 EmployeeCode = employees.EmpCode,
                                 EmployeeName = employees.FName + " " + employees.MInitial + " " + employees.LName,
                                 Department = departments.Department,
                                 DepartmentCode = departments.DepartmentCode,
                                 Designation = designations.Designation
                             }).ToList();
            return officials;
        }
        public List<HRISDepartment>GetAllUnits(string DepartmentCode)
        {
            var unitsList = new List<HRISDepartment>();
            var parentDepartment = db.HRISDepartments.Where(d => d.DepartmentCode == DepartmentCode).FirstOrDefault();
            unitsList.Add(parentDepartment);
            unitsList.AddRange(db.HRISDepartments.Where(d => (d.RootID == parentDepartment.DepartmentID) && (!d.Department.Contains("Inactive"))).ToList());
            return unitsList;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                fmis.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}