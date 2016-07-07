using Checkout.Infrastructure.Data.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Checkout.Model
{
    public class Product : BaseIdEntity
    {
        [Key, Column(Order = 1)]
        public string SKU { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

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

        #region Singleton Pattern - Campaigns
        private IList<Campaign> _campaigns;
        public virtual IList<Campaign> Campaigns
        {
            get
            {
                if (_campaigns == null)
                    _campaigns = new List<Campaign>();

                return _campaigns;
            }
            set
            {
                _campaigns = value;
            }
        }
        #endregion
    }
}
