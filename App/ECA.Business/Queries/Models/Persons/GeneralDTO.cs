using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Lookup;


namespace ECA.Business.Queries.Models.Persons
{
    /// <summary>
    /// A SimplePersonGeneralDTO is used to represent a person/participant's general info in the ECA system.
    /// </summary>
    public class GeneralDTO
    {

        public GeneralDTO ()
        {
            ProminentCategories = new List<SimpleLookupDTO>();
            Events = new List<SimpleLookupDTO>();
            Memberships = new List<SimpleOrganizationDTO>();
            LanguageProficiencies = new List<SimpleLookupDTO>();
            Dependants = new List<SimpleLookupDTO>();
            RelatedReports = new List<SimpleLookupDTO>();
            ImpactStories = new List<SimpleLookupDTO>();
        }
        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the Person Id.
        /// </summary>
        public int? PersonId { get; set; }

        /// <summary>
        /// Gets or sets the participant id.
        /// </summary>
        public int ParticipantTypeId { get; set; }

        /// <summary>E:\S\ECA-KMT\App\ECA.Business\Queries\Models\Persons\GeneralDTO.cs
        /// Gets or sets the participant type.
        /// </summary>
        public string ParticipantType { get; set; }

        /// <summary>
        /// Gets or sets the name of the participant.
        /// </summary>
        public string Status { get; set; }

        public DateTimeOffset StatusDate { get; set;}
        
        public SimpleOrganizationDTO ParticipantOrigination {get; set;}

        public ICollection<SimpleLookupDTO> ProminentCategories {get; set;}
        
        public ICollection<SimpleLookupDTO> Events {get; set;}

        public ICollection<SimpleOrganizationDTO> Memberships { get; set; }

        public ICollection<SimpleLookupDTO> LanguageProficiencies { get; set; }

        public ICollection<SimpleLookupDTO> Dependants { get; set; }

        public ICollection<SimpleLookupDTO> RelatedReports { get; set; }

        public ICollection<SimpleLookupDTO> ImpactStories { get; set; }

        /// <summary>
        /// Gets or sets the date revised on.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }
    }
}

