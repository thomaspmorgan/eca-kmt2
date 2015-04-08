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
    public class ItineraryStopConfiguration : EntityTypeConfiguration<ItineraryStop>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public ItineraryStopConfiguration()
        {
            HasMany<Participant>(p => p.Participants)
                .WithMany(t => t.ItineraryStops)
                .Map(p =>
                {
                    p.MapLeftKey("ItineraryStopId");
                    p.MapRightKey("ParticipantId");
                    p.ToTable("ItineraryStopParticipant");
                });
        }
    }
}
