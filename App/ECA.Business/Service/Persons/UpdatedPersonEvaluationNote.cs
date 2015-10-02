using System;
using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Persons
{
    public class UpdatedPersonEvaluationNote
    {
        public UpdatedPersonEvaluationNote(User updator, int evaluationNoteId, string evaluationNote, string userName, DateTimeOffset revisedOn)
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            Update = new Update(updator);
            EvaluationNoteId = evaluationNoteId;
            EvaluationNote = evaluationNote;
            UserName = userName;
            RevisedOn = revisedOn;
        }

        /// <summary>
        /// Gets the update audit.
        /// </summary>
        public Update Update { get; private set; }

        /// <summary>
        /// Gets the EvaluationNoteId.
        /// </summary>
        public int EvaluationNoteId { get; private set; }

        /// <summary>
        /// Gets the Evaluation Note value.
        /// </summary>
        public string EvaluationNote { get; private set; }

        public string UserName { get; set; }
        
        public DateTimeOffset RevisedOn { get; set; }

    }
}
