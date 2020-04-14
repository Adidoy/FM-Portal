using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PUPFMIS.Models;
using PUPFMIS.Models.AIS;
using PUPFMIS.Models.HRIS;

namespace PUPFMIS.DataAccess
{
    public class ItemMasterAccess
    {
        private FMISDbContext db = new FMISDbContext();

        public List<Item> GetItems(bool DeleteFlag)
        {
            return db.Items.Where(d => d.PurgeFlag == DeleteFlag).OrderBy(d => d.ItemName).ToList();
        }

        public Item GetItem(string ItemName, bool DeleteFlag)
        {
            return db.Items.Where(d => d.ItemName == ItemName && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }

        public Item GetItem(int ID, bool DeleteFlag)
        {
            return db.Items.Where(d => d.ID == ID && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }


    }
    
    public class ChartOfAccountsAccess
    {
        private ABDBContext abdb = new ABDBContext();

        public List<ChartOfAccounts> GetChartOfAccounts()
        {
            return abdb.ChartOfAccounts.ToList();
        }

        public ChartOfAccounts GetChartOfAccountByUACS(string UACS)
        {
            return abdb.ChartOfAccounts.Where(d => d.UACS == UACS).FirstOrDefault();
        }

        public ChartOfAccounts GetChartOfAccountByAccountTitle(string AccountTitle)
        {
            return abdb.ChartOfAccounts.Where(d => d.AccountTitle == AccountTitle).FirstOrDefault();
        }
    }
}