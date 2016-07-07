namespace Checkout.Model.Migrations
{
    using Contexts;
    using Model;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;

    public sealed class CheckoutConfiguration : DbMigrationsConfiguration<CheckoutDbContext>
    {
        public CheckoutConfiguration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(CheckoutDbContext context)
        {
            this.createData(context);
        }

        private void createData(CheckoutDbContext context)
        {
            try
            {
                context.Products.AddOrUpdate(
                            p =>
                            p.SKU,
                            new Product
                            {
                                SKU = "ipd",
                                Name = "Super iPad",
                                Price = 549.99m,
                            },
                            new Product()
                            {
                                SKU = "mbp",
                                Name = "MacBook Pro",
                                Price = 1399.99m,
                            },
                            new Product()
                            {
                                SKU = "atv",
                                Name = "Apple TV",
                                Price = 109.50m,
                            },
                            new Product()
                            {
                                SKU = "vga",
                                Name = "VGA adapter",
                                Price = 30.0m,
                            }
                        );

                context.SaveChanges();

                var atv = context.Products.First(p => p.SKU == "atv");
                var ipd = context.Products.First(p => p.SKU == "ipd");
                var mbp = context.Products.First(p => p.SKU == "mbp");
                var vga = context.Products.First(p => p.SKU == "vga");

                context.Campaign.AddOrUpdate(
                        c => c.ProductPurchasedSKU,
                        new CampaignBigDeal()
                        {
                            ProductPurchasedSKU = "atv",
                            ProductPurchasedId = atv.Id,
                            PurchaseQuantity = 3,
                            PayQuantity = 2,
                            Active = true
                        },
                        new CampaignPriceDrop()
                        {
                            ProductPurchasedSKU = "ipd",
                            ProductPurchasedId = ipd.Id,
                            PurchaseQuantity = 4,
                            NewPrice = 499.99m,
                            Active = true
                        },
                        new CampaignBundle()
                        {
                            ProductPurchasedSKU = "mbp",
                            ProductPurchasedId = mbp.Id,
                            ProductFreeOfChargeSKU = "vga",
                            ProductFreeOfChargeId = vga.Id,
                            Active = true
                        }
                    );

                context.SaveChanges();
            }
            catch (DbEntityValidationException exception)
            {
                foreach (DbValidationError validationError in
                    exception.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors))
                {
                    Trace.TraceInformation(
                        "Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                }
            }
        }
    }
}
