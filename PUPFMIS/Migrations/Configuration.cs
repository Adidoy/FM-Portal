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
            context.Roles.AddOrUpdate(
                new Models.Roles { ID = 1, Role = "Super User" },
                new Models.Roles { ID = 2, Role = "System Administrator" },
                new Models.Roles { ID = 3, Role = "Procurement Administrator" },
                new Models.Roles { ID = 4, Role = "Property Administrator" },
                new Models.Roles { ID = 5, Role = "Supplies Administrator" },
                new Models.Roles { ID = 6, Role = "Warehouse Administrator" },
                new Models.Roles { ID = 7, Role = "Supplies Officer" },
                new Models.Roles { ID = 8, Role = "Budget Administrator" },
                new Models.Roles { ID = 9, Role = "Budget Officer" }
            );
        }
    }
}
