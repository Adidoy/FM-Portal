using FluentValidation.Results;
using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ItemAllowedUsersDataAccess : Controller
    {
        FMISDbContext db = new FMISDbContext();

        public List<string> GetAllowedUsers(int ItemID)
        {
            return db.ItemAllowedUsers.Where(d => d.ItemReference == ItemID).Select(d => d.DepartmentReference).ToList();
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