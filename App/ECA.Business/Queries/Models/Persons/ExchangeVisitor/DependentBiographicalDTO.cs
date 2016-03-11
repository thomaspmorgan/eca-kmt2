using ECA.Business.Validation.Sevis.Bio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Queries.Models.Persons.ExchangeVisitor
{
    /// <summary>
    /// A DependentBiographicalDTO contains biography information for participating person's dependent.
    /// </summary>
    public class DependentBiographicalDTO : BiographicalDTO
    {
        /// <summary>
        /// Gets or sets the relationship.
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// Gets or sets the person type id.
        /// </summary>
        public int PersonTypeId { get; set; }

        /// <summary>
        /// Gets or sets the sevis id.
        /// </summary>
        public string SevisId { get; set; }

        /// <summary>
        /// Gets or sets the participant id.  The participant id is used to relate
        /// this dependent back to a participant and then back to the participant person.
        /// </summary>
        public int ParticipantId { get; set; }        
    }
}
