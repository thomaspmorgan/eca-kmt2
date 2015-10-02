using System;
using System.ComponentModel.DataAnnotations;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing Evaluation Note
    /// </summary>
    public class UpdatedPersonEvaluationNoteBindingModel
    {
        /// <summary>
        /// Gets and sets the Evaluation Note id
        /// </summary>
        [Required]
        public int EvaluationNoteId { get; set; }

        /// <summary>
        /// Gets and sets the Evaluation Note for the user
        /// </summary>
        public string EvaluationNote { get; set; }

        public string UserName { get; set; }
        
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user updating the Evaluation Note</param>
        /// <returns>Updated Evaluation Note business model</returns>
        public UpdatedPersonEvaluationNote ToUpdatedPersonEvaluationNote(User user)
        {
            return new UpdatedPersonEvaluationNote(
                updator: user,
                evaluationNoteId: this.EvaluationNoteId,
                evaluationNote: this.EvaluationNote,
                userName:this.UserName,
                revisedOn:this.RevisedOn
                );
        }
    }
}