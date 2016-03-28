using System.Data.Entity.ModelConfiguration;

namespace ECA.Data.Configuration
{
    public class PersonDependentConfiguration : EntityTypeConfiguration<PersonDependent>
    {
        public PersonDependentConfiguration()
        {
            HasMany(x => x.CountriesOfCitizenship)
                .WithMany()
                .Map(p =>
                {
                    p.MapLeftKey("DependentId");
                    p.MapRightKey("LocationId");
                    p.ToTable("PersonDependentCitizenCountry");
                });

        }

    }
}
