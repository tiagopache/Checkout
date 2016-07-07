using Checkout.Infrastructure.Data.Contexts;
using Checkout.Model.Initialize;
using System.Data.Entity;
using System.Diagnostics;

namespace Checkout.Model.Contexts
{
    public class CheckoutDbContext : DbContext, IDbContext
    {
        public IDbSet<Product> Products { get; set; }

        public IDbSet<ShoppingCart> ShoppingCarts { get; set; }

        public IDbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public IDbSet<Campaign> Campaign { get; set; }

        public CheckoutDbContext() : base("CheckoutDbContext")
        {
            this.basicContextConfiguration();
        }

        public void Initialize(IDatabaseInitializer<DbContext> databaseInitializer = null)
        {
            Database.SetInitializer(databaseInitializer == null ? new DataSeedingInitializer() : databaseInitializer as IDatabaseInitializer<CheckoutDbContext>);
            Migrator.RunMigrations();

            this.Database.Initialize(force: true);
        }

        private void basicContextConfiguration()
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
            this.logSql();
        }

        [Conditional("DEBUG")]
        private void logSql()
        {
            this.Database.Log = s => Debug.WriteLine(s);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
