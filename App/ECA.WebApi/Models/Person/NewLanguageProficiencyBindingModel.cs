using System.ComponentModel.DataAnnotations;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.WebApi.Models.Person
{
    /// <summary>
    /// Binding model for editing a language proficiency for a person
    /// </summary>
    public class NewPersonLanguageProficiencyBindingModel
    {
        /// <summary>
        /// Gets and sets the person id
        /// </summary>
        [Required]
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets and sets the personid for the language proficiency
        /// </summary>
        ///
        [Required]
        public int PersonId { get; set; }

        /// <summary>
        /// Get or sets the Native Language for the person
        /// </summary>
        public bool IsNativeLanguage { get; set; }

        /// <summary>
        /// Get or sets the Speaking Proficiency
        /// </summary>
        public int SpeakingProficiency { get; set; }

        /// <summary>
        /// Get or sets the Reading Proficiency
        /// </summary>
        public int ReadingProficiency { get; set; }

        /// <summary>
        /// Get or sets the Conprehension Proficiency
        /// </summary>
        public int ComprehensionProficiency { get; set; }

        /// <summary>
        /// Convert binding model to business model 
        /// </summary>
        /// <param name="user">The user creating the language proficiency</param>
        /// <returns>Create language proficiency business model</returns>
        public NewPersonLanguageProficiency ToNewPersonLanguageProficiency(User user)
        {
            return new NewPersonLanguageProficiency(
                user: user,
                languageId: this.LanguageId,
                personId: this.PersonId,
                isNativeLanguage: this.IsNativeLanguage,
                readingProficiency: this.ReadingProficiency,
                speakingProficiency: this.SpeakingProficiency,
                comprehensionProficiency: this.ComprehensionProficiency
                );
        }
    }
}