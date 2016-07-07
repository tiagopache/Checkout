using Checkout.Model;
using Newtonsoft.Json;

namespace Checkout.Application.Contract.ViewModels
{
    public class ShoppingCartItemViewModel : BaseViewModel<ShoppingCartItemViewModel, ShoppingCartItem>
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("product")]
        public ProductViewModel Product { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        public override ShoppingCartItemViewModel ToContract(ShoppingCartItem entity)
        {
            var result = base.ToContract(entity);

            result.Product = ProductViewModel.Instance.ToContract(entity.Product);

            return result;
        }
    }
}
