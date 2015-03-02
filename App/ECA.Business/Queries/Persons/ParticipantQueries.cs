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
        public static IQueryable<SimpleParticipantDTO> CreateGetSimpleParticipantsDTOQuery(EcaContext context, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(queryOperator != null, "The query operator must not be null.");
            var query = from participant in context.Participants

                        join participantType in context.ParticipantTypes
                        on participant.ParticipantType equals participantType

                        join organization in context.Organizations
                        on participant.Organization equals organization into tempOrgs
                        from org in tempOrgs.DefaultIfEmpty()

                        join person in context.People
                        on participant.Person equals person into tempPerson
                        from tp in tempPerson.DefaultIfEmpty()

                        //if this get re-enabled remember that gender doesn't work unless you do new Person() in the DefaultIfEmpty person join because of
                        //the linq to objects not supporting it.
                        //join gender in context.Genders
                        //on tp.Gender equals gender into tempGender
                        //from tg in tempGender.DefaultIfEmpty()

                        select new SimpleParticipantDTO
                        {
                            //Gender = tg != null ? tg.GenderName : null,
                            //GenderId = tg != null ? tg.GenderId : default(int?),
                            Name = tp != null ? "Person Name Here" 
                                : org != null ? org.Name : "UNKNOWN PARTICIPANT NAME",
                            OrganizationId = org != null ? org.OrganizationId : default(int?),
                            ParticipantId = participant.ParticipantId,
                            ParticipantType = participantType.Name,
                            ParticipantTypeId = participantType.ParticipantTypeId,
                            PersonId = tp != null ? tp.PersonId : default(int?)

                        };
            query = query.Apply(queryOperator);
            return query;

        }

        public static IQueryable<SimpleParticipantDTO> CreateGetSimpleParticipantsDTOByProjectIdQuery(EcaContext context, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            throw new NotImplementedException();
        }
    }
}
