using ECA.Business.Queries.Models.Persons;
using ECA.Core.Query;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq;
using System.Linq;
using ECA.Core.Service;
using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using System.Collections.Generic;
using ECA.Data;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IParticipantPersonSevisService is capable of performing crud operations on participants and their SEVIS information.
    /// </summary>
    public interface IParticipantPersonsSevisService : ISaveable
    {
        /// <summary>
        /// Returns the participantPersonSevis by id
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The participantPersonSevis</returns>
        ParticipantPersonSevisDTO GetParticipantPersonsSevisById(int projectId, int participantId);

        /// <summary>
        /// Returns the participantPersonSevis by id asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The participantPersonSevis</returns>
        Task<ParticipantPersonSevisDTO> GetParticipantPersonsSevisByIdAsync(int projectId, int participantId);

        /// <summary>
        /// Retrieve SEVIS batch XML
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        string GetSevisBatchCreateUpdateXML(int programId, User user);

        /// <summary>
        /// Retrieve a SEVIS batch to create/update exchange visitors
        /// </summary>
        /// <param name="createEVs"></param>
        /// <param name="updateEVs"></param>
        /// <param name="programId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        SEVISBatchCreateUpdateEV CreateGetSevisBatchCreateUpdateEV(List<CreateExchVisitor> createEVs, List<UpdateExchVisitor> updateEVs, int programId, User user);

        /// <summary>
        /// Retrieve participants with no sevis that are ready to submit
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Sevis exchange visitor create objects (250 max)</returns>
        List<CreateExchVisitor> GetSevisCreateEVs(User user);

        /// <summary>
        /// Retrieve participants with sevis information that are ready to submit
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Sevis exchange visitor update objects (250 max)</returns>
        List<UpdateExchVisitor> GetSevisUpdateEVs(User user);
        
        /// <summary>
        /// Retrieve XML format of SEVIS batch object
        /// </summary>
        /// <param name="validationEntity">Participant object to be validated</param>
        /// <returns>Participant object in XML format</returns>
        string GetSevisBatchXml(SEVISBatchCreateUpdateEV validationEntity);

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        void Update(UpdatedParticipantPersonSevis updatedPerson);

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        /// <returns>The task.</returns>
        Task UpdateAsync(UpdatedParticipantPersonSevis updatedParticipantPersonSevis);

        /// <summary>
        /// Sets sevis communication status for participant ids
        /// </summary>
        /// <param name="participantIds">The participant ids to update communcation status</param>
        /// <param name="projectId">The id of the project the participants belong to.</param>
        /// <returns>List of participant ids that were updated</returns>
        Task<int[]> SendToSevisAsync(int projectId, int[] participantIds);

        /// <summary>
        /// Sets sevis communication status for participant ids
        /// </summary>
        /// <param name="participantIds">The participant ids to update communcation status</param>
        /// <param name="projectId">The id of the project the participants belong to.</param>
        /// <returns>List of participant ids that were updated</returns>
        int[] SendToSevis(int projectId, int[] participantIds);
        
        /// <summary>
        /// Update a participant SEVIS pre-validation status
        /// </summary>
        /// <param name="participantId">Participant ID</param>
        /// <param name="errorCount">Validation error count</param>
        /// <param name="isValid">Indicates if SEVIS object passed validation</param>
        /// <param name="result">Validation result object</param>
        ParticipantPersonSevisCommStatus UpdateParticipantPersonSevisCommStatus(User user, int projectId, int participantId, FluentValidation.Results.ValidationResult result);

        /// <summary>
        /// Update a participant SEVIS pre-validation status
        /// </summary>
        /// <param name="participantId">Participant ID</param>
        /// <param name="projectId">The id of the project the participant belongs to.</param>
        /// <param name="user">The user performing the update.</param>
        /// <param name="result">Validation result object</param>
        /// <returns>The new comm status.</returns>
        Task<ParticipantPersonSevisCommStatus> UpdateParticipantPersonSevisCommStatusAsync(User user, int projectId, int participantId, FluentValidation.Results.ValidationResult result);
        
        /// <summary>
        /// Process SEVIS batch transaction log
        /// </summary>
        /// <param name="batchId">Batch ID</param>
        /// <param name="user">User</param>
        Task<int> UpdateParticipantPersonSevisBatchStatusAsync(User user, int batchId);
    }
}
