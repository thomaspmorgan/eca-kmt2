using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    /// <summary>
    /// A location is a specific place, city, administrative division, country or region somewhere on earth.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Creates a new instance of a Location.
        /// </summary>
        public Location()
        {
            this.BirthPlacePeople = new HashSet<Person>();
            this.RegionPrograms = new HashSet<Program>();
            this.LocationPrograms = new HashSet<Program>();
            this.TargetPrograms = new HashSet<Program>();
            this.RegionProjects = new HashSet<Project>();
            this.LocationProjects = new HashSet<Project>();
            this.TargetProjects = new HashSet<Project>();
            this.CitizensOfCountry = new HashSet<Person>();
            this.History = new History();
        }

        /// <summary>
        /// Gets or sets the Location Id.
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets the location type id.
        /// </summary>
        [Required]
        public int LocationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the location type.
        /// </summary>
        [Required]
        public virtual LocationType LocationType { get; set; }

        /// <summary>
        /// Gets or sets the Latitude.
        /// </summary>
        public float? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public float? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the Street 1 address information.
        /// </summary>
        public string Street1 { get; set; }

        /// <summary>
        /// Gets or sets the Street 2 address information.
        /// </summary>
        public string Street2 { get; set; }

        /// <summary>
        /// Gets or sets the Street 3 address information.
        /// </summary>
        public string Street3 { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the Iso.
        /// </summary>
        public string LocationIso { get; set; }

        /// <summary>
        /// Gets or sets the location iso 2
        /// </summary>
        public string LocationIso2 { get; set; }

        /// <summary>
        /// Gets or sets the region id.
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// Gets or sets the Region.  This is useful for example, when the location
        /// is a country and you need the region that country belongs to.
        /// </summary>
        public virtual Location Region { get; set; }

        /// <summary>
        /// Gets or sets the country id
        /// </summary>
        public int? CountryId { get; set; }

        /// <summary>
        /// Gets or sets the country
        /// </summary>
        public virtual Location Country { get; set; }

        /// <summary>
        /// Gets or sets the city id
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// Gets or sets the city
        /// </summary>
        public virtual Location City { get; set; }

        /// <summary>
        /// Gets or sets the division id
        /// </summary>
        public int? DivisionId { get; set; }

        /// <summary>
        /// Gets or sets the division
        /// </summary>
        public virtual Location Division { get; set; }

        /// <summary>
        /// Gets or sets the birth place people
        /// </summary>
        public virtual ICollection<Person> BirthPlacePeople { get; set; }

        /// <summary>
        /// Gets or sets the region programs.
        /// </summary>
        public virtual ICollection<Program> RegionPrograms { get; set; }

        /// <summary>
        /// Gets or sets the location programs.
        /// </summary>
        public virtual ICollection<Program> LocationPrograms { get; set; }

        /// <summary>
        /// Gets or sets the target programs.
        /// </summary>
        public virtual ICollection<Program> TargetPrograms { get; set; }

        /// <summary>
        /// Gets or sets the region projects.
        /// </summary>
        public virtual ICollection<Project> RegionProjects { get; set; }

        /// <summary>
        /// Gets or sets the location projects.
        /// </summary>
        public virtual ICollection<Project> LocationProjects { get; set; }

        /// <summary>
        /// Gets or sets the target projects.
        /// </summary>
        public virtual ICollection<Project> TargetProjects { get; set; }

        /// <summary>
        /// Gets or sets the citizens of this country.
        /// </summary>
        public virtual ICollection<Person> CitizensOfCountry { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }
    }
}
