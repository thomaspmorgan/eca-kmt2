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
        [Key]
        public int LocationId { get; set; }
        [Required]
        public int LocationTypeId { get; set; }
        [Required]
        public LocationType LocationType { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Street3 { get; set; }
        public string City { get; set; }
        public string Division { get; set; }
        public string PostalCode { get; set; }
        public string LocationName { get; set; }
        public string LocationIso { get; set; }
        public Location Region { get; set; }
        public Location Country { get; set; }

        // Relationships
        [ForeignKey("PersonId")]
        public ICollection<Person> BirthPlacePeople { get; set; }
        public ICollection<Program> RegionPrograms { get; set; }
        public ICollection<Program> LocationPrograms { get; set; }
        public ICollection<Program> TargetPrograms { get; set; }
        public ICollection<Project> RegionProjects { get; set; }
        public ICollection<Project> LocationProjects { get; set; }
        public ICollection<Project> TargetProjects { get; set; }
        public ICollection<Person> CitizensOfCountry { get; set; }

        public History History { get; set; }
    }

}
