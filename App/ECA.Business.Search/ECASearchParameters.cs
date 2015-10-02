using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Search
{
    /// <summary>
    /// The ECASearchParameters is a wrapper object for the Azure Search Parameters.
    /// </summary>
    public class ECASearchParameters
    {
        /// <summary>
        /// Creates a new ECASearchParameters with the search parameters themselves.
        /// </summary>
        /// <param name="start">The number of records to skip.</param>
        /// <param name="limit">The number of records to return.</param>
        /// <param name="filter">The Azure Search OData filter parameter.</param>
        /// <param name="facets">The search facets.</param>
        /// <param name="fields">The names of the fields to return.</param>
        /// <param name="searchTerm">The search term or keyword.</param>
        public ECASearchParameters(int start, int limit, string filter, IEnumerable<string> facets, IEnumerable<string> fields, string searchTerm)
        {
            this.Start = start;
            this.Limit = limit;
            this.Facets = facets;
            this.Fields = fields;
            this.Filter = filter;
            this.SearchTerm = searchTerm;
        }

        /// <summary>
        /// Gets the start.
        /// </summary>
        public int Start { get; private set; }

        /// <summary>
        /// Gets the limit.
        /// </summary>
        public int Limit { get; private set; }

        /// <summary>
        /// Gets the facets.
        /// </summary>
        public IEnumerable<string> Facets { get; private set; }

        /// <summary>
        /// Gets the fields.
        /// </summary>
        public IEnumerable<string> Fields { get; private set; }

        /// <summary>
        /// Gets the search term.
        /// </summary>
        public string SearchTerm { get; private set; }

        /// <summary>
        /// Gets OData azure search filter.
        /// </summary>
        public string Filter { get; private set; }
    }
}
