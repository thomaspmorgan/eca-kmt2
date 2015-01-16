using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            this.HasMany(x => x.RelatedProjects)
                .WithMany(x => x.OtherRelatedProjects)
                .Map(x =>
                {
                    x.ToTable("RelatedProjects");
                    x.MapLeftKey("ProjectId");
                    x.MapRightKey("RelatedProjectId");
                });
        }
    }
}
