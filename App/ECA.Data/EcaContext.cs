using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Infrastructure;

namespace ECA.Data
{
    /// <summary>
    /// The EcaContext is the entity framework context for the Eca system and contains all of the entities for operating
    /// on such.
    /// </summary>
    public class EcaContext : DbContext
    {
        /// <summary>
        /// The key for retrieving the context in an IValidatatableObject instance.
        /// </summary>
        public const string VALIDATABLE_CONTEXT_KEY = "Context";

        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public EcaContext() : base()
        {
            Database.SetInitializer<EcaContext>(null);
        }

        /// <summary>
        /// Creates a new EcaContext and initializes it with the given connection string or key.
        /// </summary>
        /// <param name="connectionStringOrKey">The key to a connection string in a config, or the connection string itself.</param>
        public EcaContext(string connectionStringOrKey)
            : base(connectionStringOrKey)
        {
            Database.SetInitializer<EcaContext>(null);
        }

        /// <summary>
        /// Gets or sets the accomodations.
        /// </summary>
        public DbSet<Accommodation> Accommodations { get; set; }

        /// <summary>
        /// Gets or sets the actors.
        /// </summary>
        public DbSet<Actor> Actors { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the address types.
        /// </summary>
        public DbSet<AddressType> AddressTypes { get; set; }

        /// <summary>
        /// Gets or sets the artifacts.
        /// </summary>
        public DbSet<Artifact> Artifacts { get; set; }

        /// <summary>
        /// Gets or sets the artifact types.
        /// </summary>
        public DbSet<ArtifactType> ArtifactTypes { get; set; }

        /// <summary>
        /// Gets or sets the Bookmarks
        /// </summary>
        public DbSet<Bookmark> Bookmarks { get; set; }

        /// <summary>
        /// Gets or sets the categories for a focus
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        public DbSet<Contact> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        public DbSet<Course> Courses { get; set; }

        /// <summary>
        /// Gets or sets the itinerary groups.
        /// </summary>
        public DbSet<ItineraryGroup> ItineraryGroups { get; set; }

        /// <summary>
        /// Gets or sets the email addresses.
        /// </summary>
        public DbSet<EmailAddress> EmailAddresses { get; set; }

        /// <summary>
        /// Gets or sets the email address types.
        /// </summary>
        public DbSet<EmailAddressType> EmailAddressTypes { get; set; }

        /// <summary>
        /// Gets or sets the activities.
        /// </summary>
        public DbSet<Activity> Activities { get; set; }

        /// <summary>
        /// Gets or sets the acivity types.
        /// </summary>
        public DbSet<ActivityType> ActivityTypes { get; set; }

        public DbSet<DataPointConfiguration> DataPointConfigurations { get; set; }

        public DbSet<DataPointCategoryProperty> DataPointCategoryProperties { get; set; }
        /// <summary>
        /// Gets or sets external ids.
        /// </summary>
        public DbSet<ExternalId> ExternalIds { get; set; }

        /// <summary>
        /// Gets or sets the Foci.
        /// </summary>
        public DbSet<Focus> Foci { get; set; }

        /// <summary>
        /// Gets or sets the genders.
        /// </summary>
        public DbSet<Gender> Genders { get; set; }

        /// <summary>
        /// Gets or sets the impacts.
        /// </summary>
        public DbSet<Impact> Impacts { get; set; }

        /// <summary>
        /// Gets or sets the impact types.
        /// </summary>
        public DbSet<ImpactType> ImpactTypes { get; set; }

        /// <summary>
        /// Gets or sets interest specializations.
        /// </summary>
        public DbSet<InterestSpecialization> InterestSpecializations { get; set; }

        /// <summary>
        /// Gets or sets itieneries.
        /// </summary>
        public DbSet<Itinerary> Itineraries { get; set; }

        /// <summary>
        /// Gets or sets Itinerary stops.
        /// </summary>
        public DbSet<ItineraryStop> ItineraryStops { get; set; }

        /// <summary>
        /// Gets or sets Justifications
        /// </summary>
        public DbSet<Justification> Justifications { get; set; }

        /// <summary>
        /// Gets or sets language proficiences.
        /// </summary>
        public DbSet<Language> Languages { get; set; }

        /// <summary>
        /// Gets or sets locations.
        /// </summary>
        public DbSet<Location> Locations { get; set; }

        /// <summary>
        /// Gets or sets location types.
        /// </summary>
        public DbSet<LocationType> LocationTypes { get; set; }

        /// <summary>
        /// Gets or sets marital statuses.
        /// </summary>
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }

        /// <summary>
        /// Gets or sets memberships.
        /// </summary>
        public DbSet<Membership> Memberships { get; set; }

        /// <summary>
        /// Gets or sets money flows.
        /// </summary>
        public DbSet<MoneyFlow> MoneyFlows { get; set; }

