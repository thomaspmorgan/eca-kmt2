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

    public class Language: IHistorical
    {
        /// <summary>
        /// The max length of the language name.
        /// </summary>
        public const int NAME_MAX_LENGTH = 100;

        /// <summary>
        /// The Id of the Language
        /// </summary>
        [Key]
        public int LanguageId { get; set; }

        /// <summary>
        /// The name of the language
        /// </summary>
        [Required]
        [MaxLength(NAME_MAX_LENGTH)]
        public string LanguageName { get; set; }

        /// <summary>
        /// THe language proficiencies for this language
        /// </summary>
        public ICollection<PersonLanguageProficiency> LanguageProficiencies { get; set; }

        public History History { get; set; }
    }
}
