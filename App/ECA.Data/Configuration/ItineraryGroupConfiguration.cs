using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The entity type configuration for the ItineraryGroup entity.
    /// </summary>
    public class ItineraryGroupConfiguration : EntityTypeConfiguration<ItineraryGroup>
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public ItineraryGroupConfiguration()
        {
            HasMany<Participant>(x => x.Participants).WithMany(x => x.ItineraryGroups).Map(x =>
            {
                x.MapLeftKey("ItineraryGroupId");
                x.MapRightKey("ParticipantId");
                x.ToTable("ItineraryGroupParticipant");
            });
        }
    }
}
