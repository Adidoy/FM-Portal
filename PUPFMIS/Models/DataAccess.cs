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
            return db.Items.Where(d => d.PurgeFlag == DeleteFlag).OrderBy(d => d.FKItemTypeReference.ItemTypeName).ToList();
        }
        public Item GetItem(string ItemName, bool DeleteFlag)
        {
            return db.Items.Where(d => d.FKItemTypeReference.ItemTypeName == ItemName && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }
        public Item GetItem(int ID, bool DeleteFlag)
        {
            return db.Items.Where(d => d.ID == ID && d.PurgeFlag == DeleteFlag).FirstOrDefault();
        }


    }
    
    public class ChartOfAccountsAccess
    {
        private TEMPAccounting abdb = new TEMPAccounting();

        public List<ChartOfAccounts> GetChartOfAccounts()
        {
            return abdb.ChartOfAccounts.ToList();
        }
        public ChartOfAccounts GetChartOfAccountByUACS(string UACS)
        {
            return abdb.ChartOfAccounts.Where(d => d.UACS_Code == UACS).FirstOrDefault();
        }
        public ChartOfAccounts GetChartOfAccountByAccountTitle(string AccountTitle)
        {
            return abdb.ChartOfAccounts.Where(d => d.UACS_Code == AccountTitle).FirstOrDefault();
        }
    }
}