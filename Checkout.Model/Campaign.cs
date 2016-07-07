using Checkout.Infrastructure.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.Model
{
    public abstract class Campaign : BaseIdEntity
    {
        [ForeignKey("ProductPurchased"), Column(Order = 1)]
        public int ProductPurchasedId { get; set; }

        [ForeignKey("ProductPurchased"), Column(Order = 2)]
        public string ProductPurchasedSKU { get; set; }

        public virtual Product ProductPurchased { get; set; }

        public bool Active { get; set; }
    }

    [Table("CampaignBundle")]
    public class CampaignBundle : Campaign
    {
        [ForeignKey("ProductFreeOfCharge"), Column(Order = 3)]
        public int ProductFreeOfChargeId { get; set; }

        [ForeignKey("ProductFreeOfCharge"), Column(Order = 4)]
        public string ProductFreeOfChargeSKU { get; set; }

        public virtual Product ProductFreeOfCharge { get; set; }
    }

    [Table("CampaignBigDeal")]
    public class CampaignBigDeal : Campaign
    {
        public int PurchaseQuantity { get; set; }

        public int PayQuantity { get; set; }
    }

    [Table("CampaignPriceDrop")]
    public class CampaignPriceDrop : Campaign
    {
        public int PurchaseQuantity { get; set; }

        public decimal NewPrice { get; set; }
    }
}
