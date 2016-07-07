using Checkout.Application.Contract.Contracts;
using Checkout.Application.Contract.ViewModels;
using Checkout.Business.Contract;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Application.Service
{
    public class ProductApplicationService : IProductApplicationService
    {
        [Dependency]
        protected IProductBusinessService ProductBusinessService { get; set; }

        public IList<ProductViewModel> Get(string includeProperties = null)
        {
            return ProductViewModel.Instance.ToContract(ProductBusinessService.Get(includeProperties: includeProperties)).ToList();
        }

        public ProductViewModel GetById(int id)
        {
            return ProductViewModel.Instance.ToContract(this.ProductBusinessService.GetById(id));
        }

        public ProductViewModel GetBySKU(string sku)
        {
            return ProductViewModel.Instance.ToContract(this.ProductBusinessService.GetBySKU(sku));
        }
    }
}
