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
    public static class ParticipantPersonQueries
    {

        private static IQueryable<SimpleParticipantPersonDTO> CreateGetSimpleParticipantPersonsDTOQuery(EcaContext context)
        {
            var query = (from p in context.ParticipantPersons
                         select new SimpleParticipantPersonDTO
                         {
                             ParticipantId = p.ParticipantId,
                             FieldOfStudy = p.FieldOfStudyCode,
                             SevisId = p.SevisId,
                             Position = p.PositionCode,
                             HomeInstitution = p.HomeInstitution != null ? new SimpleOrganizationDTO
                             {
                                 OrganizationId = p.HomeInstitution.OrganizationId,
                                 Name = p.HomeInstitution.Name,
                                 OrganizationType = p.HomeInstitution.OrganizationType.OrganizationTypeName,
                                 Status = p.HomeInstitution.Status,
                                 Location = p.HomeInstitution.Addresses.FirstOrDefault().Location.Country.LocationName
                             } : null,
                             HostInstitution = p.HostInstitution != null ? new SimpleOrganizationDTO
                             {
                                 OrganizationId = p.HostInstitution.OrganizationId,
                                 Name = p.HostInstitution.Name,
                                 OrganizationType = p.HostInstitution.OrganizationType.OrganizationTypeName,
                                 Status = p.HostInstitution.Status,
                                 Location = p.HostInstitution.Addresses.FirstOrDefault().Location.Country.LocationName
                             } : null
                         });
            return query;
        }

        /// <summary>
        /// Creates a query to return all participantPersons in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted query to retrieve participantPersons.</returns>
        public static IQueryable<SimpleParticipantPersonDTO> CreateGetSimpleParticipantPersonsDTOQuery(EcaContext context, QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetSimpleParticipantPersonsDTOQuery(context);
            query = query.Apply(queryOperator);
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
            var query = CreateGetSimpleParticipantPersonsDTOQuery(context);

            query = from participant in context.Participants
                    join q in query
                    on participant.ParticipantId equals q.ParticipantId

                    from project in context.Projects
                    where project.ProjectId == projectId && participant.Projects.Contains(project)
                    select q;

            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Returns the participantPerson by participant id 
        /// </summary>
        /// <param name="context">The context to query</param>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPerson</returns>
        public static IQueryable<SimpleParticipantPersonDTO> CreateGetParticipantPersonDTOByIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = CreateGetSimpleParticipantPersonsDTOQuery(context);
                query.Where(p => p.ParticipantId == participantId);
            return query;
        }
    }
}
