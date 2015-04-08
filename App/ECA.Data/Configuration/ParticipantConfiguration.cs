using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The entity configuration for the Participant class.
    /// </summary>
    public class ParticipantConfiguration : EntityTypeConfiguration<Participant>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
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
