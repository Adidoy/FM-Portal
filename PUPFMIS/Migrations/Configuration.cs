namespace PUPFMIS.Migrations.FMISMigrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<PUPFMIS.Models.FMISDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = "Migrations";
        }

        protected override void Seed(PUPFMIS.Models.FMISDbContext context)
        {
            context.InventoryTypes.AddOrUpdate(
                new Models.InventoryType { ID = 1, InventoryTypeName = "Common Use Office Supplies", IsTangible = true, CreatedAt = System.DateTime.Now },
                new Models.InventoryType { ID = 2, InventoryTypeName = "Property and Equipment", IsTangible = true,  CreatedAt = System.DateTime.Now },
                new Models.InventoryType { ID = 3, InventoryTypeName = "Semi-Expendable Property and Equipment", IsTangible = true, CreatedAt = System.DateTime.Now },
                new Models.InventoryType { ID = 4, InventoryTypeName = "Services / Consultancy", IsTangible = false, CreatedAt = System.DateTime.Now },
                new Models.InventoryType { ID = 4, InventoryTypeName = "Infrastructure / Plant", IsTangible = false, CreatedAt = System.DateTime.Now }
            );
        }
    }
}
