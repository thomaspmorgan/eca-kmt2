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
            HasRequired(x => x.DataPointCategoryProperty).WithMany().HasForeignKey(x => x.DataPointCategoryPropertyId).WillCascadeOnDelete(false);
        }
    }
}
