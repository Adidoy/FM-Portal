using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PUPFMIS.Models.AIS;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class ChartOfAccountsBL
    {
        private ABDBContext accountingContext = new ABDBContext();

        public List<ChartOfAccounts> GetChartOfAccounts()
        {
            return accountingContext.ChartOfAccounts.ToList();
        }
    }
    public class ChartOfAccountsDAL
    {
        private ABDBContext accountingContext = new ABDBContext();

        public List<ChartOfAccounts> GetChartOfAccounts()
        {
            return accountingContext.ChartOfAccounts.ToList();
        }
    }
}