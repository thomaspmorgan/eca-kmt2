using ECA.Business.Sevis.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Validation.Sevis.Bio
{
    /// <summary>
    /// The AddedDependentBiography is used when an exchange visitor has received a sevis id and a dependent must be added to that participant.
    /// </summary>
    public class AddedDependent : Dependent, IFormPrintable, IUserDefinable
    {
        /// <summary>
        /// Gets or sets the print form flag.
        /// </summary>
        public bool PrintForm { get; set; }


        /// <summary>
        /// Returns a SEVISEVBatchTypeExchangeVisitorDependentAdd instance to be used when an exchange visitor is registered
        /// and a dependent must be added to the participant.
        /// </summary>
        /// <returns>A SEVISEVBatchTypeExchangeVisitorDependentAdd instance to be used when an exchange visitor is registered
        /// and a dependent must be added to the participant.</returns>
        public override object GetSevisExhangeVisitorDependentInstance()
        {
            Contract.Requires(this.BirthDate.HasValue, "The birth date must have a value.");   
            Contract.Requires(this.FullName != null, "The full name should be specified.");
            Contract.Requires(this.BirthCountryCode != null, "The BirthCountryCode should be specified.");
            Contract.Requires(this.CitizenshipCountryCode != null, "The CitizenshipCountryCode should be specified.");
            Contract.Requires(this.PermanentResidenceCountryCode != null, "The PermanentResidenceCountryCode should be specified.");
            Contract.Requires(this.Relationship != null, "The relationship should be specified.");
            Contract.Requires(this.Gender != null, "The Gender should be specified.");

            return new SEVISEVBatchTypeExchangeVisitorDependentAdd
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode.GetBirthCntryCodeType(),
                BirthCountryReasonSpecified = false,
                BirthDate = this.BirthDate.Value,
                CitizenshipCountryCode = this.CitizenshipCountryCode.GetCountryCodeWithType(),
                EmailAddress = this.EmailAddress,
                FormPurpose = EVPrintReasonType.Item06,
                FullName = this.FullName.GetNameType(),
                Gender = this.Gender.GetEVGenderCodeType(),
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode.GetCountryCodeWithType(),
                Relationship = this.Relationship.GetDependentCodeType(),
                printForm = this.PrintForm
            };
        }

        /// <summary>
        /// Returns a EVPersonTypeDependent instance representing a new sevis depenedent with a new sevis exchange visitor.
        /// </summary>
        /// <returns>A EVPersonTypeDependent instance representing a new sevis depenedent with a new sevis exchange visitor.</returns>
        public EVPersonTypeDependent GetEVPersonTypeDependent()
        {
            Contract.Requires(this.BirthDate.HasValue, "The birth date must have a value.");
            Contract.Requires(this.FullName != null, "The full name should be specified.");
            Contract.Requires(this.BirthCountryCode != null, "The BirthCountryCode should be specified.");
            Contract.Requires(this.CitizenshipCountryCode != null, "The CitizenshipCountryCode should be specified.");
            Contract.Requires(this.PermanentResidenceCountryCode != null, "The PermanentResidenceCountryCode should be specified.");
            Contract.Requires(this.Gender != null, "The Gender should be specified.");

            return new EVPersonTypeDependent
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode.GetBirthCntryCodeType(),
                BirthCountryReasonSpecified = false,
                BirthDate = this.BirthDate.Value,
                CitizenshipCountryCode = this.CitizenshipCountryCode.GetCountryCodeWithType(),
                EmailAddress = this.EmailAddress,
                FullName = this.FullName.GetNameType(),
                Gender = this.Gender.GetEVGenderCodeType(),
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode.GetCountryCodeWithType(),
                Relationship = this.Relationship.GetDependentCodeType(),
                UserDefinedA = this.UserDefinedA,
                UserDefinedB = this.UserDefinedB,
            };
        }
    }
}
