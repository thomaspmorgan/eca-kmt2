
namespace ECA.Business.Queries.Models.Programs
{
    /// <summary>
    /// A SimpleProgramDTO is used to represent programs within the ECA system.
    /// </summary>
    public class SimpleProgramDTO
    {
        /// <summary>
        /// Gets or sets the Program Id.
        /// </summary>
        public int ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Owner Id.
        /// </summary>
        public int OwnerId { get; set; }

        public int ProgramStatusId { get; set; }
    }
}
