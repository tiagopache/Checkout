using Checkout.Application.Contract.Contracts;
using Checkout.Application.Contract.ViewModels;
using Checkout.Business.Contract;
using Checkout.Model;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Application.Service
{
    public class ShoppingCartApplicationService : IShoppingCartApplicationService
    {
        [Dependency]
        protected IShoppingCartBusinessService ShoppingCartBusinessService { get; set; }

        [Dependency]
        protected IShoppingCartItemBusinessService ShoppingCartItemBusinessService { get; set; }

        [Dependency]
        protected IProductBusinessService ProductBusinessService { get; set; }

        [Dependency]
        protected ICampaignBusinessService CampaignBusinessService { get; set; }

        public ShoppingCartViewModel AddItem(string productSKU, int quantity, int shoppingCartId = -1)
        {
            var product = this.ProductBusinessService.GetBySKU(productSKU);

            return addItem(quantity, shoppingCartId, product);
        }

        public ShoppingCartViewModel AddItem(int productId, int quantity, int shoppingCartId = -1)
        {
            var product = this.ProductBusinessService.GetById(productId);

            return addItem(quantity, shoppingCartId, product);
        }

        public IList<ShoppingCartViewModel> Get(string includeProperties = null)
        {
            return ShoppingCartViewModel.Instance.ToContract(ShoppingCartBusinessService.Get(includeProperties: includeProperties)).ToList();
        }

        public ShoppingCartViewModel RemoveItem(int shoppingCartId, string productSKU)
        {
            throw new NotImplementedException();
        }

        public ShoppingCartViewModel RemoveItem(int shoppingCartId, int productId)
        {
            throw new NotImplementedException();
        }

        public ShoppingCartViewModel GetById(int id)
        {
            return ShoppingCartViewModel.Instance.ToContract(this.ShoppingCartBusinessService.GetById(id));
        }

        public void Remove(int id)
        {
            this.ShoppingCartBusinessService.Remove(id);
        }

        public CheckoutViewModel Checkout(int shoppingCartId)
        {
            var shoppingCart = this.ShoppingCartBusinessService.GetById(shoppingCartId);

            if (shoppingCart == null)
                throw new ArgumentException($"Shopping cart not found! Id: {shoppingCartId}");

            var total = 0.00m;
            for (int i = 0; i < shoppingCart.ShoppingCartItems.Count; i++)
            {
                var item = shoppingCart.ShoppingCartItems[i];

                var campaigns = this.CampaignBusinessService.Get(c => c.Active);

                if (campaigns != null)
                {
                    var _campaigns = campaigns.Where(c => c.ProductPurchasedSKU == item.ProductSKU);
                    //var productsFreeOfCharge = campaigns.OfType<CampaignBundle>().Where(c => c.Active).Select(c => c.ProductFreeOfCharge);

                    var campaign = _campaigns.FirstOrDefault(c => c.Active);

                    if (campaign != null)
                    {
                        if (campaign.ProductPurchasedSKU.Equals(item.ProductSKU))
                        {
                            if (campaign is CampaignBigDeal)
                            {
                                var bigDeal = campaign as CampaignBigDeal;

                                total = calculateBigDealWithStrategy(total, item, bigDeal);
                            }
                            else if (campaign is CampaignBundle)
                            {
                                var bundle = campaign as CampaignBundle;

                                total = calculateBundleStrategy(shoppingCartId, shoppingCart, total, item, bundle);
                            }
                            else if (campaign is CampaignPriceDrop)
                            {
                                var priceDrop = campaign as CampaignPriceDrop;

                                total = calculatePriceDropStrategy(total, item, priceDrop);
                            }
                        }
                        else
                            total = calculateValue(total, item);
                    }
                    else
                        total = calculateValue(total, item);
                }
                else
                    total = calculateValue(total, item);
            }

            var result = new CheckoutViewModel()
            {
                ShoppingCartId = shoppingCartId,
                Cart = ShoppingCartViewModel.Instance.ToContract(shoppingCart),
                Total = total
            };

            return result;
        }

        private decimal calculatePriceDropStrategy(decimal total, ShoppingCartItem item, CampaignPriceDrop priceDrop)
        {
            if (item.Quantity >= priceDrop.PurchaseQuantity)
                total += priceDrop.NewPrice * item.Quantity;
            else
                total = calculateValue(total, item);
            return total;
        }

        private decimal calculateBundleStrategy(int shoppingCartId, ShoppingCart shoppingCart, decimal total, ShoppingCartItem item, CampaignBundle bundle)
        {
            // Fixing the cart to reflect the campaign
            var itemFree = shoppingCart.ShoppingCartItems.FirstOrDefault(sci => sci.ProductSKU == bundle.ProductFreeOfChargeSKU);

            if (itemFree != null)
            {
                if (itemFree.Quantity < item.Quantity)
                {
                    shoppingCart = this.ShoppingCartBusinessService.GetById(shoppingCartId);

                    var _item = shoppingCart.ShoppingCartItems.FirstOrDefault(sci => sci.ProductSKU == bundle.ProductFreeOfCharge.SKU);
                    if (_item != null)
                        this.ShoppingCartItemBusinessService.Remove(_item.Id);

                    this.ShoppingCartBusinessService.AddProduct(shoppingCartId, bundle.ProductFreeOfCharge, item.Quantity);
                }
            }
            else
            {
                this.ShoppingCartBusinessService.AddProduct(shoppingCartId, bundle.ProductFreeOfCharge, item.Quantity);
            }

            total = calculateValue(total, item) - (bundle.ProductFreeOfCharge.Price * item.Quantity);

            return total;
        }

        private decimal calculateBigDealWithStrategy(decimal total, ShoppingCartItem item, CampaignBigDeal bigDeal)
        {
            if (item.Quantity >= bigDeal.PurchaseQuantity)
            {
                total += item.Product.Price * bigDeal.PayQuantity * (item.Quantity / bigDeal.PurchaseQuantity);

                var diff = item.Quantity % bigDeal.PurchaseQuantity;

                if (diff != 0)
                {
                    total += item.Product.Price * diff;
                }
            }
            else
            {
                total = calculateValue(total, item);
            }

            return total;
        }

        private decimal calculateValue(decimal total, ShoppingCartItem item)
        {
            total += item.Product.Price * item.Quantity;
            return total;
        }

        private ShoppingCartViewModel addItem(int quantity, int shoppingCartId, Product product)
        {
            ShoppingCart shoppingCart = null;

            if (shoppingCartId == -1)
            {
                shoppingCart = this.ShoppingCartBusinessService.Save(new ShoppingCart());

                this.ShoppingCartBusinessService.AddProduct(shoppingCart.Id, product, quantity);
            }
            else
            {
                shoppingCart = this.ShoppingCartBusinessService.GetById(shoppingCartId);

                var item = shoppingCart.ShoppingCartItems.FirstOrDefault(sci => sci.ProductSKU == product.SKU);
                if (item != null)
                    this.ShoppingCartItemBusinessService.Remove(item.Id);

                this.ShoppingCartBusinessService.AddProduct(shoppingCartId, product, quantity);
            }

            return ShoppingCartViewModel.Instance.ToContract(this.ShoppingCartBusinessService.GetById(shoppingCart != null ? shoppingCart.Id : shoppingCartId));
        }
    }
}
