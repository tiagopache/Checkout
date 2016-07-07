using Checkout.Infrastructure.DependencyInjection;
using Checkout.Tests.Common;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Checkout.Application.Service.Tests
{
    [TestClass]
    public abstract class ServiceTestBase : TestBase
    {
        protected MockRepository mockRepository { get; set; }
        protected static IUnityContainer container { get; set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            container = UnityConfig.GetConfiguredContainer();

            InjectFactory.SetContainer(container);

            UnityHelper.GetRegistrations(container);
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }

        [TestInitialize]
        public virtual void Arrange()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
        }

        [TestCleanup]
        public virtual void CleanUp()
        {

        }
    }
}
