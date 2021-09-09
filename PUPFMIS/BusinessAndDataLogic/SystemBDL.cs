using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using PUPFMIS.Models;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class SystemBDL : Controller
    {
        private FMISDbContext db = new FMISDbContext();

        private class Months
        {
            public int MonthNo { get; set; }
            public string MonthName { get; set; }
        }
        public SelectList GetMonths()
        {

            List<Months> months = new List<Months>();
            var monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            for (int i = 0; i < monthNames.Length - 1; i++)
            {
                months.Add(new Months { MonthNo = i + 1, MonthName = monthNames[i] });
            }
            return new SelectList(months, "MonthNo", "MonthName");
        }
        public string GetMonthName(int MonthNo)
        {
            var monthName = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[MonthNo - 1];
            return monthName;
        }
        public int GetMonthNo(string MonthName)
        {
            int month = 0;
            switch (MonthName)
            {
                case "January": month = 1; break;
                case "February": month = 2; break;
                case "March": month = 3; break;
                case "April": month = 4; break;
                case "May": month = 5; break;
                case "June": month = 6; break;
                case "July": month = 7; break;
                case "August": month = 8; break;
                case "September": month = 9; break;
                case "October": month = 10; break;
                case "November": month = 11; break;
                case "December": month = 12; break;
            }
            return month;
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