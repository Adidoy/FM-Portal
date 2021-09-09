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
        private LogsMasterTables log = new LogsMasterTables();
        private HRISDataAccess hrisDataAccess = new HRISDataAccess();


        public List<Roles> GetRoles()
        {
            return db.Roles.ToList();
        }
        public List<UsersVM> GetUserAccountsList()
        {
            var accounts = db.UserAccounts.Where(d => d.PurgeFlag == false).OrderBy(d => d.RoleReference).ToList();
            return (from accts in accounts
                    join emps in hrisDataAccess.GetEmployees() on accts.EmpCode equals emps.EmpCode
                    join depts in hrisDataAccess.GetAllDepartments() on accts.DepartmentCode equals depts.DepartmentCode
                    join units in hrisDataAccess.GetAllDepartments() on accts.UnitCode equals units.DepartmentCode
                    select new UsersVM
                    {
                        UserID = accts.ID,
                        Email = accts.Email,
                        EmpCode = accts.EmpCode,
                        DepartmentCode = accts.DepartmentCode,
                        UnitCode = accts.DepartmentCode,
                        UserRole = accts.FKRoleReference.Role,
                        Employee = emps.LName + ", " + emps.FName + " " + emps.MInitial,
                        Department = depts.Department,
                        Unit = units.Department,
                        Designation = emps.PlantillaPosition,
                        RoleID = accts.RoleReference.Value
                    }
                   ).OrderBy(d => d.RoleID).ThenBy(d => d.Department).ThenBy(d => d.Unit).ThenBy(d => d.Employee).ToList();
        }
        public UsersVM GetUser(int UserID)
        {
            var account = db.UserAccounts.Find(UserID);
            var employee = hrisDataAccess.GetEmployeeByCode(account.EmpCode);
            var department = hrisDataAccess.GetDepartmentDetails(account.DepartmentCode);
            var unit = hrisDataAccess.GetDepartmentDetails(account.UnitCode);
            return new UsersVM
            {
                UserID = account.ID,
                Email = account.Email,
                EmpCode = employee.EmployeeCode,
                Employee = employee.EmployeeName,
                DepartmentCode = department.DepartmentCode,
                Department = department.Department,
                UnitCode = unit.DepartmentCode,
                Unit = unit.Department,
                Designation = employee.Designation,
                RoleID = (int) account.RoleReference,
                UserRole = account.FKRoleReference.Role,
            };
        }
        public UsersVM GetUser(string UserEmail)
        {
            var account = db.UserAccounts.Where(d => d.Email == UserEmail).FirstOrDefault();
            var employee = hrisDataAccess.GetEmployeeByCode(account.EmpCode);
            var department = hrisDataAccess.GetDepartmentDetails(account.DepartmentCode);
            var unit = hrisDataAccess.GetDepartmentDetails(account.UnitCode);
            return new UsersVM
            {
                UserID = account.ID,
                Email = account.Email,
                EmpCode = employee.EmployeeCode,
                Employee = employee.EmployeeName,
                DepartmentCode = department.DepartmentCode,
                Department = department.Department,
                UnitCode = unit.DepartmentCode,
                Unit = unit.Department,
                Designation = employee.Designation,
                UserRole = account.FKRoleReference.Role
            };
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
                userAccount.UnitCode = User.UnitCode;
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
        public bool DeleteUser(int UserID)
        {
            var userAccount = db.UserAccounts.Find(UserID);
            if (userAccount == null)
            {
                return false;
            }
            userAccount.PurgeFlag = true;
            userAccount.UpdatedAt = DateTime.Now;
            if (db.SaveChanges() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public JsonResult GetEmployeeDetails(string EmpCode)
        {
            var employee = hrisDataAccess.GetEmployeeByCode(EmpCode);
            var emailAddress = employee.Email;
            var designation = employee.Designation;
            return Json(new { Email = emailAddress, Designation = designation }, JsonRequestBehavior.AllowGet);
        }
        public bool RegisterUser(UsersVM userAccount)
        {
            var role = int.Parse(userAccount.UserRole);
            db.UserAccounts.Add(new UserAccounts {
                Email = userAccount.Email,
                EmpCode = userAccount.EmpCode,
                DepartmentCode = userAccount.DepartmentCode,
                UnitCode = userAccount.UnitCode,
                RoleReference = role,
                CreatedAt = DateTime.Now,
                IsLockedOut = false,
                LockoutDuration = null
            });
            if(db.SaveChanges() == 0)
            {
                return false;
            }
            else
            {
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

            var hrisDB = new HRISDbContext();
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

                var userInfo = hrisDataAccess.GetEmployeeDetailByCode(user.EmpCode);
                UserAccount = GetUser(user.ID);
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
                hrisDataAccess.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}