using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The entity configuration for the Project class.
    /// </summary>
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ProjectConfiguration()
        {
            Property(x => x.RowVersion).IsRowVersion();
            HasMany(x => x.RelatedProjects)
                .WithMany(x => x.OtherRelatedProjects)
                .Map(x =>
                {
                    x.ToTable("RelatedProjects");
                    x.MapLeftKey("ProjectId");
                    x.MapRightKey("RelatedProjectId");
                });
            HasMany<Goal>(p => p.Goals)
                .WithMany(t => t.Projects)
                .Map(p =>
                {
                    p.MapLeftKey("ProjectId");
                    p.MapRightKey("GoalId");
                    p.ToTable("ProjectGoal");
                });
            HasMany<Location>(p => p.Regions)
                .WithMany(t => t.RegionProjects)
                .Map(p =>
                {
                    p.MapLeftKey("ProjectId");
                    p.MapRightKey("LocationId");
                    p.ToTable("ProjectRegion");
                });
            HasMany<Location>(p => p.Locations)
                .WithMany(t => t.LocationProjects)
                .Map(p =>
                {
                    p.MapLeftKey("ProjectId");
                    p.MapRightKey("LocationId");
                    p.ToTable("ProjectLocation");
                });
            HasMany<Location>(p => p.Targets)
                .WithMany(t => t.TargetProjects)
                .Map(p =>
                {
                    p.MapLeftKey("ProjectId");
                    p.MapRightKey("LocationId");
                    p.ToTable("ProjectTarget");
                });
            HasMany<Theme>(p => p.Themes)
                .WithMany(t => t.Projects)
                .Map(p =>
                {
                    p.MapLeftKey("ProjectId");
                    p.MapRightKey("ThemeId");
                    p.ToTable("ProjectTheme");
                });
            HasMany<Contact>(p => p.Contacts)
                .WithMany(t => t.Projects)
                .Map(p =>
                {
                    p.MapLeftKey("ProjectId");
                    p.MapRightKey("ContactId");
                    p.ToTable("ProjectContact");
                });
            HasMany(p => p.Categories).WithMany(t => t.Projects)
                .Map(p =>
                {
                    p.MapLeftKey("ProjectId");
                    p.MapRightKey("CategoryId");
                    p.ToTable("ProjectCategory");
                });
            HasMany(p => p.Objectives).WithMany(t => t.Projects)
              .Map(p =>
              {
                  p.MapLeftKey("ProjectId");
                  p.MapRightKey("ObjectiveId");
                  p.ToTable("ProjectObjective");
              });
            HasOptional(a => a.DefaultExchangeVisitorFunding).WithRequired(p => p.Project).WillCascadeOnDelete(false);
        }
    }
}
