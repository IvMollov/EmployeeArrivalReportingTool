using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using ArrivalReporting.BAL.IServices;
using ArrivalReporting.BAL.Services;
using ArrivalReporting.DAL;
using Unity.Lifetime;

namespace ArrivalReporting.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IDALService, DALService>();
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<IArrivalService, ArrivalService>(new HierarchicalLifetimeManager());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}