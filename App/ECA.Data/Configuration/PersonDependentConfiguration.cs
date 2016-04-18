using System.Data.Entity.ModelConfiguration;

namespace ECA.Data.Configuration
{
    public class PersonDependentConfiguration : EntityTypeConfiguration<PersonDependent>
    {
        public PersonDependentConfiguration()
        {            
            //HasMany<PersonDependentCitizenCountry>(p => p.CountriesOfCitizenship)
            //    .WithRequired(x => x.Dependent)
            //    .Map(p =>
            //    {
            //        p.MapKey("DependentId");
            //        p.ToTable("PersonDependentCitizenCountry");
            //    });

            HasRequired(x => x.PlaceOfBirth).WithMany().HasForeignKey(x => x.PlaceOfBirthId).WillCascadeOnDelete(false);
            Property(x => x.PlaceOfBirthId).HasColumnName("PlaceOfBirthId");

            HasRequired(x => x.PlaceOfResidence).WithMany().HasForeignKey(x => x.PlaceOfResidenceId).WillCascadeOnDelete(false);
            Property(x => x.PlaceOfResidenceId).HasColumnName("PlaceOfResidenceId");            
        }
    }
}
