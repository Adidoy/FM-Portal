using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;

namespace PUPFMIS.Models.HRIS
{
    public class HRISDbContext : DbContext
    {
        public HRISDbContext() : base("HRISDbContext")
        {
            Database.SetInitializer<HRISDbContext> (null);
        }
        
        public DbSet<Offices> OfficeModel { get; set; }
    }

}