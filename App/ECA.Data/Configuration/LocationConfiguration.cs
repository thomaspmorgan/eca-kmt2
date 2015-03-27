using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    public class LocationConfiguration : EntityTypeConfiguration<Location>
    {
        public LocationConfiguration()
        {
            ToTable("Location");
            HasKey(x => x.LocationId);
            HasOptional(x => x.Country).WithMany().HasForeignKey(x => x.CountryId).WillCascadeOnDelete(false);
            Property(x => x.CountryId).HasColumnName("Country_LocationId");
        }
    }
}
