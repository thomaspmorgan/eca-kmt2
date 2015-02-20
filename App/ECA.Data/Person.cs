using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// A person is someone who has applied to, is participating in, or has completed an ECA project.
    /// </summary>
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        [Required]
        public virtual ICollection<NamePart> Names { get; set; }

        public Gender Gender { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
        //public int? PlaceOfBirth_LocationId { get; set; }
        //[InverseProperty("BirthPlacePeople")]
        //[ForeignKey("PlaceOfBirth_LocationId")]
        //public Location PlaceOfBirth { get; set; }
       
        public string MedicalConditions { get; set; }
        
        [InverseProperty("CitizensOfCountry")]
        public ICollection<Location> CountriesOfCitizenship { get; set; }
        public string Ethnicity { get; set; }
        public string Awards { get; set; }
        public ICollection<Publication> Publications { get; set; }
        public bool PermissionToContact { get; set; }
        public ICollection<SpecialStatus> SpecialStatuses { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<InterestSpecialization> InterestsAndSpecializations { get; set; }
        public ICollection<ProminentCategory> ProminentCategories { get; set; }
        public ICollection<ProfessionEducation> ProfessionalHistory { get; set; }
        public ICollection<ProfessionEducation> EducationalHistory { get; set; }
        public ICollection<LanguageProficiency> LanguageProficiencies { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public ICollection<EmailAddress> Emails { get; set; }
        public ICollection<ExternalId> ExternalIds { get; set; }
        public ICollection<SocialMedia> SocialMediaIds { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Event> Events { get; set; }
        public string EvaluationRetention { get; set; }
        public ICollection<Person> Family { get; set; }
        public ICollection<Person> OtherFamily { get; set; }
        public ICollection<Impact> Impacts { get; set; }

        public History History { get; set; }
    }

}
