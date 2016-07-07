using Checkout.Business.Contract;
using Checkout.Model;
using System;
using System.Linq;

namespace Checkout.Business.Service
{
    public class ShoppingCartBusinessService : ServiceIdBase<ShoppingCart>, IShoppingCartBusinessService
    {
        public void AddProduct(int shoppingCartId, Product product, int quantity)
        {
            var shoppingCart = this.UnitOfWork.Repository.GetById(shoppingCartId);

            if (shoppingCart == null)
                throw new ArgumentException($"Shopping Cart not Found! Id: {shoppingCartId}");

            var cartItem = new ShoppingCartItem()
            {
                ProductId = product.Id,
                ProductSKU = product.SKU,
                Product = product,
                ShoppingCartId = shoppingCartId,
                Quantity = quantity
            };

            shoppingCart.ShoppingCartItems.Add(cartItem);

            this.UnitOfWork.Save();
        }

        [Obsolete]
        public void RemoveProduct(int shoppingCartId, string productSKU)
        {
            var shoppingCart = this.UnitOfWork.Repository.GetById(shoppingCartId);

            if (shoppingCart == null)
                throw new ArgumentException($"Shopping Cart not Found! Id: {shoppingCartId}");

            var item = shoppingCart.ShoppingCartItems.FirstOrDefault(sci => sci.ProductSKU == productSKU);

            if (item == null)
                throw new ArgumentException($"Item not found on Cart! Product SKU: {productSKU}");

            shoppingCart.ShoppingCartItems.Remove(item);

            this.UnitOfWork.Save();
        }

        [Obsolete]
        public void RemoveProduct(int shoppingCartId, int productId)
        {
            var shoppingCart = this.UnitOfWork.Repository.GetById(shoppingCartId);

            if (shoppingCart == null)
                throw new ArgumentException($"Shopping Cart not Found! Id: {shoppingCartId}");

            var item = shoppingCart.ShoppingCartItems.FirstOrDefault(sci => sci.ProductId == productId);

            if (item == null)
                throw new ArgumentException($"Item not found on Cart! Product Id: {productId}");

            shoppingCart.ShoppingCartItems.Remove(item);

            this.UnitOfWork.Save();
        }
    }
}
