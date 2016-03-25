using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
