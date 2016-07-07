using Checkout.Model.Migrations;
using System.Data.Entity.Migrations;

namespace Checkout.Model.Initialize
{
    public class Migrator
    {
        public static void RunMigrations()
        {
            var migrationConfigurations = new CheckoutConfiguration();
            var dbMigrator = new DbMigrator(migrationConfigurations);

            dbMigrator.Update();
        }
    }
}
