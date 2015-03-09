﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    public class ProgramConfiguration : EntityTypeConfiguration<Program>
    {
        public ProgramConfiguration()
        {
            Property(x => x.OwnerId).HasColumnName("Owner_OrganizationId");
            HasRequired(e => e.Owner).WithMany(e => e.OwnerPrograms).HasForeignKey(x => x.OwnerId).WillCascadeOnDelete(false);
            HasRequired(a => a.ProgramStatus).WithMany().HasForeignKey(x => x.ProgramStatusId).WillCascadeOnDelete(false);
            HasRequired(a => a.Focus).WithMany().Map(m =>
            {
                m.MapKey("FocusId");
            }).WillCascadeOnDelete(false);

            HasMany<Theme>(p => p.Themes).WithMany(t => t.Programs).Map(p =>
            {
                p.MapLeftKey("ProgramId");
                p.MapRightKey("ThemeId");
                p.ToTable("ProgramTheme");
            });
            HasMany<Goal>(p => p.Goals).WithMany(t => t.Programs).Map(p =>
            {
                p.MapLeftKey("ProgramId");
                p.MapRightKey("GoalId");
                p.ToTable("ProgramGoal");
            });
            HasMany(p => p.Regions).WithMany(t => t.RegionPrograms)
            .Map(p =>
            {
                p.MapLeftKey("ProgramId");
                p.MapRightKey("LocationId");
                p.ToTable("ProgramRegion");
            });
            HasMany(p => p.Locations).WithMany(t => t.LocationPrograms)
             .Map(p =>
             {
                 p.MapLeftKey("ProgramId");
                 p.MapRightKey("LocationId");
                 p.ToTable("ProgramLocation");
             });
            HasMany<Location>(p => p.Targets).WithMany(t => t.TargetPrograms)
             .Map(p =>
             {
                 p.MapLeftKey("ProgramId");
                 p.MapRightKey("LocationId");
                 p.ToTable("ProgramTarget");
             });
            HasMany<Contact>(p => p.Contacts)
            .WithMany(t => t.Programs)
            .Map(p =>
            {
                p.MapLeftKey("ProgramId");
                p.MapRightKey("ContactId");
                p.ToTable("ProgramContact");
            });
        }
    }
}