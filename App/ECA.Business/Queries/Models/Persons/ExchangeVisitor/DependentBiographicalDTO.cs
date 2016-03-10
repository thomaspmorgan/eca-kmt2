using ECA.Business.Validation.Sevis.Bio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons.ExchangeVisitor
{
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
        /// Gets or sets the participant id.  The participant id is used to relate
        /// this dependent back to a participant and then back to the participant person.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Returns a dependent instance, based on this dependents SevisId value.
        /// </summary>
        /// <returns>Returns a dependent instance, based on this dependents SevisId value.</returns>
        public Dependent GetDependent()
        {
            if (String.IsNullOrWhiteSpace(this.SevisId))
            {
                return GetAddedDependent();
            }
            else
            {
                return GetUpdatedDependent();
            }
        }

        /// <summary>
        /// Returns an AddedDependent instance from this biography.
        /// </summary>
        /// <returns>The AddedDependent instance.</returns>
        public AddedDependent GetAddedDependent()
        {
            var instance = new AddedDependent
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode,
                BirthCountryReason = null,
                BirthDate = this.BirthDate,
                CitizenshipCountryCode = this.CitizenshipCountryCode,
                EmailAddress = this.EmailAddress,
                Gender = this.Gender,
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode,
                PrintForm = true,
                Relationship = this.Relationship,
            };
            if(this.FullName != null)
            {
                instance.FullName = this.FullName.GetFullName();
            }
            instance.SetParticipantId(this.ParticipantId);
            instance.SetPersonId(this.PersonId);
            return instance;
        }

        /// <summary>
        /// Returns an UpdatedDependent instance from this biography.
        /// </summary>
        /// <returns>The UpdatedDependent instance.</returns>
        public UpdatedDependent GetUpdatedDependent()
        {
            var instance = new UpdatedDependent
            {
                BirthCity = this.BirthCity,
                BirthCountryCode = this.BirthCountryCode,
                BirthCountryReason = null,
                BirthDate = this.BirthDate,
                CitizenshipCountryCode = this.CitizenshipCountryCode,
                EmailAddress = this.EmailAddress,
                Gender = this.Gender,
                PermanentResidenceCountryCode = this.PermanentResidenceCountryCode,
                PrintForm = true,
                Relationship = this.Relationship,
                IsRelationshipFieldSpecified = true,
                Remarks = null,
                SevisId = this.SevisId,
            };
            if (this.FullName != null)
            {
                instance.FullName = this.FullName.GetFullName();
            }
            instance.SetParticipantId(this.ParticipantId);
            instance.SetPersonId(this.PersonId);
            return instance;
        }
    }
}
