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
            //Contract.Requires(context != null, "The context must not be null.");
            //Contract.Requires(queryOperator != null, "The query operator must not be null.");
            //var query = from participant in context.Participants

            //            let person = participant.Person
            //            let gender = person.Gender
            //            let names = person.Names
            //            let firstNames = names.Where(x => x.NameTypeId == NameType.Firstname.Id)
            //            let lastName = names.Where(x => x.NameTypeId == NameType.Lastname.Id)

            //            select new SimpleParticipantDTO
            //            {
            //                Gender = gender.GenderName,
            //                ParticipantId = participant.ParticipantId,
            //                PersonId = person.PersonId,
            //                //FirstName = firstName.
            //            };
            ////query
            throw new NotImplementedException();

        }
    }
}
