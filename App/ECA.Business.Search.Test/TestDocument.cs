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

        public IEnumerable<string> Foci { get; set; }

        public IEnumerable<string> Goals { get; set; }

        public IEnumerable<string> Objectives { get; set; }

        public IEnumerable<string> Themes { get; set; }

        public IEnumerable<string> PointsOfContact { get; set; }

    }
}
