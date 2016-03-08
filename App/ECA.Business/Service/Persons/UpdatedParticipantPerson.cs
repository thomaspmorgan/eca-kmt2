using ECA.Core.Exceptions;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An UpdatedParticipantPerson is used by a business layer client to update a person that is a project participant.
    /// </summary>
    public class UpdatedParticipantPerson : IAuditable
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="updater">The user performing the update.</param>
        /// <param name="participantId">The participant id.</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="hostInstitutionId">The host institution id.</param>
        /// <param name="homeInstitutionId">The home institution id.</param>
        /// <param name="hostInstitutionAddressId">The host instutition address id.</param>
        /// <param name="homeInstitutionAddressId">The home institutution address id.</param>
        /// <param name="participantTypeId">The participant type id.</param>
        /// <param name="participantStatusId">The participant status id.</param>
        public UpdatedParticipantPerson(
            User updater, 
            int participantId, 
            int projectId,
            int? hostInstitutionId,
            int? homeInstitutionId,
            int? hostInstitutionAddressId,
            int? homeInstitutionAddressId,
            int participantTypeId,
            int? participantStatusId)
        {
            if(ParticipantType.GetStaticLookup(participantTypeId) == null)
            {
                throw new UnknownStaticLookupException(String.Format("The participant type id [{0}] is not recognized.", participantTypeId));
            }
            if (participantStatusId.HasValue && ParticipantStatus.GetStaticLookup(participantStatusId.Value) == null)
            {
                throw new UnknownStaticLookupException(String.Format("The participant status id [{0}] is not recognized.", participantStatusId));
            }
            this.Audit = new Update(updater);
            this.ProjectId = projectId;
            this.ParticipantId = participantId;
            this.HostInstitutionId = hostInstitutionId;
            this.HostInstitutionAddressId = hostInstitutionAddressId;
            this.HomeInstitutionAddressId = homeInstitutionAddressId;
            this.HomeInstitutionId = homeInstitutionId;
            this.ParticipantTypeId = participantTypeId;
            this.ParticipantStatusId = participantStatusId;
        }

        /// <summary>
        /// Gets the project id.
        /// </summary>
        public int ProjectId { get; private set; }

        /// <summary>
        /// Gets the participant id.
        /// </summary>
        public int ParticipantId { get; private set; }

        /// <summary>
        /// Gets the host instituion id.
        /// </summary>
        public int? HostInstitutionId { get; private set; }

        /// <summary>
        /// Gets the home institution id.
        /// </summary>
        public int? HomeInstitutionId { get; private set; }

        /// <summary>
        /// Gets the host institution address id.
        /// </summary>
        public int? HostInstitutionAddressId { get; private set; }

        /// <summary>
        /// Gets the home institution address id.
        /// </summary>
        public int? HomeInstitutionAddressId { get; private set; }

        /// <summary>
        /// Gets the participant type id.
        /// </summary>
        public int ParticipantTypeId { get; private set; }

        /// <summary>
        /// Gets the participant status id.
        /// </summary>
        public int? ParticipantStatusId { get; private set; }

        /// <summary>
        /// Gets the update audit.
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
