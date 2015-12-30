using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    public class DataPointCategoryPropertyConfiguration : EntityTypeConfiguration<DataPointCategoryProperty>
    {
        public DataPointCategoryPropertyConfiguration()
        {
            HasRequired(x => x.DataPointCategory).WithMany().HasForeignKey(x => x.DataPointCategoryId).WillCascadeOnDelete(false);
            HasRequired(x => x.DataPointProperty).WithMany().HasForeignKey(x => x.DataPointPropertyId).WillCascadeOnDelete(false);
        }
    }
}
