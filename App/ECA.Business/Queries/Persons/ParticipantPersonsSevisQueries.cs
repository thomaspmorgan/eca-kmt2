using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;

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
                             StudyProject = p.StudyProject,
                             FieldOfStudy = p.FieldOfStudy != null ? p.FieldOfStudy.Description : string.Empty,
                             ProgramCategory = p.ProgramCategory != null ? p.ProgramCategory.Description : string.Empty,
                             Position = p.Position != null ? p.Position.Description : string.Empty,
                             FieldOfStudyId = p.FieldOfStudyId,
                             ProgramCategoryId = p.ProgramCategoryId,
                             PositionId = p.PositionId,
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
                             FundingGovtAgency1 = p.FundingGovtAgency1 ?? 0,
                             GovtAgency1Id = p.GovtAgency1Id,
                             GovtAgency1Name = p.GovtAgency1Name,
                             FundingGovtAgency2 = p.FundingGovtAgency2 ?? 0,
                             GovtAgency2Id = p.GovtAgency2Id,
                             GovtAgency2Name = p.GovtAgency2Name,
                             FundingIntlOrg1 = p.FundingIntlOrg1 ?? 0,
                             IntlOrg1Id = p.IntlOrg1Id,
                             IntlOrg1Name = p.IntlOrg1Name,
                             FundingIntlOrg2 = p.FundingIntlOrg2 ?? 0,
                             IntlOrg2Id = p.IntlOrg2Id,
                             IntlOrg2Name = p.IntlOrg2Name,
                             FundingOther = p.FundingOther ?? 0,
                             OtherName = p.OtherName,
                             FundingPersonal = p.FundingPersonal ?? 0,
                             FundingSponsor = p.FundingSponsor ?? 0,
                             FundingTotal = p.FundingTotal ?? 0,
                             FundingVisBNC = p.FundingVisBNC ?? 0,
                             FundingVisGovt = p.FundingVisGovt ?? 0,
                             SevisCommStatuses = p.ParticipantPersonSevisCommStatuses.Select(s => new ParticipantPersonSevisCommStatusDTO()
                             { Id = s.Id, ParticipantId = s.ParticipantId, SevisCommStatusId = s.SevisCommStatusId, SevisCommStatusName = s.SevisCommStatus.SevisCommStatusName, AddedOn = s.AddedOn
                             }).OrderBy(s => s.AddedOn),
                             LastBatchDate =  p.ParticipantPersonSevisCommStatuses.Max(s => s.AddedOn)
                         });
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantPersonSevises in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted query to retrieve participantPersonSevises.</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonsSevisDTOQuery(EcaContext context, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantPersonsSevisDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantPersonSevises for the project with the given id in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The filtered and sorted query to retrieve participantPersonSevises.</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonsSevisDTOByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetParticipantPersonsSevisDTOQuery(context).Where(x => x.ProjectId == projectId);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the participantPersonSevis by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public static IQueryable<ParticipantPersonSevisDTO> CreateGetParticipantPersonsSevisDTOByIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetParticipantPersonsSevisDTOQuery(context).
                Where(p => p.ParticipantId == participantId);
            return query;
        }
    }
}
