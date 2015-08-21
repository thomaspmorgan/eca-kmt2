using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing language proficiencies for a person
    /// </summary>
    public class UpdatedPersonLanguageProficiencyBindingModel
    {
        /// <summary>
        /// Gets and sets the language id
        /// </summary>
        [Required]
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets and sets the person id
        /// </summary>
        [Required]
        public int PersonId { get; set; }

        /// <summary>
        /// Gets and sets the membership for the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or sets the Native Language for the person
        /// </summary>
        public bool IsNativeLanguage { get; set; }

        /// <summary>
        /// Get or sets the Speaking Proficiency
        /// </summary>
        public int SpeakingProficiency { get; set; }

        /// <summary>
        /// Get or sets the Speaking Proficiency
        /// </summary>
        public int ReadingProficiency { get; set; }

        /// <summary>
        /// Get or sets the Speaking Proficiency
        /// </summary>
        public int ComprehensionProficiency { get; set; }

        /// <summary>
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user updating the membership</param>
        /// <returns>Update LanguageProficiency business model</returns>
        public UpdatedPersonLanguageProficiency ToUpdatedPersonLanguageProficiency(User user)
        {
            return new UpdatedPersonLanguageProficiency(
                updator: user,
                languageId: this.LanguageId,
                personId: this.PersonId,
                isNativeLanguage: this.IsNativeLanguage,
                speakingProficiency: this.SpeakingProficiency,
                readingProficiency: this.ReadingProficiency,
                comprehensionProficiency: this.ComprehensionProficiency
                );
        }
    }
}