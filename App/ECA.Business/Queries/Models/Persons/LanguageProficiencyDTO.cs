using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// A LanguageProficiencyDTO is used to represent a LanguageProficiency entity in the ECA System.
    /// </summary>
    public class LanguageProficiencyDTO
    {
        /// <summary>
        /// The max length of the language name.
        /// </summary>
        public const int NAME_MAX_LENGTH = 100;

        /// <summary>
        /// Gets or sets the language Id.
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the PersonLanguageProficiency.
        /// </summary>
        public int PersonId { get; set; }
        
        /// <summary>
        /// Gets or sets the language name.
        /// </summary>
        ///
        [MaxLength(NAME_MAX_LENGTH)]
        public string LanguageName { get; set; }

        /// <summary>
        /// Get or sets the Native Languange for the person
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
    }
}
