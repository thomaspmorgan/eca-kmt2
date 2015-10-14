using System;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Diagnostics.Contracts;
using ECA.Core.Settings;
using ECA.Data;
using ECA.Core.Service;
using System.Data.Entity;
using Microsoft.Azure.Search;
using ECA.Business.Search;
using System.Collections.Generic;
using ECA.WebApi.Models.Admin;
using ECA.Business.Service.Lookup;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Fundings;
using ECA.Business.Service.Persons;
using ECA.Business.Service.Programs;
using ECA.Business.Service.Projects;
using ECA.Core.Generation;
using ECA.Business.Service.Reports;
using ECA.Business.Validation;
using CAM.Data;
using CAM.Business.Service;
using ECA.WebApi.Security;
using System.Runtime.Caching;
using System.Diagnostics;

namespace ECA.WebApi.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterComponents(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>
        /// Registers the components to a UnityContainer and sets the web api dependency resolver to a UnityDependencyResolver.
        /// </summary>
        public static void RegisterComponents(IUnityContainer container)
        {
            Contract.Requires(container != null, "The container must not be null.");
            RegisterContexts(container);
            RegisterServices(container);
            RegisterSearch(container);
            RegisterSecurityConcerns(container);
            RegisterValidations(container);
        }

        /// <summary>
        /// Registers the EcaContext.
        /// </summary>
        /// <param name="container">The unity container.</param>
        public static void RegisterContexts(IUnityContainer container)
        {
            var appSettings = new AppSettings();
            var connectionString = appSettings.EcaContextConnectionString.ConnectionString;
            container.RegisterType<EcaContext>(new HierarchicalLifetimeManager(), new InjectionConstructor(connectionString));
            container.RegisterType<DbContext, EcaContext>(new HierarchicalLifetimeManager(), new InjectionConstructor(connectionString));
            container.RegisterType<List<ISaveAction>>(new InjectionFactory((c) =>
            {
                var list = new List<ISaveAction>();
                list.Add(new PermissableSaveAction(c.Resolve<IPermissableService>()));
                list.Add(new GenericDocumentSaveAction<Program>(new AppSettings(), ProgramDTODocumentConfiguration.PROGRAM_DTO_DOCUMENT_TYPE_ID, x => x.ProgramId));
                list.Add(new GenericDocumentSaveAction<Project>(new AppSettings(), ProjectDTODocumentConfiguration.PROJECT_DTO_DOCUMENT_TYPE_ID, x => x.ProjectId));

                //list.Add(new DocumentsSaveAction<Program>(new AppSettings(), new DocumentSaveActionConfiguration<Program>(x => x.ProgramId, ProgramDTODocumentConfiguration.PROGRAM_DTO_DOCUMENT_TYPE_ID)));
                //list.Add(new DocumentsSaveAction<Project>(new AppSettings(), new DocumentSaveActionConfiguration<Project>(x => x.ProjectId, ProjectDTODocumentConfiguration.PROJECT_DTO_DOCUMENT_TYPE_ID)));
                list.Add(new DocumentsSaveAction<Organization>(new AppSettings(), new OrganizationDocumentSaveActionConfiguration()));
                list.Add(new DocumentsSaveAction<Organization>(new AppSettings(), new OfficeDocumentSaveActionConfiguration()));
                list.Add(new DocumentsSaveAction<Address>(new AppSettings(), new AddressDocumentSaveActionConfiguration()));
                return list;
            }));
        }

        /// <summary>
        /// Registers search related types.
        /// </summary>
        /// <param name="container">The unity container.</param>
        public static void RegisterSearch(IUnityContainer container)
        {
            var appSettings = new AppSettings();
            var serviceName = appSettings.SearchServiceName;
            var apiKey = appSettings.SearchApiKey;
            var indexName = appSettings.SearchIndexName;

            container.RegisterType<SearchServiceClient>(new HierarchicalLifetimeManager(), new InjectionFactory((c) =>
            {
                return new SearchServiceClient(serviceName, new SearchCredentials(apiKey));
            }));
            container.RegisterType<IIndexService>(new HierarchicalLifetimeManager(), new InjectionFactory((c) =>
            {
                var configs = IndexService.GetAllConfigurations(typeof(ProgramDTODocumentConfiguration).Assembly).ToList();
                var client = c.Resolve<SearchServiceClient>();
                var indexService = new IndexService(indexName, client, configs);
                return indexService;
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
            container.RegisterType<ILocationTypeService, LocationTypeService>(new HierarchicalLifetimeManager());
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
            container.RegisterType<IEduEmpService, EduEmpService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEvaluationNoteService, EvaluationNoteService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProgramService, ProgramService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProgramStatusService, ProgramStatusService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProjectService, ProjectService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProjectStatusService, ProjectStatusService>(new HierarchicalLifetimeManager());
            container.RegisterType<IReportService, ReportService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISocialMediaService, SocialMediaService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISocialMediaTypeService, SocialMediaTypeService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISocialMediaPresenceModelHandler, SocialMediaPresenceModelHandler>(new HierarchicalLifetimeManager());
            container.RegisterType<IStaticGeneratorValidator, DbContextStaticLookupValidator>(new HierarchicalLifetimeManager());
            container.RegisterType<IThemeService, ThemeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IParticipantPersonService, ParticipantPersonService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProminentCategoryService, ProminentCategoryService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBookmarkService, BookmarkService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMembershipService, MembershipService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILanguageService, LanguageService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILanguageProficiencyService, LanguageProficiencyService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmailAddressService, EmailAddressService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmailAddressTypeService, EmailAddressTypeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmailAddressHandler, EmailAddressHandler>(new HierarchicalLifetimeManager());
            container.RegisterType<IPhoneNumberService, PhoneNumberService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPhoneNumberTypeService, PhoneNumberTypeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPhoneNumberHandler, PhoneNumberHandler>(new HierarchicalLifetimeManager());
            container.RegisterType<IParticipantPersonSevisService, ParticipantPersonSevisService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISnapshotService, SnapshotService>(new HierarchicalLifetimeManager());
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
                IBusinessValidator<OrganizationValidationEntity, OrganizationValidationEntity>,
                OrganizationServiceValidator>();
            container.RegisterType<
                IBusinessValidator<EcaAddressValidationEntity, EcaAddressValidationEntity>,
                LocationServiceAddressValidator>();
            container.RegisterType<
                IBusinessValidator<LocationValidationEntity, LocationValidationEntity>,
                LocationServiceValidator>();
        }

        public static void RegisterSecurityConcerns(IUnityContainer container)
        {
            var appSettings = new AppSettings();
            var cacheLifeInSeconds = 10 * 60; //10 minutes
#if DEBUG
            cacheLifeInSeconds = 20;
#endif
            var connectionString = appSettings.CamContextConnectionString.ConnectionString;
            container.RegisterType<CamModel>(new HierarchicalLifetimeManager(), new InjectionConstructor(connectionString));
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
