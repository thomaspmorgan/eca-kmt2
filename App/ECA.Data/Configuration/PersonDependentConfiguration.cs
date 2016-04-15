using System.Data.Entity.ModelConfiguration;

namespace ECA.Data.Configuration
{
    public class PersonDependentConfiguration : EntityTypeConfiguration<PersonDependent>
    {
        public PersonDependentConfiguration()
        {

            //HasMany<PersonDependentCitizenCountry>(p => p.CountriesOfCitizenship)
            //    .WithMany(t => t.CitizenshipCountryPeople)
            //    .Map(p =>
            //    {
            //        p.MapLeftKey("DependentId");
            //        p.MapRightKey("LocationId");
            //        p.ToTable("PersonDependentCitizenCountry");
            //    });
            
            HasRequired(x => x.PlaceOfBirth).WithMany().HasForeignKey(x => x.PlaceOfBirthId).WillCascadeOnDelete(false);
            Property(x => x.PlaceOfBirthId).HasColumnName("PlaceOfBirthId");

            HasRequired(x => x.PlaceOfResidence).WithMany().HasForeignKey(x => x.PlaceOfResidenceId).WillCascadeOnDelete(false);
            Property(x => x.PlaceOfResidenceId).HasColumnName("PlaceOfResidenceId");            
        }

    }
}
