using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// An OfficeDTO is used by a client to see an overview of an office.
    /// </summary>
    public class OfficeDTO
    {
        /// <summary>
        /// Creates a new instance and initializes the themes, goals, and contacts.
        /// </summary>
        public OfficeDTO()
        {
            this.Themes = new List<SimpleLookupDTO>();
            this.Goals = new List<SimpleLookupDTO>();
            this.Contacts = new List<SimpleLookupDTO>();
        }

        /// <summary>
        /// Gets or sets the Id of the office.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name of the office.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the date of last revision
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Themes.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Themes { get; set; }

        /// <summary>
        /// Gets or sets the Goals.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Goals { get; set; }

        /// <summary>
        /// Gets or sets the Contacts.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the Foci.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Foci { get; set; }
    }
}
