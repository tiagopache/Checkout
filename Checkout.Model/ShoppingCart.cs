using Checkout.Infrastructure.Data.Base;
using System.Collections.Generic;

namespace Checkout.Model
{
    public class ShoppingCart : BaseIdEntity
    {
        #region Singleton Pattern - ShoppingCartItems
        private IList<ShoppingCartItem> _shoppingCartItems;
        public virtual IList<ShoppingCartItem> ShoppingCartItems
        {
            get
            {
                if (_shoppingCartItems == null)
                    _shoppingCartItems = new List<ShoppingCartItem>();

                return _shoppingCartItems;
            }
            set
            {
                _shoppingCartItems = value;
            }
        }
        #endregion
    }
}
