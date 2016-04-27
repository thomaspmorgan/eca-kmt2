﻿using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Models.Sevis;
using ECA.Business.Queries.Persons;
using ECA.Business.Queries.Sevis;
using ECA.Core.DynamicLinq;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// A ParticipantPersonService is capable of performing crud operations on participantPersons in the ECA system.
    /// </summary>
    public class ParticipantPersonsSevisService : DbContextService<EcaContext>, IParticipantPersonsSevisService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;
        private Action<int, int, Participant> throwSecurityViolationIfParticipantDoesNotBelongToProject;
        private Action<Participant> throwValidationErrorIfParticipantSevisInfoIsLocked;
        public readonly int[] LOCKED_SEVIS_COMM_STATUSES = { 5, 13, 14 };

        /// <summary>
        /// Creates a new ParticipantPersonService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ParticipantPersonsSevisService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfModelDoesNotExist = (id, instance, type) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The model of type [{0}] with id [{1}] was not found.", type.Name, id));
                }
            };
            throwSecurityViolationIfParticipantDoesNotBelongToProject = (userId, projectId, participant) =>
            {
                if (participant != null && participant.ProjectId != projectId)
                {
                    throw new BusinessSecurityException(
                        String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        userId,
                        participant.ParticipantId,
                        projectId));
                }
            };
            throwValidationErrorIfParticipantSevisInfoIsLocked = (participant) =>
            {
                var sevisStatusId = participant.ParticipantExchangeVisitor.ParticipantPerson.ParticipantPersonSevisCommStatuses.OrderByDescending(x => x.AddedOn).Select(x => x.SevisCommStatusId).FirstOrDefault();

                if (participant != null && IndexOfInt(LOCKED_SEVIS_COMM_STATUSES, sevisStatusId) == -1)
                {
                    throw new ValidationRulesException(
                        String.Format("An update was attempted on participant with id [{0}] but should have failed validation.",
                        participant.ParticipantId));
                }
            };
        }

        static int IndexOfInt(int[] arr, int value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == value)
                {
                    return i;
                }
            }
            return -1;
        }

        #region Get
        /// <summary>
        /// Returns list of sevis participants
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="queryOperator">The query operator</param>
        /// <returns>List of sevis participants</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetSevisParticipantsByProjectIdAsync(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var sevisParticipants = ParticipantPersonsSevisQueries.CreateGetSevisParticipantsByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved sevis participants by project id {0} and query operator {1}.", projectId, queryOperator);
            return sevisParticipants;
        }

        /// <summary>
        /// Returns a participantPersonSevis
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The participantPersonSevis</returns>
        public ParticipantPersonSevisDTO GetParticipantPersonsSevisById(int projectId, int participantId)
        {
            var participantPersonSevis = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(this.Context, projectId, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevis;
        }

        /// <summary>
        /// Returns a participantPersonSevis asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The participantPersonSevis</returns>
        public Task<ParticipantPersonSevisDTO> GetParticipantPersonsSevisByIdAsync(int projectId, int participantId)
        {
            var participantPersonSevis = ParticipantPersonsSevisQueries.CreateGetParticipantPersonsSevisDTOByIdQuery(this.Context, projectId, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevis;
        }

        /// <summary>
        /// Returns the paged, filtered, sorted sevis comm statuses for the participant.
        /// </summary>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The id of the participant.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted sevis comm statuses.</returns>
        public PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetSevisCommStatusesByParticipantId(int projectId, int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            return ParticipantPersonsSevisQueries.CreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery(this.Context, projectId, participantId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the paged, filtered, sorted sevis comm statuses for the participant.
        /// </summary>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="participantId">The id of the participant.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The paged, filtered, and sorted sevis comm statuses.</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetSevisCommStatusesByParticipantIdAsync(int projectId, int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            return ParticipantPersonsSevisQueries.CreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery(this.Context, projectId, participantId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the batch info with the given batch id.
        /// </summary>
        /// <param name="batchId">The batch id.</param>
        /// <param name="userId">The id of the user requesting the batch status.</param>
        /// <param name="participantId">The participant to get the status for.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The info dto or null of it does not exist.</returns>
        public SevisBatchInfoDTO GetBatchInfoByBatchId(int userId, int projectId, int participantId, string batchId)
        {
            var participant = Context.Participants.Find(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(userId, projectId, participant);

            var commStatuses = CreateGetParticipantPersonSevisCommStatusQuery(participantId, batchId).Count();
            if (commStatuses == 0)
            {
                return null;
            }
            else
            {
                return SevisBatchProcessingQueries.CreateGetSevisBatchInfoDTOByBatchIdQuery(this.Context, batchId).FirstOrDefault();
            }
        }

        /// <summary>
        /// Returns the batch info with the given batch id.
        /// </summary>
        /// <param name="batchId">The batch id.</param>
        /// /// <param name="userId">The id of the user requesting the batch status.</param>
        /// <param name="participantId">The participant to get the status for.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The info dto or null of it does not exist.</returns>
        public async Task<SevisBatchInfoDTO> GetBatchInfoByBatchIdAsync(int userId, int projectId, int participantId, string batchId)
        {
            var participant = Context.Participants.Find(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));
            throwSecurityViolationIfParticipantDoesNotBelongToProject(userId, projectId, participant);
            var commStatuses = await CreateGetParticipantPersonSevisCommStatusQuery(participantId, batchId).CountAsync();
            if (commStatuses == 0)
            {
                return null;
            }
            else
            {
                return await SevisBatchProcessingQueries.CreateGetSevisBatchInfoDTOByBatchIdQuery(this.Context, batchId).FirstOrDefaultAsync();
            }
        }

        private IQueryable<ParticipantPersonSevisCommStatus> CreateGetParticipantPersonSevisCommStatusQuery(int participantId, string batchId)
        {
            return Context.ParticipantPersonSevisCommStatuses.Where(x => x.ParticipantId == participantId && x.BatchId == batchId).Distinct();
        }
        public string GetDS2019FileName(int projectId, int participantId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetDS2019FileNameAsync(int projectId, int participantId)
        {
            String fileName = null;
            var participantPerson = await Context.ParticipantPersons.FindAsync(participantId);
            if (participantPerson != null)
            {
                fileName = participantPerson.DS2019FileName; 
            }
            return fileName;
        }
        #endregion

        #region Send To Sevis

        private IQueryable<ParticipantPersonSevisCommStatus> CreateGetCommStatusesThatAreReadyToSubmitQuery(int projectId, IEnumerable<int> participantIds)
        {
            var statuses = Context.ParticipantPersonSevisCommStatuses
                .Where(x => x.ParticipantPerson.Participant.ProjectId == projectId)
                .GroupBy(x => x.ParticipantId)
                .Select(s => s.OrderByDescending(x => x.AddedOn).FirstOrDefault())
                .Where(w => (w.SevisCommStatusId == SevisCommStatus.ReadyToSubmit.Id || w.SevisCommStatusId == SevisCommStatus.BatchCancelledBySystem.Id) && participantIds.Contains(w.ParticipantId));
            return statuses;
        }

        /// <summary>
        /// Sets sevis communication status for participant ids to queued
        /// </summary>
        /// <param name="participants">The participants that will be sent to sevis.</param>
        /// <returns>List of participant ids that were updated</returns>
        public async Task<int[]> SendToSevisAsync(ParticipantsToBeSentToSevis participants)
        {
            var statuses = await CreateGetCommStatusesThatAreReadyToSubmitQuery(participants.ProjectId, participants.ParticipantIds).ToListAsync();
            return DoSendToSevis(participants, statuses).Select(x => x.ParticipantId).ToArray();
        }

        /// <summary>
        /// Sets sevis communication status for participant ids to queued
        /// </summary>
        /// <param name="participants">The participants that will be sent to sevis.</param>
        /// <returns>List of participant ids that were updated</returns>
        public int[] SendToSevis(ParticipantsToBeSentToSevis participants)
        {
            var statuses = CreateGetCommStatusesThatAreReadyToSubmitQuery(participants.ProjectId, participants.ParticipantIds).ToList();
            return DoSendToSevis(participants, statuses).Select(x => x.ParticipantId).ToArray();
        }

        private IEnumerable<ParticipantPersonSevisCommStatus> DoSendToSevis(ParticipantsToBeSentToSevis model, IEnumerable<ParticipantPersonSevisCommStatus> readyToSubmitStatuses)
        {
            var addedParticipantStatuses = new List<ParticipantPersonSevisCommStatus>();
            foreach (var status in readyToSubmitStatuses)
            {
                var newStatus = new ParticipantPersonSevisCommStatus
                {
                    ParticipantId = status.ParticipantId,
                    SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                    AddedOn = DateTimeOffset.Now,
                    SevisOrgId = model.SevisOrgId,
                    SevisUsername = model.SevisUsername,
                    PrincipalId = model.Audit.User.Id
                };
                Context.ParticipantPersonSevisCommStatuses.Add(newStatus);
                addedParticipantStatuses.Add(status);
            }
            return addedParticipantStatuses.ToList();
        }
        #endregion

        #region Update

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        public void Update(UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            var participantPerson = CreateGetParticipantPersonsByIdQuery(updatedParticipantPersonSevis.ParticipantId).FirstOrDefault();
            throwIfModelDoesNotExist(updatedParticipantPersonSevis.ParticipantId, participantPerson, typeof(ParticipantPerson));
            throwValidationErrorIfParticipantSevisInfoIsLocked(participantPerson.Participant);

            DoUpdate(participantPerson, updatedParticipantPersonSevis);
        }

        /// <summary>
        /// Updates a participant person SEVIS info with given updated SEVIS information.
        /// </summary>
        /// <param name="updatedParticipantPersonSevis">The updated participant person SEVIS info.</param>
        /// <returns>The task.</returns>
        public async Task UpdateAsync(UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            var participantPerson = await CreateGetParticipantPersonsByIdQuery(updatedParticipantPersonSevis.ParticipantId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(updatedParticipantPersonSevis.ParticipantId, participantPerson, typeof(ParticipantPerson));
            throwValidationErrorIfParticipantSevisInfoIsLocked(participantPerson.Participant);

            DoUpdate(participantPerson, updatedParticipantPersonSevis);
        }

        private void DoUpdate(ParticipantPerson participantPerson, UpdatedParticipantPersonSevis updatedParticipantPersonSevis)
        {
            updatedParticipantPersonSevis.Audit.SetHistory(participantPerson);

            participantPerson.SevisId = updatedParticipantPersonSevis.SevisId;
            participantPerson.IsSentToSevisViaRTI = updatedParticipantPersonSevis.IsSentToSevisViaRTI;
            participantPerson.IsValidatedViaRTI = updatedParticipantPersonSevis.IsValidatedViaRTI;
            participantPerson.IsCancelled = updatedParticipantPersonSevis.IsCancelled;
            participantPerson.IsDS2019Printed = updatedParticipantPersonSevis.IsDS2019Printed;
            participantPerson.IsDS2019SentToTraveler = updatedParticipantPersonSevis.IsDS2019SentToTraveler;
            participantPerson.StartDate = updatedParticipantPersonSevis.StartDate;
            participantPerson.EndDate = updatedParticipantPersonSevis.EndDate;
        }

        private UpdatedParticipantPersonSevisValidationEntity GetUpdatedParticipantPersonSevisValidationEntity(ParticipantPerson participantPerson, UpdatedParticipantPersonSevis participantPersonSevis)
        {
            return new UpdatedParticipantPersonSevisValidationEntity(participantPerson, participantPersonSevis);
        }

        private IQueryable<ParticipantPerson> CreateGetParticipantPersonsByIdQuery(int participantId)
        {
            return Context.ParticipantPersons.Where(x => x.ParticipantId == participantId);
        }
        #endregion
    }
}
