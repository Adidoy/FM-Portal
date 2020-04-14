using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PUPFMIS.Models
{
    [Table("accounts_roles")]
    public class Roles
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }
    }

    [Table("accounts_userRoles")]
    public class UserRoles
    {
        [Key]
        public int ID { get; set; }

        public int User { get; set; }

        public int Role { get; set; }

        [Display(Name = "Date Assigned")]
        public DateTime AssignedAt { get; set; }

        [ForeignKey("User")]
        public virtual UserAccounts FKUser { get; set; }

        [ForeignKey("Role")]
        public virtual Roles FKRoles { get; set; }
     }

    public static class SystemRoles
    {
        public const string SuperUser = "Super User";
        public const string SystemAdmin = "System Administrator";
        public const string BudgetAdmin = "Budget Administrator";
        public const string BudgetOfficer = "Budget Officer";
        public const string EndUser = "End User";
    }

    public class UsersRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }
        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }
        public override string[] GetRolesForUser(string username)
        {
            using (FMISDbContext context = new FMISDbContext())
            {
                var userRoles = (from user in context.UserAccounts
                                 join userRole in context.UserRoles on user.ID equals userRole.User
                                 join role in context.Roles on userRole.Role equals role.ID
                                 where user.Email == username
                                 select role.Role).ToArray();
                return userRoles;
            }
        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }

}