using Newtonsoft.Json;

namespace Checkout.Application.Contract.ViewModels
{
    public class CheckoutViewModel
    {
        [JsonProperty("cartId")]
        public int ShoppingCartId { get; set; }

        [JsonProperty("cart")]
        public ShoppingCartViewModel Cart { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }
    }
}
