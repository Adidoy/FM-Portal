using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PUPFMIS.Models.AIS;
using PUPFMIS.Models;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ABISDataAccess : Controller
    {
        private TEMPAccounting db = new TEMPAccounting();
        private FMISDbContext fmis = new FMISDbContext();
        private BGDBContext bgdb = new BGDBContext();

        public List<ChartOfAccounts> GetChartOfAccounts()
        {
            return db.ChartOfAccounts.ToList();
        }
        public List<FundSources> GetFundSources()
        {
            return db.FundSources.ToList();
        }
        public FundSources GetFundSources(string UACS)
        {
            return db.FundSources.Where(d => d.FUND_CLUSTER.Contains(UACS)).FirstOrDefault();
        }
        public ChartOfAccounts GetChartOfAccounts(string UACS)
        {
            return db.ChartOfAccounts.Where(d => d.UACS_Code == UACS).FirstOrDefault();
        }
        public List<ChartOfAccountsDetailed> GetDetailedChartOfAccounts()
        {
            return db.ChartOfAccounts
                .Select(d => new ChartOfAccountsDetailed {
                    UACS_Code = d.UACS_Code,
                    GenAcctName = d.GenAcctName,
                    AcctName = d.SubAcctName + " - " + d.AcctName
                }).ToList();
        }

        public List<Programs> GetPrograms()
        {
            return bgdb.Programs.Where(d => d.IsActive == true).ToList();
        }
        public Programs GetPrograms(string PAPCode)
        {
            return bgdb.Programs.Where(d => d.PAPCode == PAPCode && d.IsActive == true).FirstOrDefault();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                fmis.Dispose();
                bgdb.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}