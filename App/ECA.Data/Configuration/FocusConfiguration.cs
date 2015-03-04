using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    public class FocusConfiguration : EntityTypeConfiguration<Focus>
    {
        public FocusConfiguration()
        {
            ToTable("Focus");
            HasKey(a => a.FocusId);
            Property(a => a.FocusId).HasColumnName("FocusId");
            Property(a => a.FocusName).IsRequired().HasMaxLength(Focus.NAME_MAX_LENGTH);
        }
    }
}
