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
            for (int i = 0; i < monthNames.Length-1; i++)
            {
                months.Add(new Months { MonthNo = i + 1, MonthName = monthNames[i] });
            }
            return new SelectList(months, "MonthNo", "MonthName");
        }

        public string GetMonthName(int MonthNo)
        {
            var monthName = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[MonthNo-1];
            return monthName;
        }

        public SelectList GetFiscalYears()
        {
            return new SelectList(db.PPMPDeadlines.Where(d => d.PurgeFlag == false).GroupBy(d => d.FiscalYear).Select(d => new { FiscalYear = d.Key }).ToList(), "FiscalYear", "FiscalYear");
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