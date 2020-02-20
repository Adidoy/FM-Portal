using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models;
using PUPFMIS.Models.HRIS;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Web.Security;

namespace PUPFMIS.BusinessLayer
{
    public class AccountsManagementBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();
        private HRISDbContext hrisDB = new HRISDbContext();
        private LogsMasterTables log = new LogsMasterTables();

        public List<UsersVM> GetUsers(bool? DeleteFlag)
        {
            List<UsersVM> usersList = new List<UsersVM>();
            if (DeleteFlag != null)
            {
                usersList = (from userAccounts in db.UserAccounts
                                join userInformation in db.UserInformation
                                on userAccounts.UserInformationReference equals userInformation.ID
                                where userAccounts.PurgeFlag == DeleteFlag
                                select new UsersVM
                                {
                                    ID = userAccounts.ID,
                                    Email = userAccounts.Email,
                                    Password = userAccounts.Password,
                                    LastName = userInformation.LastName,
                                    FirstName = userInformation.FirstName,
                                    MiddleName = userInformation.MiddleName,
                                    Designation = userInformation.Designation,
                                    Office = userInformation.Office
                                }).ToList();
            }
            else
            {
                usersList = (from userAccounts in db.UserAccounts
                                join userInformation in db.UserInformation
                                on userAccounts.UserInformationReference equals userInformation.ID
                                select new UsersVM
                                {
                                    ID = userAccounts.ID,
                                    Email = userAccounts.Email,
                                    Password = userAccounts.Password,
                                    LastName = userInformation.LastName,
                                    FirstName = userInformation.FirstName,
                                    MiddleName = userInformation.MiddleName,
                                    Designation = userInformation.Designation,
                                    Office = userInformation.Office
                                }).ToList();
            }
            return usersList;
        }

        public UsersVM GetUsers(int? ID, bool? DeleteFlag)
        {
            UsersVM usersList = new UsersVM();
            if (DeleteFlag != null)
            {
                usersList = (from userAccounts in db.UserAccounts
                             join userInformation in db.UserInformation
                             on userAccounts.UserInformationReference equals userInformation.ID
                             where userAccounts.PurgeFlag == DeleteFlag && userAccounts.ID == ID
                             select new UsersVM
                             {
                                 ID = userAccounts.ID,
                                 Email = userAccounts.Email,
                                 Password = userAccounts.Password,
                                 LastName = userInformation.LastName,
                                 FirstName = userInformation.FirstName,
                                 MiddleName = userInformation.MiddleName,
                                 Designation = userInformation.Designation,
                                 Office = userInformation.Office
                             }).FirstOrDefault();
            }
            else
            {
                usersList = (from userAccounts in db.UserAccounts
                             join userInformation in db.UserInformation
                             on userAccounts.UserInformationReference equals userInformation.ID
                             where userAccounts.ID == ID
                             select new UsersVM
                             {
                                 ID = userAccounts.ID,
                                 Email = userAccounts.Email,
                                 Password = userAccounts.Password,
                                 LastName = userInformation.LastName,
                                 FirstName = userInformation.FirstName,
                                 MiddleName = userInformation.MiddleName,
                                 Designation = userInformation.Designation,
                                 Office = userInformation.Office
                             }).FirstOrDefault();
            }
            return usersList;
        }

        public UsersVM GetUsers(string Email, bool? DeleteFlag)
        {
            UsersVM usersList = new UsersVM();
            if (DeleteFlag != null)
            {
                usersList = (from userAccounts in db.UserAccounts
                             join userInformation in db.UserInformation
                             on userAccounts.UserInformationReference equals userInformation.ID
                             where userAccounts.PurgeFlag == DeleteFlag && userAccounts.Email == Email
                             select new UsersVM
                             {
                                 ID = userAccounts.ID,
                                 Email = userAccounts.Email,
                                 Password = userAccounts.Password,
                                 LastName = userInformation.LastName,
                                 FirstName = userInformation.FirstName,
                                 MiddleName = userInformation.MiddleName,
                                 Designation = userInformation.Designation,
                                 Office = userInformation.Office
                             }).FirstOrDefault();
            }
            else
            {
                usersList = (from userAccounts in db.UserAccounts
                             join userInformation in db.UserInformation
                             on userAccounts.UserInformationReference equals userInformation.ID
                             where userAccounts.Email == Email
                             select new UsersVM
                             {
                                 ID = userAccounts.ID,
                                 Email = userAccounts.Email,
                                 Password = userAccounts.Password,
                                 LastName = userInformation.LastName,
                                 FirstName = userInformation.FirstName,
                                 MiddleName = userInformation.MiddleName,
                                 Designation = userInformation.Designation,
                                 Office = userInformation.Office
                             }).FirstOrDefault();
            }
            return usersList;
        }

