using Checkout.Model;
using Newtonsoft.Json;
using System;

namespace Checkout.Application.Contract.ViewModels
{
    public class ProductViewModel : BaseViewModel<ProductViewModel, Product>
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("updatedOn")]
        public DateTime UpdatedOn { get; set; }
    }
}
