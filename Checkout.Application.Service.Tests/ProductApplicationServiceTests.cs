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

namespace Checkout.Application.Service.Tests
{
    [TestClass()]
    public class ProductApplicationServiceTests : ServiceTestBase
    {
        protected Mock<IProductBusinessService> mockProductBusiness { get; set; }

        protected IProductApplicationService sut { get; set; }

        [TestInitialize]
        public override void Arrange()
        {
            base.Arrange();

            mockProductBusiness = container.RegisterMock<IProductBusinessService>(mockRepository);

            container.RegisterType<IProductApplicationService, ProductApplicationService>();

            sut = InjectFactory.Resolve<IProductApplicationService>();
        }

        [TestMethod()]
        public void ApplicationShouldGetAListOfProducts()
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

            var actual = ProductViewModel.Instance.ToContract(prods);

            mockProductBusiness.Setup((r) => r.Get(null, null, null)).Returns(prods);

            // ACT
            var expected = sut.Get();

            // ASSERT
            mockRepository.Verify();
            CollectionAssert.AllItemsAreInstancesOfType(expected.ToArray(), typeof(ProductViewModel));
            CollectionAssert.AllItemsAreNotNull(expected.ToArray());
            for (int i = 0; i < expected.Count; i++)
            {
                this.CompareProperties(actual.ToArray()[i], expected[i]);
            }
        }

        [TestMethod()]
        public void ApplicationShouldGetProductById()
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

            var actual = ProductViewModel.Instance.ToContract(product);

            mockProductBusiness.Setup(p => p.GetById(product.Id)).Returns(product);

            // ACT
            var expected = sut.GetById(product.Id);

            // ASSERT
            mockRepository.Verify();
            Assert.IsNotNull(expected);
            Assert.IsInstanceOfType(expected, typeof(ProductViewModel));
            this.CompareProperties(actual, expected);
        }

        [TestMethod()]
        public void ApplicationShouldGetProductBySKU()
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

            var actual = ProductViewModel.Instance.ToContract(product);

            mockProductBusiness.Setup(p => p.GetBySKU(product.SKU)).Returns(product);

            // ACT
            var expected = sut.GetBySKU(product.SKU);

            // ASSERT
            mockRepository.Verify();
            Assert.IsNotNull(expected);
            Assert.IsInstanceOfType(expected, typeof(ProductViewModel));
            this.CompareProperties(actual, expected);
        }
    }
}