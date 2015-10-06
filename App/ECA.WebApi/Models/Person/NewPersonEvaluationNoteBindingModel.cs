using System;
using System.ComponentModel.DataAnnotations;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing evaluation note
    /// </summary>
    public class NewPersonEvaluationNoteBindingModel
    {
        /// <summary>
        /// Gets and sets the person id
        /// </summary>
        [Required]
        public int PersonId { get; set; }

        /// <summary>
        /// Gets and sets the evaluation note for the user
        /// </summary>
        public string EvaluationNote { get; set; }
        
        /// <summary>
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user creating the membership</param>
        /// <returns>Create evaluation note business model</returns>
        public NewPersonEvaluationNote ToPersonEvaluationNote(User user)
        {
            return new NewPersonEvaluationNote(
                user: user,
                personId: this.PersonId,
                evaluationNote: this.EvaluationNote
                );
        }
    }
}