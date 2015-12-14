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
            HasRequired(a => a.Project).WithMany(a => a.Participants).HasForeignKey(a => a.ProjectId).WillCascadeOnDelete(false);

            HasOptional(a => a.ParticipantPerson).WithRequired(p => p.Participant).WillCascadeOnDelete(false);

            HasOptional(a => a.ParticipantStudentVisitor).WithRequired(p => p.Participant).WillCascadeOnDelete(false);

            HasOptional(a => a.ParticipantExchangeVisitor).WithRequired(p => p.Participant).WillCascadeOnDelete(false);
        }
    }
}
