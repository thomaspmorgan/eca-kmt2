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
        IFormPrintable,
        IRemarkable,
        ISevisExchangeVisitorUpdatableComponent
    {
        /// <summary>
        /// Gets or sets the sevis id.
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the remarks.
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets is relationship field specified flag.
        /// </summary>
        public bool IsRelationshipFieldSpecified { get; set; }

        /// <summary>
        /// Gets or sets the print form flag.
        /// </summary>
        public bool PrintForm { get; set; }

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
            return new SEVISEVBatchTypeExchangeVisitorDependentEdit
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode.GetBirthCntryCodeType(),
                BirthCountryReasonSpecified = false,
                BirthDate = this.BirthDate.Value,
                CitizenshipCountryCode = this.CitizenshipCountryCode.GetCountryCodeWithType(),
                dependentSevisID = this.SevisId,
                EmailAddress = this.EmailAddress,
                FullName = this.FullName.GetNameType(),
                Gender = this.Gender.GetEVGenderCodeType(),
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode.GetCountryCodeWithType(),
                printForm = this.PrintForm,
                Relationship = this.Relationship.GetDependentCodeType(),
                RelationshipSpecified = this.IsRelationshipFieldSpecified,
                Remarks = this.Remarks
            };
        }

        /// <summary>
        /// Returns a SEVISEVBatchTypeExchangeVisitorDependentEdit instance for when a sevis registered exchange visitor
        /// must have a dependent updated in sevis.
        /// </summary>
        /// <returns>A SEVISEVBatchTypeExchangeVisitorDependentEdit instance for when a sevis registered exchange visitor
        /// must have a dependent updated in sevis.</returns>
        public object GetSevisEvBatchTypeExchangeVisitorUpdateComponent()
        {
            return GetSevisExhangeVisitorDependentInstance();
        }
    }
}
