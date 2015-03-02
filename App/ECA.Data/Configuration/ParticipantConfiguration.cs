using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    public class ParticipantConfiguration : EntityTypeConfiguration<Participant>
    {
        public ParticipantConfiguration()
        {
            HasMany<Project>(p => p.Projects)
                .WithMany(t => t.Participants)
                .Map(p =>
                {
                    p.MapLeftKey("ParticipantId");
                    p.MapRightKey("ProjectId");
                    p.ToTable("ParticipantProject");
                });
        }
    }
}
