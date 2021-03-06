﻿using ECA.Business.Queries.Models.Persons;
using ECA.Core.Query;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq;
using ECA.Core.Service;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// An IParticipantExchangeVisitorService is capable of performing crud operations on participants and their exchange visitor data.
    /// </summary>
    public interface IParticipantExchangeVisitorService: ISaveable
    {
        /// <summary>
        /// Returns the participantExchangeVisitors by id
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantExchangeVisitors</returns>
        ParticipantExchangeVisitorDTO GetParticipantExchangeVisitorById(int projectId, int participantId);

        /// <summary>
        /// Returns the participantExchangeVisitors by id asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantExchangeVisitors</returns>
        Task<ParticipantExchangeVisitorDTO> GetParticipantExchangeVisitorByIdAsync(int projectId, int participantId);

        /// <summary>
        /// Updates a participant person student visitor info with given updated exchange visitor information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person exchange visitor info.</param>
        void Update(UpdatedParticipantExchangeVisitor updatedPersonExchangeVisitor);

        /// <summary>
        /// Updates a participant person student visitor info with given updated exchange visitor information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person exchange visitor info.</param>
        /// <returns>The task.</returns>
        Task UpdateAsync(UpdatedParticipantExchangeVisitor updatedPersonExchangeVisitor);

        /// <summary>
        /// Creates a new participant exchange visitor record
        /// </summary>
        /// <param name="participantId"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        Task CreateParticipantExchangeVisitor(int participantId, User creator);

    }
}
