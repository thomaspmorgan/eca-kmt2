using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ECA.Data
{
    public class Participant
    {
        /// <summary>
        /// Gets the max length of the SEVIS Id.
        /// </summary>
        private const int SEVIS_ID_MAX_LENGTH = 10;

        public Participant()
        {
            this.Projects = new HashSet<Project>();
            this.ItineraryStops = new HashSet<ItineraryStop>();
            this.SourceParticipantMoneyFlows = new HashSet<MoneyFlow>();
            this.RecipientParticipantMoneyFlows = new HashSet<MoneyFlow>();
            this.History = new History();
        }

        [Key]
        public int ParticipantId { get; set; }
        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public int? PersonId { get; set; }
        public Person Person { get; set; }
        [Required]
        public int ParticipantTypeId { get; set; }
        public ParticipantType ParticipantType { get; set; }
        [MaxLength(SEVIS_ID_MAX_LENGTH)]
        public string SevisId {get; set;}


        //Relationships
        public ICollection<Project> Projects { get; set; }
        public ICollection<ItineraryStop> ItineraryStops { get; set; }
        public ICollection<MoneyFlow> SourceParticipantMoneyFlows { get; set; }
        public ICollection<MoneyFlow> RecipientParticipantMoneyFlows { get; set; }

        public History History { get; set; }
    }
}
