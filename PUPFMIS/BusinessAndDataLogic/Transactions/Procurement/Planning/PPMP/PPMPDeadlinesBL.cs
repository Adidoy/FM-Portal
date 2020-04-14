using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    [Authorize]
    public class PPMPDeadlinesBL : Controller
    {
        private FMISDbContext db = new FMISDbContext();

        public List<PPMPDeadlines> GetOpenDeadlines()
        {
            return db.PPMPDeadlines.Where(d => d.PurgeFlag == false).ToList();
        }

        public PPMPDeadlines GetDeadlineDetails(int? ID)
        {
            return db.PPMPDeadlines.Find(ID);
        }

        public bool SetNewDeadline(PPMPDeadlines ppmpDeadlines)
        {
            ppmpDeadlines.Status = "Open";
            ppmpDeadlines.CreatedAt = DateTime.Now;
            ppmpDeadlines.PurgeFlag = false;
            db.PPMPDeadlines.Add(ppmpDeadlines);
            if (db.SaveChanges() == 1)
            {
                return true;
            }
            return false;
        }

        public bool StopDeadline(PPMPDeadlines ppmpDeadlines)
        {
            PPMPDeadlines PPMPDeadlines = db.PPMPDeadlines.Find(ppmpDeadlines.ID);
            PPMPDeadlines.Status = "Closed";
            PPMPDeadlines.CreatedAt = DateTime.Now;
            PPMPDeadlines.PurgeFlag = false;
            if (db.SaveChanges() == 1)
            {
                return true;
            }
            return false;
        }

        public bool UpdateDeadline(PPMPDeadlines ppmpDeadlines, bool DeleteFlag)
        {
            PPMPDeadlines PPMPDeadlines = db.PPMPDeadlines.Find(ppmpDeadlines.ID);

            if (PPMPDeadlines != null)
            {
                if (DeleteFlag == false)
                {
                    PPMPDeadlines.FiscalYear = ppmpDeadlines.FiscalYear;
                    PPMPDeadlines.StartDate = ppmpDeadlines.StartDate;
                    PPMPDeadlines.ClosingDate = ppmpDeadlines.ClosingDate;
                    PPMPDeadlines.UpdatedAt = DateTime.Now;
                }
                else
                {
                    PPMPDeadlines.PurgeFlag = true;
                    PPMPDeadlines.DeletedAt = DateTime.Now;
                }

                if (db.SaveChanges() == 1)
                {
                    return true;
                }
            }
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