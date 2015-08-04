using CAM.Business.Service;
using CAM.Data;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Lookup;
using ECA.Business.Service.Persons;
using ECA.Business.Service.Programs;
using ECA.Business.Service.Projects;
using ECA.Business.Service.Reports;
using ECA.Business.Validation;
using ECA.Core.Generation;
using ECA.Core.Service;
using ECA.Data;
using ECA.WebApi.Custom;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Security;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.Caching;
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
        public static void RegisterComponents(IUnityContainer container)
        {
            Contract.Requires(container != null, "The container must not be null.");
            RegisterContexts(container);
            RegisterServices(container);
            RegisterSecurityConcerns(container);
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
            container.RegisterType<List<ISaveAction>>(new InjectionFactory((c) =>
            {
                var list = new List<ISaveAction>();
                list.Add(new PermissableSaveAction(c.Resolve<IPermissableService>()));
                return list;
            }));
        }

        /// <summary>
        /// Registers Admin services.
        /// </summary>
        /// <param name="container">The unity container.</param>
        public static void RegisterServices(IUnityContainer container)
        {
            Debug.Assert(container.IsRegistered<EcaContext>(), "The EcaContext is a dependency.  It should be registered.");

            container.RegisterType<IAddressModelHandler, AddressModelHandler>(new HierarchicalLifetimeManager());
            container.RegisterType<IAddressTypeService, AddressTypeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContactService, ContactService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFocusService, FocusService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFocusCategoryService, FocusCategoryService>(new HierarchicalLifetimeManager());
            container.RegisterType<IGenderService, GenderService>(new HierarchicalLifetimeManager());
            container.RegisterType<IGoalService, GoalService>(new HierarchicalLifetimeManager());
            container.RegisterType<IJustificationObjectiveService, JustificationObjectiveService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILocationService, LocationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMaritalStatusService, MaritalStatusService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMoneyFlowService, MoneyFlowService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMoneyFlowStatusService, MoneyFlowStatusService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMoneyFlowTypeService, MoneyFlowTypeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMoneyFlowSourceRecipientTypeService, MoneyFlowSourceRecipientTypeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOfficeService, OfficeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOrganizationService, OrganizationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOrganizationTypeService, OrganizationTypeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IParticipantService, ParticipantService>(new HierarchicalLifetimeManager());
            container.RegisterType<IParticipantTypeService, ParticipantTypeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPersonService, PersonService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProgramService, ProgramService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProjectService, ProjectService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProjectStatusService, ProjectStatusService>(new HierarchicalLifetimeManager());
            container.RegisterType<IReportService, ReportService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISocialMediaService, SocialMediaService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISocialMediaTypeService, SocialMediaTypeService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISocialMediaPresenceModelHandler, SocialMediaPresenceModelHandler>(new HierarchicalLifetimeManager());
            container.RegisterType<IStaticGeneratorValidator, DbContextStaticLookupValidator>(new HierarchicalLifetimeManager());
            container.RegisterType<IThemeService, ThemeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IParticipantPersonService, ParticipantPersonService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBookmarkService, BookmarkService>(new HierarchicalLifetimeManager());
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
            container.RegisterType<
                IBusinessValidator<PersonServiceValidationEntity, PersonServiceValidationEntity>,
                PersonServiceValidator>();
            container.RegisterType<
                IBusinessValidator<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity>,
                MoneyFlowServiceValidator>();
            container.RegisterType<
                IBusinessValidator<Object, UpdateOrganizationValidationEntity>,
                OrganizationServiceValidator>();
            container.RegisterType<
                IBusinessValidator<EcaAddressValidationEntity, EcaAddressValidationEntity>,
                LocationServiceAddressValidator>();
        }

        public static void RegisterSecurityConcerns(IUnityContainer container)
        {
            var cacheLifeInSeconds = 10 * 60; //10 minutes
#if DEBUG
            cacheLifeInSeconds = 20;
#endif

            container.RegisterType<CamModel>(new HierarchicalLifetimeManager(), new InjectionConstructor("CamModel"));
            container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPrincipalService, PrincipalService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPermissableService, ResourceService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserProvider, BearerTokenUserProvider>(new HierarchicalLifetimeManager());
            container.RegisterType<ObjectCache>(new InjectionFactory((c) =>
            {
                return MemoryCache.Default;
            }));
            container.RegisterType<IUserCacheService>(new HierarchicalLifetimeManager(), new InjectionFactory((c) =>
            {
                return new UserCacheService(c.Resolve<ObjectCache>(), cacheLifeInSeconds);
            }));
            container.RegisterType<IResourceService, ResourceService>(new HierarchicalLifetimeManager(), new InjectionFactory((c) =>
            {
                return new ResourceService(c.Resolve<CamModel>(), c.Resolve<ObjectCache>(), cacheLifeInSeconds);
            }));
            container.RegisterType<IPermissionService>(new HierarchicalLifetimeManager(), new InjectionFactory((c) =>
            {
                return new PermissionService(c.Resolve<CamModel>(), c.Resolve<ObjectCache>(), cacheLifeInSeconds);
            }));
            container.RegisterType<IResourceAuthorizationHandler, ResourceAuthorizationHandler>(new HierarchicalLifetimeManager());
        }
    }
}