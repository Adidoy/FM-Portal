using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace PUPFMIS.Models.HRIS
{
    public class HRISDbContext : DbContext
    {
        public HRISDbContext() : base("HRISDbContext")
        {
            Database.SetInitializer<HRISDbContext> (null);
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 250;
        }
        
        public DbSet<HRISDepartment> HRISDepartments { get; set; }
        public DbSet<HRISEmployeeDesignation> HRISEmployeeDesignation { get; set; }
        public DbSet<HRISEmployeeDetails> HRISEmployeeDetails { get; set; }
        public DbSet<HRISUserAccount> HRISUserAccounts { get; set; }
    }

}