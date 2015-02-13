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
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }
        [InverseProperty("BirthPlacePeople")]
        public virtual Location PlaceOfBirth { get; set; }
        public string[] MedicalConditions { get; set; }
        public virtual Location ParticipantOrigination { get; set; }
        [Required]
        public virtual ICollection<Location> CountriesOfCitizenship { get; set; }
        public string Ethnicity { get; set; }
        public string[] Awards { get; set; }
        public virtual ICollection<Publication> Publications { get; set; }
        public bool PermissionToContact { get; set; }
        public virtual ICollection<SpecialStatus> SpecialStatuses { get; set; }
        public virtual ICollection<Membership> Memberships { get; set; }
        public virtual ICollection<InterestSpecialization> InterestsAndSpecializations { get; set; }
        public virtual ICollection<ProminentCategory> ProminentCategories { get; set; }
        public virtual ICollection<ProfessionEducation> ProfessionalHistory { get; set; }
        public virtual ICollection<ProfessionEducation> EducationalHistory { get; set; }
        public virtual ICollection<LanguageProficiency> LanguageProficiencies { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public virtual ICollection<EmailAddress> Emails { get; set; }
        public virtual ICollection<ExternalId> ExternalIds { get; set; }
        public virtual ICollection<SocialMedia> SocialMediaIds { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public string EvaluationRetention { get; set; }
        public virtual ICollection<Person> Family { get; set; }
        public virtual Impact Impact { get; set; }
        public int ImpactId { get; set; }

        public History History { get; set; }
    }

}
