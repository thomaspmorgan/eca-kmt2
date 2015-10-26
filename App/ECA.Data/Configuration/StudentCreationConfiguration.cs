using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    class StudentCreationConfiguration : EntityTypeConfiguration<StudentCreation>
    {
        public StudentCreationConfiguration()
        {
            ToTable("StudentCreation", "sevis");
        }
    }
}
