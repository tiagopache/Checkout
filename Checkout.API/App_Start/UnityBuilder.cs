using Checkout.Application.Contract.Contracts;
using Checkout.Application.Service;
using Checkout.Business.Contract;
using Checkout.Business.Service;
using Checkout.Infrastructure.Data;
using Checkout.Infrastructure.Data.Contexts;
using Checkout.Infrastructure.Data.Repositories;
using Checkout.Infrastructure.DependencyInjection;
using Checkout.Model.Contexts;
using Microsoft.Practices.Unity;

namespace Checkout.API.App_Start
{
    public static class UnityBuilder
    {
        public static void Build(IUnityContainer container)
        {
            InjectFactory.SetContainer(container);

            buildContext(container);
            buildInfrastructure(container);
            buildBusinessServices(container);
            buildApplicationServices(container);
        }

        private static void buildContext(IUnityContainer container)
        {
            container.RegisterType<IDbContext, CheckoutDbContext>(new HierarchicalLifetimeManager());
        }

        private static void buildApplicationServices(IUnityContainer container)
        {
            container.RegisterType<IProductApplicationService, ProductApplicationService>();
            container.RegisterType<IShoppingCartApplicationService, ShoppingCartApplicationService>();
        }

        private static void buildBusinessServices(IUnityContainer container)
        {
            container.RegisterType<IProductBusinessService, ProductBusinessService>();
            container.RegisterType<IShoppingCartBusinessService, ShoppingCartBusinessService>();
            container.RegisterType<IShoppingCartItemBusinessService, ShoppingCartItemBusinessService>();
            container.RegisterType<ICampaignBusinessService, CampaignBusinessService>();
        }

        private static void buildInfrastructure(IUnityContainer container)
        {
            container.RegisterType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
        }
    }
}