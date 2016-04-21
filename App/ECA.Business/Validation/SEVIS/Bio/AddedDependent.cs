using ECA.Business.Queries.Models.Admin;
using ECA.Business.Sevis.Model;
using Newtonsoft.Json;
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
    public class AddedDependent : Dependent
    {
        [JsonConstructor]
        public AddedDependent(
                FullName fullName,
                string birthCity,
                string birthCountryCode,
                string birthCountryReasonCode,
                DateTime? birthDate,
                string citizenshipCountryCode,
                string emailAddress,
                string gender,
                string permanentResidenceCountryCode,
                string phoneNumber,
                string relationship,
                AddressDTO mailAddress,
                AddressDTO usAddress,
                bool printForm,
                int personId,
                int participantId,
                bool isTravelingWithParticipant,
                bool isDeleted
            )
            : base(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReasonCode: birthCountryReasonCode,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: emailAddress,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phoneNumber,
                relationship: relationship,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                participantId: participantId,
                personId: personId,
                isTravelingWithParticipant: isTravelingWithParticipant,
                isDeleted: isDeleted
                )
        {

        }


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

            Func<string, bool> isCodeSpecified = (value) =>
            {
                return !string.IsNullOrWhiteSpace(value);
            };

            var formPurpose = EVPrintReasonType.Item08;
            if (this.IsTravelingWithParticipant)
            {
                formPurpose = EVPrintReasonType.Item06;
            }

            var add = new SEVISEVBatchTypeExchangeVisitorDependentAdd
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode.GetBirthCntryCodeType(),
                BirthCountryReasonSpecified = isCodeSpecified(this.BirthCountryReasonCode),
                BirthDate = this.BirthDate.Value,
                CitizenshipCountryCode = this.CitizenshipCountryCode.GetCountryCodeWithType(),
                EmailAddress = this.EmailAddress,
                FormPurpose = formPurpose,
                FullName = this.FullName.GetNameType(),
                Gender = this.Gender.GetEVGenderCodeType(),
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode.GetCountryCodeWithType(),
                Relationship = this.Relationship.GetDependentCodeType(),
                printForm = this.PrintForm,
            };

            if (add.BirthCountryReasonSpecified)
            {
                add.BirthCountryReason = this.BirthCountryReasonCode.GetUSBornReasonType();
            }
            return add;
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
            Func<string, bool> isCodeSpecified = (value) =>
            {
                return !string.IsNullOrWhiteSpace(value);
            };
            var dependent = new EVPersonTypeDependent
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode.GetBirthCntryCodeType(),
                BirthCountryReasonSpecified = isCodeSpecified(this.BirthCountryReasonCode),
                BirthDate = this.BirthDate.Value,
                CitizenshipCountryCode = this.CitizenshipCountryCode.GetCountryCodeWithType(),
                EmailAddress = this.EmailAddress,
                FullName = this.FullName.GetNameType(),
                Gender = this.Gender.GetEVGenderCodeType(),
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode.GetCountryCodeWithType(),
                Relationship = this.Relationship.GetDependentCodeType(),
            };

            if (dependent.BirthCountryReasonSpecified)
            {
                dependent.BirthCountryReason = this.BirthCountryReasonCode.GetUSBornReasonType();
            }
            var sevisKey = new ParticipantSevisKey(this);
            sevisKey.SetUserDefinedFields(dependent);
            return dependent;
        }

        /// <summary>
        /// Returns false, an AddedDependent is never ignored in validation.
        /// </summary>
        /// <returns>False.</returns>
        public override bool IgnoreDependentValidation()
        {
            return this.IsDeleted;
        }

        /// <summary>
        /// Returns a request id for this dependent.
        /// </summary>
        /// <returns>A request id for this dependent.</returns>
        public override RequestId GetRequestId()
        {
            return new RequestId(this.PersonId, RequestIdType.Dependent, RequestActionType.Create);
        }
    }
}
