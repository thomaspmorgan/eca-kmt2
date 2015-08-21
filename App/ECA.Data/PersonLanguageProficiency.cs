using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// Entity that stores a Person's proficiency in a language
    /// </summary>
    public class PersonLanguageProficiency: IHistorical
    {
        /// <summary>
        /// Gets or sets the Id of the Language.
        /// </summary>
        [Key, Column(Order = 0)]
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the Person.
        /// </summary>
        [Key, Column(Order = 1)]
        public int PersonId { get; set; }

        /// <summary>
        /// Is this language the person's native language
        /// </summary>
        [Required]
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
        /// Get or sets the Comprehension Proficiency
        /// </summary>
        public int ComprehensionProficiency { get; set; }

        /// <summary>
        /// Language for this proficiency
        /// </summary>
        public Language Language { get; set;}

        /// <summary>
        /// Language for this proficiency
        /// </summary>
        public Person Person { get; set;}

        public History History { get; set; }
    }
}
