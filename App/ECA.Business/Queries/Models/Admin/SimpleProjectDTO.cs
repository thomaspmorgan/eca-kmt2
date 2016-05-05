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
            this.CountryIds = new List<int>();
            this.CountryNames = new List<string>();
            this.RegionIds = new List<int>();
            this.RegionNames = new List<string>();
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
        /// Gets or sets the name of the program.
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// Gets or sets the project status id.
        /// </summary>
        public int ProjectStatusId { get; set; }

        /// <summary>
        /// Gets or sets the project status name.
        /// </summary>
        public string ProjectStatusName { get; set; }

        /// <summary>
        /// Gets or sets the name of the sevis region id.
        /// </summary>
        public string SevisOrgId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the start year.
        /// </summary>
        public int StartYear { get; set; }

        /// <summary>
        /// Gets or sets the start year.
        /// </summary>
        public string StartYearAsString { get; set; }

        /// <summary>
        /// Gets or sets the countries the project is operating in.
        /// </summary>
        public IEnumerable<string> CountryNames { get; set; }

        /// <summary>
        /// Gets or sets the countries by id the project is operating in.
        /// </summary>
        public IEnumerable<int> CountryIds { get; set; }

        /// <summary>
        /// Gets or sets the regions the project is operating in.
        /// </summary>
        public IEnumerable<string> RegionNames { get; set; }

        /// <summary>
        /// Gets or sets the regions by id the project is operating in.
        /// </summary>
        public IEnumerable<int> RegionIds { get; set; }

        /// <summary>
        /// Gets or set the owner id
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the owner office symbol
        /// </summary>
        public string OwnerOfficeSymbol { get; set; }

        /// <summary>
        /// The visitor type for this project (exchange, student, null)
        /// </summary>
        public string VisitorTypeName { get; set; }

        /// <summary>
        /// The visitor type id
        /// </summary>
        public int VisitorTypeId { get; set; }
    }
}
