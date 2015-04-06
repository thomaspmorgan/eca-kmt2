using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The entity configuration for the Focus class.
    /// </summary>
    public class FocusConfiguration : EntityTypeConfiguration<Focus>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public FocusConfiguration()
        {
            ToTable("Focus");
            HasKey(a => a.FocusId);
            Property(a => a.FocusId).HasColumnName("FocusId");
            Property(a => a.FocusName).IsRequired().HasMaxLength(Focus.NAME_MAX_LENGTH);
        }
    }
}
