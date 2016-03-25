using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class PersonDependent : IHistorical
    {
        public PersonDependent()
        {
            this.History = new History();
        }

        #region Constants

        /// <summary>
        /// Gets the max length of the first name.
        /// </summary>
        public const int FIRST_NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Gets the max length of a person's last name.
        /// </summary>
        public const int LAST_NAME_MAX_LENGTH = 50;

        /// <summary>
        /// Gets the max length of a SEVIS id.
        /// </summary>
        public const int SEVIS_ID_MAX_LENGTH = 15;

        /// <summary>
        /// Gets max length of the name suffix.
        /// </summary>
        public const int NAME_SUFFIX_MAX_LENGTH = 10;

        /// <summary>
        /// Gets the max length of the given name.
        /// </summary>
        public const int PASSPORT_NAME_MAX_LENGTH = 100;

        /// <summary>
        /// Gets the max length of the family name.
        /// </summary>
        public const int PREFERRED_NAME_MAX_LENGTH = 100;

        /// <summary>
        /// Gets the max length of the family name.
        /// </summary>
        public const int BIRTH_COUNTRY_REASON_MAX_LENGTH = 100;

        #endregion

        /// <summary>
        /// Gets or sets the dependent id.
        /// </summary>
        [Key]
        public int DependentId { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }
        
        /// <summary>
        /// the SEVIS ID (assigned by SEVIS)
        /// </summary>
        [MaxLength(SEVIS_ID_MAX_LENGTH)]
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the person type id.
        /// </summary>
        public int PersonTypeId { get; set; }

        /// <summary>
        /// Gets or sets FirstName.
        /// </summary>
        [Required]
        [MaxLength(FIRST_NAME_MAX_LENGTH)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        [Required]
        [MaxLength(LAST_NAME_MAX_LENGTH)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name suffix.
        /// </summary>
        [MaxLength(NAME_SUFFIX_MAX_LENGTH)]
        public string NameSuffix { get; set; }

        /// <summary>
        /// Gets or sets passport name.
        /// </summary>
        [MaxLength(PASSPORT_NAME_MAX_LENGTH)]
        public string PassportName { get; set; }

        /// <summary>
        /// Gets or sets preferred name.
        /// </summary>
        [MaxLength(PREFERRED_NAME_MAX_LENGTH)]
        public string PreferredName { get; set; }

        /// <summary>
        /// Gets or sets the gender id.
        /// </summary>
        [Required]
        public int GenderId { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the place of birth.
        /// </summary>
        [Required]
        public int PlaceOfBirth_LocationId { get; set; }

        /// <summary>
        /// Gets or sets the country of residence.
        /// </summary>
        [Required]
        public int Residence_LocationId { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason.
        /// </summary>
        [MaxLength(BIRTH_COUNTRY_REASON_MAX_LENGTH)]
        public string BirthCountryReason { get; set; }

        /// <summary>
        /// Gets or sets depended travelling with participant
        /// </summary>
        public bool IsTravellingWithParticipant { get; set; }

        /// <summary>
        /// Gets or sets depended was delete in ECA
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets depended was delete in SEVIS
        /// </summary>
        public bool IsSevisDeleted { get; set; }
        
        public ICollection<Location> CountriesOfCitizenship { get; set; }

        public ICollection<EmailAddress> EmailAddresses { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        /// <summary>
        /// create/update time and user
        /// </summary>
        public History History { get; set; }

    }
}
