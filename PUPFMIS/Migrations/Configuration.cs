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
                new Models.InventoryType { ID = 1, InventoryTypeName = "Common Use Office Supplies", InventoryCode = "CUSE", IsTangible = true, CreatedAt = System.DateTime.Now },
                new Models.InventoryType { ID = 2, InventoryTypeName = "Property and Equipment", InventoryCode = "PREQ", IsTangible = true,  CreatedAt = System.DateTime.Now },
                new Models.InventoryType { ID = 3, InventoryTypeName = "Semi-Expendable Property and Equipment", InventoryCode = "SXPE", IsTangible = true, CreatedAt = System.DateTime.Now },
                new Models.InventoryType { ID = 4, InventoryTypeName = "Services / Consultancy", InventoryCode = "SERV", IsTangible = false, CreatedAt = System.DateTime.Now },
                new Models.InventoryType { ID = 5, InventoryTypeName = "Infrastructure / Plant", InventoryCode = "INFR", IsTangible = false, CreatedAt = System.DateTime.Now }
            );
            context.SystemVariables.AddOrUpdate(
                new Models.SystemVariables { ID = 1, VariableName = "Quantity Inflation Rate (%)", Value = "5"}
            );
        }
    }
}
