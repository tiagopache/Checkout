using Checkout.Model.Contexts;
using Checkout.Model.Migrations;
using System.Data.Entity;

namespace Checkout.Model.Initialize
{
    public class DataSeedingInitializer : MigrateDatabaseToLatestVersion<CheckoutDbContext, CheckoutConfiguration> { }
}
