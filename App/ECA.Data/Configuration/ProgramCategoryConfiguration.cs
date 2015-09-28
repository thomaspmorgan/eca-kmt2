using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    class ProgramCategoryConfiguration : EntityTypeConfiguration<ProgramCategory>
    {
        public ProgramCategoryConfiguration()
        {
            ToTable("ProgramCategory", "sevis");
        }
    }
}
