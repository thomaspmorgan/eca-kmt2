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
    public static class ParticipantQueries
    {

        private static IQueryable<SimpleParticipantDTO> CreateGetPersonParticipantsQuery(EcaContext context)
        {
            var query = from participant in context.Participants

                        join participantType in context.ParticipantTypes
                        on participant.ParticipantType equals participantType

                        let person = participant.Person
                        where person != null

                        //if this get re-enabled remember that gender doesn't work unless you do new Person() in the DefaultIfEmpty person join because of
                        //the linq to objects not supporting it.
                        //join gender in context.Genders
                        //on tp.Gender equals gender into tempGender
                        //from tg in tempGender.DefaultIfEmpty()

                        select new SimpleParticipantDTO
                        {
                            //Gender = tg != null ? tg.GenderName : null,
                            //GenderId = tg != null ? tg.GenderId : default(int?),
                            //Name = tp != null ? "Person Name Here" : "INVALID PERSON NAME",
                            Name = "person name",
                            OrganizationId = default(int?),
                            ParticipantId = participant.ParticipantId,
                            ParticipantType = participantType.Name,
                            ParticipantTypeId = participantType.ParticipantTypeId,
                            //PersonId = tp != null ? tp.PersonId : default(int?)
                            PersonId = person.PersonId

                        };
            return query;
        }

        private static IQueryable<SimpleParticipantDTO> CreateGetOrganizationParticipantsQuery(EcaContext context)
        {
            var query = from participant in context.Participants

                        join participantType in context.ParticipantTypes
                        on participant.ParticipantType equals participantType

                        let org = participant.Organization
                        where org != null

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


        public static IQueryable<SimpleParticipantDTO> CreateGetSimpleParticipantsDTOQuery(EcaContext context, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetUnionedSimpleParticipantsDTOQuery(context);
            query = query.Apply(queryOperator);
            return query;

        }

        public static IQueryable<SimpleParticipantDTO> CreateGetSimpleParticipantsDTOByProjectIdQuery(EcaContext context, int projectId, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = CreateGetUnionedSimpleParticipantsDTOQuery(context);
            

            query = from participant in context.Participants
                                      join q in query
                                      on participant.ParticipantId equals q.ParticipantId

                                      where participant.Projects.Select(x => x.ProjectId).Contains(projectId)
                                      select q;
            query = query.Apply(queryOperator);
            return query;
        }
    }
}
