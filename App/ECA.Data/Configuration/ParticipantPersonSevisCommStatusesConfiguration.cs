using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    class ParticipantPersonSevisCommStatusesConfiguration : EntityTypeConfiguration<ParticipantPersonSevisCommStatus>
    {
        public ParticipantPersonSevisCommStatusesConfiguration()
        {
            HasRequired(e => e.ParticipantPerson).WithMany(e => e.ParticipantPersonSevisCommStatuses).HasForeignKey(x => x.ParticipantId).WillCascadeOnDelete(false);
            HasRequired(a => a.SevisCommStatus).WithMany(e => e.ParticipantPersonSevisCommStatuses).HasForeignKey(x => x.SevisCommStatusId).WillCascadeOnDelete(false);
        }
    }
}
