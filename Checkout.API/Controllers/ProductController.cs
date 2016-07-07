using Checkout.Application.Contract.Contracts;
using Checkout.Application.Contract.ViewModels;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Http;

namespace Checkout.API.Controllers
{
    public class ProductController : ApiController
    {
        [Dependency]
        protected IProductApplicationService ProductApplicationService { get; set; }

        // GET api/<controller>
        public IEnumerable<ProductViewModel> Get()
        {
            return this.ProductApplicationService.Get();
        }

        [Route("api/product/sku/{sku}")]
        public ProductViewModel GetBySKU(string sku)
        {
            return this.ProductApplicationService.GetBySKU(sku);
        }
    }
}