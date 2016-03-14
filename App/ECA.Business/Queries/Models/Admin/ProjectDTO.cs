using ECA.Business.Service.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECA.Business.Queries.Models.Admin
{
    /// <summary>
    /// A ProjectDTO represents a project to a business layer client and contains pertinent information about a single project.
    /// </summary>
    public class ProjectDTO
    {
        /// <summary>
        /// Creates a new default instance and initializes the ienumerable properties.
        /// </summary>
        public ProjectDTO()
        {
            this.Themes = new List<SimpleLookupDTO>();
            this.CountryIsosByLocations = new List<SimpleLookupDTO>();
            this.Goals = new List<SimpleLookupDTO>();
            this.Contacts = new List<SimpleLookupDTO>();
            this.Categories = new List<FocusCategoryDTO>();
            this.Objectives = new List<JustificationObjectiveDTO>();
            this.Locations = new List<LocationDTO>();
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Gets or sets the project status id.
        /// </summary>
        public int ProjectStatusId { get; set; }

        /// <summary>
        /// Gets or sets the program id.
        /// </summary>
        public int ProgramId { get; set; }

        /// <summary>
        /// Gets or sets the program name.
        /// </summary>
        public string ProgramName { get; set; }

        /// <summary>
        /// Gets or sets the owner organization id.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the owner name.
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the owner office symbol.
        /// </summary>
        public string OwnerOfficeSymbol { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the revised on date.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// The Visitor Type Id for this project
        /// </summary>
        public int VisitorTypeId { get; set; }

        /// <summary>
        /// THe Visitor Type Name (exchange, student or null)
        /// </summary>
        public string VisitorTypeName { get; set; }


        /// <summary>
        /// The number of US Participants, Esimated
        /// </summary>
        public int? UsParticipantsEst { get; set; }

        /// <summary>
        /// The number of Non-US Participants, Esimated
        /// </summary>
        public int? NonUsParticipantsEst { get; set; }

        /// <summary>
        /// The number of US Participants, Actual
        /// </summary>
        public int? UsParticipantsActual { get; set; }

        /// <summary>
        /// The number of Non-US Participants, Actual
        /// </summary>
        public int? NonUsParticipantsActual { get; set; }

        /// <summary>
        /// Gets or sets the themes.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Themes { get; set; }

        /// <summary>
        /// Gets or sets the country isos.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> CountryIsosByLocations { get; set; }

        /// <summary>
        /// Gets or sets the goals.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Goals { get; set; }

        /// <summary>
        /// Gets or sets the contacts.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> Contacts { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        public IEnumerable<FocusCategoryDTO> Categories { get; set; }

        /// <summary>
        /// Gets or sets the objectives.
        /// </summary>
        public IEnumerable<JustificationObjectiveDTO> Objectives { get; set; }

        /// <summary>
        /// Gets or sets the locations.
        /// </summary>
        public IEnumerable<LocationDTO> Locations { get; set; }

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        public IEnumerable<LocationDTO> Regions { get; set; }

        /// <summary>
        /// Gets or sets the country isos of the countries that are part of the project region.
        /// </summary>
        public IEnumerable<SimpleLookupDTO> CountryIsosByRegions { get; set; }
    }
}
