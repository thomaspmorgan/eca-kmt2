using System.Data.Entity.ModelConfiguration;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The entity type configuration for birth country.
    /// </summary>
    public class BirthCountryReasonConfiguration : EntityTypeConfiguration<BirthCountryReason>
    {
        /// <summary>
        /// Creates a default instance.
        /// </summary>
        public BirthCountryReasonConfiguration()
        {
            ToTable("BirthCountryReason", "sevis");
            HasKey(x => x.BirthCountryReasonId);
        }


    }
}
