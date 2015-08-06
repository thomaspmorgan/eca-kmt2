using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Data.Configuration
{
    /// <summary>
    /// The entity configuration for the Person class.
    /// </summary>
    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
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

            HasOptional(x => x.PlaceOfBirth).WithMany().HasForeignKey(x => x.PlaceOfBirthId).WillCascadeOnDelete(false);
            Property(x => x.PlaceOfBirthId).HasColumnName("PlaceOfBirth_LocationId");

            HasMany(x => x.ProminentCategories)
                .WithMany(x => x.People)
                .Map(x =>
                {
                    x.ToTable("PersonProminentCategory");
                    x.MapLeftKey("PersonId");
                    x.MapRightKey("ProminentCategoryId");
                });
        }
    }
}
