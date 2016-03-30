using ECA.Business.Queries.Models.Admin;
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
    /// An UpdatedDependentBiography is used to update a dependent's biographical information in sevis.
    /// </summary>
    public class UpdatedDependent
        : Dependent,
        ISevisIdentifable,
        IRemarkable
    {
        public UpdatedDependent(
            FullName fullName,
            string birthCity,
            string birthCountryCode,
            string birthCountryReason,
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
            string sevisId,
            string remarks,
            int personId,
            int participantId,
            bool isTravelingWithParticipant,
            bool isDeleted
            )
            :base(
                 fullName: fullName,
                 birthCity: birthCity,
                 birthCountryCode: birthCountryCode,
                 birthCountryReason: birthCountryReason,
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
                 personId: personId,
                 participantId: participantId,
                 isTravelingWithParticipant: isTravelingWithParticipant
                 )
        {
            this.SevisId = sevisId;
            this.Remarks = remarks;
            this.IsDeleted = isDeleted;
        }

        /// <summary>
        /// Gets whether the dependent has been deleted in the KMT system.
        /// </summary>
        public bool IsDeleted { get; private set; }

        /// <summary>
        /// Gets or sets the sevis id.
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Returns a SEVISEVBatchTypeExchangeVisitorDependentEdit instance for when a sevis registered exchange visitor
        /// must have a dependent updated in sevis.
        /// </summary>
        /// <returns>A SEVISEVBatchTypeExchangeVisitorDependentEdit instance for when a sevis registered exchange visitor
        /// must have a dependent updated in sevis.</returns>
        public override object GetSevisExhangeVisitorDependentInstance()
        {
            Contract.Requires(this.BirthDate.HasValue, "The birth date must have a value.");
            Contract.Requires(this.FullName != null, "The full name should be specified.");
            Contract.Requires(this.BirthCountryCode != null, "The BirthCountryCode should be specified.");
            Contract.Requires(this.CitizenshipCountryCode != null, "The CitizenshipCountryCode should be specified.");
            Contract.Requires(this.PermanentResidenceCountryCode != null, "The PermanentResidenceCountryCode should be specified.");
            Contract.Requires(this.Gender != null, "The Gender should be specified.");
            Contract.Requires(this.Relationship != null, "The Gender should be specified.");
            Func<string, bool> isCodeSpecified = (value) =>
            {
                return !string.IsNullOrWhiteSpace(value);
            };
            if (this.IsDeleted)
            {
                return new SEVISEVBatchTypeExchangeVisitorDependentDelete
                {
                    dependentSevisID = this.SevisId
                };
            }
            else
            {
                return new SEVISEVBatchTypeExchangeVisitorDependentEdit
                {
                    BirthCity = this.BirthCity,
                    BirthCountryCode = this.BirthCountryCode.GetBirthCntryCodeType(),
                    BirthCountryReasonSpecified = isCodeSpecified(this.BirthCountryReason),
                    BirthDate = this.BirthDate.Value,
                    CitizenshipCountryCode = this.CitizenshipCountryCode.GetCountryCodeWithType(),
                    dependentSevisID = this.SevisId,
                    EmailAddress = this.EmailAddress,
                    FullName = this.FullName.GetNameType(),
                    Gender = this.Gender.GetEVGenderCodeType(),
                    PermanentResidenceCountryCode = this.PermanentResidenceCountryCode.GetCountryCodeWithType(),
                    printForm = this.PrintForm,
                    Relationship = this.Relationship.GetDependentCodeType(),
                    RelationshipSpecified = isCodeSpecified(this.Relationship),
                    Remarks = this.Remarks,
                };
            }
        }
    }
}
