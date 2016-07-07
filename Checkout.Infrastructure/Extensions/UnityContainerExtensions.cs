using Microsoft.Practices.Unity;
using Moq;

namespace Checkout.Infrastructure.Extensions
{
    public static class UnityContainerExtensions
    {
        public static Mock<T> RegisterMock<T>(this IUnityContainer container, MockRepository mockRepo = null) where T : class
        {
            var mock = mockRepo == null ? new Mock<T>() : mockRepo.Create<T>();

            container.RegisterInstance<Mock<T>>(mock);
            container.RegisterInstance<T>(mock.Object);

            return mock;
        }
    }
}
