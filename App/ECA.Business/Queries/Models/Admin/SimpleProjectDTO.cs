using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// A SimpleProjectDTO is used when a client requests projects by program.
    /// </summary>
    public class SimpleProjectDTO
    {
        /// <summary>
        /// Creates a new simple project dto and initializes the location names.
        /// </summary>
        public SimpleProjectDTO()
        {
            this.LocationNames = new List<string>();
        }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the project name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the project status id.
        /// </summary>
        public int ProjectStatusId { get; set; }

        /// <summary>
        /// Gets or sets the project status name.
        /// </summary>
        public string ProjectStatusName { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the start year.
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        /// Gets or sets the location names.
        /// </summary>
        public IEnumerable<string> LocationNames { get; set; }
    }
}
