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
            Activities = new List<SimpleLookupDTO>();
            Memberships = new List<MembershipDTO>();
            LanguageProficiencies = new List<LanguageProficiencyDTO>();
            Dependants = new List<SimpleLookupDTO>();
            RelatedReports = new List<SimpleLookupDTO>();
            ImpactStories = new List<SimpleLookupDTO>();
        }

        /// <summary>
        /// Gets or sets the Person Id.
        /// </summary>
        public int? PersonId { get; set; }

        public DateTimeOffset StatusDate { get; set;}
        
        public IEnumerable<SimpleLookupDTO> ProminentCategories {get; set;}

        public IEnumerable<SimpleLookupDTO> Activities { get; set; }

        public IEnumerable<MembershipDTO> Memberships { get; set; }

        public IEnumerable<LanguageProficiencyDTO> LanguageProficiencies { get; set; }

        public IEnumerable<SimpleLookupDTO> Dependants { get; set; }

        public IEnumerable<SimpleLookupDTO> RelatedReports { get; set; }

        public IEnumerable<SimpleLookupDTO> ImpactStories { get; set; }

        public string CurrentStatus { get; set; }

        /// <summary>
        /// Gets or sets the date revised on.
        /// </summary>
        public DateTimeOffset RevisedOn { get; set; }

        /// <summary>
        /// Gets or sets the participant's sevis id
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Sevis Status (person participants only)
        /// </summary>
        public string SevisStatus { get; set; }
    }
}

