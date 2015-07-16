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

            HasRequired(x => x.LocationType).WithMany().HasForeignKey(x => x.LocationTypeId).WillCascadeOnDelete(false);

            HasOptional(x => x.Country).WithMany().HasForeignKey(x => x.CountryId).WillCascadeOnDelete(false);
            Property(x => x.CountryId).HasColumnName("Country_LocationId");

            HasOptional(x => x.Region).WithMany().HasForeignKey(x => x.RegionId).WillCascadeOnDelete(false);
            Property(x => x.RegionId).HasColumnName("Region_LocationId");

            HasOptional(x => x.City).WithMany().HasForeignKey(x => x.CityId).WillCascadeOnDelete(false);
            Property(x => x.CityId).HasColumnName("City_LocationId");

            HasOptional(x => x.Division).WithMany().HasForeignKey(x => x.DivisionId).WillCascadeOnDelete(false);
            Property(x => x.DivisionId).HasColumnName("Division_LocationId");

            Property(x => x.LocationIso2).HasColumnName("LocationISO-2");
        }
    }
}
