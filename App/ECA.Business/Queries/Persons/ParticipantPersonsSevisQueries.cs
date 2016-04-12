using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Data;
using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ParticipantQueries are used to query a DbContext for Participant information.
    /// </summary>
    public static class ParticipantPersonsSevisQueries
    {
        /// <summary>
        /// The none sevis comm status, when a participant has no statuses yet.
        /// </summary>
        public const string NONE_SEVIS_COMM_STATUS_NAME = "None";

        /// <summary>
        /// The sevis comm status id to use when a participant does not yet have any comm statuses.
        /// </summary>
        public const int NONE_SEVIS_COMM_STATUS_ID = 0;

        /// <summary>
        /// Query to get a list of participant people with SEVIS information
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>List of participant people with SEVIS</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonsSevisDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from p in context.ParticipantPersons

                        let participant = p.Participant != null ? p.Participant : null
                        let participantType = participant != null ? participant.ParticipantType : null
                        let participantTypeName = participantType != null ? participantType.Name : null

                        let participantStatus = participant != null ? participant.Status : null
                        let participantStatusName = participantStatus != null ? participantStatus.Status : null

                        let commStatuses = p.ParticipantPersonSevisCommStatuses.OrderByDescending(s => s.AddedOn)
                        let latestBatchStatus = commStatuses.Where(x => x.BatchId != null).FirstOrDefault()
                        let latestStatus = commStatuses.FirstOrDefault()
                        let latestStatusCommStatus = latestStatus != null ? latestStatus.SevisCommStatus : null

                        let person = participant.Person

                        where participant.PersonId.HasValue

                        select new ParticipantPersonSevisDTO
                        {
                            ParticipantId = p.ParticipantId,
                            SevisId = p.SevisId,
                            ProjectId = participant.ProjectId,
                            ParticipantType = participantTypeName,
                            ParticipantTypeId = participant.ParticipantTypeId,
                            ParticipantStatusId = participant.ParticipantStatusId,
                            ParticipantStatus = participantStatusName,
                            IsCancelled = p.IsCancelled,
                            IsDS2019Printed = p.IsDS2019Printed,
                            IsDS2019SentToTraveler = p.IsDS2019SentToTraveler,
                            IsSentToSevisViaRTI = p.IsSentToSevisViaRTI,
                            IsValidatedViaRTI = p.IsValidatedViaRTI,
                            PersonId = person.PersonId,
                            FullName = person.FullName,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            LastBatchDate = latestBatchStatus != null ? latestBatchStatus.AddedOn : default(DateTimeOffset?),
                            SevisValidationResult = p.SevisValidationResult,
                            SevisBatchResult = p.SevisBatchResult,
                            SevisStatus = latestStatusCommStatus != null ? latestStatusCommStatus.SevisCommStatusName : NONE_SEVIS_COMM_STATUS_NAME,
                            SevisStatusId = latestStatusCommStatus != null ? latestStatusCommStatus.SevisCommStatusId : NONE_SEVIS_COMM_STATUS_ID,
                        };
            return query;
        }

        /// <summary>
        /// Creates a query to return all participant person sevis comm status dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <returns>The query.</returns>
        public static IQueryable<ParticipantPersonSevisCommStatusDTO> CreateGetParticipantPersonSevisCommStatusesQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from status in context.ParticipantPersonSevisCommStatuses
                        let participantPerson = status.ParticipantPerson
                        let participant = participantPerson.Participant
                        let userAccount = context.UserAccounts.Where(x => x.PrincipalId == status.PrincipalId).FirstOrDefault()
                        select new ParticipantPersonSevisCommStatusDTO
                        {
                            Id = status.Id,
                            ParticipantId = status.ParticipantId,
                            ProjectId = participant.ProjectId,
                            SevisCommStatusId = status.SevisCommStatusId,
                            SevisCommStatusName = status.SevisCommStatus.SevisCommStatusName,
                            AddedOn = status.AddedOn,
                            BatchId = status.BatchId,
                            SevisOrgId = status.SevisOrgId,
                            EmailAddress = userAccount != null ? userAccount.EmailAddress : null,
                            DisplayName = userAccount != null ? userAccount.DisplayName : null,
                            PrincipalId = userAccount != null ? userAccount.PrincipalId : default(int?)
                        };
            return query;

        }

        /// <summary>
        /// Returns a query to get filtered and sorted participant person sevis comm status dtos.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="participantId">The participant id.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The query.</returns>
        public static IQueryable<ParticipantPersonSevisCommStatusDTO> CreateGetParticipantPersonSevisCommStatusesByParticipantIdQuery(EcaContext context, int projectId, int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantPersonSevisCommStatusesQuery(context).Where(x => x.ParticipantId == participantId && x.ProjectId == projectId);
            query = query.Apply(queryOperator);
            return query;
        }


        /// <summary>
        /// Returns the participantPersonSevis by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The participantPersonSevis</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonsSevisDTOByIdQuery(EcaContext context, int projectId, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetParticipantPersonsSevisDTOQuery(context)
                .Where(p => p.ProjectId == projectId)
                .Where(p => p.ParticipantId == participantId);
            return query;
        }
    }
}
