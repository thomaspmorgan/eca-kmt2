using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.Model.Shared;
using System;
using System.Diagnostics.Contracts;

namespace ECA.Business.Queries.Models.Persons
{
    public class BiographicalDTO
    { 
        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public int PersonId { get; set; }

        /// <summary>
        /// Gets or sets the address id, this relates to the permanent residence address.
        /// </summary>
        public int? AddressId { get; set; }

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
        /// Position held in home country
        /// </summary>
        public string PositionCode { get; set; }

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
        /// Country of legal permanent residence
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
        /// Returns a sevis biographical model instance.
        /// </summary>
        /// <returns>The biographical model instance.</returns>
        public Biographical GetBiographical()
        {
            Contract.Requires(this.FullName != null, "The full name must not be null.");
            return new Biographical
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode,
                BirthCountryReason = this.BirthCountryReason,
                BirthDate = this.BirthDate,
                CitizenshipCountryCode = this.CitizenshipCountryCode,
                EmailAddress = this.EmailAddress,
                FullName = this.FullName.GetFullName(),
                Gender = this.Gender,
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode,
            };
        }

        /// <summary>
        /// Returns a sevis biographical update model instance.
        /// </summary>
        /// <returns>The biographical update instance.</returns>
        public BiographicalUpdate GetBiographicalUpdate()
        {
            Contract.Requires(this.FullName != null, "The full name must not be null.");
            return new BiographicalUpdate
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode,
                BirthCountryReason = this.BirthCountryReason,
                BirthDate = this.BirthDate,
                CitizenshipCountryCode = this.CitizenshipCountryCode,
                EmailAddress = this.EmailAddress,
                FullName = this.FullName.GetFullName(),
                Gender = this.Gender,
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode,
                PhoneNumber = this.PhoneNumber,
                PositionCode = this.PositionCode,
                printForm = true,
                Remarks = String.Empty,
            };
        }
        //this relates to NonImgBioType in sevis xsd
    }

    /// <summary>
    /// A DependentBiographicalDTO contains biography information for participating person's dependent.
    /// </summary>
    public class DependentBiographicalDTO : BiographicalDTO
    {
        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// Gets or sets the person type id.
        /// </summary>
        public int PersonTypeId { get; set; }

        /// <summary>
        /// Gets or sets the sevis id.
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Returns an AddDependent instance from this biographical dependent dto.  The sevis id must be null.
        /// </summary>
        /// <returns>An AddDependent instance.</returns>
        public AddDependent GetAddDependent()
        {
            Contract.Requires(this.SevisId == null, "The sevis id should be null for an add dependent.");
            return new AddDependent
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode,
                BirthCountryReason = this.BirthCountryReason,
                BirthDate = this.BirthDate,
                CitizenshipCountryCode = this.CitizenshipCountryCode,
                EmailAddress = this.EmailAddress,
                FullName = this.FullName.GetFullName(),
                Gender = this.Gender,
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode,
                printForm = false,
                Relationship = this.Relationship,
                UserDefinedA = null,
                UserDefinedB = null
            };
        }
        
        /// <summary>
        /// Returns a CreateDependent instance from this dependent dto.
        /// </summary>
        /// <returns>A CreateDependent instance.</returns>
        public CreateDependent GetCreateDependent()
        {
            return new CreateDependent
            {
                AddTIPP = new EcaAddTIPP(),
                Dependent = GetAddDependent()
            };
        }
    }
}
