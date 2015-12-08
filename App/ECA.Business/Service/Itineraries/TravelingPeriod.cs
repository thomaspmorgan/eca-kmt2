using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// A TravelingPeriod is an eca business layer travel object that has a name and start and end dates.
    /// </summary>
    public class TravelingPeriod
    {
        /// <summary>
        /// Creates a new instance and initializes it with the given values.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="name">The name</param>
        public TravelingPeriod(
            DateTimeOffset startDate,
            DateTimeOffset endDate,
            string name)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.Name = name;
        }

        /// <summary>
        /// Gets the start date.
        /// </summary>
        public DateTimeOffset StartDate { get; private set; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        public DateTimeOffset EndDate { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }
    }
}
