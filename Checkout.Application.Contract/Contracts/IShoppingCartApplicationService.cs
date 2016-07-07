using Checkout.Application.Contract.ViewModels;

namespace Checkout.Application.Contract.Contracts
{
    public interface IShoppingCartApplicationService : IApplicationServiceBase<ShoppingCartViewModel>
    {
        ShoppingCartViewModel GetById(int id);

        void Remove(int id);

        ShoppingCartViewModel AddItem(int productId, int quantity, int shoppingCartId = -1);

        ShoppingCartViewModel AddItem(string productSKU, int quantity, int shoppingCartId = -1);

        ShoppingCartViewModel RemoveItem(int shoppingCartId, int productId);

        ShoppingCartViewModel RemoveItem(int shoppingCartId, string productSKU);

        CheckoutViewModel Checkout(int shoppingCartId);
    }
}
