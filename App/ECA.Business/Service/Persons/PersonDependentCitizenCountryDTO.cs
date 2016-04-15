
namespace ECA.Business.Service.Persons
{
    public class PersonDependentCitizenCountryDTO
    {
        /// <summary>
        /// Gets or sets the Location Id.
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets the birth country reason name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the is primary flag.
        /// </summary>
        public bool IsPrimary { get; set; }        
    }
}
