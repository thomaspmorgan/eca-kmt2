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
                             StudyProject = p.StudyProject,
                             HomeInstitutionAddressId = p.HomeInstitutionAddressId,
                             HostInstitutionAddressId = p.HostInstitutionAddressId,
                             FieldOfStudy = p.FieldOfStudy != null ? p.FieldOfStudy.Description : null,
                             ProgramCategory = p.ProgramCategory != null ? p.ProgramCategory.Description : null,
                             Position = p.Position != null ? p.Position.Description : null,
                             ParticipantType = p.Participant.ParticipantType != null ? p.Participant.ParticipantType.Name : null,
                             ParticipantTypeId = p.Participant.ParticipantTypeId,
                             ParticipantStatus = p.Participant.Status != null ? p.Participant.Status.Status : null,
                             ParticipantStatusId = p.Participant.ParticipantStatusId,
                             HomeInstitution = organizationQuery.Where(x => x.OrganizationId == p.HomeInstitutionId).FirstOrDefault(),
                             HostInstitution = organizationQuery.Where(x => x.OrganizationId == p.HostInstitutionId).FirstOrDefault(),
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
            var query = CreateGetSimpleParticipantPersonsDTOQuery(context).Where(x => x.ProjectId == projectId);
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
            var query = CreateGetSimpleParticipantPersonsDTOQuery(context).
                Where(p => p.ParticipantId == participantId);
            return query;
        }
    }
}
