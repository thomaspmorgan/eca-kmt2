using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    public class ItineraryConfiguration : EntityTypeConfiguration<Itinerary>
    {
        public ItineraryConfiguration()
        {
            HasMany(i => i.Participants).WithMany(p => p.Itineraries)
            .Map(p =>
            {
                p.MapLeftKey("ParticipantId");
                p.MapRightKey("ItineraryId");
                p.ToTable("ItineraryParticipant");
            });
        }
    }
}
