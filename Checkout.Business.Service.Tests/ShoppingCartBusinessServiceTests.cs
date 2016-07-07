using Checkout.Business.Contract;
using Checkout.Infrastructure.Data;
using Checkout.Infrastructure.Data.Repositories;
using Checkout.Infrastructure.DependencyInjection;
using Checkout.Infrastructure.Extensions;
using Checkout.Model;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Business.Service.Tests
{
    [TestClass()]
    public class ShoppingCartBusinessServiceTests : ServiceTestBase
    {
        protected Mock<IRepository<ShoppingCart>> mockShoppingCartRepository { get; set; }
        protected Mock<IUnitOfWork<ShoppingCart>> mockUnitOfWork { get; set; }

        protected IShoppingCartBusinessService sut { get; set; }

        [TestInitialize]
        public override void Arrange()
        {
            base.Arrange();

            container.RegisterType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

            mockUnitOfWork = container.RegisterMock<IUnitOfWork<ShoppingCart>>(mockRepository);
            mockShoppingCartRepository = container.RegisterMock<IRepository<ShoppingCart>>(mockRepository);
            mockUnitOfWork.SetupProperty(u => u.Repository, mockShoppingCartRepository.Object);

            container.RegisterType<IShoppingCartBusinessService, ShoppingCartBusinessService>();

            sut = InjectFactory.Resolve<IShoppingCartBusinessService>();
        }

        [TestMethod()]
        public void ShouldGetAListOfShoppingCart()
        {
            // ARRANGE
            var cart1 = new ShoppingCart()
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var cart2 = new ShoppingCart()
            {
                Id = 2,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var actual = new List<ShoppingCart>();
            actual.Add(cart1);
            actual.Add(cart2);

            mockShoppingCartRepository.Setup((r) => r.Get(null, null, null)).Returns(actual);

            // ACT
            var expected = sut.Get();

            // ASSERT
            mockRepository.Verify();
            CollectionAssert.AllItemsAreInstancesOfType(expected.ToArray(), typeof(ShoppingCart));
            CollectionAssert.AllItemsAreNotNull(expected.ToArray());
            CollectionAssert.AreEqual(expected.ToArray(), actual);
        }

        [TestMethod()]
        public void ShouldGetShoppingCartById()
        {
            // ARRANGE
            var actual = new ShoppingCart()
            {
                Id = 1,
            };

            mockShoppingCartRepository.Setup((r) => r.GetById(It.IsAny<int>())).Returns(actual);

            // ACT
            var expected = sut.GetById(actual.Id);

            // ASSERT
            mockRepository.Verify();
            Assert.IsNotNull(expected);
            Assert.IsInstanceOfType(expected, typeof(ShoppingCart));
            Assert.AreEqual<ShoppingCart>(expected, actual);
        }

        [TestMethod()]
        public void ShouldRemoveAShoppingCart()
        {
            mockShoppingCartRepository.Setup(r => r.Delete(It.IsAny<int>()));

            mockUnitOfWork.Setup(u => u.Save());

            // ACT
            sut.Remove(1);

            // ASSERT
            mockRepository.Verify();
        }

        [TestMethod()]
        public void ShouldSaveANewShoppingCart()
        {
            // ARRANGE
            var actual = new ShoppingCart()
            {
                Id = 1
            };

            mockShoppingCartRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(default(ShoppingCart));
            mockShoppingCartRepository.Setup(r => r.Insert(It.IsAny<ShoppingCart>()));
            mockUnitOfWork.Setup(u => u.Save());

            // ACT
            var expected = sut.Save(actual);

            // ASSERT
            mockRepository.Verify();
            Assert.IsNotNull(expected);
            Assert.IsInstanceOfType(expected, typeof(ShoppingCart));
            Assert.AreEqual<ShoppingCart>(expected, actual);
        }

        [TestMethod()]
        public void ShouldSaveAnExistingShoppingCart()
        {
            // ARRANGE
            var old = new ShoppingCart()
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            mockShoppingCartRepository.Setup(r => r.GetById(old.Id)).Returns(old);

            var actual = new ShoppingCart()
            {
                Id = 1,
            };

            mockShoppingCartRepository.Setup(r => r.Update(It.IsAny<ShoppingCart>()));

            mockUnitOfWork.Setup(u => u.Save());

            // ACT
            var expected = sut.Save(actual);

            // ASSERT
            mockRepository.Verify();
            Assert.IsNotNull(expected);
            Assert.IsInstanceOfType(expected, typeof(ShoppingCart));
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.CreatedOn, actual.CreatedOn);
            Assert.AreEqual(expected.EntityKey, actual.EntityKey);
        }

        [TestMethod()]
        public void ShouldAddProductToShoppingCart()
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

            var shoppingCart = new ShoppingCart()
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var actual = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = product.Id,
                ProductSKU = product.SKU,
                ShoppingCartId = 1,
                Quantity = 3
            };

            mockShoppingCartRepository.Setup((s) => s.GetById(shoppingCart.Id)).Returns(shoppingCart);
            mockUnitOfWork.Setup((u) => u.Save());

            // ACT
            sut.AddProduct(shoppingCart.Id, product, 3);

            // ASSERT
            mockRepository.Verify();
            CollectionAssert.AllItemsAreInstancesOfType(shoppingCart.ShoppingCartItems.ToArray(), typeof(ShoppingCartItem));
            CollectionAssert.AllItemsAreNotNull(shoppingCart.ShoppingCartItems.ToArray());
            Assert.AreEqual(shoppingCart.ShoppingCartItems[0].ProductId, actual.ProductId);
            Assert.AreEqual(shoppingCart.ShoppingCartItems[0].ProductSKU, actual.ProductSKU);
            Assert.AreEqual(shoppingCart.ShoppingCartItems[0].Quantity, actual.Quantity);
            Assert.AreEqual(shoppingCart.ShoppingCartItems[0].ShoppingCartId, actual.ShoppingCartId);
        }

        [TestMethod()]
        public void ShouldRemoveAProductByProductIdFromShoppingCart()
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

            var shoppingCart = new ShoppingCart()
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var item = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = product.Id,
                ProductSKU = product.SKU,
                ShoppingCartId = 1,
                Quantity = 3
            };

            shoppingCart.ShoppingCartItems.Add(item);

            mockShoppingCartRepository.Setup((s) => s.GetById(shoppingCart.Id)).Returns(shoppingCart);
            mockUnitOfWork.Setup((u) => u.Save());

            // ACT
            sut.RemoveProduct(shoppingCart.Id, product.Id);

            // ASSERT
            mockRepository.Verify();
            Assert.AreEqual(shoppingCart.ShoppingCartItems.Count, 0);
        }

        [TestMethod()]
        public void ShouldRemoveAProductByProductSKUFromShoppingCart()
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

            var shoppingCart = new ShoppingCart()
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var item = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = product.Id,
                ProductSKU = product.SKU,
                ShoppingCartId = 1,
                Quantity = 3
            };

            shoppingCart.ShoppingCartItems.Add(item);

            mockShoppingCartRepository.Setup((s) => s.GetById(shoppingCart.Id)).Returns(shoppingCart);
            mockUnitOfWork.Setup((u) => u.Save());

            // ACT
            sut.RemoveProduct(shoppingCart.Id, product.SKU);

            // ASSERT
            mockRepository.Verify();
            Assert.AreEqual(shoppingCart.ShoppingCartItems.Count, 0);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenShoppingCartDoesNotExist()
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

            var shoppingCart = new ShoppingCart()
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var actual = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = product.Id,
                ProductSKU = product.SKU,
                ShoppingCartId = 1,
                Quantity = 3
            };

            mockShoppingCartRepository.Setup((s) => s.GetById(shoppingCart.Id)).Returns(shoppingCart);
            mockUnitOfWork.Setup((u) => u.Save());

            // ACT
            sut.AddProduct(5, product, 3);

            // ASSERT
            mockRepository.Verify();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenRemovingShoppingCartItemByProductIdAndItemWasNotFound()
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

            var unexistentProductId = 5;

            var shoppingCart = new ShoppingCart()
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var item = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = product.Id,
                ProductSKU = product.SKU,
                ShoppingCartId = 1,
                Quantity = 3
            };

            shoppingCart.ShoppingCartItems.Add(item);

            mockShoppingCartRepository.Setup((s) => s.GetById(shoppingCart.Id)).Returns(shoppingCart);
            mockUnitOfWork.Setup((u) => u.Save());

            // ACT
            sut.RemoveProduct(shoppingCart.Id, unexistentProductId);

            // ASSERT
            mockRepository.Verify();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenRemovingShoppingCartItemByProductSKUAndItemWasNotFound()
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

            var unexistentProductSKU = "aaa";

            var shoppingCart = new ShoppingCart()
            {
                Id = 1,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            };

            var item = new ShoppingCartItem()
            {
                Id = 1,
                ProductId = product.Id,
                ProductSKU = product.SKU,
                ShoppingCartId = 1,
                Quantity = 3
            };

            shoppingCart.ShoppingCartItems.Add(item);

            mockShoppingCartRepository.Setup((s) => s.GetById(shoppingCart.Id)).Returns(shoppingCart);
            mockUnitOfWork.Setup((u) => u.Save());

            // ACT
            sut.RemoveProduct(shoppingCart.Id, unexistentProductSKU);

            // ASSERT
            mockRepository.Verify();
        }
    }
}