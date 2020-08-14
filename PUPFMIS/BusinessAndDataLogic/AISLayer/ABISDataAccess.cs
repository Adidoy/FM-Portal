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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                fmis.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}