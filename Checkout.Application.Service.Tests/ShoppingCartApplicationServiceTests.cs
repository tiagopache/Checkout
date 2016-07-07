using Checkout.Application.Contract.Contracts;
using Checkout.Application.Contract.ViewModels;
using Checkout.Business.Contract;
using Checkout.Infrastructure.DependencyInjection;
using Checkout.Infrastructure.Extensions;
using Checkout.Model;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Checkout.Application.Service.Tests
{
    [TestClass()]
    public class ShoppingCartApplicationServiceTests : ServiceTestBase
    {
        protected Mock<IShoppingCartBusinessService> mockShoppingCartBusiness { get; set; }

        protected Mock<IShoppingCartItemBusinessService> mockShoppingCartItemBusiness { get; set; }

        protected Mock<IProductBusinessService> mockProductBusiness { get; set; }

        protected Mock<ICampaignBusinessService> mockCampaignBusiness { get; set; }

        protected IShoppingCartApplicationService sut { get; set; }

        protected Product ipd { get; set; }

        protected Product mbp { get; set; }

        protected Product atv { get; set; }

        protected Product vga { get; set; }

        [TestInitialize]
        public override void Arrange()
        {
            base.Arrange();

            mockShoppingCartBusiness = container.RegisterMock<IShoppingCartBusinessService>(mockRepository);
            mockShoppingCartItemBusiness = container.RegisterMock<IShoppingCartItemBusinessService>(mockRepository);
            mockProductBusiness = container.RegisterMock<IProductBusinessService>(mockRepository);
            mockCampaignBusiness = container.RegisterMock<ICampaignBusinessService>(mockRepository);

            container.RegisterType<IShoppingCartApplicationService, ShoppingCartApplicationService>();

            sut = InjectFactory.Resolve<IShoppingCartApplicationService>();

            ipd = new Product()
            {
                SKU = "ipd",
                Name = "Super iPad",
                Price = 549.99m,
            };

            mbp = new Product()
            {
                SKU = "mbp",
                Name = "MacBook Pro",
                Price = 1399.99m,
            };

            atv = new Product()
            {
                SKU = "atv",
                Name = "Apple TV",
                Price = 109.50m,
            };

            vga = new Product()
            {
                SKU = "vga",
                Name = "VGA adapter",
                Price = 30.0m,
            };
        }

        [TestMethod()]
        public void ApplicationShouldAddItemToAnExistentShoppingCart()
        {
            // ARRANGE
            var product = new Product()
            {
                Id = 1,
                SKU = "ipd",
                Name = "Super iPad",
                Price = 549.99m,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var cart = new ShoppingCart()
            {
                Id = 0
            };

            var cartItem = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = product.Id,
                ProductSKU = product.SKU,
                Product = product,
                ShoppingCartId = cart.Id,
                Quantity = 3,
                ShoppingCart = cart
            };

            cart.ShoppingCartItems.Add(cartItem);

            var actualPrd = ProductViewModel.Instance.ToContract(product);
            var actualCart = ShoppingCartViewModel.Instance.ToContract(cart);
            var actualCartItem = ShoppingCartItemViewModel.Instance.ToContract(cartItem);

            mockShoppingCartBusiness.Setup(p => p.GetById(cart.Id)).Returns(cart);
            mockShoppingCartBusiness.Setup(p => p.AddProduct(cartItem.ShoppingCartId, cartItem.Product, cartItem.Quantity));
            mockProductBusiness.Setup(p => p.GetById(product.Id)).Returns(product);

            // ACT
            var expected = sut.AddItem(cartItem.ProductId, cartItem.Quantity, cartItem.ShoppingCartId);

            // ASSERT
            mockRepository.Verify();
            Assert.IsNotNull(expected);
            Assert.IsInstanceOfType(expected, typeof(ShoppingCartViewModel));
            Assert.AreEqual(expected.Id, actualCart.Id);
            Assert.AreEqual(expected.ShoppingCartItems.Count, actualCart.ShoppingCartItems.Count);
        }

        [TestMethod()]
        public void ApplicationShouldAddItemToNewShoppingCart()
        {
            // ARRANGE
            var product = new Product()
            {
                Id = 1,
                SKU = "ipd",
                Name = "Super iPad",
                Price = 549.99m,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var cart = new ShoppingCart()
            {
                Id = 0
            };

            var cartItem = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = product.Id,
                ProductSKU = product.SKU,
                Product = product,
                ShoppingCartId = cart.Id,
                Quantity = 3,
                ShoppingCart = cart
            };

            cart.ShoppingCartItems.Add(cartItem);

            var actualPrd = ProductViewModel.Instance.ToContract(product);
            var actualCart = ShoppingCartViewModel.Instance.ToContract(cart);
            var actualCartItem = ShoppingCartItemViewModel.Instance.ToContract(cartItem);

            mockShoppingCartBusiness.Setup(s => s.Save(It.IsAny<ShoppingCart>())).Returns(cart);
            mockShoppingCartBusiness.Setup(p => p.GetById(cart.Id)).Returns(cart);
            mockShoppingCartBusiness.Setup(s => s.AddProduct(cartItem.ShoppingCartId, cartItem.Product, cartItem.Quantity));
            mockProductBusiness.Setup(p => p.GetById(product.Id)).Returns(product);

            // ACT
            var expected = sut.AddItem(cartItem.ProductId, cartItem.Quantity);

            // ASSERT
            mockRepository.Verify();
            Assert.IsNotNull(expected);
            Assert.IsInstanceOfType(expected, typeof(ShoppingCartViewModel));
            Assert.AreEqual(expected.Id, actualCart.Id);
            Assert.AreEqual(expected.ShoppingCartItems.Count, actualCart.ShoppingCartItems.Count);
        }

        [TestMethod()]
        public void ApplicationShouldAddItemToAnExistentShoppingCartByProductSKU()
        {
            // ARRANGE
            var product = new Product()
            {
                Id = 1,
                SKU = "ipd",
                Name = "Super iPad",
                Price = 549.99m,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var cart = new ShoppingCart()
            {
                Id = 0
            };

            var cartItem = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = product.Id,
                ProductSKU = product.SKU,
                Product = product,
                ShoppingCartId = cart.Id,
                Quantity = 3,
                ShoppingCart = cart
            };

            cart.ShoppingCartItems.Add(cartItem);

            var actualPrd = ProductViewModel.Instance.ToContract(product);
            var actualCart = ShoppingCartViewModel.Instance.ToContract(cart);
            var actualCartItem = ShoppingCartItemViewModel.Instance.ToContract(cartItem);

            mockShoppingCartBusiness.Setup(p => p.GetById(cart.Id)).Returns(cart);
            mockShoppingCartBusiness.Setup(p => p.AddProduct(cartItem.ShoppingCartId, cartItem.Product, cartItem.Quantity));
            mockProductBusiness.Setup(p => p.GetBySKU(product.SKU)).Returns(product);

            // ACT
            var expected = sut.AddItem(cartItem.ProductSKU, cartItem.Quantity, cartItem.ShoppingCartId);

            // ASSERT
            mockRepository.Verify();
            Assert.IsNotNull(expected);
            Assert.IsInstanceOfType(expected, typeof(ShoppingCartViewModel));
            Assert.AreEqual(expected.Id, actualCart.Id);
            Assert.AreEqual(expected.ShoppingCartItems.Count, actualCart.ShoppingCartItems.Count);
        }

        [TestMethod()]
        public void ApplicationShouldAddItemToNewShoppingCartByProductSKU()
        {
            // ARRANGE
            var product = new Product()
            {
                Id = 1,
                SKU = "ipd",
                Name = "Super iPad",
                Price = 549.99m,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var cart = new ShoppingCart()
            {
                Id = 0
            };

            var cartItem = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = product.Id,
                ProductSKU = product.SKU,
                Product = product,
                ShoppingCartId = cart.Id,
                Quantity = 3,
                ShoppingCart = cart
            };

            cart.ShoppingCartItems.Add(cartItem);

            var actualPrd = ProductViewModel.Instance.ToContract(product);
            var actualCart = ShoppingCartViewModel.Instance.ToContract(cart);
            var actualCartItem = ShoppingCartItemViewModel.Instance.ToContract(cartItem);

            mockShoppingCartBusiness.Setup(s => s.Save(It.IsAny<ShoppingCart>())).Returns(cart);
            mockShoppingCartBusiness.Setup(p => p.GetById(cart.Id)).Returns(cart);
            mockShoppingCartBusiness.Setup(s => s.AddProduct(cartItem.ShoppingCartId, cartItem.Product, cartItem.Quantity));
            mockProductBusiness.Setup(p => p.GetBySKU(product.SKU)).Returns(product);

            // ACT
            var expected = sut.AddItem(cartItem.ProductSKU, cartItem.Quantity);

            // ASSERT
            mockRepository.Verify();
            Assert.IsNotNull(expected);
            Assert.IsInstanceOfType(expected, typeof(ShoppingCartViewModel));
            Assert.AreEqual(expected.Id, actualCart.Id);
            Assert.AreEqual(expected.ShoppingCartItems.Count, actualCart.ShoppingCartItems.Count);
        }

        [TestMethod()]
        public void ApplicationShouldGetAListOfShoppingCarts()
        {
            // ARRANGE
            var product1 = new Product()
            {
                Id = 1,
                SKU = "ipd",
                Name = "Super iPad",
                Price = 549.99m,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var product2 = new Product()
            {
                Id = 2,
                SKU = "mbp",
                Name = "MacBook Pro",
                Price = 1399.99m,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var prods = new List<Product>();
            prods.Add(product1);
            prods.Add(product2);

            var cart = new ShoppingCart()
            {
                Id = 0
            };

            var cartItem1 = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = product1.Id,
                ProductSKU = product1.SKU,
                Product = product1,
                ShoppingCartId = cart.Id,
                Quantity = 3,
                ShoppingCart = cart
            };

            var cartItem2 = new ShoppingCartItem()
            {
                Id = 2,
                ProductId = product2.Id,
                ProductSKU = product2.SKU,
                Product = product2,
                ShoppingCartId = cart.Id,
                Quantity = 7,
                ShoppingCart = cart
            };

            cart.ShoppingCartItems.Add(cartItem1);
            cart.ShoppingCartItems.Add(cartItem2);

            var actual = new List<ShoppingCart>();
            actual.Add(cart);

            var actualViewModel = ShoppingCartViewModel.Instance.ToContract(actual);

            mockShoppingCartBusiness.Setup((r) => r.Get(null, null, null)).Returns(actual);

            // ACT
            var expected = sut.Get();

            // ASSERT
            mockRepository.Verify();
            CollectionAssert.AllItemsAreInstancesOfType(expected.ToArray(), typeof(ShoppingCartViewModel));
            CollectionAssert.AllItemsAreNotNull(expected.ToArray());
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].Id, actualViewModel.ToArray()[i].Id);

                for (int j = 0; j < expected[i].ShoppingCartItems.Count; j++)
                {
                    Assert.AreEqual(expected[i].ShoppingCartItems[j].Id, actualViewModel.ToArray()[i].ShoppingCartItems[j].Id);
                    Assert.AreEqual(expected[i].ShoppingCartItems[j].Quantity, actualViewModel.ToArray()[i].ShoppingCartItems[j].Quantity);
                    CompareProperties(actualViewModel.ToArray()[i].ShoppingCartItems[j].Product, expected[i].ShoppingCartItems[j].Product);
                }

            }
        }

        [TestMethod()]
        public void ApplicationShouldReturnConsolidatedCheckoutOfAShoppingCartWithoutCampaign()
        {
            // ARRANGE
            var cart = new ShoppingCart()
            {
                Id = 0
            };

            var cartItem1 = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = atv.Id,
                ProductSKU = atv.SKU,
                Product = atv,
                Quantity = 3
            };

            var cartItem2 = new ShoppingCartItem()
            {
                Id = 2,
                ProductId = vga.Id,
                ProductSKU = vga.SKU,
                Product = vga,
                Quantity = 1
            };

            cart.ShoppingCartItems.Add(cartItem1);
            cart.ShoppingCartItems.Add(cartItem2);

            var actual = new CheckoutViewModel()
            {
                ShoppingCartId = cart.Id,
                Cart = ShoppingCartViewModel.Instance.ToContract(cart),
                Total = 358.50m
            };

            mockShoppingCartBusiness
                .Setup(s => s.GetById(cart.Id))
                .Returns(cart);
            mockCampaignBusiness
                .SetupSequence(c => c.Get(It.IsNotNull<Expression<Func<Campaign, bool>>>(), null, null))
                .Returns(new List<Campaign>());

            // ACT
            var expected = sut.Checkout(cart.Id);

            // ASSERT
            mockRepository.Verify();
            Assert.IsInstanceOfType(expected, typeof(CheckoutViewModel));
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.ShoppingCartId, actual.ShoppingCartId);
            Assert.AreEqual(expected.Total, actual.Total);

            var expCartItems = expected.Cart.ShoppingCartItems;
            var actCartItems = actual.Cart.ShoppingCartItems;
            for (int i = 0; i < expCartItems.Count; i++)
            {
                CompareProperties(actCartItems[i], expCartItems[i]);
            }
        }

        [TestMethod()]
        public void ApplicationShouldReturnConsolidatedCheckoutOfAShoppingCartWithBigDealCampaign()
        {
            // ARRANGE
            var cart = new ShoppingCart()
            {
                Id = 0
            };

            var cartItem1 = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = atv.Id,
                ProductSKU = atv.SKU,
                Product = atv,
                Quantity = 3
            };

            var cartItem2 = new ShoppingCartItem()
            {
                Id = 2,
                ProductId = vga.Id,
                ProductSKU = vga.SKU,
                Product = vga,
                Quantity = 1
            };

            cart.ShoppingCartItems.Add(cartItem1);
            cart.ShoppingCartItems.Add(cartItem2);

            var actual = new CheckoutViewModel()
            {
                ShoppingCartId = cart.Id,
                Cart = ShoppingCartViewModel.Instance.ToContract(cart),
                Total = 249.00m
            };

            var campaignBigDeal = new CampaignBigDeal()
            {
                ProductPurchasedSKU = "atv",
                ProductPurchasedId = atv.Id,
                PurchaseQuantity = 3,
                PayQuantity = 2,
                Active = true
            };

            var lstCampaign = new List<Campaign>();
            lstCampaign.Add(campaignBigDeal);

            mockShoppingCartBusiness
                .Setup(s => s.GetById(cart.Id))
                .Returns(cart);
            mockCampaignBusiness
                .SetupSequence(c => c.Get(It.IsNotNull<Expression<Func<Campaign, bool>>>(), null, null))
                .Returns(lstCampaign)
                .Returns(new List<Campaign>())
                .Returns(new List<Campaign>());

            // ACT
            var expected = sut.Checkout(cart.Id);

            // ASSERT
            mockRepository.Verify();
            Assert.IsInstanceOfType(expected, typeof(CheckoutViewModel));
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.ShoppingCartId, actual.ShoppingCartId);
            Assert.AreEqual(expected.Total, actual.Total);

            var expCartItems = expected.Cart.ShoppingCartItems;
            var actCartItems = actual.Cart.ShoppingCartItems;
            for (int i = 0; i < expCartItems.Count; i++)
            {
                CompareProperties(actCartItems[i], expCartItems[i]);
            }
        }

        [TestMethod()]
        public void ApplicationShouldReturnConsolidatedCheckoutOfAShoppingCartWithBigDealCampaignAndBrokenMultiplier()
        {
            // ARRANGE
            var cart = new ShoppingCart()
            {
                Id = 0
            };

            var cartItem1 = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = atv.Id,
                ProductSKU = atv.SKU,
                Product = atv,
                Quantity = 8
            };

            var cartItem2 = new ShoppingCartItem()
            {
                Id = 2,
                ProductId = vga.Id,
                ProductSKU = vga.SKU,
                Product = vga,
                Quantity = 1
            };

            cart.ShoppingCartItems.Add(cartItem1);
            cart.ShoppingCartItems.Add(cartItem2);

            var actual = new CheckoutViewModel()
            {
                ShoppingCartId = cart.Id,
                Cart = ShoppingCartViewModel.Instance.ToContract(cart),
                Total = 687.0m
            };

            var campaignBigDeal = new CampaignBigDeal()
            {
                ProductPurchasedSKU = "atv",
                ProductPurchasedId = atv.Id,
                PurchaseQuantity = 3,
                PayQuantity = 2,
                Active = true
            };

            var lstCampaign = new List<Campaign>();
            lstCampaign.Add(campaignBigDeal);

            mockShoppingCartBusiness
                .Setup(s => s.GetById(cart.Id))
                .Returns(cart);
            mockCampaignBusiness
                .SetupSequence(c => c.Get(It.IsNotNull<Expression<Func<Campaign, bool>>>(), null, null))
                .Returns(lstCampaign)
                .Returns(new List<Campaign>())
                .Returns(new List<Campaign>());

            // ACT
            var expected = sut.Checkout(cart.Id);

            // ASSERT
            mockRepository.Verify();
            Assert.IsInstanceOfType(expected, typeof(CheckoutViewModel));
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.ShoppingCartId, actual.ShoppingCartId);
            Assert.AreEqual(expected.Total, actual.Total);

            var expCartItems = expected.Cart.ShoppingCartItems;
            var actCartItems = actual.Cart.ShoppingCartItems;
            for (int i = 0; i < expCartItems.Count; i++)
            {
                CompareProperties(actCartItems[i], expCartItems[i]);
            }
        }

        [TestMethod()]
        public void ApplicationShouldReturnConsolidatedCheckoutOfAShoppingCartWithBundleCampaign()
        {
            // ARRANGE
            var cart = new ShoppingCart()
            {
                Id = 0
            };

            var cartItem1 = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = mbp.Id,
                ProductSKU = mbp.SKU,
                Product = mbp,
                Quantity = 4
            };

            var cartItem2 = new ShoppingCartItem()
            {
                Id = 2,
                ProductId = ipd.Id,
                ProductSKU = ipd.SKU,
                Product = ipd,
                Quantity = 1
            };

            var vgaItem = new ShoppingCartItem()
            {
                Id = 3,
                ProductId = vga.Id,
                ProductSKU = vga.SKU,
                Product = vga,
                Quantity = 4
            };

            cart.ShoppingCartItems.Add(cartItem1);
            cart.ShoppingCartItems.Add(cartItem2);
            cart.ShoppingCartItems.Add(vgaItem);

            var actual = new CheckoutViewModel()
            {
                ShoppingCartId = cart.Id,
                Cart = ShoppingCartViewModel.Instance.ToContract(cart),
                Total = 6149.95m
            };

            var campaignBundle = new CampaignBundle()
            {
                ProductPurchasedSKU = "mbp",
                ProductPurchasedId = mbp.Id,
                ProductPurchased = mbp,
                ProductFreeOfChargeSKU = vga.SKU,
                ProductFreeOfChargeId = vga.Id,
                ProductFreeOfCharge = vga,
                Active = true
            };

            var lstCampaign = new List<Campaign>();
            lstCampaign.Add(campaignBundle);

            mockShoppingCartBusiness
                .Setup(s => s.GetById(cart.Id))
                .Returns(cart);
            mockCampaignBusiness
                .SetupSequence(c => c.Get(It.IsNotNull<Expression<Func<Campaign, bool>>>(), null, null))
                .Returns(lstCampaign)
                .Returns(new List<Campaign>())
                .Returns(new List<Campaign>());

            // ACT
            var expected = sut.Checkout(cart.Id);
            actual.Cart.ShoppingCartItems.Add(ShoppingCartItemViewModel.Instance.ToContract(vgaItem));

            // ASSERT
            mockRepository.Verify();
            Assert.IsInstanceOfType(expected, typeof(CheckoutViewModel));
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.ShoppingCartId, actual.ShoppingCartId);
            Assert.AreEqual(expected.Total, actual.Total);

            var expCartItems = expected.Cart.ShoppingCartItems;
            var actCartItems = actual.Cart.ShoppingCartItems;
            for (int i = 0; i < expCartItems.Count; i++)
            {
                CompareProperties(actCartItems[i], expCartItems[i]);
            }
        }

        [TestMethod()]
        public void ApplicationShouldReturnConsolidatedCheckoutOfAShoppingCartWithPriceDropCampaign()
        {
            // ARRANGE
            var cart = new ShoppingCart()
            {
                Id = 0
            };

            var cartItem1 = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = ipd.Id,
                ProductSKU = ipd.SKU,
                Product = ipd,
                Quantity = 5
            };

            var cartItem2 = new ShoppingCartItem()
            {
                Id = 2,
                ProductId = atv.Id,
                ProductSKU = atv.SKU,
                Product = atv,
                Quantity = 2
            };

            cart.ShoppingCartItems.Add(cartItem1);
            cart.ShoppingCartItems.Add(cartItem2);

            var actual = new CheckoutViewModel()
            {
                ShoppingCartId = cart.Id,
                Cart = ShoppingCartViewModel.Instance.ToContract(cart),
                Total = 2718.95m
            };

            var campaignPriceDrop = new CampaignPriceDrop()
            {
                ProductPurchasedSKU = "ipd",
                ProductPurchasedId = ipd.Id,
                PurchaseQuantity = 4,
                NewPrice = 499.99m,
                Active = true
            };

            var lstCampaign = new List<Campaign>();
            lstCampaign.Add(campaignPriceDrop);

            mockShoppingCartBusiness
                .Setup(s => s.GetById(cart.Id))
                .Returns(cart);
            mockCampaignBusiness
                .SetupSequence(c => c.Get(It.IsNotNull<Expression<Func<Campaign, bool>>>(), null, null))
                .Returns(lstCampaign)
                .Returns(new List<Campaign>())
                .Returns(new List<Campaign>());

            // ACT
            var expected = sut.Checkout(cart.Id);

            // ASSERT
            mockRepository.Verify();
            Assert.IsInstanceOfType(expected, typeof(CheckoutViewModel));
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.ShoppingCartId, actual.ShoppingCartId);
            Assert.AreEqual(expected.Total, actual.Total);

            var expCartItems = expected.Cart.ShoppingCartItems;
            var actCartItems = actual.Cart.ShoppingCartItems;
            for (int i = 0; i < expCartItems.Count; i++)
            {
                CompareProperties(actCartItems[i], expCartItems[i]);
            }
        }

        //[TestMethod(), Ignore]
        //public void RemoveItemTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod(), Ignore]
        //public void RemoveItemTest1()
        //{
        //    Assert.Fail();
        //}
    }
}