using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ECA.Data
{

    public class EcaContext : DbContext
    {
        public const string VALIDATABLE_CONTEXT_KEY = "Context";

        public EcaContext() : base() 
        {
        
        }

        public EcaContext(string connectionStringOrKey)
            : base(connectionStringOrKey)
        {

        }

        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<ArtifactType> ArtifactTypes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<ExternalId> ExternalIds { get; set; }
        public DbSet<Focus> Foci { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Impact> Impacts { get; set; }
        public DbSet<ImpactType> ImpactTypes { get; set; }
        public DbSet<InterestSpecialization> InterestSpecializations { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<ItineraryStop> ItineraryStops { get; set; }
        public DbSet<LanguageProficiency> LanguangeProficiencies { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MoneyFlow> MoneyFlows { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationType> OrganizationTypes { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantStatus> ParticipantStatuses { get; set; }
        public DbSet<ParticipantType> ParticipantTypes { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<PhoneNumberType> PhoneNumberTypes { get; set; }
        public DbSet<ProfessionEducation> ProfessionEducations { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<ProgramType> ProgramTypes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }
        public DbSet<ProminentCategory> ProminentCategories { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<SocialMediaType> SocialMediaTypes { get; set; }
        public DbSet<SpecialStatus> SpecialStatuses { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MoneyFlowStatus> MoneyFlowStatuses { get; set; }
        public DbSet<MoneyFlowType> MoneyFlowTypes { get; set; }
        public DbSet<MoneyFlowSourceRecipientType> MoneyFlowSourceRecipientTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.AddFromAssembly(typeof(EcaContext).Assembly);
        }

        protected override System.Data.Entity.Validation.DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            items.Add(VALIDATABLE_CONTEXT_KEY, this);
            return base.ValidateEntity(entityEntry, items);
        }
    }
}
