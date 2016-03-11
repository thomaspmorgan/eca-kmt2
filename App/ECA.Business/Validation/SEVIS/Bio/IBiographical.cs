using ECA.Business.Queries.Models.Admin;
using System;

namespace ECA.Business.Validation.Sevis.Bio
{
    public interface IBiographical
    {
        /// <summary>
        /// Full name of person
        /// </summary>
        FullName FullName { get; }

        /// <summary>
        /// Student date of birth.
        /// </summary>
        DateTime? BirthDate { get; }

        /// <summary>
        /// Gender code.
        /// </summary>
        string Gender { get; }

        /// <summary>
        /// City of birth
        /// </summary>
        string BirthCity { get; }

        /// <summary>
        /// Country of birth
        /// </summary>
        string BirthCountryCode { get; }

        /// <summary>
        /// Country of citizenship
        /// </summary>
        string CitizenshipCountryCode { get; }

        /// <summary>
        /// Country of legal permanent residence
        /// </summary>
        string PermanentResidenceCountryCode { get; }

        /// <summary>
        /// Birth country reason (01 = U.S. - Born to foreign diplomat, 02 = U.S. - Expatriated)
        /// </summary>
        string BirthCountryReason { get; }

        /// <summary>
        /// Email address
        /// </summary>
        string EmailAddress { get; }

        /// <summary>
        /// Gets or sets the phone number
        /// </summary>
        string PhoneNumber { get; }

        /// <summary>
        /// Gets or sets the mailing address.
        /// </summary>
        AddressDTO MailAddress { get; }

        /// <summary>
        /// Gets or sets the US address.
        /// </summary>
        AddressDTO USAddress { get; }
    }
}
