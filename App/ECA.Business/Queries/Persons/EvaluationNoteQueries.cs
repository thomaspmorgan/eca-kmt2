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
            return context.PersonEvaluationNotes.Select(x => new EvaluationNoteDTO
            {
                EvaluationNoteId = x.EvaluationNoteId,
                EvaluationNote = x.EvaluationNote,
                PersonId = x.PersonId
            });
        }

        public static IQueryable<EvaluationNoteDTO> CreateGetEvaluationNoteDTOByIdQuery(EcaContext context, int id)
        {
            Contract.Requires(context != null, "The context must not be null.");
            return CreateGetEvaluationNoteDTOQuery(context).Where(x => x.EvaluationNoteId == id);
        }
    }
}
