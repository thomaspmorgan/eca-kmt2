using ECA.Business.Queries.Models.Admin;
using ECA.Business.Validation.Sevis.Bio;
using System;

namespace ECA.Business.Queries.Models.Persons
{
    public class BiographicalDTO
    { 
        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the permanent residence address id.
        /// </summary>
        public int? PermanentResidenceAddressId { get; set; }

        /// <summary>
        /// Gets or sets the phone number id.
        /// </summary>
        public int? PhoneNumberId { get; set; }

        /// <summary>
        /// Gets or sets the email address id.
        /// </summary>
        public int? EmailAddressId { get; set; }

        /// <summary>
        /// Gets or sets the gender id.
        /// </summary>
        public int GenderId { get; set; }

        /// <summary>
        /// Current phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Full name of person
        /// </summary>
        public FullNameDTO FullName { get; set; }

        /// <summary>
        /// Student date of birth.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gender code.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// City of birth
        /// </summary>
        public string BirthCity { get; set; }

        /// <summary>
        /// Country of birth
        /// </summary>
        public string BirthCountryCode { get; set; }

        /// <summary>
        /// Country of citizenship
        /// </summary>
        public string CitizenshipCountryCode { get; set; }

        /// <summary>
        /// Gets or sets the permamanet residence country code, i.e the code of the country
        /// from the person's addresses, ordered by primary, that are not in the united states.
        /// </summary>
        public string PermanentResidenceCountryCode { get; set; }

        /// <summary>
        /// Birth country reason (01 = U.S. - Born to foreign diplomat, 02 = U.S. - Expatriated)
        /// </summary>
        public string BirthCountryReason { get; set; }

        /// <summary>
        /// Email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the number of cuntry citizenships.
        /// </summary>
        public int NumberOfCitizenships { get; set; }

        /// <summary>
        /// Gets or sets the mailing address i.e. the person's host address in the united states.
        /// </summary>
        public AddressDTO MailAddress { get; set; }
    }
}
