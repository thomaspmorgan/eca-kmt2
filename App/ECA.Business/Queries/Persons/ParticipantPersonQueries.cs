using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using ECA.Business.Queries.Admin;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ParticipantQueries are used to query a DbContext for Participant information.
    /// </summary>
    public static class ParticipantPersonQueries
    {
        /// <summary>
        /// Query to get a list of participant people 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <returns>List of participant people</returns>
        public static IQueryable<SimpleParticipantPersonDTO> CreateGetSimpleParticipantPersonsDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");

            var organizationQuery = OrganizationQueries.CreateGetOrganizationDTOsQuery(context);
            var query = (from p in context.ParticipantPersons
                         select new SimpleParticipantPersonDTO
                         {
                             ParticipantId = p.ParticipantId,
                             SevisId = p.SevisId,
                             ProjectId = p.Participant.ProjectId,
                             HomeInstitutionAddressId = p.HomeInstitutionAddressId,
                             HostInstitutionAddressId = p.HostInstitutionAddressId,
                             ParticipantType = p.Participant.ParticipantType != null ? p.Participant.ParticipantType.Name : null,
                             ParticipantTypeId = p.Participant.ParticipantTypeId,
                             ParticipantStatus = p.Participant.Status != null ? p.Participant.Status.Status : null,
                             ParticipantStatusId = p.Participant.ParticipantStatusId,
                             HomeInstitution = organizationQuery.Where(x => x.OrganizationId == p.HomeInstitutionId).FirstOrDefault(),
                             HostInstitution = organizationQuery.Where(x => x.OrganizationId == p.HostInstitutionId).FirstOrDefault(),
                             SevisStatus = p.ParticipantPersonSevisCommStatuses.Count == 0 ? "None" : p.ParticipantPersonSevisCommStatuses.OrderByDescending(s => s.AddedOn).FirstOrDefault().SevisCommStatus.SevisCommStatusName,
                             SevisStatusId = p.ParticipantPersonSevisCommStatuses.Count == 0 ? 0 : p.ParticipantPersonSevisCommStatuses.OrderByDescending(s => s.AddedOn).FirstOrDefault().SevisCommStatus.SevisCommStatusId,
                             PlacementOrganization = organizationQuery.Where(x => x.OrganizationId == p.PlacementOrganizationId).FirstOrDefault(),
                             PlacementOrganizationAddressId = p.PlacementOrganizationAddressId
                         });
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantPersons for the project with the given id in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The filtered and sorted query to retrieve participantPersons.</returns>
        public static IQueryable<SimpleParticipantPersonDTO> CreateGetSimpleParticipantPersonsDTOByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetSimpleParticipantPersonsDTOQuery(context).Where(x => x.ProjectId == projectId);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the participantPerson by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="projectId">The project id.</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPerson</returns>
        public static IQueryable<SimpleParticipantPersonDTO> CreateGetParticipantPersonDTOByIdQuery(EcaContext context, int projectId, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetSimpleParticipantPersonsDTOQuery(context)
                .Where(p => p.ProjectId == projectId)
                .Where(p => p.ParticipantId == participantId);
            return query;
        }

        /// <summary>
        /// Returns the participantPerson by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPerson</returns>
        public static IQueryable<SimpleParticipantPersonDTO> CreateGetParticipantPersonDTOByIdQuery(EcaContext context, int personId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = (from p in context.ParticipantPersons
                         where p.Participant.PersonId == personId
                         select new SimpleParticipantPersonDTO
                         {
                             ParticipantId = p.ParticipantId,
                             SevisId = p.SevisId,
                             ProjectId = p.Participant.ProjectId,
                             SevisStatus = p.ParticipantPersonSevisCommStatuses.Count == 0 ? "None" : p.ParticipantPersonSevisCommStatuses.OrderByDescending(s => s.AddedOn).FirstOrDefault().SevisCommStatus.SevisCommStatusName,
                             SevisStatusId = p.ParticipantPersonSevisCommStatuses.Count == 0 ? 0 : p.ParticipantPersonSevisCommStatuses.OrderByDescending(s => s.AddedOn).FirstOrDefault().SevisCommStatus.SevisCommStatusId
                         });

            return query;
        }
    }
}
