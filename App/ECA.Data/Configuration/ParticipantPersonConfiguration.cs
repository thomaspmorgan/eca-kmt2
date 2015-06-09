using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    class ParticipantPersonConfiguration : EntityTypeConfiguration<ParticipantPerson>
    {
        public ParticipantPersonConfiguration()
        {
            HasOptional(a => a.HomeInstitution).WithMany().HasForeignKey(a => a.HomeInstitutionId).WillCascadeOnDelete(false);
            HasOptional(a => a.HostInstitution).WithMany().HasForeignKey(a => a.HostInstitutionId).WillCascadeOnDelete(false);

            HasRequired(a => a.Participant).WithRequiredDependent().WillCascadeOnDelete(false);

            Property(a => a.HomeInstitutionId).HasColumnName("HomeInstitutionId");
            Property(a => a.HostInstitutionId).HasColumnName("HostInstitutionId");
        }
    }
}
