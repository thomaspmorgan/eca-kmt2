using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    public class ECASearchParameters
    {
        public ECASearchParameters(int start, int limit, string filter, IEnumerable<string> facets, IEnumerable<string> fields, string searchTerm)
        {
            this.Start = start;
            this.Limit = limit;
            this.Facets = facets;
            this.Fields = fields;
            this.Filter = filter;
            this.SearchTerm = searchTerm;
        }

        public int Start { get; private set; }
        public int Limit { get; private set; }
        public IEnumerable<string> Facets { get; private set; }
        public IEnumerable<string> Fields { get; private set; }
        public string SearchTerm { get; private set; }

        public string Filter { get; private set; }
    }
}
