using ECA.Business.Service.Admin;
using ECA.Business.Service.Lookup;
using ECA.Business.Service.Persons;
using ECA.Business.Service.Programs;
using ECA.Business.Validation;
using ECA.Core.Logging;
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
            RegisterLogging(container);
            RegisterContexts(container);
            RegisterAdminDependencies(container);
            RegisterProgramDependencies(container);
            RegisterValidations(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        public static void RegisterContexts(IUnityContainer container)
        {
            //var localDbConnectionString = @"Data Source=(LocalDb)\v11.0;Initial Catalog=Test;Integrated Security=True";
            var connectionString = "EcaContext";
            container.RegisterType<EcaContext>(new HierarchicalLifetimeManager(), new InjectionConstructor(connectionString));
            //container.RegisterType<EcaContext>(new HierarchicalLifetimeManager(), new InjectionConstructor(localDbConnectionString));
            //container.RegisterType<EcaContext>(new InjectionFactory((c) =>
            //{
            //    return DbContextHelper.GetInMemoryContext();
            //}));
        }

        public static void RegisterLogging(IUnityContainer container)
        {
            container.RegisterType<ILogger, TraceLogger>(new HierarchicalLifetimeManager());
        }

        public static void RegisterAdminDependencies(IUnityContainer container)
        {
            Debug.Assert(container.IsRegistered<EcaContext>(), "The EcaContext is a dependency.  It should be registered.");
            container.RegisterType<IProjectService, ProjectService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILocationService, LocationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IGoalService, GoalService>(new HierarchicalLifetimeManager());
        }

        public static void RegisterProgramDependencies(IUnityContainer container)
        {
            Debug.Assert(container.IsRegistered<EcaContext>(), "The EcaContext is a dependency.  It should be registered.");
            container.RegisterType<IProgramService, ProgramService>(new HierarchicalLifetimeManager());
            container.RegisterType<IThemeService, ThemeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContactService, ContactService>(new HierarchicalLifetimeManager());
            container.RegisterType<IParticipantService, ParticipantService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFocusService, FocusService>(new HierarchicalLifetimeManager());
        }

        public static void RegisterValidations(IUnityContainer container)
        {
            container.RegisterType<IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity>, ProgramServiceValidator>();
        }
    }
}