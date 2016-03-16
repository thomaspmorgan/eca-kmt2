using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Data;
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
        /// Query to get a list of participant people with SEVIS information
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>List of participant people with SEVIS</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonsSevisDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from p in context.ParticipantPersons
                         select new ParticipantPersonSevisDTO
                         {
                             ParticipantId = p.ParticipantId,
                             SevisId = p.SevisId,
                             ProjectId = p.Participant.ProjectId,
                             ParticipantType = p.Participant.ParticipantType != null ? p.Participant.ParticipantType.Name : null,
                             ParticipantStatus = p.Participant.Status != null ? p.Participant.Status.Status : null,
                             IsCancelled = p.IsCancelled,
                             IsDS2019Printed = p.IsDS2019Printed,
                             IsDS2019SentToTraveler = p.IsDS2019SentToTraveler,
                             IsNeedsUpdate = p.IsNeedsUpdate,
                             IsSentToSevisViaRTI = p.IsSentToSevisViaRTI,
                             IsValidatedViaRTI = p.IsValidatedViaRTI,
                             StartDate = p.StartDate,
                             EndDate = p.EndDate,
                             SevisCommStatuses = p.ParticipantPersonSevisCommStatuses.Select(s => new ParticipantPersonSevisCommStatusDTO()
                             {
                                 Id = s.Id, ParticipantId = s.ParticipantId, SevisCommStatusId = s.SevisCommStatusId,
                                 SevisCommStatusName = s.SevisCommStatus.SevisCommStatusName, AddedOn = s.AddedOn
                             }).OrderBy(s => s.AddedOn),
                             LastBatchDate =  p.ParticipantPersonSevisCommStatuses.Max(s => s.AddedOn),
                             SevisValidationResult = p.SevisValidationResult,
                             SevisBatchResult = p.SevisBatchResult,
                             SevisStatus = p.ParticipantPersonSevisCommStatuses.Count == 0 ? "None" : p.ParticipantPersonSevisCommStatuses.OrderByDescending(s => s.AddedOn).FirstOrDefault().SevisCommStatus.SevisCommStatusName,
                             SevisStatusId = p.ParticipantPersonSevisCommStatuses.Count == 0 ? 0 : p.ParticipantPersonSevisCommStatuses.OrderByDescending(s => s.AddedOn).FirstOrDefault().SevisCommStatus.SevisCommStatusId
                         });
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
