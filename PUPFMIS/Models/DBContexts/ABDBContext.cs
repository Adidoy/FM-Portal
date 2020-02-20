using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PUPFMIS.Models.AIS
{
    public class ABDBContext : DbContext
    {
        public ABDBContext() : base("ABDbContext")
        {
            Database.SetInitializer<ABDBContext>(null);
        }

        public DbSet<ChartOfAccounts> ChartOfAccounts { get; set; }
    }
}