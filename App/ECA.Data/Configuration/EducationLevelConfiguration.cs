using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    class EducationLevelConfiguration : EntityTypeConfiguration<EducationLevel>
    {
        public EducationLevelConfiguration()
        {
            ToTable("EducationLevel", "sevis");
        }
    }
}
