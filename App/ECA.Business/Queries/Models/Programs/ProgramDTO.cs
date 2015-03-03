using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Programs
{
    public class ProgramDTO
    {
        /// <summary>
        /// Creates a new default instance and initializes IEnumerable properties.
        /// </summary>
        public ProgramDTO()
        {
            this.Contacts = new List<SimpleLookupDTO>();
            this.CountryIsos = new List<SimpleLookupDTO>();
            this.RegionIsos = new List<SimpleLookupDTO>();
            this.Goals = new List<SimpleLookupDTO>();
            this.Themes = new List<SimpleLookupDTO>();
        }

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Start Date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the Revised On date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the Parent Program Id.
        /// </summary>
        public int? ParentProgramId { get; set; }

        /// <summary>
        /// Gets or sets the Theme Ids.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Themes { get; set; }

        /// <summary>
        /// Gets or sets the Country Ids.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> CountryIsos { get; set; }

        /// <summary>
        /// Gets or sets the Country Ids.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> RegionIsos { get; set; }

        /// <summary>
        /// Gets or sets the Goal Ids.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Goals { get; set; }

        /// <summary>
        /// Gets or sets the Contact Ids.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the Owner Name.
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets the owner description.
        /// </summary>
        public string OwnerDescription { get; set; }

        /// <summary>
        /// Gets or sets the owner organization id.
        /// </summary>
        public int OwnerOrganizationId { get; set; }
    }
}
