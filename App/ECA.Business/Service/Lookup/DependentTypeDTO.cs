
namespace ECA.Business.Service.Lookup
{
    /// <summary>
    /// The PersonTypeDTO is used to detail person types to a business layer client.
    /// </summary>
    public class DependentTypeDTO
    {
        /// <summary>
        /// Gets or sets the person type id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the person type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sevis dependent type code i.e. child = 02, spouse = 01.
        /// </summary>
        public string SevisDependentTypeCode { get; set; }
    }
}
