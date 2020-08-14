using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace PUPFMIS.Models.AIS
{
    public class TEMPAccounting : DbContext
    {
        public TEMPAccounting() : base("ABDbContext")
        {
            Database.SetInitializer<TEMPAccounting>(null);
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 250;
        }

        public DbSet<ChartOfAccounts> ChartOfAccounts { get; set; }
        public DbSet<FundSources> FundSources { get; set; }
    }
}