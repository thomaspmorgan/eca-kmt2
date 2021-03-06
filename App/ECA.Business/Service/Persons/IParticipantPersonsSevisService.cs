﻿using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using ECA.Business.Queries.Models.Sevis;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IParticipantPersonSevisService is capable of performing crud operations on participants and their SEVIS information.
    /// </summary>
    public interface IParticipantPersonsSevisService : ISaveable
    {

        /// <summary>
        /// Returns list of sevis participants
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="queryOperator">The query operator</param>
        /// <returns>List of sevis participants</returns>
        Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetSevisParticipantsByProjectIdAsync(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator);

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
        /// Sets sevis communication status for participant ids to queued
        /// </summary>
        /// <param name="participants">The participants that will be sent to sevis.</param>
        /// <returns>List of participant ids that were updated</returns>
        int[] SendToSevis(ParticipantsToBeSentToSevis participants);

        /// <summary>
        /// Sets sevis communication status for participant ids to queued
        /// </summary>
        /// <param name="participants">The participants that will be sent to sevis.</param>
        /// <returns>List of participant ids that were updated</returns>
        Task<int[]> SendToSevisAsync(ParticipantsToBeSentToSevis participants);

        /// <summary>
        /// Returns the paged, filtered, sorted sevis comm statuses for the participant.
        /// </summary>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The id of the participant.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted sevis comm statuses.</returns>
        PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetSevisCommStatusesByParticipantId(int projectId, int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator);

        /// <summary>
        /// Returns the paged, filtered, sorted sevis comm statuses for the participant.
        /// </summary>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The id of the participant.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted sevis comm statuses.</returns>
        Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetSevisCommStatusesByParticipantIdAsync(int projectId, int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator);

        /// <summary>
        /// Returns the batch info with the given batch id.
        /// </summary>
        /// <param name="batchId">The batch id.</param>
        /// /// <param name="userId">The id of the user requesting the batch status.</param>
        /// <param name="participantId">The participant to get the status for.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The info dto or null of it does not exist.</returns>
        SevisBatchInfoDTO GetBatchInfoByBatchId(int userId, int projectId, int participantId, string batchId);

        /// <summary>
        /// Returns the batch info with the given batch id.
        /// </summary>
        /// <param name="batchId">The batch id.</param>
        /// /// <param name="userId">The id of the user requesting the batch status.</param>
        /// <param name="participantId">The participant to get the status for.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The info dto or null of it does not exist.</returns>
        Task<SevisBatchInfoDTO> GetBatchInfoByBatchIdAsync(int userId, int projectId, int participantId, string batchId);

        /// <summary>
        /// Gets DS2019 file name
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="projectId">The project id</param>
        /// <param name="participantId">The participant id</param>
        /// <returns>The DS2019 file name</returns>
        Task<string> GetDS2019FileNameAsync(User user, int projectId, int participantId);

        /// <summary>
        /// Gets DS2019 file name
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="projectId">The project id</param>
        /// <param name="participantId">The participant id</param>
        /// <returns>The DS2019 file name</returns>
        string GetDS2019FileName(User user, int projectId, int participantId);

        /// <summary>
        /// Returns a paged, filtered, sorterd collection of participants that have a sevis id and whose start date has passed and are ready to start the sevis validation process.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants that have a sevis id and whose start date has passed </returns>
        PagedQueryResults<ReadyToValidateParticipantDTO> GetReadyToValidateParticipants(QueryableOperator<ReadyToValidateParticipantDTO> queryOperator);

        /// <summary>
        /// Returns a paged, filtered, sorterd collection of participants that have a sevis id and whose start date has passed and are ready to start the sevis validation process.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants that have a sevis id and whose start date has passed </returns>
        Task<PagedQueryResults<ReadyToValidateParticipantDTO>> GetReadyToValidateParticipantsAsync(QueryableOperator<ReadyToValidateParticipantDTO> queryOperator);

        /// <summary>
        /// Returns true if the participant with the given id is ready to validate.
        /// </summary>
        /// <param name="participantId">The participant by id.</param>
        /// <returns>True, if the participant is ready to be validated.</returns>
        bool IsParticipantReadyToValidate(int participantId);

        /// <summary>
        /// Returns true if the participant with the given id is ready to validate.
        /// </summary>
        /// <param name="participantId">The participant by id.</param>
        /// <returns>True, if the participant is ready to be validated.</returns>
        Task<bool> IsParticipantReadyToValidateAsync(int participantId);
    }
}
