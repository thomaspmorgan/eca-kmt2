using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search.Test
{
    public class OtherTestDocument : TestDocument
    {   

    }

    public class TestDocument
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string OfficeSymbol { get; set; }

        public string Status { get; set; }

        public IEnumerable<string> Foci { get; set; }

        public IEnumerable<string> Goals { get; set; }

        public IEnumerable<string> Objectives { get; set; }

        public IEnumerable<string> Themes { get; set; }

        public IEnumerable<string> PointsOfContact { get; set; }

        public IEnumerable<string> Regions { get; set; }

        public IEnumerable<string> Countries { get; set; }

        public IEnumerable<string> Locations { get; set; }

        public IEnumerable<string> Websites { get; set; }

        public DateTimeOffset? StartDate { get; set; }

        public DateTimeOffset? EndDate { get; set; }

        public IEnumerable<string> Addresses { get; set; }

        public IEnumerable<string> PhoneNumbers { get; set; }
    }
}
