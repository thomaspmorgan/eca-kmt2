using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public partial class Gender
    {

        /// <summary>
        /// The sevis male gender code value.
        /// </summary>
        public const string SEVIS_MALE_GENDER_CODE_VALUE = "M";

        /// <summary>
        /// The sevis female gender code.
        /// </summary>
        public const string SEVIS_FEMALE_GENDER_CODE_VALUE = "F";


        [Key]
        public int GenderId { get; set; }
        [Required]
        [MaxLength(20)]
        public string GenderName { get; set; }
        public History History { get; set; }

        /// <summary>
        /// Gets or sets the sevis gender code.
        /// </summary>
        public string SevisGenderCode { get; set; }
    }
}
