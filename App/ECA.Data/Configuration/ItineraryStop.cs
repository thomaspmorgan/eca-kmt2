using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    public class ItineraryStopConfiguration : EntityTypeConfiguration<ItineraryStop>
    {
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
