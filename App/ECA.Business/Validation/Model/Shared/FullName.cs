using ECA.Business.Validation.Model.Shared;
using FluentValidation.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ECA.Business.Validation.Model
{
    [Validator(typeof(FullNameValidator))]
    public class FullName
    {
        /// <summary>
        /// Person first name.
        /// </summary>
        public string FirsName { get; set; }

        /// <summary>
        /// Person last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Person name suffix.
        /// </summary>
        public string NameSuffix { get; set; }

        /// <summary>
        /// Person passport name.
        /// </summary>
        public string PassportName { get; set; }

        /// <summary>
        /// Person preferred name.
        /// </summary>
        public string PreferredName { get; set; }
    }
}
