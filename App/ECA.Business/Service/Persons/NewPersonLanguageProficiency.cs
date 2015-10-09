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
    public class NewPersonLanguageProficiency
    {
        /// <summary>
        /// Creates a new Language Proficiency for a person
        /// </summary>
        /// <param name="user"></param>
        /// <param name="personId"></param>
        /// <param name="languageId"></param>
        /// <param name="isNativeLanguage"></param>
        /// <param name="speakingProficiency"></param>
        /// <param name="readingProficiency"></param>
        /// <param name="comprehensionProficiency"></param>
        public NewPersonLanguageProficiency(User user, int personId, int languageId, bool isNativeLanguage, 
            int speakingProficiency, int readingProficiency, int comprehensionProficiency )
        {
            this.PersonId = personId;
            this.LanguageId = languageId;
            this.IsNativeLanguage = isNativeLanguage;
            this.SpeakingProficiency = speakingProficiency;
            this.ReadingProficiency = readingProficiency;
            this.ComprehensionProficiency = comprehensionProficiency;
            this.Create = new Create(user);
        }

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
        /// Gets/sets the create audit info.
        /// </summary>
        public Create Create { get; private set; }

        /// <summary>
        /// Adds the given PersonLanguageProficiency to a person
        /// </summary>
        /// <param name="person">The person entity.</param>
        /// <returns>The LanguageProficiency that should be added to the context.</returns>
        public PersonLanguageProficiency AddPersonLanguageProficiency(Person person)
        {
            Contract.Requires(person != null, "The person entity must not be null.");
            var languageProficiency = new ECA.Data.PersonLanguageProficiency
            {
                PersonId = person.PersonId,
                LanguageId = LanguageId,
                IsNativeLanguage = IsNativeLanguage,
                SpeakingProficiency = SpeakingProficiency,
                ReadingProficiency = ReadingProficiency,
                ComprehensionProficiency = ComprehensionProficiency
            };
            this.Create.SetHistory(languageProficiency);
            person.LanguageProficiencies.Add(languageProficiency);
            return languageProficiency;
        }
    }
}