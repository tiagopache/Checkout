using Checkout.Business.Contract;
using Checkout.Model;
using System.Linq;

namespace Checkout.Business.Service
{
    public class ProductBusinessService : ServiceIdBase<Product>, IProductBusinessService
    {
        public Product GetBySKU(string sku)
        {
            var result = this.UnitOfWork.Repository.Get(p => p.SKU == sku);

            return result.FirstOrDefault();
        }
    }
}
