using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PUPFMIS.Models.AIS;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ChartOfAccountsBL
    {
        private TEMPAccounting accountingContext = new TEMPAccounting();

        public List<ChartOfAccounts> GetChartOfAccounts()
        {
            return accountingContext.ChartOfAccounts.ToList();
        }
        public ChartOfAccounts GetChartOfAccounts(string UACS)
        {
            return accountingContext.ChartOfAccounts.Where(d => d.UACS_Code == UACS).FirstOrDefault();
        }
    }
    public class ChartOfAccountsDAL
    {
        private TEMPAccounting accountingContext = new TEMPAccounting();

        public List<ChartOfAccounts> GetChartOfAccounts()
        {
            return accountingContext.ChartOfAccounts.ToList();
        }
    }
}