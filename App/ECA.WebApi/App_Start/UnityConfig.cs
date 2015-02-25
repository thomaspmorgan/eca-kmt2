using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Test;
using ECA.Data;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace ECA.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            //var localDbConnectionString = @"Data Source=(LocalDb)\v11.0;Initial Catalog=Test;Integrated Security=True";

            container.RegisterType<EcaContext>(new HierarchicalLifetimeManager());
            //container.RegisterType<EcaContext>(new HierarchicalLifetimeManager(), new InjectionConstructor(localDbConnectionString));
            //container.RegisterType<EcaContext>(new InjectionFactory((c) =>
            //{
            //    return DbContextHelper.GetInMemoryContext();
            //}));

            container.RegisterType<IProjectService, ProjectService>(new HierarchicalLifetimeManager());
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}