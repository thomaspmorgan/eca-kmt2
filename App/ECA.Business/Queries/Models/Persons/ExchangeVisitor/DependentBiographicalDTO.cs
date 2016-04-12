using ECA.Business.Queries.Models.Admin;
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
        /// Gets or sets the dependent type id.
        /// </summary>
        public int DependentTypeId { get; set; }

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
        /// Gets or sets whether the dependent is traveling with the participant.
        /// </summary>
        public bool IsTravelingWithParticipant { get; set; }

        /// <summary>
        /// Gets or sets whether the dependent has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
        
        /// <summary>
        /// Returns the dependent instance from this dto.  If the sevis id has a value, an UpdatedDependent is returned, otherwise,
        /// an AddedDependent is returned.
        /// </summary>
        /// <param name="usAddress">The us address of this dependent.</param>
        /// <returns>The dependent instance from this dto.</returns>
        public Dependent GetDependent(AddressDTO usAddress, string remarks)
        {
            if (!string.IsNullOrWhiteSpace(this.SevisId))
            {
                return GetUpdatedDependent(usAddress, remarks);
            }
            else
            {
                return GetAddedDependent(usAddress);
            }
        }    

        /// <summary>
        /// Returns an Added Dependent from this dto instance.
        /// </summary>
        /// <param name="usAddress">The US adress of this dependent.</param>
        /// <returns>The added dependent instance.</returns>
        public AddedDependent GetAddedDependent(AddressDTO usAddress)
        {
            FullName fullName = null;
            if(this.FullName != null)
            {
                fullName = this.FullName.GetFullName();
            }
            return new AddedDependent(
                fullName: fullName,
                birthCity: this.BirthCity,
                birthCountryCode: this.BirthCountryCode,
                birthCountryReasonCode: this.BirthCountryReasonCode,
                birthDate: this.BirthDate,
                citizenshipCountryCode: this.CitizenshipCountryCode,
                emailAddress: this.EmailAddress,
                gender: this.Gender,
                permanentResidenceCountryCode: this.PermanentResidenceCountryCode,
                phoneNumber: this.PhoneNumber,
                relationship: this.Relationship,
                mailAddress: this.MailAddress,
                usAddress: usAddress,
                printForm: true,
                participantId: this.ParticipantId,
                personId: this.PersonId,
                isTravelingWithParticipant: this.IsTravelingWithParticipant,
                isDeleted: this.IsDeleted
                );
        }

        /// <summary>
        /// Returns an Added Dependent from this dto instance.
        /// </summary>
        /// <param name="usAddress">The US adress of this dependent.</param>
        /// <returns>The added dependent instance.</returns>
        public UpdatedDependent GetUpdatedDependent(AddressDTO usAddress, string remarks)
        {
            FullName fullName = null;
            if (this.FullName != null)
            {
                fullName = this.FullName.GetFullName();
            }
            return new UpdatedDependent(
                fullName: fullName,
                birthCity: this.BirthCity,
                birthCountryCode: this.BirthCountryCode,
                birthCountryReasonCode: this.BirthCountryReasonCode,
                birthDate: this.BirthDate,
                citizenshipCountryCode: this.CitizenshipCountryCode,
                emailAddress: this.EmailAddress,
                gender: this.Gender,
                permanentResidenceCountryCode: this.PermanentResidenceCountryCode,
                phoneNumber: this.PhoneNumber,
                relationship: this.Relationship,
                mailAddress: this.MailAddress,
                usAddress: usAddress,
                printForm: true,
                participantId: this.ParticipantId,
                personId: this.PersonId,
                remarks: remarks,
                sevisId: this.SevisId,
                isTravelingWithParticipant: this.IsTravelingWithParticipant,
                isDeleted: this.IsDeleted
                );
        }
    }
}
