using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            HasMany(x => x.Family)
                .WithMany(x => x.OtherFamily)
                .Map(x =>
                {
                    x.ToTable("PersonFamily");
                    x.MapLeftKey("PersonId");
                    x.MapRightKey("RelatedPersonId");
                });
            HasMany<Location>(p => p.CountriesOfCitizenship)
                .WithMany(t => t.BirthPlacePeople)
                .Map(p =>
                {
                    p.MapLeftKey("PersonId");
                    p.MapRightKey("LocationId");
                    p.ToTable("CitizenCountry");
                });
        }
    }
}
