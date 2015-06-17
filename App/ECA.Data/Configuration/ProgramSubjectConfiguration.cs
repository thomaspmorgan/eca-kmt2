using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    class ProgramSubjectConfiguration : EntityTypeConfiguration<ProgramSubject>
    {
        public ProgramSubjectConfiguration()
        {
            ToTable("ProgramSubject", "sevis");
        }
    }
}
