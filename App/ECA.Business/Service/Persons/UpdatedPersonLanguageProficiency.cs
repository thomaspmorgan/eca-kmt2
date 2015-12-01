using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace ECA.Business.Service.Persons
{
    /// <summary>
    /// Allows a business layer client to add a LanguageProficiency to a person.
    /// </summary>
    public class UpdatedPersonLanguageProficiency
    {
        /// <summary>
        /// Updates a Language Proficiency for a person
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newLanguageId">The Id of the new Language Proficiency</param>
        /// <param name="personId"></param>
        /// <param name="languageId"></param>
        /// <param name="nativeLanguageInd"></param>
        /// <param name="speakingProficiency"></param>
        /// <param name="readingProficiency"></param>
        /// <param name="comprehensionProficiency"></param>
        public UpdatedPersonLanguageProficiency(User updator, int? newLanguageId, int personId, int languageId, bool isNativeLanguage, 
            int speakingProficiency, int readingProficiency, int comprehensionProficiency )
        {
            Contract.Requires(updator != null, "The updator must not be null.");
            this.NewLanguageId = newLanguageId;
            this.PersonId = personId;
            this.LanguageId = languageId;
            this.IsNativeLanguage = isNativeLanguage;
            this.SpeakingProficiency = speakingProficiency;
            this.ReadingProficiency = readingProficiency;
            this.ComprehensionProficiency = comprehensionProficiency;
            this.Update = new Update(updator);
        }

        /// <summary>
        /// The Id of the language proficiency
        /// </summary>
        public int? NewLanguageId { get; set; }

        /// <summary>
        /// Gets/sets the person id.
        /// </summary>
        public int PersonId { get; private set; }

        /// <summary>
        /// Gets or sets the language Id.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Get or sets the Native Languange indicator
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
        /// Gets/sets the update audit info.
        /// </summary>
        public Update Update { get; private set; }

    }
}