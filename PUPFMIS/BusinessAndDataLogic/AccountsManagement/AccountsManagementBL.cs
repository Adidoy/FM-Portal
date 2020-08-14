using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class AccountsManagementBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDbContext hrisDB = new HRISDbContext();
        private LogsMasterTables log = new LogsMasterTables();

        public List<Roles> GetRoles()
        {
            return db.Roles.ToList();
        }
        public List<DepartmentListVM> GetDepartment()
        {
            return hrisDB.HRISDepartments
                .Select(d => new DepartmentListVM { DepartmentCode = d.DepartmentCode, Department = d.Department })
                .OrderBy(d => d.Department).ToList();
        }
        public List<UsersVM> GetUserAccountsList()
        {
            var accounts = db.UserAccounts.ToList();
            var employees = (from employeeList in hrisDB.HRISEmployeeDetails
                             join departments in hrisDB.HRISDepartments
                             on employeeList.DepartmentCode equals departments.DepartmentCode
                             where employeeList.IsActive == "Y"
                             select new
                             {
                                 EmpCode = employeeList.EmpCode,
                                 EmployeeName = employeeList.FName + " " + employeeList.MInitial + " " + employeeList.LName,
                                 DeptCode = employeeList.DepartmentCode,
                                 Department = hrisDB.HRISDepartments.Where(d => d.DepartmentCode == employeeList.DepartmentCode).FirstOrDefault().Department,
                                 Designation = employeeList.PlantillaPosition
                             }).ToList();
            var userAccounts = (from accountList in accounts
                                join employeeList in employees
                                on accountList.EmpCode equals employeeList.EmpCode
                                select new UsersVM
                                {
                                    UserID = accountList.ID,
                                    Email = accountList.Email,
                                    EmpCode = employeeList.EmpCode,
                                    Employee = employeeList.EmployeeName,
                                    OfficeCode = employeeList.DeptCode,
                                    Office = employeeList.Department,
                                    Designation = employeeList.Designation,
                                    UserRole = accountList.FKRoleReference.Role
                                }).ToList();
            return userAccounts;
        }
        public UsersVM GetUser(int UserID)
        {
            var employee = (from hrisEmployee in hrisDB.HRISEmployeeDetails
                        join hrisAccount in hrisDB.HRISUserAccounts.Where(d => d.UserName.Contains("@"))
                        on hrisEmployee.EmpCode equals hrisAccount.EmpCode
                        join hrisDepartments in hrisDB.HRISDepartments
                        on hrisEmployee.DepartmentCode equals hrisDepartments.DepartmentCode
                        select new
                        {
                            EmpCode = hrisEmployee.EmpCode,
                            Employee = hrisEmployee.FName + " " + hrisEmployee.MInitial + " " + hrisEmployee.LName,
                            Email = hrisAccount.UserName,
                            DeptCode = hrisDepartments.DepartmentCode,
                            Department = hrisDepartments.Department,
                            Designation = hrisEmployee.PlantillaPosition
                        }).ToList();
            var users = db.UserAccounts.ToList();

            return (from employeeList in employee
                    join usersList in users
                    on employeeList.EmpCode equals usersList.EmpCode
                    where usersList.ID == UserID
                    select new UsersVM
                    {
                        UserID = usersList.ID,
                        Email = employeeList.Email,
                        EmpCode = employeeList.EmpCode,
                        Employee = employeeList.Employee,
                        OfficeCode = employeeList.DeptCode,
                        Office = employeeList.Department,
                        Designation = employeeList.Designation,
                        UserRole = usersList.RoleReference.ToString()
                    }).FirstOrDefault();
        }
        public UsersVM GetUser(string UserEmail)
        {
            var employee = (from hrisEmployee in hrisDB.HRISEmployeeDetails
                            join hrisAccount in hrisDB.HRISUserAccounts
                            on hrisEmployee.EmpCode equals hrisAccount.EmpCode
                            join hrisDepartments in hrisDB.HRISDepartments
                            on hrisEmployee.DepartmentCode equals hrisDepartments.DepartmentCode
                            select new
                            {
                                EmpCode = hrisEmployee.EmpCode,
                                Employee = hrisEmployee.FName + " " + hrisEmployee.MInitial + " " + hrisEmployee.LName,
                                Email = hrisAccount.UserName,
                                DeptCode = hrisDepartments.DepartmentCode,
                                Department = hrisDepartments.Department,
                                Designation = hrisEmployee.PlantillaPosition
                            }).ToList();
            var users = db.UserAccounts.ToList();

            return (from employeeList in employee
                    join usersList in users
                    on employeeList.EmpCode equals usersList.EmpCode
                    where usersList.Email == UserEmail
                    select new UsersVM
                    {
                        UserID = usersList.ID,
                        Email = employeeList.Email,
                        EmpCode = employeeList.EmpCode,
                        Employee = employeeList.Employee,
                        OfficeCode = employeeList.DeptCode,
                        Office = employeeList.Department,
                        Designation = employeeList.Designation,
                        UserRole = usersList.RoleReference.ToString()
                    }).FirstOrDefault();
        }
        public bool UpdateUser(UsersVM User)
        {
            if(User == null)
            {
                return false;
            }
            else
            {
                var userAccount = db.UserAccounts.Find(User.UserID);
                if(userAccount == null)
                {
                    return false;
                }
                userAccount.RoleReference = int.Parse(User.UserRole);
                userAccount.UpdatedAt = DateTime.Now;
                if(db.SaveChanges() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        public List<EmployeeListVM> GetEmployees(string DepartmentCode)
        {
            var userAccounts = db.UserAccounts.Select(d => d.EmpCode).ToList();
            var employees = (from employeesList in hrisDB.HRISEmployeeDetails
                             select new
                             {
                                 EmpCode = employeesList.EmpCode,
                                 EmployeeName = employeesList.FName + " " + employeesList.MInitial + " " + employeesList.LName,
                                 DeptCode = employeesList.DepartmentCode
                             }).OrderBy(d => d.EmployeeName).ToList();
            var employeeListVM = employees.Where(d => d.DeptCode == DepartmentCode)
                                 .Select(d => new EmployeeListVM {
                                     EmpCode = d.EmpCode,
                                     EmployeeName = d.EmployeeName
                                 }).ToList();
            return employeeListVM.Where(d => !userAccounts.Contains(d.EmpCode)).ToList();
        }
        public JsonResult GetEmployeeDetails(string EmpCode)
        {
            var emailAddress = (from employees in hrisDB.HRISEmployeeDetails
                                join accounts in hrisDB.HRISUserAccounts.Where(d => d.UserName.Contains("@"))
                                on employees.EmpCode equals accounts.EmpCode
                                where employees.EmpCode == EmpCode && employees.IsActive == "Y"
                                select accounts.UserName
                               ).FirstOrDefault();

            var designation = (from employees in hrisDB.HRISEmployeeDetails
                               join accounts in hrisDB.HRISUserAccounts
                               on employees.EmpCode equals accounts.EmpCode
                               where employees.EmpCode == EmpCode && employees.IsActive == "Y"
                               select employees.PlantillaPosition
                              ).FirstOrDefault();

            return Json(new { Email = emailAddress, Designation = designation }, JsonRequestBehavior.AllowGet);
        }
        public bool RegisterUser(UsersVM userAccount, out string Message)
        {
            var role = int.Parse(userAccount.UserRole);
            db.UserAccounts.Add(new UserAccounts {
                Email = userAccount.Email,
                EmpCode = userAccount.EmpCode,
                DepartmentCode = userAccount.OfficeCode,
                RoleReference = role,
                CreatedAt = DateTime.Now,
                IsLockedOut = false,
                LockoutDuration = null
            });
            if(db.SaveChanges() == 0)
            {
                Message = "An error occurred. Please try again.";
                return false;
            }
            else
            {
                Message = "User Information successfully saved!";
                return true;
            }
        }
        public bool VerifyUserCredentials(LoginVM LoginCredentials, out UsersVM UserAccount, out string Error)
        {
            var user = db.UserAccounts.Where(d => d.Email == LoginCredentials.Email).FirstOrDefault();
            if(user == null)
            {
                UserAccount = null;
                Error = "Invalid Email";
                return false;
            }

            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@Username",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 50,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = LoginCredentials.Email
                        },
                        new SqlParameter() {
                            ParameterName = "@Password",
                            SqlDbType =  System.Data.SqlDbType.VarChar,
                            Size = 50,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = LoginCredentials.Password
                        }};

            var userData = hrisDB.HRISUserAccounts.SqlQuery("[dbo].[ValidateUser] @Username, @Password", param).FirstOrDefault();
            if (userData != null)
            {
                user = db.UserAccounts.Where(d => d.Email == userData.UserName).FirstOrDefault();
                if(user == null)
                {
                    UserAccount = null;
                    Error = "No Account";
                    return false;
                }

                if(user.IsLockedOut == true && DateTime.Now <= user.LockoutDuration)
                {
                    UserAccount = null;
                    Error = "Locked Out";
                    return false;
                }

                if (user.IsLockedOut == true && DateTime.Now > user.LockoutDuration)
                {
                    RevokeUserLockout(LoginCredentials.Email);
                }

                UserAccount = (from userInfo in hrisDB.HRISEmployeeDetails
                               where userInfo.EmpCode == userData.EmpCode
                               select new UsersVM
                               {
                                   Email = userData.UserName,
                                   EmpCode = userInfo.EmpCode,
                                   Employee = userInfo.FName + " " + userInfo.MInitial + " " + userInfo.LName,
                                   Designation = userInfo.PlantillaPosition,
                                   OfficeCode = userInfo.DepartmentCode,
                                   Office = hrisDB.HRISDepartments.Where(d => d.DepartmentCode == userInfo.DepartmentCode).FirstOrDefault().Department,
                                   UserRole = user.FKRoleReference.Role
                               }).FirstOrDefault();
                Error = string.Empty;
                return true;
            }
            else
            {
                if(LoginCredentials.NoOfAttempts == 5)
                {
                    UserLockout(LoginCredentials.Email);
                    UserAccount = null;
                    Error = "Locked Out";
                    return false;
                }
                else
                {
                    UserAccount = null;
                    Error = "Incorrect Password";
                    return false;
                }
            }
        }
        private void UserLockout(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            user.IsLockedOut = true;
            user.LockoutDuration = DateTime.Now.AddMinutes(5.0);
            db.SaveChanges();
        }
        private void RevokeUserLockout(string UserEmail)
        {
            var user = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            user.IsLockedOut = false;
            user.LockoutDuration = null;
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                hrisDB.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}