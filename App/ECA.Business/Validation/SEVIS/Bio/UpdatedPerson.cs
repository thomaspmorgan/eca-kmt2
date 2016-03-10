using ECA.Business.Queries.Models.Admin;
using ECA.Business.Sevis.Model;
using System.Diagnostics.Contracts;

namespace ECA.Business.Validation.Sevis.Bio
{
    public class UpdatedPerson 
        : Person, 
        IFormPrintable, 
        IRemarkable
    {
        /// <summary>
        /// Gets or sets the birth country specified flag.
        /// </summary>
        public bool BirthCountryCodeSpecified { get; set; }

        /// <summary>
        /// Gets or sets the birth date specified flag.
        /// </summary>
        public bool BirthDateSpecified { get; set; }

        /// <summary>
        /// Gets or sets the citizenship code specified flag.
        /// </summary>
        public bool CitizenshipCountryCodeSpecified { get; set; }

        /// <summary>
        /// Gets or sets the gender specified flag.
        /// </summary>
        public bool GenderSpecified { get; set; }

        /// <summary>
        /// Gets or sets the permamenent residence country code specified flag.
        /// </summary>
        public bool PermanentResidenceCountryCodeSpecified { get; set; }

        /// <summary>
        /// Gets or sets the print form flag.
        /// </summary>
        public bool PrintForm { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets the mailing address.
        /// </summary>
        public AddressDTO MailAddress { get; set; }

        /// <summary>
        /// Gets or sets the US address.
        /// </summary>
        public AddressDTO USAddress { get; set; }

        /// <summary>
        /// Gets or sets the position code.
        /// </summary>
        public int PositionCode { get; set; }

        /// <summary>
        /// Gets or sets the position code specified flag.
        /// </summary>
        public bool PositionCodeSpecified { get; set; }

        /// <summary>
        /// Returns a registered sevis exchange visitor's updated biographical information model to be used
        /// to update an exchange visitor's biography.
        /// </summary>
        /// <returns>A registered sevis exchange visitor's updated biographical information model to be used
        /// to update an exchange visitor's biography.</returns>
        public SEVISEVBatchTypeExchangeVisitorBiographical GetSEVISEVBatchTypeExchangeVisitorBiographical()
        {
            Contract.Requires(this.BirthDate.HasValue, "The birth date must have a value.");
            Contract.Requires(this.FullName != null, "The full name should be specified.");
            Contract.Requires(this.BirthCountryCode != null, "The BirthCountryCode should be specified.");
            Contract.Requires(this.CitizenshipCountryCode != null, "The CitizenshipCountryCode should be specified.");
            Contract.Requires(this.PermanentResidenceCountryCode != null, "The PermanentResidenceCountryCode should be specified.");
            Contract.Requires(this.Gender != null, "The Gender should be specified.");
            Contract.Requires(this.USAddress != null, "The us address should be specified.");

            var instance = new SEVISEVBatchTypeExchangeVisitorBiographical
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode.GetBirthCntryCodeType(),
                BirthDate = this.BirthDate.Value,
                CitizenshipCountryCode = this.CitizenshipCountryCode.GetCountryCodeWithType(),
                EmailAddress = this.EmailAddress,
                FullName = this.FullName.GetNameNullableType(),
                Gender = this.Gender.GetGenderCodeType(),
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode.GetCountryCodeWithType(),
                printForm = this.PrintForm,
                Remarks = this.Remarks,
                BirthCountryCodeSpecified = this.BirthCountryCodeSpecified,
                BirthDateSpecified = this.BirthDateSpecified,
                CitizenshipCountryCodeSpecified = this.CitizenshipCountryCodeSpecified,
                GenderSpecified = this.GenderSpecified,
                PermanentResidenceCountryCodeSpecified = this.PermanentResidenceCountryCodeSpecified,
                PhoneNumber = this.PhoneNumber,
                PositionCode = (short)this.PositionCode,
                PositionCodeSpecified = this.PositionCodeSpecified,
                ResidentialAddress = null,
                BirthCountryReason = null,

                USAddress = null,
                MailAddress = null
            };

            if (this.USAddress != null)
            {
                var address = this.USAddress.GetUSAddress();
                var usAddress = address.GetUSAddressDoctorType();
                instance.USAddress = usAddress;
            }
            if (this.MailAddress != null)
            {
                var address = this.MailAddress.GetUSAddress();
                var usAddress = address.GetUSAddressDoctorType();
                instance.MailAddress = usAddress;
            }
            return instance;
        }
    }
}

