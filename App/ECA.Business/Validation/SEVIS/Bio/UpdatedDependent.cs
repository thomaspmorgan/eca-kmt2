﻿using ECA.Business.Queries.Models.Admin;
using ECA.Business.Sevis.Model;
using System;
using System.Diagnostics.Contracts;

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
                 personId: personId,
                 participantId: participantId,
                 isTravelingWithParticipant: isTravelingWithParticipant,
                 isDeleted: isDeleted
                 )
        {
            this.SevisId = sevisId;
            this.Remarks = remarks;
        }



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
                    dependentSevisID = this.SevisId,
                };
            }
            else
            {
                var edit = new SEVISEVBatchTypeExchangeVisitorDependentEdit
                {
                    BirthCity = this.BirthCity,
                    BirthCountryCode = this.BirthCountryCode.GetBirthCntryCodeType(),
                    BirthCountryReasonSpecified = isCodeSpecified(this.BirthCountryReasonCode),
                    BirthDate = this.BirthDate.Value,
                    CitizenshipCountryCode = this.CitizenshipCountryCode.GetCountryCodeWithType(),
                    dependentSevisID = this.SevisId,
                    EmailAddress = this.EmailAddress,
                    FullName = this.FullName.GetNameType(),
                    Gender = this.Gender.GetEVGenderCodeType(),
                    PermanentResidenceCountryCode = this.PermanentResidenceCountryCode.GetCountryCodeWithType(),
                    printForm = this.PrintForm,
                    RelationshipSpecified = isCodeSpecified(this.Relationship),
                    Remarks = this.Remarks,
                };

                if (edit.BirthCountryReasonSpecified)
                {
                    edit.BirthCountryReason = this.BirthCountryReasonCode.GetUSBornReasonType();
                }
                if (edit.RelationshipSpecified)
                {
                    edit.Relationship = this.Relationship.GetDependentCodeType();
                }
                return edit;
            }
        }

        /// <summary>
        /// Returns whether or not the dependent is deleted.
        /// </summary>
        /// <returns>True, if the dependent is deleted, otherwise false.</returns>
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
            return new RequestId(this.PersonId, RequestIdType.Dependent, RequestActionType.Update);
        }
    }
}
