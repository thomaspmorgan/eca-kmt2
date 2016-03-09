using ECA.Business.Validation.Sevis.Bio;

namespace ECA.Business.Queries.Models.Persons
{
    public class FullNameDTO
    {
        /// <summary>
        /// Person last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Person first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Person passport name.
        /// </summary>
        public string PassportName { get; set; }

        /// <summary>
        /// Person preferred name.
        /// </summary>
        public string PreferredName { get; set; }

        /// <summary>
        /// Person name suffix.
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// Returns the sevis full name instance.
        /// </summary>
        /// <returns>The sevis full name instance.</returns>
        public FullName GetFullName()
        {
            return new FullName
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                PassportName = this.PassportName,
                PreferredName = this.PreferredName,
                Suffix = this.Suffix
            };
        }
    }
}
