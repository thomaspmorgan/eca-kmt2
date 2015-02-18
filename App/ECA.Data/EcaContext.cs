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
        public DbSet<Course> Courses { get; set; }
        public DbSet<EmailAddress> EmailAddresses { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<ExternalId> ExternalIds { get; set; }
        public DbSet<Impact> Impacts { get; set; }
        public DbSet<ImpactType> ImpactTypes { get; set; }
        public DbSet<InterestSpecialization> InterestSpecializations { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<ItineraryStop> ItineraryStops { get; set; }
        public DbSet<LanguageProficiency> LanguangeProficiencies { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MoneyFlow> MoneyFlows { get; set; }
        public DbSet<NamePart> NameParts { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationType> OrganizationTypes { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantStatus> ParticipantStatuses { get; set; }
        public DbSet<ParticipantType> ParticipantTypes { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<ProfessionEducation> ProfessionEducations { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectStatus> ProjectStatuses { get; set; }
        public DbSet<ProminentCategory> ProminentCategories { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
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
            modelBuilder.Entity<Program>()
                .HasMany<Goal>(p => p.Goals)
                .WithMany(t => t.Programs)
             .Map(p =>
                {
                  p.MapLeftKey("ProgramId");
                  p.MapRightKey("GoalId");
                  p.ToTable("ProgramGoal");
                });
            modelBuilder.Entity<Project>()
                .HasMany<Goal>(p => p.Goals)
                .WithMany(t => t.Projects)
             .Map(p =>
             {
                 p.MapLeftKey("ProjectId");
                 p.MapRightKey("GoalId");
                 p.ToTable("ProjectGoal");
             });
            modelBuilder.Entity<Project>()
                .HasMany<Location>(p => p.Regions)
                .WithMany(t => t.RegionProjects)
             .Map(p =>
             {
                 p.MapLeftKey("ProjectId");
                 p.MapRightKey("LocationId");
                 p.ToTable("ProjectRegion");
             });
            modelBuilder.Entity<Project>()
                .HasMany<Location>(p => p.Locations)
                .WithMany(t => t.LocationProjects)
             .Map(p =>
             {
                 p.MapLeftKey("ProjectId");
                 p.MapRightKey("LocationId");
                 p.ToTable("ProjectLocation");
             });
            modelBuilder.Entity<Project>()
                .HasMany<Location>(p => p.Targets)
                .WithMany(t => t.TargetProjects)
             .Map(p =>
             {
                 p.MapLeftKey("ProjectId");
                 p.MapRightKey("LocationId");
                 p.ToTable("ProjectTarget");
             });
            modelBuilder.Entity<Program>()
                .HasMany<Location>(p => p.Regions)
                .WithMany(t => t.RegionPrograms)
             .Map(p =>
             {
                 p.MapLeftKey("ProgramId");
                 p.MapRightKey("LocationId");
                 p.ToTable("ProgramRegion");
             });
            modelBuilder.Entity<Program>()
                .HasMany<Location>(p => p.Locations)
                .WithMany(t => t.LocationPrograms)
             .Map(p =>
             {
                 p.MapLeftKey("ProgramId");
                 p.MapRightKey("LocationId");
                 p.ToTable("ProgramLocation");
             });
            modelBuilder.Entity<Program>()
                .HasMany<Location>(p => p.Targets)
                .WithMany(t => t.TargetPrograms)
             .Map(p =>
             {
                 p.MapLeftKey("ProgramId");
                 p.MapRightKey("LocationId");
                 p.ToTable("ProgramTarget");
             });
            modelBuilder.Entity<Organization>()
                .HasMany<Contact>(p => p.Contacts)
                .WithMany(t => t.Organizations)
                .Map(p =>
                {
                    p.MapLeftKey("OrganizationId");
                    p.MapRightKey("ContactId");
                    p.ToTable("OrganizationContact");
                });
            modelBuilder.Entity<Program>()
                .HasMany<Contact>(p => p.Contacts)
                .WithMany(t => t.Programs)
                .Map(p =>
                {
                    p.MapLeftKey("ProgramId");
                    p.MapRightKey("ContactId");
                    p.ToTable("ProgramContact");
                });
            modelBuilder.Entity<Project>()
                .HasMany<Contact>(p => p.Contacts)
                .WithMany(t => t.Projects)
                .Map(p =>
                {
                    p.MapLeftKey("ProjectId");
                    p.MapRightKey("ContactId");
                    p.ToTable("ProjectContact");
                });
            modelBuilder.Entity<Impact>()
                .HasMany<Person>(p => p.People)
                .WithMany(t => t.Impacts)
                .Map(p =>
                {
                    p.MapLeftKey("ImpactId");
                    p.MapRightKey("PersonId");
                    p.ToTable("ImpactPerson");
                });
        }
    }
}
