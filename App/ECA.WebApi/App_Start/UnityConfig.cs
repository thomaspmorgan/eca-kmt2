using ECA.Business.Service.Admin;
using ECA.Business.Service.Programs;
using ECA.Data;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using System.Web.Http;
using Unity.WebApi;

namespace ECA.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            RegisterContexts(container);
            RegisterAdminDependencies(container);
            RegisterProgramDependencies(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        public static void RegisterContexts(IUnityContainer container)
        {
            //var localDbConnectionString = @"Data Source=(LocalDb)\v11.0;Initial Catalog=Test;Integrated Security=True";
            container.RegisterType<EcaContext>(new HierarchicalLifetimeManager());
            //container.RegisterType<EcaContext>(new HierarchicalLifetimeManager(), new InjectionConstructor(localDbConnectionString));
            //container.RegisterType<EcaContext>(new InjectionFactory((c) =>
            //{
            //    return DbContextHelper.GetInMemoryContext();
            //}));
        }


        public static void RegisterAdminDependencies(IUnityContainer container)
        {
            Debug.Assert(container.IsRegistered<EcaContext>(), "The EcaContext is a dependency.  It should be registered.");
            container.RegisterType<IProjectService, ProjectService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILocationService, LocationService>(new HierarchicalLifetimeManager());
        }

        public static void RegisterProgramDependencies(IUnityContainer container)
        {
            Debug.Assert(container.IsRegistered<EcaContext>(), "The EcaContext is a dependency.  It should be registered.");
            container.RegisterType<IProgramService, ProgramService>(new HierarchicalLifetimeManager());
            container.RegisterType<IThemeService, ThemeService>(new HierarchicalLifetimeManager());
        }
    }
}