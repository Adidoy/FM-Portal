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

        public List<HRISDepartment> GetAllDepartments()
        {
            return db.HRISDepartments.ToList();
        }
        public HRISDepartmentDetailsVM GetFullDepartmentDetails(string DepartmentCode)
        {
            var office = db.HRISDepartments.Where(d => d.DepartmentCode == DepartmentCode).FirstOrDefault();
            if(office == null)
            {
                return null;
            }
            var orderSeqArray = office.OrderSequence.Split("_".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries).ToArray();
            string sectorSequence = orderSeqArray[0];
            int? departmentRoot = null;
            int? sectionRoot = null;
            int? unitRoot = null;

            switch(orderSeqArray.Count())
            {
                case 1:
                    {
                        var officeDetails = (from employee in db.HRISEmployeeDetails
                                      join designation in db.HRISEmployeeDesignation on employee.EmpCode equals designation.EmpCode
                                      join department in db.HRISDepartments on designation.DepartmentID equals department.DepartmentID
                                      where department.OrderSequence == sectorSequence && (DateTime.Now >= designation.IncStart && DateTime.Now <= designation.IncEnd)
                                      orderby designation.EmployeeDesignationID descending
                                      select new
                                      {
                                          Code = department.DepartmentCode,
                                          Name = department.Department,
                                          Head = employee.FName + " " + employee.MInitial + " " + employee.LName,
                                          Designation = designation.Designation
                                      }).FirstOrDefault();

                        return new HRISDepartmentDetailsVM
                        {
                            DepartmentCode = officeDetails.Code,
                            Department = officeDetails.Name,
                            DepartmentHead = officeDetails.Head,
                            DepartmentHeadDesignation = officeDetails.Designation
                        };
                    }
                case 2:
                    {
                        departmentRoot = int.Parse(orderSeqArray[1]);

                        var sectorDetails = (from employee in db.HRISEmployeeDetails
                                             join designation in db.HRISEmployeeDesignation on employee.EmpCode equals designation.EmpCode
                                             join department in db.HRISDepartments on designation.DepartmentID equals department.DepartmentID
                                             where department.OrderSequence == sectorSequence && (DateTime.Now >= designation.IncStart && DateTime.Now <= designation.IncEnd)
                                             orderby designation.EmployeeDesignationID descending
                                             select new
                                             {
                                                 Code = department.DepartmentCode,
                                                 Name = department.Department,
                                                 Head = employee.FName + " " + employee.MInitial + " " + employee.LName,
                                                 Designation = designation.Designation
                                             }).FirstOrDefault();

                        var officeDetails = (from employee in db.HRISEmployeeDetails
                                             join designation in db.HRISEmployeeDesignation on employee.EmpCode equals designation.EmpCode
                                             join department in db.HRISDepartments on designation.DepartmentID equals department.DepartmentID
                                             where department.DepartmentID == departmentRoot && (DateTime.Now >= designation.IncStart && DateTime.Now <= designation.IncEnd)
                                             orderby designation.EmployeeDesignationID descending
                                             select new
                                             {
                                                 Code = department.DepartmentCode,
                                                 Name = department.Department,
                                                 Head = employee.FName + " " + employee.MInitial + " " + employee.LName,
                                                 Designation = designation.Designation
                                             }).FirstOrDefault();

                        return new HRISDepartmentDetailsVM
                        {
                            SectorCode = sectorDetails.Code,
                            Sector = sectorDetails.Name,
                            SectorHead = sectorDetails.Head,
                            SectorHeadDesignation = sectorDetails.Designation,
                            DepartmentCode = officeDetails.Code,
                            Department = officeDetails.Name,
                            DepartmentHead = officeDetails.Head,
                            DepartmentHeadDesignation = officeDetails.Designation
                        };
                    }
                case 3:
                    {
                        departmentRoot = int.Parse(orderSeqArray[1]);
                        sectionRoot = int.Parse(orderSeqArray[2]);

                        var sectorDetails = (from employee in db.HRISEmployeeDetails
                                             join designation in db.HRISEmployeeDesignation on employee.EmpCode equals designation.EmpCode
                                             join department in db.HRISDepartments on designation.DepartmentID equals department.DepartmentID
                                             where department.OrderSequence == sectorSequence && (DateTime.Now >= designation.IncStart && DateTime.Now <= designation.IncEnd)
                                             orderby designation.EmployeeDesignationID descending
                                             select new
                                             {
                                                 Code = department.DepartmentCode,
                                                 Name = department.Department,
                                                 Head = employee.FName + " " + employee.MInitial + " " + employee.LName,
                                                 Designation = designation.Designation
                                             }).FirstOrDefault();

                        var officeDetails = (from employee in db.HRISEmployeeDetails
                                             join designation in db.HRISEmployeeDesignation on employee.EmpCode equals designation.EmpCode
                                             join department in db.HRISDepartments on designation.DepartmentID equals department.DepartmentID
                                             where department.DepartmentID == departmentRoot && (DateTime.Now >= designation.IncStart && DateTime.Now <= designation.IncEnd)
                                             orderby designation.EmployeeDesignationID descending
                                             select new
                                             {
                                                 Code = department.DepartmentCode,
                                                 Name = department.Department,
                                                 Head = employee.FName + " " + employee.MInitial + " " + employee.LName,
                                                 Designation = designation.Designation
                                             }).FirstOrDefault();

                        var sectionDesignation = db.HRISEmployeeDesignation.Where(d => d.DepartmentID == sectionRoot).FirstOrDefault();
                        
                        var sectionDetails = sectionDesignation == null ? db.HRISDepartments.Where(d => d.DepartmentID == sectionRoot).Select(d => new { Code = d.DepartmentCode, Name = d.Department, Head = "", Designation = "" }).FirstOrDefault() : (from employee in db.HRISEmployeeDetails
                                             join designation in db.HRISEmployeeDesignation on employee.EmpCode equals designation.EmpCode
                                             join department in db.HRISDepartments on designation.DepartmentID equals department.DepartmentID
                                             where department.DepartmentID == sectionRoot && (DateTime.Now >= designation.IncStart && DateTime.Now <= designation.IncEnd)
                                             orderby designation.EmployeeDesignationID descending
                                             select new
                                             {
                                                 Code = department.DepartmentCode,
                                                 Name = department.Department,
                                                 Head = employee.FName + " " + employee.MInitial + " " + employee.LName,
                                                 Designation = designation.Designation
                                             }).FirstOrDefault();

                        return new HRISDepartmentDetailsVM
                        {
                            SectorCode = sectorDetails.Code,
                            Sector = sectorDetails.Name,
                            SectorHead = sectorDetails.Head,
                            SectorHeadDesignation = sectorDetails.Designation,
                            DepartmentCode = officeDetails.Code,
                            Department = officeDetails.Name,
                            DepartmentHead = officeDetails.Head,
                            DepartmentHeadDesignation = officeDetails.Designation,
                            SectionCode = sectionDetails.Code,
                            Section = sectionDetails.Name,
                            SectionHead = sectionDetails.Head == "" ? officeDetails.Head : sectionDetails.Head,
                            SectionHeadDesignation = sectionDetails.Designation == "" ? officeDetails.Designation : sectionDetails.Designation
                        };
                    }
                case 4:
                    {
                        departmentRoot = int.Parse(orderSeqArray[1]);
                        unitRoot = int.Parse(orderSeqArray[3]);

                        var sectorDetails = (from employee in db.HRISEmployeeDetails
                                             join designation in db.HRISEmployeeDesignation on employee.EmpCode equals designation.EmpCode
                                             join department in db.HRISDepartments on designation.DepartmentID equals department.DepartmentID
                                             where department.OrderSequence == sectorSequence && (DateTime.Now >= designation.IncStart && DateTime.Now <= designation.IncEnd)
                                             orderby designation.EmployeeDesignationID descending
                                             select new
                                             {
                                                 Code = department.DepartmentCode,
                                                 Name = department.Department,
                                                 Head = employee.FName + " " + employee.MInitial + " " + employee.LName,
                                                 Designation = designation.Designation
                                             }).FirstOrDefault();

                        var officeDetails = (from employee in db.HRISEmployeeDetails
                                             join designation in db.HRISEmployeeDesignation on employee.EmpCode equals designation.EmpCode
                                             join department in db.HRISDepartments on designation.DepartmentID equals department.DepartmentID
                                             where department.DepartmentID == departmentRoot && (DateTime.Now >= designation.IncStart && DateTime.Now <= designation.IncEnd)
                                             orderby designation.EmployeeDesignationID descending
                                             select new
                                             {
                                                 Code = department.DepartmentCode,
                                                 Name = department.Department,
                                                 Head = employee.FName + " " + employee.MInitial + " " + employee.LName,
                                                 Designation = designation.Designation
                                             }).FirstOrDefault();

                        var unitDetails = (from employee in db.HRISEmployeeDetails
                                              join designation in db.HRISEmployeeDesignation on employee.EmpCode equals designation.EmpCode
                                              join department in db.HRISDepartments on designation.DepartmentID equals department.DepartmentID
                                              where department.DepartmentID == unitRoot && (DateTime.Now >= designation.IncStart && DateTime.Now <= designation.IncEnd)
                                              orderby designation.EmployeeDesignationID descending
                                           select new
                                              {
                                                  Code = department.DepartmentCode,
                                                  Name = department.Department,
                                                  Head = employee.FName + " " + employee.MInitial + " " + employee.LName,
                                                  Designation = designation.Designation
                                              }).FirstOrDefault();

                        return new HRISDepartmentDetailsVM
                        {
                            SectorCode = sectorDetails.Code,
                            Sector = sectorDetails.Name,
                            SectorHead = sectorDetails.Head,
                            SectorHeadDesignation = sectorDetails.Designation,
                            DepartmentCode = officeDetails.Code,
                            Department = officeDetails.Name,
                            DepartmentHead = officeDetails.Head,
                            DepartmentHeadDesignation = officeDetails.Designation,
                            SectionCode = unitDetails.Code,
                            Section = unitDetails.Name,
                            SectionHead = unitDetails.Head,
                            SectionHeadDesignation = unitDetails.Designation
                        };
                    }
            }
            return null;
        }
        public HRISDepartmentVM GetDepartmentDetails(string DepartmentCode)
        {
            var department = db.HRISDepartments.Where(d => d.DepartmentCode == DepartmentCode).FirstOrDefault();
            var departmentDesignations = db.HRISEmployeeDesignation.Where(d => d.DepartmentID == department.DepartmentID && (DateTime.Now >= d.IncStart && DateTime.Now <= d.IncEnd)).OrderByDescending(d => d.EmployeeDesignationID).FirstOrDefault();
            var employee = departmentDesignations == null ? null : db.HRISEmployeeDetails.Where(d => d.EmpCode == departmentDesignations.EmpCode).FirstOrDefault();

            return new HRISDepartmentVM
            {
                DepartmentCode = department.DepartmentCode,
                Department = department.Department,
                DepartmentHead = employee == null ? null : employee.FName + " " + employee.MInitial + " " + employee.LName,
                DepartmentHeadDesignation = employee == null ? null : departmentDesignations.Designation
            };
        }
        public List<HRISDepartment> GetUserDepartments(string EmailAddress)
        {
            var departmentCode = fmis.UserAccounts.Where(d => d.Email == EmailAddress).FirstOrDefault().DepartmentCode;
            var motherUnit = db.HRISDepartments.Where(d => d.DepartmentCode == departmentCode).FirstOrDefault();
            var departments = db.HRISDepartments.Where(d => d.RootID == motherUnit.DepartmentID || d.DepartmentID == motherUnit.DepartmentID).ToList();
            //departments.Add(motherUnit);
            var designations = db.HRISEmployeeDesignation.Where(d => DateTime.Now >= d.IncStart && DateTime.Now <= d.IncEnd).OrderBy(d => d.DepartmentID).ToList();
            var departmentList = (from designation in designations
                                  join depts in departments
                                  on designation.DepartmentID equals depts.DepartmentID
                                  select depts
                                 ).ToList();
            return departmentList.OrderBy(d => d.DepartmentID).ToList();
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
                        EmployeeName = hrisEmployees.FName + " " + hrisEmployees.MInitial + " " + hrisEmployees.LName,
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