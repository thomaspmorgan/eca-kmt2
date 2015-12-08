using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An AddedTravlingPeriod represents a business layer client's request to add a new traveling period for participants in a project.
    /// </summary>
    public class AddedTravelingPeriod : TravelingPeriod, IAuditable
    {
        public AddedTravelingPeriod(
            User creator,
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string name)
            : base(
                startDate,
                endDate,
                name)
        {
            this.Audit = new Create(creator);
        }


        /// <summary>
        /// Gets the audit.
        /// </summary>
        public Audit Audit { get; private set; }
    }
}
