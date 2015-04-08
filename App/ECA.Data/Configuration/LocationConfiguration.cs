using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The entity configuration for the Location class.
    /// </summary>
    public class LocationConfiguration : EntityTypeConfiguration<Location>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public LocationConfiguration()
        {
            ToTable("Location");

            HasKey(x => x.LocationId);

            HasOptional(x => x.Country).WithMany().HasForeignKey(x => x.CountryId).WillCascadeOnDelete(false);
            Property(x => x.CountryId).HasColumnName("Country_LocationId");

            HasOptional(x => x.Region).WithMany().HasForeignKey(x => x.RegionId).WillCascadeOnDelete(false);
            Property(x => x.RegionId).HasColumnName("Region_LocationId");
        }
    }
}
