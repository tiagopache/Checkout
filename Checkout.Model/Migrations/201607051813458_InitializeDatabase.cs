namespace Checkout.Model.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitializeDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaigns",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ProductPurchasedId = c.Int(nullable: false),
                    ProductPurchasedSKU = c.String(maxLength: 128),
                    Active = c.Boolean(nullable: false),
                    CreatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    DeletedOn = c.DateTime(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => new { t.ProductPurchasedId, t.ProductPurchasedSKU })
                .Index(t => new { t.ProductPurchasedId, t.ProductPurchasedSKU });

            CreateTable(
                "dbo.Products",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SKU = c.String(nullable: false, maxLength: 128),
                    Name = c.String(),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CreatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    DeletedOn = c.DateTime(),
                })
                .PrimaryKey(t => new { t.Id, t.SKU });

            CreateTable(
                "dbo.ShoppingCartItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ProductId = c.Int(nullable: false),
                    ProductSKU = c.String(maxLength: 128),
                    ShoppingCartId = c.Int(nullable: false),
                    Quantity = c.Int(nullable: false),
                    CreatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    DeletedOn = c.DateTime(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => new { t.ProductId, t.ProductSKU })
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCartId, cascadeDelete: false)
                .Index(t => new { t.ProductId, t.ProductSKU })
                .Index(t => t.ShoppingCartId);

            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CreatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedOn = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    DeletedOn = c.DateTime(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CampaignBigDeal",
                c => new
                {
                    Id = c.Int(nullable: false),
                    PurchaseQuantity = c.Int(nullable: false),
                    PayQuantity = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.CampaignBundle",
                c => new
                {
                    Id = c.Int(nullable: false),
                    ProductFreeOfChargeId = c.Int(nullable: false),
                    ProductFreeOfChargeSKU = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.Id)
                .ForeignKey("dbo.Products", t => new { t.ProductFreeOfChargeId, t.ProductFreeOfChargeSKU })
                .Index(t => t.Id)
                .Index(t => new { t.ProductFreeOfChargeId, t.ProductFreeOfChargeSKU });

            CreateTable(
                "dbo.CampaignPriceDrop",
                c => new
                {
                    Id = c.Int(nullable: false),
                    PurchaseQuantity = c.Int(nullable: false),
                    NewPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Campaigns", t => t.Id)
                .Index(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.CampaignPriceDrop", "Id", "dbo.Campaigns");
            DropForeignKey("dbo.CampaignBundle", new[] { "ProductFreeOfChargeId", "ProductFreeOfChargeSKU" }, "dbo.Products");
            DropForeignKey("dbo.CampaignBundle", "Id", "dbo.Campaigns");
            DropForeignKey("dbo.CampaignBigDeal", "Id", "dbo.Campaigns");
            DropForeignKey("dbo.Campaigns", new[] { "ProductPurchasedId", "ProductPurchasedSKU" }, "dbo.Products");
            DropForeignKey("dbo.ShoppingCartItems", "ShoppingCartId", "dbo.ShoppingCarts");
            DropForeignKey("dbo.ShoppingCartItems", new[] { "ProductId", "ProductSKU" }, "dbo.Products");
            DropIndex("dbo.CampaignPriceDrop", new[] { "Id" });
            DropIndex("dbo.CampaignBundle", new[] { "ProductFreeOfChargeId", "ProductFreeOfChargeSKU" });
            DropIndex("dbo.CampaignBundle", new[] { "Id" });
            DropIndex("dbo.CampaignBigDeal", new[] { "Id" });
            DropIndex("dbo.ShoppingCartItems", new[] { "ShoppingCartId" });
            DropIndex("dbo.ShoppingCartItems", new[] { "ProductId", "ProductSKU" });
            DropIndex("dbo.Campaigns", new[] { "ProductPurchasedId", "ProductPurchasedSKU" });
            DropTable("dbo.CampaignPriceDrop");
            DropTable("dbo.CampaignBundle");
            DropTable("dbo.CampaignBigDeal");
            DropTable("dbo.ShoppingCarts");
            DropTable("dbo.ShoppingCartItems");
            DropTable("dbo.Products");
            DropTable("dbo.Campaigns");
        }
    }
}
