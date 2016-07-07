using Checkout.Application.Contract.ViewModels;

namespace Checkout.Application.Contract.Contracts
{
    public interface IProductApplicationService : IApplicationServiceBase<ProductViewModel>
    {
        ProductViewModel GetById(int id);

        ProductViewModel GetBySKU(string sku);
    }
}
