using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    public class DataPointConfigurationConfiguration : EntityTypeConfiguration<DataPointConfiguration>
    {
        public DataPointConfigurationConfiguration()
        {
            HasRequired(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).WillCascadeOnDelete(false);
            HasRequired(x => x.Property).WithMany().HasForeignKey(x => x.PropertyId).WillCascadeOnDelete(false);
        }
    }
}
