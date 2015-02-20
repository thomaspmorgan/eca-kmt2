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
    /// Transportation describes a method of moving a participant from place to place during the course of an itinerary.
    /// </summary>
    public class Transportation
    {
        [Key]
        public int TransportationId { get; set; }
        public virtual Organization Carrier { get; set; }
        public Method Method { get; set; }
        public int MethodId { get; set; }
        public string CarriageId { get; set; }
        public string RecordLocator { get; set; }
        public ItineraryStop ItineraryStop { get; set; }
        public int? ItineraryStopId { get; set; }
        public virtual ICollection<MoneyFlow> RecipientTransportationExpenses { get; set; }

        public History History { get; set; }
    }
}
