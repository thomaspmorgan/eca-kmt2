using ECA.Business.Queries.Models.Persons;
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

                        let commStatuses = p.ParticipantPersonSevisCommStatuses.Select(s => new ParticipantPersonSevisCommStatusDTO()
                        {
                            Id = s.Id,
                            ParticipantId = s.ParticipantId,
                            SevisCommStatusId = s.SevisCommStatusId,
                            SevisCommStatusName = s.SevisCommStatus.SevisCommStatusName,
                            AddedOn = s.AddedOn,
                            BatchId = s.BatchId,
                            SevisOrgId = s.SevisOrgId,
                            SevisUsername = s.SevisUsername
                        }).OrderByDescending(s => s.AddedOn)

                        let latestBatchStatus = commStatuses.Where(x => x.BatchId != null).FirstOrDefault()
                        let latestStatus = commStatuses.FirstOrDefault()

                        select new ParticipantPersonSevisDTO
                        {
                            ParticipantId = p.ParticipantId,
                            SevisId = p.SevisId,
                            ProjectId = participant.ProjectId,
                            ParticipantType = participantTypeName,
                            ParticipantStatus = participantStatusName,
                            IsCancelled = p.IsCancelled,
                            IsDS2019Printed = p.IsDS2019Printed,
                            IsDS2019SentToTraveler = p.IsDS2019SentToTraveler,
                            IsNeedsUpdate = p.IsNeedsUpdate,
                            IsSentToSevisViaRTI = p.IsSentToSevisViaRTI,
                            IsValidatedViaRTI = p.IsValidatedViaRTI,
                            StartDate = p.StartDate,
                            EndDate = p.EndDate,
                            SevisCommStatuses = commStatuses,
                            LastBatchDate = latestBatchStatus != null ? latestBatchStatus.AddedOn : default(DateTimeOffset?),
                            SevisValidationResult = p.SevisValidationResult,
                            SevisBatchResult = p.SevisBatchResult,
                            SevisStatus = latestStatus != null ? latestStatus.SevisCommStatusName : NONE_SEVIS_COMM_STATUS_NAME,
                            SevisStatusId = latestStatus != null ? latestStatus.SevisCommStatusId : NONE_SEVIS_COMM_STATUS_ID,
                        };
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
