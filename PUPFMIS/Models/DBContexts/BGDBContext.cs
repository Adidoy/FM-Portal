using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace PUPFMIS.Models
{
    public class BGDBContext : DbContext
    {
        public BGDBContext() : base("FMISDbContext")
        {
            Database.SetInitializer<BGDBContext>(null);
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 250;
        }

        public DbSet<Programs> Programs { get; set; }
    }
}