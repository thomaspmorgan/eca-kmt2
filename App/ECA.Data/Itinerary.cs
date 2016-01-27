using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace ECA.Data
{
    /// <summary>
    /// Itineraries describe the travel participants take during a project.
    /// </summary>
    public class Itinerary : IHistorical, IValidatableObject
    {
        /// <summary>
        /// The max length of the description.
        /// </summary>
        public const int NAME_MAX_LENGTH = 100;

        /// <summary>
        /// Creates a new default instance and initializes the history.
        /// </summary>
        public Itinerary()
        {
            this.History = new History();
            this.Stops = new HashSet<ItineraryStop>();
            this.Participants = new HashSet<Participant>();
        }

        /// <summary>
        /// Gets or sets the itinerary id.
        /// </summary>
        [Key]
        public int ItineraryId { get; set; }

        /// <summary>
        /// Gets or sets the itinerary status.
        /// </summary>
        public virtual ItineraryStatus ItineraryStatus { get; set; }

        /// <summary>
        /// Gets or sets the itinerary status id.
        /// </summary>
        [Required]
        public int ItineraryStatusId { get; set; }

        /// <summary>
        /// Gets or sets the intinerary stops.
        /// </summary>
        public virtual ICollection<ItineraryStop> Stops { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        public History History { get; set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the arrival destination location id of this itinerary.
        /// </summary>
        [Column("Arrival_LocationId")]
        public int? ArrivalLocationId { get; set; }

        /// <summary>
        /// Gets or sets the arrival destination of this itinerary.
        /// </summary>
        [ForeignKey("ArrivalLocationId")]
        public virtual Location Arrival { get; set; }

        /// <summary>
        /// Gets or sets the departure destination location id of this itinerary.
        /// </summary>
        [Column("Departure_LocationId")]
        public int? DepartureLocationId { get; set; }

        /// <summary>
        /// Gets or sets the departure destination of this itinerary.
        /// </summary>
        [ForeignKey("DepartureLocationId")]
        public virtual Location Departure { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        [MaxLength(NAME_MAX_LENGTH)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets all participants that are on this itinerary.
        /// </summary>
        public virtual ICollection<Participant> Participants { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Contract.Assert(validationContext != null, "The validation context must not be null.");
            Contract.Assert(validationContext.Items.ContainsKey(EcaContext.VALIDATABLE_CONTEXT_KEY), "The validation context must have a context to query.");
            var context = validationContext.Items[EcaContext.VALIDATABLE_CONTEXT_KEY] as EcaContext;
            Contract.Assert(context != null, "The context must not be null.");

            var existingItinerariesByName = context.Itineraries
                .Where(x =>
                x.Name.ToLower().Trim() == this.Name.ToLower().Trim()
                && x.ProjectId == this.ProjectId
                && x.ItineraryId != this.ItineraryId)
                .FirstOrDefault();
            if (existingItinerariesByName != null)
            {
                yield return new ValidationResult(
                    String.Format("The itinerary with the name [{0}] already exists.",
                        this.Name),
                    new List<string> { "Name" });
            }
        }
    }
}