        /// <summary>
        /// Gets or sets OfficeSettings for an office
        /// </summary>
        public DbSet<OfficeSetting> OfficeSettings { get; set; }

        /// <summary>
        /// Gets or sets objectives for a Justification
        /// </summary>
        public DbSet<Objective> Objectives { get; set; }

        /// <summary>
        /// Gets or sets organizations.
        /// </summary>
        public DbSet<Organization> Organizations { get; set; }

        /// <summary>
        /// Gets or sets organization types.
        /// </summary>
        public DbSet<OrganizationType> OrganizationTypes { get; set; }

        /// <summary>
        /// Gets or sets participants.
        /// </summary>
        public DbSet<Participant> Participants { get; set; }

        /// <summary>
        /// Gets or sets the ParticipanteExchangeVisitors
        /// </summary>
        public DbSet<ParticipantExchangeVisitor> ParticipantExchangeVisitors { get; set; }

        /// <summary>
        /// Gets or sets participantPersons.
        /// </summary>
        public DbSet<ParticipantPerson> ParticipantPersons { get; set; }

        /// <summary>
        /// Gets or sets participantPerson SEVIS communication Status.
        /// </summary>
        public DbSet<ParticipantPersonSevisCommStatus> ParticipantPersonSevisCommStatuses { get; set; }

        /// <summary>
        /// Gets or sets participant statuses.
        /// </summary>
        public DbSet<ParticipantStatus> ParticipantStatuses { get; set; }

        /// <summary>
        /// Gets or sets the participant student visitors
        /// </summary>
        public DbSet<ParticipantStudentVisitor> ParticipantStudentVisitors { get; set; }

        /// <summary>
        /// Gets or sets participant types.
        /// </summary>
        public DbSet<ParticipantType> ParticipantTypes { get; set; }

        /// <summary>
        /// Gets or sets people.
        /// </summary>
        public DbSet<Person> People { get; set; }

        /// <summary>
        /// Gets or sets EvaluationNotes for a Person
        /// </summary>
        public DbSet<PersonEvaluationNote> PersonEvaluationNotes { get; set; }

        /// <summary>
        /// Gets or sets Language Proficiencies for a Person
        /// </summary>
        public DbSet<PersonLanguageProficiency> PersonLanguageProficiencies { get; set; }

        /// <summary>
        /// Gets or sets phone numbers.
        /// </summary>
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        /// <summary>
        /// Gets or sets phone number types.
        /// </summary>
        public DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }

        /// <summary>
        /// Gets or sets profession educations.
        /// </summary>
        public DbSet<ProfessionEducation> ProfessionEducations { get; set; }

        /// <summary>
        /// Gets or sets programs.
        /// </summary>
        public DbSet<Program> Programs { get; set; }

        /// <summary>
        /// Gets or sets program types.
        /// </summary>
        public DbSet<ProgramType> ProgramTypes { get; set; }

        /// <summary>
        /// Gets or sets program statuses.
        /// </summary>
        public DbSet<ProgramStatus> ProgramStatuses { get; set; }

        /// <summary>
        /// Gets or sets projects.
        /// </summary>
        public DbSet<Project> Projects { get; set; }

