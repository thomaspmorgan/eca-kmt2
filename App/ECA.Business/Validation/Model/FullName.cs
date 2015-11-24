using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    public class FullName
    {
        /// <summary>
        /// Gets the max length of the first name.
        /// </summary>
        public const int FIRST_NAME_MAX_LENGTH = 80;

        /// <summary>
        /// Gets the max length of a the last name.
        /// </summary>
        public const int LAST_NAME_MAX_LENGTH = 40;

        /// <summary>
        /// Gets max length of the name suffix.
        /// </summary>
        public const int NAME_SUFFIX_MAX_LENGTH = 3;

        /// <summary>
        /// Gets the max length of the passport name.
        /// </summary>
        public const int PASSPORT_NAME_MAX_LENGTH = 39;

        /// <summary>
        /// Gets the max length of the preferred name.
        /// </summary>
        public const int PREFERRED_NAME_MAX_LENGTH = 145;


        /// <summary>
        /// Person first name.
        /// </summary>
        [MaxLength(FIRST_NAME_MAX_LENGTH)]
        [Required(ErrorMessage = "First name is required")]
        public string FirsName { get; set; }

        /// <summary>
        /// Person last name.
        /// </summary>
        [MaxLength(LAST_NAME_MAX_LENGTH)]
        public string LastName { get; set; }

        /// <summary>
        /// Person name suffix.
        /// </summary>
        [MaxLength(NAME_SUFFIX_MAX_LENGTH)]
        public string NameSuffix { get; set; }

        /// <summary>
        /// Person passport name.
        /// </summary>
        [MaxLength(PASSPORT_NAME_MAX_LENGTH)]
        public string PassportName { get; set; }

        /// <summary>
        /// Person preferred name.
        /// </summary>
        [MaxLength(PREFERRED_NAME_MAX_LENGTH)]
        public string PreferredName { get; set; }



    }
}
