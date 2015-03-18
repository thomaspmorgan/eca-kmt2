using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Persons
{
    /// <summary>
    /// The ParticipantQueries are used to query a DbContext for Participant information.
    /// </summary>
    public static class ParticipantQueries
    {
        private static IQueryable<SimpleParticipantDTO> CreateGetPersonParticipantsQuery(EcaContext context)
        {
            var query = from participant in context.Participants
                        let person = participant.Person
                        let participantType = participant.ParticipantType
                        where participant.ParticipantTypeId == ParticipantType.Individual.Id
                        select new SimpleParticipantDTO
                        {
                            Name = (person.FirstName != null ? person.FirstName : String.Empty)
                                + (person.FirstName != null && person.LastName != null ? " " : String.Empty)
                                + (person.LastName != null ? person.LastName : String.Empty),
                            OrganizationId = default(int?),
                            ParticipantId = participant.ParticipantId,
                            ParticipantType = participantType.Name,
                            ParticipantTypeId = participantType.ParticipantTypeId,
                            PersonId = person.PersonId

                        };
            return query;
        }

        private static IQueryable<SimpleParticipantDTO> CreateGetOrganizationParticipantsQuery(EcaContext context)
        {
            var query = from participant in context.Participants
                        let org = participant.Organization
                        let participantType = participant.ParticipantType
                        where participant.ParticipantTypeId != ParticipantType.Individual.Id

                        select new SimpleParticipantDTO
                        {
                            Name = org.Name,
                            OrganizationId = org.OrganizationId,
                            ParticipantId = participant.ParticipantId,
                            ParticipantType = participantType.Name,
                            ParticipantTypeId = participantType.ParticipantTypeId,
                            PersonId = default(int?)

                        };
            return query;
        }

        private static IQueryable<SimpleParticipantDTO> CreateGetUnionedSimpleParticipantsDTOQuery(EcaContext context)
        {
            return CreateGetOrganizationParticipantsQuery(context).Union(CreateGetPersonParticipantsQuery(context));
        }

        /// <summary>
        /// Creates a query to return all participants in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The filtered and sorted query to retrieve participants.</returns>
        public static IQueryable<SimpleParticipantDTO> CreateGetSimpleParticipantsDTOQuery(EcaContext context, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetUnionedSimpleParticipantsDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;
        }

        /// <summary>
        /// Creates a query to return all participants for the project with the given id in the context.
        /// </summary>
        /// <param name="context">The context to query.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The project id.</param>
        /// <returns>The filtered and sorted query to retrieve participants.</returns>
        public static IQueryable<SimpleParticipantDTO> CreateGetSimpleParticipantsDTOByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetUnionedSimpleParticipantsDTOQuery(context);

            query = from participant in context.Participants
                    join q in query
                    on participant.ParticipantId equals q.ParticipantId

                    from project in context.Projects
                    where project.ProjectId == projectId && participant.Projects.Contains(project)
                    select q;

            query = query.Apply(queryOperator);
            return query;
        }

        public static IQueryable<ParticipantDTO> CreateGetParticipantDTOByIdQuery(EcaContext context, int participantId)
        {
            Contract.Requires(context != null, "The context must not be null.");
            var query = from participant in context.Participants
                        let person = participant.Person
                        let org = participant.Organization
                        where participant.ParticipantId == participantId

                        select new ParticipantDTO
                        {
                            Name = participant.ParticipantId == ParticipantType.Individual.Id ?  ((person.FirstName != null ? person.FirstName : String.Empty)
                                                                                               + (person.FirstName != null && person.LastName != null ? " " : String.Empty)
                                                                                               + (person.LastName != null ? person.LastName : String.Empty)) :
                                                                                               org.Name
                        };
            return query;
        }
    }
}
