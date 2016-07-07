using Checkout.Model;

namespace Checkout.Business.Contract
{
    public interface IProductBusinessService : IBusinessServiceBase<Product>
    {
        Product GetBySKU(string sku);
    }
}
