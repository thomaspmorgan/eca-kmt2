using ECA.Business.Service.Admin;
using ECA.Business.Service.Lookup;
using ECA.Business.Service.Persons;
using ECA.Business.Service.Programs;
using ECA.Business.Validation;
using ECA.Core.Generation;
using ECA.Core.Logging;
using ECA.Data;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Diagnostics;
using System.Web.Http;
using Unity.WebApi;

namespace ECA.WebApi
{
    /// <summary>
    /// The UnityConfig is used to register components through the Unity IoC container.
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Registers the components to a UnityContainer and sets the web api dependency resolver to a UnityDependencyResolver.
        /// </summary>
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            RegisterLogging(container);
            RegisterContexts(container);
            RegisterServices(container);
            RegisterValidations(container);
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        /// <summary>
        /// Registers the EcaContext.
        /// </summary>
        /// <param name="container">The unity container.</param>
        public static void RegisterContexts(IUnityContainer container)
        {
            var connectionString = "EcaContext";
            container.RegisterType<EcaContext>(new HierarchicalLifetimeManager(), new InjectionConstructor(connectionString));
            container.RegisterType<DbContext, EcaContext>(new HierarchicalLifetimeManager(), new InjectionConstructor(connectionString));
        }

        /// <summary>
        /// Registers the logger.
        /// </summary>
        /// <param name="container">The unity container.</param>
        public static void RegisterLogging(IUnityContainer container)
        {
            container.RegisterType<ILogger, TraceLogger>(new HierarchicalLifetimeManager());
        }

        /// <summary>
        /// Registers Admin services.
        /// </summary>
        /// <param name="container">The unity container.</param>
        public static void RegisterServices(IUnityContainer container)
        {
            Debug.Assert(container.IsRegistered<EcaContext>(), "The EcaContext is a dependency.  It should be registered.");
            
            container.RegisterType<IContactService, ContactService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFocusService, FocusService>(new HierarchicalLifetimeManager());
            container.RegisterType<IGoalService, GoalService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILocationService, LocationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMoneyFlowService, MoneyFlowService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOfficeService, OfficeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IParticipantService, ParticipantService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPersonService, PersonService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProgramService, ProgramService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProjectService, ProjectService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProjectStatusService, ProjectStatusService>(new HierarchicalLifetimeManager());
            container.RegisterType<IStaticGeneratorValidator, DbContextStaticLookupValidator>(new HierarchicalLifetimeManager());
            container.RegisterType<IThemeService, ThemeService>(new HierarchicalLifetimeManager());
                        
        }

        /// <summary>
        /// Registers business validation.
        /// </summary>
        /// <param name="container">Registers business validations.</param>
        public static void RegisterValidations(IUnityContainer container)
        {
            container.RegisterType<
                IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity>, 
                ProgramServiceValidator>();
            container.RegisterType<
                IBusinessValidator<ProjectServiceCreateValidationEntity, ProjectServiceUpdateValidationEntity>, 
                ProjectServiceValidator>();
        }
    }
}