        public Offices GetUserOffice(string Email)
        {
            int OfficeID = GetUsers(Email, false).Office;
            return hrisDB.OfficeModel.Find(OfficeID);
        }
        
        public bool RegisterUser(UsersVM userAccount, out string Message)
        {
            var salt = Crypto.GenerateSalt();
            var saltedPassword = userAccount.Password + salt;
            var hashedPassword = Crypto.HashPassword(saltedPassword);
            userAccount.Password = hashedPassword;
            userAccount.PasswordSalt = salt;

            DbPropertyValues _currentValues;

            UserAccounts account = new UserAccounts();
            UserInformation information = new UserInformation();

            information.FirstName = userAccount.FirstName;
            information.MiddleName = userAccount.MiddleName;
            information.LastName = userAccount.LastName;
            information.Designation = userAccount.Designation;

            db.UserInformation.Add(information);
            _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;

            if (db.SaveChanges() == 1)
            {
                log.Action = "Add Record";
                log.AuditableKey = information.ID;
                log.ProcessedBy = null;
                log.TableName = "accounts_userInformation";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, null, _currentValues);
            }
            else
            {
                Message = "Unable to save record. Please try again.";
                return false;
            }

            account.Email = userAccount.Email;
            account.Password = userAccount.Password;
            account.PasswordSalt = userAccount.PasswordSalt;
            account.PurgeFlag = false;
            account.CreatedAt = DateTime.Now;
            account.UserInformationReference = information.ID;

            db.UserAccounts.Add(account);
            _currentValues = db.ChangeTracker.Entries().Where(d => d.State == EntityState.Added).First().CurrentValues;

            if (db.SaveChanges() == 1)
            {
                log.Action = "Add Record";
                log.AuditableKey = account.ID;
                log.ProcessedBy = null;
                log.TableName = "accounts_userAccounts";
                MasterTablesLogger logger = new MasterTablesLogger();
                logger.Log(log, null, _currentValues);

                Message = "User account saved!";
                return true;
            }
            else
            {
                Message = "Unable to save record. Please try again.";
                return false;
            }
        }

        public bool VerifyUserCredentials(LoginVM loginCredentials, out string ErrorMessage, out UsersVM userAccount)
        {
            var Email = loginCredentials.Email;
            var IsEmailValid = (db.UserAccounts.Where(d => d.Email == Email).Count() == 1) ? true : false;

            if(IsEmailValid)
            {
                var salt = db.UserAccounts.Where(d => d.Email == Email).FirstOrDefault().PasswordSalt;
                var hashedPassword = db.UserAccounts.Where(d => d.Email == loginCredentials.Email).FirstOrDefault().Password;
                var saltedPassword = loginCredentials.Password + salt;
                if (Crypto.VerifyHashedPassword(hashedPassword, saltedPassword) && db.UserAccounts.Where(d => d.Email == Email && d.Password == hashedPassword).Count() == 1)
                {
                    ErrorMessage = "Success";
                    userAccount = (from account in db.UserAccounts
                                   join information in db.UserInformation
                                   on account.UserInformationReference equals information.ID
                                   where account.Email == loginCredentials.Email
                                   select new UsersVM
                                   {
                                       Email = account.Email,
                                       FirstName = information.FirstName,
                                       LastName = information.LastName,
                                       Designation = information.Designation
                                   }).FirstOrDefault();
                    return true;
                }
            }
            else
            {
                ErrorMessage = "Email is invalid. Please check your input.";
                userAccount = null;
                return false;
            }
            ErrorMessage = "An error occured. Please try again.";
            userAccount = null;
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}