using Checkout.Application.Contract.Contracts;
using Checkout.Application.Contract.ViewModels;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Http;

namespace Checkout.API.Controllers
{
    public class ShoppingCartController : ApiController
    {
        [Dependency]
        protected IShoppingCartApplicationService ShoppingCartApplicationService { get; set; }

        // GET api/<controller>
        public IEnumerable<ShoppingCartViewModel> Get()
        {
            return this.ShoppingCartApplicationService.Get();
        }

        // GET api/<controller>/5
        public ShoppingCartViewModel Get(int id)
        {
            return this.ShoppingCartApplicationService.GetById(id);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            this.ShoppingCartApplicationService.Remove(id);
        }

        [Route("api/shoppingcart/add/{sku}/{qty}/{id:int?}")]
        [HttpGet]
        public ShoppingCartViewModel AddItem(string sku, int qty, int id = -1)
        {
            return this.ShoppingCartApplicationService.AddItem(sku, qty, id);
        }

        [Route("api/shoppingcart/remove/{sku}/{id}")]
        [HttpGet]
        public ShoppingCartViewModel RemoveItem(string sku, int id)
        {
            return this.ShoppingCartApplicationService.RemoveItem(id, sku);
        }

        [Route("api/shoppingcart/checkout/{id}")]
        [HttpGet]
        public CheckoutViewModel Checkout(int id)
        {
            return this.ShoppingCartApplicationService.Checkout(id);
        }
    }
}