using ECA.Business.Service;
using ECA.Business.Service.Persons;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// An UpdatedParticipantPersonBindingModel is used by a web api client to update a project's participant person details.
    /// </summary>
    public class UpdatedParticipantPersonBindingModel
    {
        /// <summary>
        /// The id of the participant.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// The host instituion by id.
        /// </summary>
        public int? HostInstitutionId { get; set; }

        /// <summary>
        /// The home institution by id.
        /// </summary>
        public int? HomeInstitutionId { get; set; }

        /// <summary>
        /// The host institution address by id.
        /// </summary>
        public int? HostInstitutionAddressId { get; set; }

        /// <summary>
        /// The home institution address by id.
        /// </summary>
        public int? HomeInstitutionAddressId { get; set; }

        /// <summary>
        /// The participant type by id.
        /// </summary>
        public int ParticipantTypeId { get; set; }

        /// <summary>
        /// The participant status by id.
        /// </summary>
        public int? ParticipantStatusId { get; set; }

        /// <summary>
        /// Returns a business layer UpdatedParticipantPerson instance.
        /// </summary>
        /// <param name="user">The user performing the update.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The business layer update entity.</returns>
        public UpdatedParticipantPerson ToUpdatedParticipantPerson(User user, int projectId)
        {
            var model = new UpdatedParticipantPerson(
                updater: user,
                projectId: projectId,
                participantId: this.ParticipantId,
                hostInstitutionId: this.HostInstitutionId,
                homeInstitutionId: this.HomeInstitutionId,
                hostInstitutionAddressId: this.HostInstitutionAddressId,
                homeInstitutionAddressId: this.HomeInstitutionAddressId,
                participantTypeId: this.ParticipantTypeId,
                participantStatusId: this.ParticipantStatusId
                );
            return model;
        }
    }
}