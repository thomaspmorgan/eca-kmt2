using ECA.Business.Queries.Models.Persons;
using ECA.Data;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ECA.Business.Queries.Persons
{
    public static class EvaluationNoteQueries
    {
        public static IQueryable<EvaluationNoteDTO> CreateGetEvaluationNoteDTOQuery(EcaContext context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            //return context.PersonEvaluationNotes.Select(x => new EvaluationNoteDTO
            //{
            //    EvaluationNoteId = x.EvaluationNoteId,
            //    EvaluationNote = x.EvaluationNote,
            //    PersonId = x.PersonId,
            //    UserName = x.Person.Patronym,
            //    AddedOn = x.History.CreatedOn
            //});
            var query = from evaluationNote in context.PersonEvaluationNotes
                        join user in context.UserAccounts on evaluationNote.History.CreatedBy equals user.PrincipalId
                        join participant in context.Participants on user.PrincipalId equals participant.ParticipantId
                        orderby evaluationNote.History.CreatedOn descending
                        select new EvaluationNoteDTO
                        {
                            EvaluationNoteId = evaluationNote.EvaluationNoteId,
                            EvaluationNote = evaluationNote.EvaluationNote,
                            AddedOn = evaluationNote.History.CreatedOn,
                            UserName = user.DisplayName
                        };
            return query;
        }

        public static IQueryable<EvaluationNoteDTO> CreateGetEvaluationNoteDTOByIdQuery(EcaContext context, int id)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetEvaluationNoteDTOQuery(context).Where(x => x.EvaluationNoteId == id);
        }
    }
}
