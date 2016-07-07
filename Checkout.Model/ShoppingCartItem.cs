using Checkout.Infrastructure.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.Model
{
    public class ShoppingCartItem : BaseIdEntity
    {
        [ForeignKey("Product"), Column(Order = 1)]
        public int ProductId { get; set; }

        [ForeignKey("Product"), Column(Order = 2)]
        public string ProductSKU { get; set; }

        [ForeignKey("ShoppingCart"), Column(Order = 3)]
        public int ShoppingCartId { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}
