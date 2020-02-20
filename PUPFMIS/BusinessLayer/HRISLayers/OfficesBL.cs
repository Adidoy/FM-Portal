﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models.HRIS;

namespace PUPFMIS.BusinessLayer
{
    public class OfficesBL : Controller
    {
        private HRISDbContext db = new HRISDbContext();

        public List<Offices> GetOffices()
        {
            return db.OfficeModel.ToList();
        }
    }
}