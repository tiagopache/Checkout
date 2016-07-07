using Checkout.Model;

namespace Checkout.Business.Contract
{
    public interface IShoppingCartBusinessService : IBusinessServiceBase<ShoppingCart>
    {
        void AddProduct(int shoppingCartId, Product product, int quantity);

        void RemoveProduct(int shoppingCartId, int productId);

        void RemoveProduct(int shoppingCartId, string productSKU);
    }
}
