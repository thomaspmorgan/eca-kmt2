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
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<ArtifactType> ArtifactTypes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<ExternalId> ExternalIds { get; set; }
        public DbSet<Impact> Impacts { get; set; }
        public DbSet<ImpactType> ImpactTypes { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<ItineraryStop> ItineraryStops { get; set; }
        public DbSet<LanguageProficiency> LanguangeProficiencies { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<MoneyFlow> MoneyFlows { get; set; }
        public DbSet<NamePart> NameParts { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationType> OrganizationTypes { get; set; }
        public DbSet<ParticipantStatus> ParticipantStatuses { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<ProfessionEducation> ProfessionEducations { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Transportation> Transportations { get; set; }

        public System.Data.Entity.DbSet<ECA.Data.SpecialStatus> SpecialStatus { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<MoneyFlow>()
                .HasRequired(e => e.Source)
                .WithMany(e => e.MoneyFlowSources)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<MoneyFlow>()
                .HasRequired(e => e.Recipient)
                .WithMany(e => e.MoneyFlowRecipients)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Impact>()
                .HasOptional(e => e.Program)
                .WithMany(e => e.Impacts)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Impact>()
               .HasOptional(e => e.Project)
               .WithMany(e => e.Impacts)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<Program>()
               .HasRequired(e => e.Owner)
                .WithMany(e => e.OwnerPrograms)
                .WillCascadeOnDelete(false);
            modelBuilder.Configurations.Add(new ProjectConfiguration());
            modelBuilder.Entity<Project>()
                .HasMany<Theme>(p => p.Themes)
                .WithMany(t => t.Projects)
                .Map(p =>
                {
                    p.MapLeftKey("ProjectId");
                    p.MapRightKey("ThemeId");
                    p.ToTable("ProjectTheme");
                });
            modelBuilder.Entity<Program>()
                .HasMany<Theme>(p => p.Themes)
                .WithMany(t => t.Programs)
                .Map(p =>
                {
                    p.MapLeftKey("ProgramId");
                    p.MapRightKey("ThemeId");
                    p.ToTable("ProgramTheme");
                });
        }
    }
}