        /// <summary>
        /// Gets or sets project statuses.
        /// </summary>
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }

        /// <summary>
        /// Gets or sets prominent categories.
        /// </summary>
        public DbSet<ProminentCategory> ProminentCategories { get; set; }

        /// <summary>
        /// Gets or sets publications.
        /// </summary>
        public DbSet<Publication> Publications { get; set; }

        /// <summary>
        /// Gets or sets social medias.
        /// </summary>
        public DbSet<SocialMedia> SocialMedias { get; set; }

        /// <summary>
        /// Gets or sets social media types.
        /// </summary>
        public DbSet<SocialMediaType> SocialMediaTypes { get; set; }

        /// <summary>
        /// Gets or sets special statuses.
        /// </summary>
        public DbSet<SpecialStatus> SpecialStatuses { get; set; }

        /// <summary>
        /// Gets or sets themes.
        /// </summary>
        public DbSet<Theme> Themes { get; set; }

        /// <summary>
        /// Gets or sets transportations.
        /// </summary>
        public DbSet<Transportation> Transportations { get; set; }

        /// <summary>
        /// Gets or sets the goals.
        /// </summary>
        public DbSet<Goal> Goals { get; set; }

        /// <summary>
        /// Gets or sets the materials.
        /// </summary>
        public DbSet<Material> Materials { get; set; }

        /// <summary>
        /// Gets or sets the money flow statuses.
        /// </summary>
        public DbSet<MoneyFlowStatus> MoneyFlowStatuses { get; set; }

        /// <summary>
        /// Gets or sets the money flow types.
        /// </summary>
        public DbSet<MoneyFlowType> MoneyFlowTypes { get; set; }

        /// <summary>
        /// Gets or sets the money flow source recipient types.
        /// </summary>
        public DbSet<MoneyFlowSourceRecipientType> MoneyFlowSourceRecipientTypes { get; set; }

        /// <summary>
        /// Gets or sets the money flow source recipient type settings.
        /// </summary>
        public DbSet<MoneyFlowSourceRecipientTypeSetting> MoneyFlowSourceRecipientTypeSettings { get; set; }

        /// <summary>
        /// Gets or sets users.
        /// </summary>
        public DbSet<UserAccount> UserAccounts { get; set; }

        /// <summary>
        /// Gets or sets the websites.
        /// </summary>
        public DbSet<Website> Websites { get; set; }

        /// <summary>
        /// Gets or sets the organization roles
        /// </summary>
        public DbSet<OrganizationRole> OrganizationRoles { get; set; }

        /// <summary>
        /// Gets or sets the Visitor Types for a project
        /// </summary>
        public DbSet<VisitorType> VisitorTypes { get; set; }


        // Sevis Lookup Tables

        /// <summary>
        /// Gets or sets the education levels
        /// </summary>
        public DbSet<EducationLevel> EducationLevels { get; set; }

        /// <summary>
        /// Gets or sets the field of studies collection
        /// </summary>
        public DbSet<FieldOfStudy> FieldOfStudies { get; set; }

        /// <summary>
        /// Gets or sets International Organizations
        /// </summary>
        public DbSet<InternationalOrganization> InternationalOrganizations { get; set; }

        /// <summary>
        /// Gets or sets the positions
        /// </summary>
        public DbSet<Position> Positions { get; set; }

        /// <summary>
        /// List of SEVIS program categories, used for relating to a participant
        /// </summary>
        public DbSet<ProgramCategory> ProgramCategories { get; set; }

        /// <summary>
        /// Gets or sets the SevisCommStatuses.
        /// </summary>
        public DbSet<SevisCommStatus> SevisCommStatuses { get; set; }

        /// <summary>
        /// Gets or sets the StudenCreations
        /// </summary>
        public DbSet<StudentCreation> StudentCreations { get; set; }

        /// <summary>
        /// Gets or sets USGovernmentAgencies
        /// </summary>
        public DbSet<USGovernmentAgency> USGovernmentAgencies { get; set; }


        /// <summary>
        /// Overrides the DbContext OnModelCreating method.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.AddFromAssembly(typeof(EcaContext).Assembly);
        }

        /// <summary>
        /// The ValidateEntity method override that addes this context to instance to the validation items.
        /// </summary>
        /// <param name="entityEntry">The entity entry to validate.</param>
        /// <param name="items">The items that will contain the DbContext.</param>
        /// <returns>The validation results.</returns>
        protected override System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            items.Add(VALIDATABLE_CONTEXT_KEY, this);
            return base.ValidateEntity(entityEntry, items);
        }

        /// <summary>
        /// Wraps the DbContext.Entry method to retrieve a context entry.  This is useful for unit testing.
        /// </summary>
        /// <param name="x">The object to retrieve.</param>
        /// <returns>The entry.</returns>
        public virtual DbEntityEntry GetEntry(object x)
        {
            return base.Entry(x);
        }


        /// <summary>
        /// Wraps the DbContext.Entry method to retrieve a context entry.  This is useful for unit testing.
        /// </summary>
        /// <param name="x">The object to retrieve.</param>
        /// <typeparam name="T">The object type to retieve.</typeparam>
        /// <returns>The entry.</returns>
        public virtual DbEntityEntry<T> GetEntry<T>(T x) where T : class
        {
            return base.Entry<T>(x);
        }

        /// <summary>
        /// Wraps the context entry state property call.  This is useful for unit testing.
        /// </summary>
        /// <param name="x">The object to retrieve entity state for.</param>
        /// <returns>The entity state.</returns>
        public virtual EntityState GetEntityState(object x)
        {
            return GetEntry(x).State;
        }

        /// <summary>
        /// Wraps the context entry state property call.  This is useful for unit testing.
        /// </summary>
        /// <param name="x">The object to retrieve entity state for.</param>
        /// <typeparam name="T">The object type of the entry to retrieve.</typeparam>
        /// <returns>The entity state.</returns>
        public virtual EntityState GetEntityState<T>(T x) where T : class
        {
            return base.Entry<T>(x).State;
        }

        /// <summary>
        /// Returns the entity that is cached locally in the db set.  This method is virtual for unit testing.
        /// </summary>
        /// <typeparam name="T">The set type.</typeparam>
        /// <param name="whereClause">The where clause to locate the entity.</param>
        /// <returns>The local entity instance or null if not found.</returns>
        public virtual T GetLocalEntity<T>(Func<T, bool> whereClause) where T : class
        {
            return this.Set<T>().Local.Where(whereClause).FirstOrDefault();
        }
    }
}
