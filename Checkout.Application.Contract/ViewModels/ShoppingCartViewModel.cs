using Checkout.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Checkout.Application.Contract.ViewModels
{
    public class ShoppingCartViewModel : BaseViewModel<ShoppingCartViewModel, ShoppingCart>
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        #region Singleton Pattern - ShoppingCartItems
        private IList<ShoppingCartItemViewModel> _shoppingCartItems;
        [JsonProperty("items")]
        public IList<ShoppingCartItemViewModel> ShoppingCartItems
        {
            get
            {
                if (_shoppingCartItems == null)
                    _shoppingCartItems = new List<ShoppingCartItemViewModel>();

                return _shoppingCartItems;
            }
            set
            {
                _shoppingCartItems = value;
            }
        }
        #endregion

        public override IEnumerable<ShoppingCartViewModel> ToContract(IEnumerable<ShoppingCart> entity)
        {
            var result = new List<ShoppingCartViewModel>();

            foreach (var ent in entity)
            {
                result.Add(this.ToContract(ent));
            }

            return result;
        }

        public override ShoppingCartViewModel ToContract(ShoppingCart entity)
        {
            var result = base.ToContract(entity);

            if (entity != null)
            {
                foreach (var item in entity.ShoppingCartItems)
                {
                    result.ShoppingCartItems.Add(ShoppingCartItemViewModel.Instance.ToContract(item));
                }
            }

            return result;
        }
    }
}
