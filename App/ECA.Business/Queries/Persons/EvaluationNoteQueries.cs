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
            var query = from evaluationNote in context.PersonEvaluationNotes
                        join user in context.UserAccounts on evaluationNote.History.CreatedBy equals user.PrincipalId
                        join participant in context.Participants on user.PrincipalId equals participant.ParticipantId
                        select new EvaluationNoteDTO
                        {
                            EvaluationNoteId = evaluationNote.EvaluationNoteId,
                            EvaluationNote = evaluationNote.EvaluationNote,
                            AddedOn = evaluationNote.History.CreatedOn,
                            RevisedOn = evaluationNote.History.RevisedOn,
                            UserId = evaluationNote.History.CreatedBy,
                            UserName = user.DisplayName,
                            EmailAddress = user.EmailAddress,
                            OfficeSymbol = participant.Organization.OfficeSymbol
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
