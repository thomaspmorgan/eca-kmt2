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
    public class Person : IHistorical, IAddressable, ISocialable, IEmailAddressable, IPhoneNumberable
    {
        /// <summary>
        /// Gets the max length of the first name.
        /// </summary>
        public const int FIRST_NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Gets the max length of a person's last name.
        /// </summary>
        public const int LAST_NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Gets the max length of the name prefix.
        /// </summary>
        public const int NAME_PREFIX_MAX_LENGTH = 10;

        /// <summary>
        /// Gets max length of the name suffix.
        /// </summary>
        public const int NAME_SUFFIX_MAX_LENGTH = 10;

        /// <summary>
        /// Gets the max length of the given name.
        /// </summary>
        public const int GIVEN_NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Gets the max length of the family name.
        /// </summary>
        public const int FAMILY_NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Gets the max length of the middle name.
        /// </summary>
        public const int MIDDLE_NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Gets the max length of the patronym.
        /// </summary>
        public const int PATRONYM_NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Gets the max length of the name alias.
        /// </summary>
        public const int ALIAS_NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Creates a new instances and initializes the collection properties.
        /// </summary>
        public Person()
        {
            CountriesOfCitizenship = new HashSet<Location>();
            Publications = new HashSet<Publication>();
            SpecialStatuses = new HashSet<SpecialStatus>();
            Memberships = new HashSet<Membership>();
            InterestsAndSpecializations = new HashSet<InterestSpecialization>();
            ProminentCategories = new HashSet<ProminentCategory>();
            ProfessionalHistory = new HashSet<ProfessionEducation>();
            EducationalHistory = new HashSet<ProfessionEducation>();
            LanguageProficiencies = new HashSet<PersonLanguageProficiency>();
            PhoneNumbers = new HashSet<PhoneNumber>();
            EmailAddresses = new HashSet<EmailAddress>();
            ExternalIds = new HashSet<ExternalId>();
            SocialMedias = new HashSet<SocialMedia>();
            Addresses = new HashSet<Address>();
            Activities = new HashSet<Activity>();
            EvaluationNotes = new HashSet<PersonEvaluationNote>();
            Family = new HashSet<Person>();
            OtherFamily = new HashSet<Person>();
            Impacts = new HashSet<Impact>();
            History = new History();
            Participations = new HashSet<Participant>();
        }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        [Key]
        public int PersonId { get; set; }
        
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the gender id.
        /// </summary>
        [Required]
        public int GenderId { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Denotes if date of birth is unknown
        /// </summary>
        public bool? IsDateOfBirthUnknown { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        [MaxLength(FIRST_NAME_MAX_LENGTH)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        [MaxLength(LAST_NAME_MAX_LENGTH)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name prefix.
        /// </summary>
        [MaxLength(NAME_PREFIX_MAX_LENGTH)]
        public string NamePrefix { get; set; }

        /// <summary>
        /// Gets or sets the name suffix.
        /// </summary>
        [MaxLength(NAME_SUFFIX_MAX_LENGTH)]
        public string NameSuffix { get; set; }

        /// <summary>
        /// Gets or sets the given name.
        /// </summary>
        [MaxLength(GIVEN_NAME_MAX_LENGTH)]
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the family name.
        /// </summary>
        [MaxLength(FAMILY_NAME_MAX_LENGTH)]
        public string FamilyName { get; set; }

        /// <summary>
        /// Gets or sets the middle name.
        /// </summary>
        [MaxLength(MIDDLE_NAME_MAX_LENGTH)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the patronym.
        /// </summary>
        [MaxLength(PATRONYM_NAME_MAX_LENGTH)]
        public string Patronym { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        [MaxLength(ALIAS_NAME_MAX_LENGTH)]
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the place of birth.
        /// </summary>
        public int? PlaceOfBirthId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the person.  This value is a computed column in the database.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string FullName { get; set; }

        public Location PlaceOfBirth { get; set; }
        public bool? IsPlaceOfBirthUnknown { get; set; }

        /// <summary>
        /// Gets or sets whether the date of birth is estimated.
        /// </summary>
        public bool? IsDateOfBirthEstimated { get; set; }

        public string MedicalConditions { get; set; }

        /// <summary>
        /// Can the participant be contacted? (agreement to contact is in place)
        /// </summary>
        public bool HasContactAgreement { get; set; }

        [InverseProperty("CitizensOfCountry")]
        public ICollection<Location> CountriesOfCitizenship { get; set; }
        public string Ethnicity { get; set; }
        public string Awards { get; set; }
        public ICollection<Publication> Publications { get; set; }
        public ICollection<SpecialStatus> SpecialStatuses { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<InterestSpecialization> InterestsAndSpecializations { get; set; }
        [InverseProperty("ProminentPeople")]
        public ICollection<ProminentCategory> ProminentCategories { get; set; }
        public ICollection<ProfessionEducation> ProfessionalHistory { get; set; }
        public ICollection<ProfessionEducation> EducationalHistory { get; set; }
        public ICollection<PersonLanguageProficiency> LanguageProficiencies { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public ICollection<EmailAddress> EmailAddresses { get; set; }
        public ICollection<ExternalId> ExternalIds { get; set; }
        public ICollection<SocialMedia> SocialMedias { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ICollection<PersonEvaluationNote> EvaluationNotes { get; set; }
        public virtual ICollection<Person> Family { get; set; }
        public ICollection<Person> OtherFamily { get; set; }
        public ICollection<Impact> Impacts { get; set; }
        public ICollection<Participant> Participations { get; set; }
        public int? MaritalStatusId { get; set; }
        public MaritalStatus MaritalStatus { get; set; }

        public History History { get; set; }

        /// <summary>
        /// Gets or sets the person type id.
        /// </summary>
        [Column("PersonTypeId")]
        public int PersonTypeId { get; set; }

        /// <summary>
        /// Gets or sets the person type.
        /// </summary>
        [ForeignKey("PersonTypeId")]
        public virtual PersonType PersonType { get; set; }

        public int GetId()
        {
            return this.PersonId;
        }
    }

}
