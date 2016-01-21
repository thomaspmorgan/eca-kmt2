using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECA.Data
{
    public class Participant : IHistorical
    {
        public Participant()
        {
            this.ItineraryStops = new HashSet<ItineraryStop>();
            this.SourceParticipantMoneyFlows = new HashSet<MoneyFlow>();
            this.RecipientParticipantMoneyFlows = new HashSet<MoneyFlow>();
            this.Itineraries = new HashSet<Itinerary>();
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

        public ParticipantStatus Status { get; set; }
        public int? ParticipantStatusId { get; set; }

        public DateTimeOffset? StatusDate { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        //Relationships
        public ICollection<ItineraryStop> ItineraryStops { get; set; }
        public ICollection<MoneyFlow> SourceParticipantMoneyFlows { get; set; }
        public ICollection<MoneyFlow> RecipientParticipantMoneyFlows { get; set; }

        /// <summary>
        /// Gets or sets the itineraries of this participant.
        /// </summary>
        public virtual ICollection<Itinerary> Itineraries { get; set; }
        
        [ForeignKey("ParticipantId")]
        /// <summary>
        /// reference to the ParticipantPerson record for this Participant if a person
        /// </summary>
        public ParticipantPerson ParticipantPerson { get; set; }

        /// <summary>
        /// reference to the ParticipantExchangeVisitor record for this Participant if person is an ExchangeVisitor
        /// </summary>
        [ForeignKey("ParticipantId")]
        public ParticipantExchangeVisitor ParticipantExchangeVisitor { get; set; }

        /// <summary>
        /// History data for this participant
        /// </summary>
        public History History { get; set; }
    }
}
