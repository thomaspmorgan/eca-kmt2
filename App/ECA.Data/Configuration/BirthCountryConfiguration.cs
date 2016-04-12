using System.Data.Entity.ModelConfiguration;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The entity type configuration for birth country.
    /// </summary>
    public class BirthCountryConfiguration : EntityTypeConfiguration<BirthCountry>
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public BirthCountryConfiguration()
        {
            ToTable("BirthCountry", "sevis");
            HasKey(x => x.BirthCountryId);
        }
    }
}
