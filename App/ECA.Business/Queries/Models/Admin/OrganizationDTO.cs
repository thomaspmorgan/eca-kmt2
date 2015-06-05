using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// An OrganizationDTO represents a generic organization within the ECA system.
    /// </summary>
    public class OrganizationDTO
    {
        /// <summary>
        /// Creates and initializes a new organization dto.
        /// </summary>
        public OrganizationDTO()
        {
            this.Contacts = new List<SimpleLookupDTO>();
            this.SocialMedias = new List<SimpleTypeLookupDTO>();
            this.Locations = new List<ECA.Business.Queries.Models.Persons.LocationDTO>();
        }

        /// <summary>
        /// Gets or sets the organization id.
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the organization's website.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        public IEnumerable<ECA.Business.Queries.Models.Persons.LocationDTO> Locations { get; set; }

        /// <summary>
        /// Gets or sets the social medias.
        /// </summary>
        public IEnumerable<SimpleTypeLookupDTO> SocialMedias { get; set; }

        /// <summary>
        /// Gets or sets the last revised on date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the parent organization id.
        /// </summary>
        public int? ParentOrganizationId { get; set; }

        /// <summary>
        /// Gets or sets the parent organization name.
        /// </summary>
        public string ParentOrganizationName { get; set; }

        /// <summary>
        /// Gets or sets the organization type.
        /// </summary>
        public string OrganizationType { get; set; }

        /// <summary>
        /// Gets or sets the organization type id.
        /// </summary>
        public int OrganizationTypeId { get; set; }
    }
}
